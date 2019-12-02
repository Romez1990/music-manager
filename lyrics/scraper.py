from typing import Optional
from bs4 import BeautifulSoup
import requests


def scrap_lyrics(url: str) -> Optional[str]:
    html = fetch_html(url)
    lyrics = parse_html(html)
    lyrics = process_lyrics(lyrics)
    return lyrics


def fetch_html(url: str) -> str:
    result = requests.get(url)
    html: str = result.text
    return html


def parse_html(html: str) -> str:
    soup = BeautifulSoup(html, 'lxml')
    lyrics = soup.find('div', class_='lyrics').get_text()
    return lyrics


def process_lyrics(lyrics: str) -> str:
    lyrics = lyrics.strip()
    lyrics = lyrics.replace('’', '\'')
    return lyrics
