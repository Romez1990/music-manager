from pathlib import Path
from typing import Set, Callable, Optional, Any

from filesystem import Directory
from .mode import Mode
from .flags import Flag


class ProcessorInterface:
    def __init__(self, handlers: Callable[[str, Any], Any]) -> None:
        self._filesystem: Optional[Directory] = None
        self.handlers = handlers

    @property
    def filesystem(self) -> Optional[Directory]:
        return self._filesystem

    def scan(self, path: Path, mode: Mode) -> None:
        raise NotImplementedError

    def process(self, flags: Set[Flag]) -> None:
        raise NotImplementedError
