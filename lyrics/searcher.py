from processing_system.configuration import Config
from urllib.parse import urlencode
import requests


def find_lyrics(search_request):
    config = Config()
    parameters = {
        'access_token': config.get('genius', 'api_token'),
        'q':            search_request,
    }
    url = 'https://api.genius.com/search?' + urlencode(parameters)
    res = requests.get(url)
    data = res.json()
    try:
        path = data['response']['hits'][0]['result']['path']
    except(KeyError, IndexError):
        return None
    return 'https://genius.com' + path
