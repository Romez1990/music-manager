import eyed3


class Song:
    def __init__(self, path):
        self.song = eyed3.load(path)
    
    def __del__(self):
        self.song.tag.save()
    
    def artist(self):
        return self.song.tag.artist
    
    def title(self):
        return self.song.tag.title
    
    def write_lyrics(self, lyrics):
        if lyrics is not None:
            lyrics = Song.process_lyrics(lyrics)
            self.song.tag.lyrics.set(lyrics)
    
    @staticmethod
    def process_lyrics(text):
        text = text.replace('’', '\'')
        return text
