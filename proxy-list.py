#!/usr/bin/env python
print('If you get error "ImportError: No module named \'six\'"'+\
    'install six:\n$ sudo pip install six');
import sys
import ssl
if sys.version_info[0]==2:
    import six
    from six.moves.urllib import request
    ctx = ssl.create_default_context()
    ctx.verify_flags = ssl.VERIFY_DEFAULT
    opener = request.build_opener(
        request.ProxyHandler({'http': 'http://127.0.0.1:24000'}),
        request.HTTPSHandler(context=ctx))
    print(opener.open('https://lumtest.com/myip.json').read())
if sys.version_info[0]==3:
    import urllib.request
    ctx = ssl.create_default_context()
    ctx.verify_flags = ssl.VERIFY_DEFAULT
    opener = urllib.request.build_opener(
        urllib.request.ProxyHandler({'http': 'http://127.0.0.1:24000'}),
        urllib.request.HTTPSHandler(context=ctx))
    print(opener.open('https://lumtest.com/myip.json').read())