from pathlib import Path
from sys import platform
from os import getenv as env
from typing import List

from .config import Config
from .config_parser_interface import ConfigParserInterface


def get_config_path() -> Path:
    filename = 'config.cfg'
    if platform == 'win32':
        appdata = env('APPDATA')
        if appdata is None:
            raise SystemError('No APPDATA environment variable')
        return Path(appdata) / 'Music Manager' / filename
    if platform == 'linux':
        home = env('HOME')
        if home is None:
            raise SystemError('No HOME environment variable')
        return Path(home) / '.config' / 'music-manager' / filename
    raise SystemError(f'platform {platform} is not supported')


def load(config_parser: ConfigParserInterface) -> None:
    if config_parser.path.exists():
        config_parser.load()
        sections = get_sections()
        [load_section(config_parser, section) for section in sections]


def load_section(config_parser: ConfigParserInterface, section: str) -> None:
    if not config_parser.has_section(section):
        config_parser[section] = {}
    options = get_options(section)
    [load_option(config_parser, section, option) for option in options]


def load_option(
    config_parser: ConfigParserInterface,
    section_name: str,
    option: str,
) -> None:
    if option not in config_parser[section_name]:
        return
    value_type = type(getattr(getattr(Config, section_name), option))
    section = getattr(Config, section_name)
    if value_type is bool:
        setattr(section, option, config_parser.get_bool(section_name, option))
    elif value_type is str:
        setattr(section, option, config_parser[section_name][option])
    elif value_type is int:
        setattr(section, option, int(config_parser[section_name][option]))
    elif value_type is float:
        setattr(section, option, float(config_parser[section_name][option]))
    else:
        raise TypeError(f'option {option} in section {section_name} is of '
                        f'unsupported type {value_type}')


def save(config_parser: ConfigParserInterface) -> None:
    sections = get_sections()
    [save_section(config_parser, section) for section in sections]
    config_parser.path.parent.mkdir(parents=True, exist_ok=True)
    config_parser.save()


def save_section(config_parser: ConfigParserInterface, section: str) -> None:
    if not config_parser.has_section(section):
        config_parser[section] = {}
    options = get_options(section)
    [save_option(config_parser, section, option) for option in options]


def save_option(
    config_parser: ConfigParserInterface,
    section: str,
    option: str,
) -> None:
    value = getattr(getattr(Config, section), option)
    value_type = type(value)
    if value_type is bool:
        config_parser[section][option] = str(value).lower()
    elif value_type is str:
        config_parser[section][option] = value
    elif value_type in [int, float]:
        config_parser[section][option] = str(value)
    else:
        raise TypeError(f'option {option} in section {section} is of '
                        f'unsupported type {value_type}')


def get_sections() -> List[str]:
    return [section for section in dir(Config)
            if not section.startswith('_') and section not in ['save']]


def get_options(section: str) -> List[str]:
    return [option for option in dir(getattr(Config, section))
            if not option.startswith('_')]
