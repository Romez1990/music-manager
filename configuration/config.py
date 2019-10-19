from pathlib import Path
from sys import platform
from os import getenv as env
from typing import Callable
from configparser import ConfigParser

from helpers import singleton


@singleton
class Config:
    # Sections:

    class genius:
        api_token = ''

    def __init__(self) -> None:
        self._config_dir_path = self._get_config_dir_path()
        self._config_path = self._config_dir_path / 'config.cfg'
        self._config = ConfigParser()
        if self._config_path.exists():
            self._read()
            self._load()

    def _get_config_dir_path(self) -> Path:
        if platform == 'win32':
            appdata = env('APPDATA')
            if appdata is None:
                raise SystemError('No APPDATA environment variable')
            return Path(appdata) / 'Music Manager'

        if platform == 'linux':
            home = env('HOME')
            if home is None:
                raise SystemError('No HOME environment variable')
            return Path(home) / '.config' / 'music-manager'

        raise SystemError(f'platform {platform} is unsupported')

    def _read(self) -> None:
        self._config.read(self._config_path)

    def _load(self) -> None:
        self._iterate_sections(self._load_sections)

    def _load_sections(self, section: str) -> None:
        if section not in self._config.sections():
            self._config[section] = {}
        self._iterate_keys(section, self._load_keys)

    def _load_keys(self, section_name: str, key: str) -> None:
        if key not in self._config[section_name]:
            return

        type_ = type(getattr(getattr(self, section_name), key))
        section = getattr(self, section_name)
        if type_ is bool:
            setattr(section, key, self._config.getboolean(section_name, key))
        elif type_ is str:
            setattr(section, key, self._config[section_name][key])
        else:
            raise TypeError(f'key {key} in section {section_name} '
                            f'is of unsupported type {type_}')

    def save(self) -> None:
        self._iterate_sections(self._save_sections)
        self._config_dir_path.mkdir(parents=True, exist_ok=True)
        with self._config_path.open('w') as file:
            self._config.write(file)

    def _save_sections(self, section: str) -> None:
        if section not in self._config.sections():
            self._config[section] = {}

        self._iterate_keys(section, self._save_keys)

    def _save_keys(self, section: str, key: str) -> None:
        value = getattr(getattr(self, section), key)
        if type(value) is bool:
            self._config[section][key] = str(value).lower()
        elif type(value) is str:
            self._config[section][key] = value
        else:
            raise TypeError(f'key {key} in section {section} '
                            f'is of unsupported type {type(value)}')

    def _iterate_sections(self, callback: Callable[[str], None]) -> None:
        for section in dir(self):
            if section.startswith('_') or section in ['save']:
                continue
            callback(section)

    def _iterate_keys(self, section: str,
                      callback: Callable[[str, str], None]) -> None:
        for key in dir(getattr(self, section)):
            if key.startswith('_'):
                continue
            callback(section, key)
