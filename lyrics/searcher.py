from urllib.parse import urlencode
from typing import Optional
import requests

from configuration import Config


def search_lyrics(search_query: str) -> Optional[str]:
    config = Config()
    parameters = {
        'access_token': config.genius.api_token,
        'q': search_query,
    }
    request_url = 'https://api.genius.com/search?' + urlencode(parameters)
    result = requests.get(request_url)
    data = result.json()
    try:
        result_url = data['response']['hits'][0]['result']['path']
    except(KeyError, IndexError):
        return None
    else:
        return 'https://genius.com' + result_url
