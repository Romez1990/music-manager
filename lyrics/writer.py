from mutagen.id3 import ID3, USLT


class Song:
    def __init__(self, path):
        self.path = path
        self.tags = ID3(path)
    
    def __del__(self):
        self.tags.save(self.path)
    
    def artist(self):
        return self.tags['TPE1'].text[0]
    
    def album(self):
        return self.tags['TALB'].text[0]
    
    def title(self):
        return self.tags['TIT2'].text[0]
    
    def write_lyrics(self, lyrics):
        if lyrics is not None:
            lyrics = Song.process_lyrics(lyrics)
            self.tags['USLT'] = USLT(encoding=3, text=lyrics)
    
    @staticmethod
    def process_lyrics(text):
        text = text.replace('’', '\'')
        return text
