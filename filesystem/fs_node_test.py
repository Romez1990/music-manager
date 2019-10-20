from .fs_node import FsNode


def test_creating(use_copied_files: None) -> None:
    _pytest_fixtures = [use_copied_files]
    fs_node = FsNode('.')
    assert fs_node
