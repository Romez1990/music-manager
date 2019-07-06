from lyrics.browser import Browser
from lyrics.writer import Song
from lyrics.searcher import find_lyrics


class Handler:
    def __init__(self, error_handlers):
        self.browser = Browser()
        self.error_handlers = error_handlers
    
    def handle(self, filesystem, mode):
        self.iterate(filesystem)
    
    def iterate(self, filesystem):
        if type(filesystem) is dict:
            list_ = filesystem.values()
        else:
            list_ = filesystem
        
        for item in list_:
            if type(item) is dict or type(item) is list:
                self.iterate(item)
            else:
                self.handle_song(item)
    
    def handle_song(self, path):
        song = Song(path)
        artist = song.artist()
        album = song.album()
        title = song.title()
        url = find_lyrics(artist, title)
        if url is None:
            self.browser.open('https://genius.com')
            self.browser.maximize()
            if self.error_handlers['lyrics_not_found'](artist, album, title):
                lyrics = self.browser.get_lyrics()
            else:
                lyrics = None
        else:
            lyrics = self.browser.get_lyrics(url)
        song.write_lyrics(lyrics)
