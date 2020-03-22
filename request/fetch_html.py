from aiohttp import ClientSession
from typing import Mapping


async def fetch_html(url: str, params: Mapping[str, str] = None) -> str:
    async with ClientSession() as session:
        async with session.get(
            url,
            allow_redirects=False,
            raise_for_status=True,
            params=params,
        ) as response:
            html = await response.text()
            return html
