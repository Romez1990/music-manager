from lyrics.browser import Browser
from lyrics.writer import Song
from lyrics.searcher import find_lyrics
from urllib.parse import quote


class Handler:
    def __init__(self, error_handlers):
        self.browser = None
        self.error_handlers = error_handlers
    
    def handle(self, filesystem, mode):
        self.browser = Browser()
        self.iterate(filesystem)
        self.browser = None
    
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
        search_request = artist + ' ' + title
        url = find_lyrics(search_request)
        if url is None:
            try:
                parentheses_index = search_request.index('(')
            except ValueError:
                pass
            else:
                search_request_without_parentheses = search_request[:parentheses_index]
                url = find_lyrics(search_request_without_parentheses)
        if url is None:
            lyrics = self.lyrics_not_found(search_request, artist, album, title)
        else:
            lyrics = self.browser.get_lyrics(url)
        song.write_lyrics(lyrics)
    
    def lyrics_not_found(self, search_request, artist, album, title):
        self.browser.open(f'https://genius.com/search?q={quote(search_request)}')
        self.browser.focus()
        self.browser.maximize()
        if self.error_handlers['lyrics_not_found'](artist, album, title):
            return self.browser.get_lyrics()
        else:
            return None
