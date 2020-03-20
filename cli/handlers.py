from typing import Any
from textwrap import dedent

from .utils import query_yes_no


def handlers(name: str, *args) -> Any:
    def lyrics_not_found(artist: str, album: str, title: str) -> bool:
        return query_yes_no(
            dedent(f'''\
                We can not find the lyrics for this song:
                    Artist: {artist}
                    Album: {album}
                    Title: {title}
                Find the page for this song in the browser
                Accept this lyrics?'''),
            'no'
        )

    return locals()[name](*args)
