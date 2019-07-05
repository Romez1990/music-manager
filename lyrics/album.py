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
        artist = song.artist()
        album = song.album()
        title = song.title()
        url = find_lyrics(artist, title)
        if url is None:
            browser.open('https://genius.com')
            print('We can\'t find the lyrics for this song:')
            print(f'\tArtist: {artist}')
            print(f'\tAlbum: {album}')
            print(f'\tTitle: {title}')
            print('Find the page for this song in the browser')
            os.system('pause')
            lyrics = browser.get_lyrics()
        else:
            lyrics = browser.get_lyrics(url)
        song.write_lyrics(lyrics)
