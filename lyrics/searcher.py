from urllib.parse import urlencode
from typing import Optional
from aiohttp import ClientSession

from configuration import Config


async def search_lyrics(search_query: str) -> Optional[str]:
    config = Config()
    parameters = {
        'access_token': config.genius.api_token,
        'q': search_query,
    }
    request_url = 'https://api.genius.com/search?' + urlencode(parameters)
    data = await fetch_json(request_url)
    try:
        result_url = data['response']['hits'][0]['result']['path']
    except(KeyError, IndexError):
        return None
    else:
        return 'https://genius.com' + result_url


async def fetch_json(url: str) -> str:
    async with ClientSession() as session:
        async with session.get(url) as response:
            json = await response.json()
            return json
