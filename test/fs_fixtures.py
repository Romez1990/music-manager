from pathlib import Path
from os import chdir
from shutil import copytree, rmtree
from pytest import fixture


@fixture
def use_copied_files() -> None:
    project_root: Path = Path(__file__).parent.parent
    test_dir = project_root / 'test' / 'test_compilation'
    copy_dir = project_root / 'test' / 'copied_compilation'
    if copy_dir.exists():
        rmtree(str(copy_dir))
    copytree(str(test_dir), str(copy_dir))
    chdir(str(copy_dir))
    yield
    chdir(str(project_root))
    rmtree(str(copy_dir))


@fixture
def use_real_files() -> None:
    project_root: Path = Path(__file__).parent.parent
    test_files = project_root / 'test' / 'test_compilation'
    chdir(str(test_files))
