from .song import Song


def test_creating(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    song = Song('Asking Alexandria/2009 - Asking Alexandria - '
                'Stand Up And Scream/01. Alerion.mp3')
    assert song


def test_reading(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    song = Song('Asking Alexandria/2009 - Asking Alexandria - '
                'Stand Up And Scream/01. Alerion.mp3')
    assert song.artist == 'Asking Alexandria'
    assert song.title == 'Alerion'
    assert song.lyrics == ''


def test_writing(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    song = Song('Asking Alexandria/2009 - Asking Alexandria - '
                'Stand Up And Scream/01. Alerion.mp3')
    song.lyrics = 'Some text...'
    song.save()
    assert song.lyrics == 'Some text...'
