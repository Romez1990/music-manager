from configuration import Config
from urllib.parse import quote
import requests


def find_lyrics(artist, title):
    config = Config()
    token = config.get('genius', 'api_token')
    search_request = artist + ' ' + title
    url = f'https://api.genius.com/search?access_token={token}&q={quote(search_request)}'
    res = requests.get(url)
    data = res.json()
    try:
        path = data['response']['hits'][0]['result']['path']
        return 'https://genius.com' + path
    except(KeyError, IndexError):
        return None
