from typing import Optional
from bs4 import Tag

from scraper import scrap


async def scrap_lyrics(url: str) -> Optional[str]:
    soup = await scrap(url)
    lyrics_block: Optional[Tag] = soup.select_one('.lyrics')
    if lyrics_block is None:
        return None
    lyrics: str = lyrics_block.get_text()
    lyrics = process_lyrics(lyrics)
    return lyrics


def process_lyrics(lyrics: str) -> str:
    lyrics = lyrics.strip()
    lyrics = lyrics.replace('’', '\'')
    return lyrics
