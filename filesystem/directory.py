from pathlib import Path
from typing import Union, Optional, Callable, List, Iterator

from .fs_node import FsNode
from .file import File
from .state import State


class Directory(FsNode):
    def __init__(self, path: Union[str, Path]) -> None:
        super().__init__(path)
        if not self._path.exists():
            raise FileNotFoundError(
                f'{path} should be a directory, but it is a file')
        self._read_content()
        self.on_mark_partially: Optional[Callable] = None

    def _read_content(self) -> None:
        self._content: List[FsNode] = []
        for fs_node in self.path.iterdir():
            if fs_node.is_dir():
                directory = Directory(fs_node)
                directory.on_mark = self.on_mark_handler
                directory.on_mark_partially = self.on_mark_handler
                directory.on_unmark = self.on_unmark_handler
                self._content.append(directory)
            else:
                file = File(fs_node)
                file.on_mark = self.on_mark_handler
                file.on_unmark = self.on_unmark_handler
                self._content.append(file)

    @property
    def content(self) -> List[FsNode]:
        return self._content

    def __iter__(self) -> Iterator[FsNode]:
        return iter(self.content)

    def mark_partially(self) -> None:
        self.state = State.partially
        if self.on_mark is not None:
            self.on_mark()

    def on_mark_handler(self) -> None:
        if all(fs_node.state is State.enabled for fs_node in self.content):
            self.mark()
        else:
            self.mark_partially()

    def on_unmark_handler(self) -> None:
        if all(fs_node.state is State.disabled for fs_node in self.content):
            self.unmark()
        else:
            self.mark_partially()
