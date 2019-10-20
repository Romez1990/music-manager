from .file import File


def test_creating(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    file = File('Asking Alexandria/2009 - Asking Alexandria - '
                'Stand Up And Scream/01. Alerion.mp3')
    assert file
