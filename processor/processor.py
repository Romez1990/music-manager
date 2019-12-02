from pathlib import Path
from typing import Callable, Any, Dict, Set

from contracts import Processor, Action, Flag, Mode
from filesystem import File, Directory


class MainProcessor(Processor):
    def __init__(self, handlers: Callable[[str, Any], Any]) -> None:
        super().__init__(handlers)

        from lyrics import LyricsAction
        self._actions: Dict[Flag, Action] = {
            Flag.lyrics: LyricsAction(self.handlers),
        }

    def scan(self, path: Path, _mode: Mode) -> None:
        self._filesystem = Directory(path)
        self.mark_enabled(self._filesystem, _mode)

    def mark_enabled(self, directory: Directory, mode: Mode) -> None:
        for fs_node in directory:
            if mode is not Mode.album:
                if isinstance(fs_node, Directory):
                    self.mark_enabled(fs_node, Mode(mode.value - 1))
            else:
                if isinstance(fs_node, File) and fs_node.path.suffix == '.mp3':
                    fs_node.mark()

    def process(self, flags: Set[Flag]) -> None:
        if self._filesystem is None:
            raise ValueError('Nothing to process, scan first')
        for flag in flags:
            self._actions[flag].perform(self._filesystem)
