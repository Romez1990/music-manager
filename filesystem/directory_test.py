from .directory import Directory


def test_creating(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    directory = Directory('Asking Alexandria/'
                          '2009 - Asking Alexandria - Stand Up And Scream')
    assert directory
