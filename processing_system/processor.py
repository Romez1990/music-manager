from processing_system.mode import Mode
from processing_system.handler import MainHandler
import os


class Processor:
    def __init__(self, error_handlers):
        self.error_handlers = error_handlers
        self.filesystem = None
        self.on_complete = None
        
        self.filters = {
            Mode.album:       self.__filter_album,
            Mode.band:        self.__filter_band,
            Mode.compilation: self.__filter_compilation,
        }
    
    def process(self, dir, mode, flags):
        self.filesystem = [] if mode is Mode.album else {}
        self.__iterate(dir, mode, self.filesystem)
        handler = MainHandler(self.error_handlers)
        handler.handle(flags, mode, self.filesystem)
        self.on_complete()
    
    def __iterate(self, dir, mode, container):
        for filename in sorted(os.listdir(dir)):
            path = os.path.join(dir, filename)
            self.filters[mode](path, container)
    
    def __filter_album(self, path, container):
        if path.endswith('.mp3') and os.path.isfile(path):
            container.append(path)
    
    def __filter_band(self, path, container):
        if os.path.isdir(path):
            container[path] = []
            self.__iterate(path, Mode(Mode.album), container[path])
    
    def __filter_compilation(self, path, container):
        if os.path.isdir(path):
            container[path] = {}
            self.__iterate(path, Mode(Mode.band), container[path])
