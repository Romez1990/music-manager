from typing import Mapping, Any
from aiohttp import ClientSession
from returns.result import Result, Success, Failure


async def fetch_json(
    url: str,
    params: Mapping[str, str] = None
) -> Result[Any, Any]:
    async with ClientSession() as session:
        async with session.get(
            url,
            allow_redirects=False,
            params=params,
        ) as response:
            json = await response.json()
            if response.status != 200:
                return Failure(json)
            return Success(json)
