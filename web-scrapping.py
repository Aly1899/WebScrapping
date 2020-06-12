# import requests
from urllib.request import Request, urlopen
import urllib.request
import requests
from bs4 import BeautifulSoup as soup
import xlwt 
from xlwt import Workbook 
import mysql.connector
from mysql.connector import Error
from datetime import datetime
import pytz
import logging
import sys
from decimal import Decimal
from http_request_randomizer.requests.proxy.requestProxy import RequestProxy
from selenium import webdriver
from random import seed
from random import randint
import progressbar

# def connect():
#     logging.basicConfig(filename="test.log", level=logging.DEBUG)
#     """ Connect to MySQL database """
#     conn = None
#     try:
#         conn = mysql.connector.connect(host='localhost',
#                                        database='ingatlan',
#                                        user='root',
#                                        password='password',
#                                        auth_plugin='mysql_native_password')
#         if conn.is_connected():
#             print('Connected to MySQL database')
        
#         queryNewItem = "INSERT INTO hirdetesek(adID, adType, Price, PricePerSqm, Address, Area,PlotArea, LeaseRights, Balcony, Date) " \
#             "VALUES(%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)"
#         queryChange = "INSERT INTO arvaltozas(adID, new_price, Date) " \
#             "VALUES(%s,%s,%s)"
#         try:
#           existingRecords = getAllRecordsFromDb(conn)
#           telek = createAdCollection('+telek','', existingRecords)
#           print (telek[0])
#           print ('****************')
#           print (telek[1])
#           cursor = conn.cursor()
#           cursor.executemany(queryNewItem, telek[0])
#           cursor.executemany(queryChange, telek[1])
#           conn.commit()
          # lakas = dataCollect('+lakas','+budapest')
          # print (lakas)
          # cursor = conn.cursor()
          # cursor.executemany(query, lakas)
          # conn.commit()
    #     except Exception as e:
    #               logging.debug(f'Error:{e}')
    # except Error as e:
    #     print(e)

    # finally:
    #     if conn is not None and conn.is_connected():
    #         conn.close()

def openConnection():
  """ Connect to MySQL database """
  # conn = None
  global conn
  conn = mysql.connector.connect(host='localhost',
                                  database='ingatlan',
                                  user='root',
                                  password='password',
                                  auth_plugin='mysql_native_password')
  if conn.is_connected():
    print('Connected to MySQL database')

def closeConnection():
  if conn is not None and conn.is_connected():
    conn.close()
    print('Connection closed')

def queryExecution(query, dataSet):
  logging.basicConfig(filename="test.log", level=logging.DEBUG)
  try:
    openConnection()
    try:
      cursor = conn.cursor()
      cursor.executemany(query, dataSet)
      conn.commit()
    except Exception as e:
      logging.debug(f'Error:{e}')
  except Error as e:
    print(e)
  finally:
    closeConnection()
# def dataCollect(adType, place):
#   url = f'https://ingatlan.com/lista/elado{adType}{place}?page=1'
#   req = Request(url, headers={'User-Agent': 'Chrome/81.0.4044.138'})
#   html = urlopen(req, timeout=20).read()
#   s = soup(html, 'html.parser')
#   paging = s.find('div', class_='pagination__page-number')
#   pageNr = paging.text.split(' ')[3]
#   print (pageNr)
#   adItems=[]

#   for page in range(2):

#     url = f'https://ingatlan.com/lista/elado{adType}{place}?page={page+1}'
#     req = Request(url, headers={'User-Agent': 'Chrome/81.0.4044.138'})
#     html = urlopen(req, timeout=20).read()
#     s = soup(html, 'html.parser')


#     results = s.find_all('div', class_=['listing', 'listing--cluster-parent'])
    

#     for result in results:
#       tz = pytz.timezone('Europe/Berlin')
#       current_datetime = datetime.now(tz)
#       # current_date = date.today() 
#       try:
#         adItem=(
#           result['data-id'],
#           'telek' if adType=='+telek' else 'lakás',
#           result.find('div',class_='price').text.split(' ')[1] if result.find('div',class_='price') is not None else 0,
#           result.find('div',class_='price--sqm').text[:-7].replace(' ','') if result.find('div',class_='price--sqm') is not None else None,
#           result.find('div',class_='listing__address').text if result.find('div',class_='listing__address') is not None else '',
#           result.find('div',class_='listing__data--area-size').text.split(' ')[1] if result.find('div',class_='listing__data--area-size') is not None else None,
#           result.find('div',class_='listing__data--plot-size').text[:-9].replace(' ','') if result.find('div',class_='listing__data--plot-size') is not None else None,
#           'Bérleti jog' if result.find('div',class_='label--alert') is not None else '',
#           result.find('div',class_='listing__data--balcony-size').text if result.find('div',class_='listing__data--balcony-size') is not None else '',
#           current_datetime
#         )
#         adItems.append(adItem)
#       except Exception as e:
#         logging.debug(f'Error: {page+1},{adType} {result["data-id"] if result["data-id"] is not None else "no ID"}, {e}')
#         continue
    
#     print(page, end='\n')
#   return adItems

def getNumberOfPages(adType, place):
  url = f'https://ingatlan.com/lista/elado{adType}{place}?page=1'
  req = Request(url, headers={'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36'})
  html = urlopen(req, timeout=120).read()
  s = soup(html, 'html.parser')
  paging = s.find('div', class_='pagination__page-number')
  numberOfPage = paging.text.split(' ')[3]
  print (numberOfPage)
  return numberOfPage

