from typing import Type, Callable

from .config_parser_interface import ConfigParserInterface
from .config_file import (
    get_config_path,
    load as load_config,
    save as save_config,
)


def init_config(
    config_parser_type: Type[ConfigParserInterface],
) -> Callable[[], None]:
    config_path = get_config_path()
    config_parser = config_parser_type(config_path)
    load_config(config_parser)
    return lambda: save_config(config_parser)
