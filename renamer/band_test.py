from filesystem import Directory
from .band import rename_band


def test_band_renaming(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    directory = Directory('Asking Alexandria')
    rename_band(directory)
