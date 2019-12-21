from asyncio import run, wait
from typing import Callable, List, Coroutine
from pathlib import Path

from contracts import Action
from filesystem import Directory, State, File
from .scraper import scrap_lyrics
from .searcher import search_lyrics
from .song import Song


class LyricsAction(Action):
    def __init__(self, handlers: Callable):
        super().__init__(handlers)

        self.tasks: List[Coroutine] = []

    def perform(self, filesystem: Directory) -> None:
        self.tasks = []
        self._iterate(filesystem)
        if self.tasks:
            run(wait(self.tasks))

    def _iterate(self, directory: Directory) -> None:
        for fs_node in directory:
            if fs_node.state is State.disabled:
                continue

            if isinstance(fs_node, Directory):
                self._iterate(fs_node)
            elif isinstance(fs_node, File):
                task = self._perform_file(fs_node.path)
                self.tasks.append(task)
            else:
                raise TypeError(
                    'fs_node must be only File or Directory type')

    async def _perform_file(self, file: Path) -> None:
        song = Song(file)
        search_query = f'{song.artist} - {song.title}'
        lyrics_url = await search_lyrics(search_query)
        if lyrics_url is None:
            return
        lyrics = await scrap_lyrics(lyrics_url)
        if lyrics is None:
            return
        song.lyrics = lyrics
        song.save()
