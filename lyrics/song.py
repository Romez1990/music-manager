from pathlib import Path
from mutagen.id3 import ID3, USLT


class Song:
    def __init__(self, path: Path) -> None:
        self._path = path
        self._tags = ID3(str(self._path))

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
        self._tags.save(str(self._path.resolve()))
