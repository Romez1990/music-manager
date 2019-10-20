from pathlib import Path
from typing import Union, Optional, Callable

from .state import State


class FsNode:
    def __init__(self, path: Union[str, Path]) -> None:
        self._path = self._resole_path(path)
        self.state: State = State.disabled
        self.on_mark: Optional[Callable] = None
        self.on_unmark: Optional[Callable] = None

    def _resole_path(self, path: Union[str, Path]) -> Path:
        if isinstance(path, str):
            return self._resole_path(Path(path))

        if not path.exists():
            raise FileNotFoundError(f'file or directory {path} not found')

        return path

    @property
    def path(self) -> Path:
        return self._path

    def mark(self) -> None:
        self.state = State.enabled
        if self.on_mark is not None:
            self.on_mark()

    def unmark(self) -> None:
        self.state = State.disabled
        if self.on_unmark is not None:
            self.on_unmark()

    def __str__(self) -> str:
        return str(self._path)
