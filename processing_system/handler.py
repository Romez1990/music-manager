from processing_system.flags import Flag


class MainHandler:
    def __init__(self, error_handlers):
        self.error_handlers = error_handlers
        self.handlers = {
            Flag.lyrics: self.handle_lyrics
        }
    
    def handle(self, flags, mode, filesystem):
        for flag in flags:
            self.handlers[flag](mode, filesystem)
    
    def handle_lyrics(self, mode, filesystem):
        from lyrics.handler import Handler
        handler = Handler(self.error_handlers)
        handler.handle(filesystem, mode)
