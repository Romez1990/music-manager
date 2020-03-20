from pathlib import Path
from typing import MutableMapping, Type
from _pytest.monkeypatch import MonkeyPatch
from pytest import fixture

from .config_parser_interface import ConfigParserInterface
from .config import Config
from .config_file import (
    get_config_path,
    load,
    load_section,
    load_option,
    save,
    save_section,
    save_option,
    get_sections,
    get_options,
)


@fixture
def config_parser() -> ConfigParserInterface:
    return ConfigParser(Path())


@fixture
def config(monkeypatch: MonkeyPatch) -> Type[Config]:
    setattr(Config, 'TestSection', None)
    monkeypatch.setattr(Config, 'TestSection', TestSection)
    return Config


class ConfigParser(ConfigParserInterface):
    def __init__(self, path: Path) -> None:
        super().__init__(path)
        self.path = Path()
        self._config: MutableMapping[str, MutableMapping[str, str]] = {
            'TestSection': {
                'int_option': '11',
                'float_option': '2.2',
                'bool_option': 'true',
            },
        }

    def load(self) -> None:
        pass

    def save(self) -> None:
        pass

    def has_section(self, section: str) -> bool:
        return section in self._config

    def __getitem__(self, item: str) -> MutableMapping[str, str]:
        if item not in self._config:
            return {}
        return self._config[item]

    def __setitem__(self, option: str, value: MutableMapping[str, str]) -> None:
        self._config[option] = value

    def get_bool(self, section: str, option: str) -> bool:
        return self._config[section][option].lower() == 'true'


class TestSection:
    int_option = 3
    float_option = 4.4
    bool_option = False


class TestConfig:
    class TestSection:
        int_option = 3
        float_option = 4.4
        bool_option = False


def test_load_option(
    config_parser: ConfigParserInterface,
    config: Type[TestConfig],
) -> None:
    load_option(config_parser, 'TestSection', 'int_option')
    assert config.TestSection.int_option == 11
    load_option(config_parser, 'TestSection', 'float_option')
    assert config.TestSection.float_option == 2.2
    load_option(config_parser, 'TestSection', 'bool_option')
    assert config.TestSection.bool_option


def test_load_section(
    config_parser: ConfigParserInterface,
    config: Type[TestConfig],
) -> None:
    load_section(config_parser, 'TestSection')
    assert config.TestSection.int_option == 11
    assert config.TestSection.float_option == 2.2
    assert config.TestSection.bool_option


def test_load(
    config_parser: ConfigParserInterface,
    config: Type[TestConfig],
) -> None:
    load(config_parser)
    assert config.TestSection.int_option == 11
    assert config.TestSection.float_option == 2.2
    assert config.TestSection.bool_option
