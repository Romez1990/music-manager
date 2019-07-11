from processing_system.mode import Mode
from processing_system.handler import MainHandler
import os


class Processor:
    def __init__(self, error_handlers):
        self.error_handlers = error_handlers
        self.filesystem = None
        self.on_complete = None
        
        self.filters = {
            Mode.album:       self.filter_album,
            Mode.band:        self.filter_band,
            Mode.compilation: self.filter_compilation,
        }
    
    def process(self, dir, mode, flags):
        self.filesystem = [] if mode is Mode.album else {}
        self.iterate(dir, mode, self.filesystem)
        handler = MainHandler(self.error_handlers)
        handler.handle(flags, mode, self.filesystem)
        self.on_complete()
    
    def iterate(self, dir, mode, container):
        for filename in sorted(os.listdir(dir)):
            path = os.path.join(dir, filename)
            self.filters[mode](path, container)
    
    def filter_album(self, path, container):
        if path.endswith('.mp3') and os.path.isfile(path):
            container.append(path)
    
    def filter_band(self, path, container):
        if os.path.isdir(path):
            container[path] = []
            self.iterate(path, Mode(Mode.album), container[path])
    
    def filter_compilation(self, path, container):
        if os.path.isdir(path):
            container[path] = {}
            self.iterate(path, Mode(Mode.band), container[path])
