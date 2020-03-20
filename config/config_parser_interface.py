from pathlib import Path
from typing import MutableMapping


class ConfigParserInterface:
    path: Path

    def __init__(self, path: Path) -> None:
        if self.__class__ == ConfigParserInterface:
            raise NotImplementedError

    def load(self) -> None:
        raise NotImplementedError

    def save(self) -> None:
        raise NotImplementedError

    def has_section(self, section: str) -> bool:
        raise NotImplementedError

    def __getitem__(self, item: str) -> MutableMapping[str, str]:
        raise NotImplementedError

    def __setitem__(self, key: str, value: MutableMapping[str, str]) -> None:
        raise NotImplementedError

    def get_bool(self, section: str, key: str) -> bool:
        raise NotImplementedError
