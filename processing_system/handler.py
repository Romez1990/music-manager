from processing_system.flags import Flag


class MainHandler:
    def __init__(self, error_handlers):
        self.error_handlers = error_handlers
        self.handlers = {}
    
    def create_handlers(self, flags):
        creates = {
            Flag.lyrics: self.__create_lyrics_handler,
        }
        for flag in flags:
            self.handlers[flag] = creates[flag]()
    
    def __create_lyrics_handler(self):
        from lyrics.handler import Handler
        return Handler(self.error_handlers)
    
    def handle(self, flags, mode, filesystem):
        self.create_handlers(flags)
        for flag in flags:
            self.handlers[flag].handle(mode, filesystem)
