from typing import Callable, List, Coroutine
from asyncio import get_event_loop, wait

from processor import Action
from filesystem import Directory, FsNodeState, File
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
            loop = get_event_loop()
            loop.run_until_complete(wait(self.tasks))

    def _iterate(self, directory: Directory) -> None:
        for fs_node in directory:
            if fs_node.state is FsNodeState.disabled:
                continue

            if isinstance(fs_node, Directory):
                self._iterate(fs_node)
            elif isinstance(fs_node, File):
                task = self._perform_file(fs_node)
                self.tasks.append(task)
            else:
                raise TypeError(
                    'fs_node must be only File or Directory type')

    async def _perform_file(self, file: File) -> None:
        song = Song(file)
        search_query = f'{song.artist} - {song.title}'
        lyrics_url = await search_lyrics(search_query)
        if lyrics_url is None:
            print(f'No results for {search_query}')
            return
        lyrics = await scrap_lyrics(lyrics_url)
        if lyrics is None:
            print(f'No results for {search_query} on {lyrics_url}')
            return
        song.lyrics = lyrics
        song.save()
