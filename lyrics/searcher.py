from typing import Optional, List

from config import Config
from request import fetch_json


async def search_lyrics(search_query: str) -> Optional[str]:
    data_result = await fetch_json(
        'https://api.genius.com/search',
        params={
            'access_token': Config.Genius.api_token,
            'q': search_query,
        }
    )
    data = data_result.unwrap()
    hits: List = data['response']['hits']
    if not hits:
        return None
    result_url = hits[0]['result']['path']
    return f'https://genius.com{result_url}'