def createAdItem(adItem, adType, page):
  tz = pytz.timezone('Europe/Berlin')
  current_datetime = datetime.now(tz)
  try:
    adRecord=(
      adItem['data-id'],
      'telek' if adType=='+telek' else 'lakás',
      adItem.find('div',class_='price').text.split(' ')[1] if adItem.find('div',class_='price') is not None else 0,
      adItem.find('div',class_='price--sqm').text[:-7].replace(' ','') if adItem.find('div',class_='price--sqm') is not None else None,
      adItem.find('div',class_='listing__address').text if adItem.find('div',class_='listing__address') is not None else '',
      adItem.find('div',class_='listing__data--area-size').text.split(' ')[1] if adItem.find('div',class_='listing__data--area-size') is not None else None,
      adItem.find('div',class_='listing__data--plot-size').text[:-9].replace(' ','') if adItem.find('div',class_='listing__data--plot-size') is not None else None,
      'Bérleti jog' if adItem.find('div',class_='label--alert') is not None else '',
      adItem.find('div',class_='listing__data--balcony-size').text if adItem.find('div',class_='listing__data--balcony-size') is not None else '',
      current_datetime
    )
    # adItems.append(adItem)
  except Exception as e:
    logging.debug(f'Error: {page+1},{adType} {adItem["data-id"] if adItem["data-id"] is not None else "no ID"}, {e}')
    # continue
  return adRecord

def createAdCollection(adType, place, existingRecords):
  tz = pytz.timezone('Europe/Berlin')
  current_datetime = datetime.now(tz)
  adRecords = [[],[]]
  nrOfPages = getNumberOfPages(adType, place)
  
  # req_proxy = RequestProxy() #you may get different number of proxy when  you run this at each time
  # proxies = req_proxy.get_proxy_list() #this will create proxy list
  # ind = [] #int is list of Indian proxy
  # s= requests.session()
  # for proxy in proxies:
  #     if proxy.country == 'Hungary' or proxy.country == 'Romania' or proxy.country == 'Slovakia':
  #         ind.append(proxy)
  # print(len(ind))
  bar = progressbar.ProgressBar(maxval=int(nrOfPages), \
    widgets=[progressbar.Bar('=', '[', ']'), ' ', progressbar.Percentage()])
  bar.start()       
  for page in range(int(nrOfPages)):
    bar.update(page)
    # proxyIndex = randint(0,len(ind)-1)
    # proxyIp = ind[proxyIndex].ip + ':' + ind[proxyIndex].port
    # print(proxyIp)
    # proxy_support = urllib.request.ProxyHandler({
    #   "httpProxy":ind[proxyIndex],
    #   "ftpProxy":ind[proxyIndex],
    #   "sslProxy":ind[proxyIndex]
    # })
    # opener = urllib.request.build_opener(proxy_support)
    # urllib.request.install_opener(opener)
    # proxyDict={
    #           "httpProxy":ind[proxyIndex],
    #           "ftpProxy":ind[proxyIndex],
    #           "sslProxy":ind[proxyIndex],
    #       }


    url = f'https://ingatlan.com/lista/elado{adType}{place}?page={page+1}'
    # url = f'http://kelevill.hu'
    req = Request(url, headers={'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Safari/537.36'})
    # req.set_proxy('92.249.156.1', 'https')
    # print(req)
    html = urlopen(req, timeout=120).read()
    s = soup(html, 'html.parser')
    adItems = s.find_all('div', class_=['listing', 'listing--cluster-parent'])
    print(page)
    for adItem in adItems :
      adRecord = createAdItem(adItem, adType, page)
      print(adRecord[0])

      res = [s for s in existingRecords if s[0]==adRecord[0]]
      if res == []:
        adRecords[0].append(adRecord)
      elif res[0][2] != Decimal(adRecord[2]) :
        updateQuery = f'UPDATE hirdetesek SET Price = {adRecord[2]} WHERE adId = {adRecord[0]}'
        updateRecord(updateQuery)
        changeRecord = (
          adRecord[0],
          res[0][2],
          adRecord[2],
          current_datetime
        )
        adRecords[1].append(changeRecord)
      # else:
      #   print(f'{res[0][2]} ---{adRecord[2]}')
  bar.finish()    
  return adRecords

def updateRecord(query):
  try:
    openConnection()
    try:
      cursor = conn.cursor()
      cursor.execute(query)
      conn.commit()
    except Exception as e:
      print (e)
      logging.debug(f'Error:{e}')
  except Error as e:
    print(e)
  finally:
    closeConnection()


def getAllRecordsFromDb():
  try:
    openConnection()
    try:
      cursor = conn.cursor()
      cursor.execute("SELECT * FROM hirdetesek")
      allRecords = cursor.fetchall()
      # print(len(allRecords))
      # for x in allRecords:
      #   print (x)
    except Exception as e:
      print (e)
      logging.debug(f'Error:{e}')
  except Error as e:
    print(e)
  finally:
    closeConnection()
  return allRecords

def mainProgram():
  queryNewItem = "INSERT INTO hirdetesek(adID, adType, Price, PricePerSqm, Address, Area,PlotArea, LeaseRights, Balcony, Date) " \
            "VALUES(%s,%s,%s,%s,%s,%s,%s,%s,%s,%s)"
  queryChange = "INSERT INTO arvaltozas(adID, old_price, new_price, Date) " \
      "VALUES(%s,%s, %s,%s)"
  existingRecords = getAllRecordsFromDb()
  # telek = createAdCollection('+telek','', existingRecords)
  telek = createAdCollection(sys.argv[1],sys.argv[2], existingRecords)
  queryExecution(queryNewItem,telek[0])
  queryExecution(queryChange,telek[1])
  # print(telek[0])
  # print('****************')
  # print(telek[1])
  # lakas = createAdCollection('+lakas','+budapest', existingRecords)
  # queryExecution(queryNewItem,lakas[0])
  # queryExecution(queryChange,lakas[1])

conn = None
mainProgram()