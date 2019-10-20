from pathlib import Path
from typing import Union

from .fs_node import FsNode


class File(FsNode):
    def __init__(self, path: Union[str, Path]) -> None:
        super().__init__(path)
        if not self._path.is_file():
            raise IsADirectoryError(
                f'{path} should be a file, but it is a directory')
