from bs4 import BeautifulSoup

from request import fetch_html


async def scrap(url: str) -> BeautifulSoup:
    html = await fetch_html(url)
    soup = BeautifulSoup(html, 'lxml')
    return soup
