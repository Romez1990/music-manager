from sys import platform

configuration_default = {
    'browser': {
        'driver':   r'C:\Program Files (x86)\Chrome Driver\Chrome Driver.exe' if platform == 'win32' else '/usr/bin/chromedriver' if platform == 'linux' else '',
        'minimize': True
    },
    'genius':  {
        'api_token': ''
    }
}
