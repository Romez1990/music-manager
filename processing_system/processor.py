from processing_system.mode import Mode
from processing_system.handler import MainHandler
import os


class Processor:
    def __init__(self, error_handlers):
        self.handler = MainHandler(error_handlers)
        self.handler.on_progress = self.progress
        self.handler.on_song_change = self.song_change
        self.filesystem = None
        self.current_points = 0
        self.all_points = 0
        self.on_progress = None
        self.on_song_change = None
        self.on_complete = None
        
        self.filters = {
            Mode.album:       self.__filter_album,
            Mode.band:        self.__filter_band,
            Mode.compilation: self.__filter_compilation,
        }
    
    def process(self, dir, mode, flags):
        self.filesystem = [] if mode is Mode.album else {}
        self.__iterate(dir, mode, self.filesystem)
        self.all_points = self.handler.evaluate(flags, self.filesystem, mode)
        self.current_points = 0
        self.handler.handle(flags, self.filesystem, mode)
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
    
    def progress(self, value):
        self.current_points += value
        self.on_progress(self.current_points / self.all_points)
    
    def song_change(self, song):
        self.on_song_change(song)
