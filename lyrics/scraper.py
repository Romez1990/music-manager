from typing import Optional
from bs4 import BeautifulSoup
from aiohttp import ClientSession


async def scrap_lyrics(url: str) -> Optional[str]:
    html = await fetch_html(url)
    lyrics = parse_html(html)
    lyrics = process_lyrics(lyrics)
    return lyrics


async def fetch_html(url: str) -> str:
    async with ClientSession() as session:
        async with session.get(url) as response:
            html = await response.text()
            return html


def parse_html(html: str) -> str:
    soup = BeautifulSoup(html, 'lxml')
    lyrics = soup.find('div', class_='lyrics').get_text()
    return lyrics


def process_lyrics(lyrics: str) -> str:
    lyrics = lyrics.strip()
    lyrics = lyrics.replace('’', '\'')
    return lyrics
