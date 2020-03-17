from filesystem import Directory
from .album import rename_album


def test_album_renaming(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    directory = Directory('Asking Alexandria/2009 - Asking Alexandria - '
                          'Stand Up And Scream')
    rename_album(directory)
