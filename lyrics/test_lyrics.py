from pytest import mark
from textwrap import dedent

from contracts import Mode
from filesystem import Directory, File
from .action import LyricsAction
from .song import Song
from .searcher import search_lyrics
from .scraper import scrap_lyrics


@mark.asyncio
async def test_song(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    song = Song('Asking Alexandria/2009 - Asking Alexandria - '
                'Stand Up And Scream/01. Alerion.mp3')
    query_string = f'{song.artist} {song.title}'
    lyrics_url = await search_lyrics(query_string)
    lyrics = await scrap_lyrics(lyrics_url)
    song.lyrics = lyrics
    song.save()
    assert song.lyrics == dedent('''\
        [Intro]
        Fuck this!
        
        [Verse]
        Cross my heart, I hope you die
        Left by the roadside
        Karma's a bitch, right?
        Cross my heart, I hope you die
        Left by the roadside
        Karma's a bitch, right''')


def test_album(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    directory = Directory('Asking Alexandria/'
                          '2009 - Asking Alexandria - Stand Up And Scream')
    mark_enabled(directory, Mode.album)
    action = LyricsAction(None)
    action.perform(directory)


def test_band(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    directory = Directory('Asking Alexandria')
    mark_enabled(directory, Mode.band)
    action = LyricsAction(None)
    action.perform(directory)


def mark_enabled(directory: Directory, mode: Mode) -> None:
    for fs_node in directory:
        if mode is not Mode.album:
            if isinstance(fs_node, Directory):
                mark_enabled(fs_node, Mode(mode.value - 1))
        else:
            if isinstance(fs_node, File) and fs_node.path.suffix == '.mp3':
                fs_node.mark()
