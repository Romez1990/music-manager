from enum import Enum
import os
from lyrics.browser import Browser
from lyrics.writer import Song
from lyrics.searcher import find_lyrics


class Processor:
    def __init__(self, flags, error_handlers):
        self.flags = flags
        self.error_handlers = error_handlers
        self.browser = Browser()
        self.handlers = {
            Mode.album:       self.handle_album,
            Mode.band:        self.handle_band,
            Mode.compilation: self.handle_compilation,
        }
    
    def process(self, dir, mode):
        for filename in sorted(os.listdir(dir)):
            path = os.path.join(dir, filename)
            self.handlers[mode](path)
    
    def handle_album(self, path):
        if path.endswith('.mp3') and os.path.isfile(path):
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
    
    def handle_band(self, path):
        if os.path.isdir(path):
            self.process(path, Mode.album)
    
    def handle_compilation(self, path):
        if os.path.isdir(path):
            self.process(path, Mode.band)


class Mode(Enum):
    album = 0
    band = 1
    compilation = 2
