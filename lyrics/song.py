from typing import Union
from mutagen.id3 import ID3, USLT

from filesystem import File


class Song:
    def __init__(self, path: Union[File, str]) -> None:
        self._file = self._resole_path(path)
        self._tags = ID3(str(self._file))

    def _resole_path(self, path: Union[File, str]) -> File:
        if isinstance(path, str):
            return self._resole_path(File(path))

        return path

    @property
    def artist(self) -> str:
        return self._get_tag('TPE1')

    @property
    def title(self) -> str:
        return self._get_tag('TIT2')

    @property
    def lyrics(self) -> str:
        return self._get_tag('USLT')

    @lyrics.setter
    def lyrics(self, lyrics: str) -> None:
        self._tags.add(USLT(encoding=3, text=lyrics))

    def _get_tag(self, key: str) -> str:
        tag = self._tags.getall(key)
        if not tag:
            return ''
        return str(tag[0])

    def save(self) -> None:
        self._tags.save(str(self._file))
