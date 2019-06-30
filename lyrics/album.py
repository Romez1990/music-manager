from lyrics.browser import Browser
from lyrics.directory_iterator import iterate
import os
from lyrics.writer import Song
from lyrics.searcher import find_lyrics


def process(dir, browser=None):
    browser = Browser.exists(browser)
    iterate(browser, dir, handler)


def handler(browser, path):
    if path.endswith('.mp3') and os.path.isfile(path):
        song = Song(path)
        url = find_lyrics(song.artist(), song.title())
        lyrics = browser.get_lyrics(url)
        song.write_lyrics(lyrics)
