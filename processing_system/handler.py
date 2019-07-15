from processing_system.flags import Flag


class MainHandler:
    def __init__(self, error_handlers):
        self.on_progress = None
        self.on_song_change = None
        self.error_handlers = error_handlers
        self.handlers = {}
    
    def create_handlers(self, flags):
        creates = {
            Flag.lyrics: self.__create_lyrics_handler,
        }
        for flag in flags:
            self.handlers[flag] = creates[flag]()
            self.handlers[flag].on_progress = self.on_progress
            self.handlers[flag].on_song_change = self.on_song_change
    
    def __create_lyrics_handler(self):
        from lyrics.handler import Handler
        return Handler(self.error_handlers)
    
    def evaluate(self, flags, filesystem, mode):
        self.create_handlers(flags)
        points = 0
        for flag in flags:
            points += self.handlers[flag].evaluate(filesystem, mode)
        return points
    
    def handle(self, flags, filesystem, mode):
        for flag in flags:
            self.handlers[flag].handle(mode, filesystem)
