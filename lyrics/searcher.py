from configuration import Config
from urllib.parse import quote
import requests


def find_lyrics(search_request):
    config = Config()
    token = config.get('genius', 'api_token')
    url = f'https://api.genius.com/search?access_token={token}&q={quote(search_request)}'
    res = requests.get(url)
    data = res.json()
    try:
        path = data['response']['hits'][0]['result']['path']
        return 'https://genius.com' + path
    except(KeyError, IndexError):
        return None
