from processing_system.mode import Mode
from lyrics.browser import Browser
from lyrics.writer import Song
from lyrics.searcher import find_lyrics
from urllib.parse import quote


class Handler:
    def __init__(self, error_handlers):
        self.browser = None
        self.error_handlers = error_handlers
        self.song_list = []
        self.on_song_change = None
        self.on_progress = None
    
    def evaluate(self, filesystem, mode):
        self.iterate(filesystem, mode)
        return len(self.song_list)
    
    def iterate(self, filesystem, mode):
        if mode is Mode.band or mode is Mode.compilation:
            for item in filesystem.values():
                self.iterate(item, Mode(mode.value - 1))
        else:
            self.song_list.extend(filesystem)
    
    def handle(self, filesystem, mode):
        if self.song_list:
            self.browser = Browser()
        for path in self.song_list:
            song = Song(path)
            self.on_song_change({
                'artist': song.artist(),
                'album':  song.album(),
                'title':  song.title(),
            })
            self.handle_song(song)
            self.on_progress(1)
        self.browser = None
    
    def handle_song(self, song):
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
