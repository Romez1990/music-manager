from pathlib import Path
from configparser import ConfigParser as Config
from typing import MutableMapping

from .config_parser_interface import ConfigParserInterface


class ConfigParser(ConfigParserInterface):
    def __init__(self, path: Path) -> None:
        super().__init__(path)
        self.path = path
        self._config = Config()

    def load(self) -> None:
        self._config.read(self.path)

    def save(self) -> None:
        with self.path.open('w') as file:
            self._config.write(file)

    def has_section(self, section: str) -> bool:
        return self._config.has_section(section)

    def __getitem__(self, item: str) -> MutableMapping[str, str]:
        if not self._config.has_section(item):
            return {}
        return self._config[item]

    def __setitem__(self, key: str, value: MutableMapping[str, str]) -> None:
        self._config[key] = value

    def get_bool(self, section: str, key: str) -> bool:
        return self._config.getboolean(section, key)
