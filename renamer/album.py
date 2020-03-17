import re

from filesystem import Directory, File


def rename_album(directory: Directory) -> None:
    rename_album_dir(directory)
    for file in directory:
        if isinstance(file, File):
            rename_song(file)


def rename_album_dir(directory: Directory) -> None:
    new_name = replace_album_name(directory.name)
    directory.rename(new_name)


def rename_song(file: File) -> None:
    new_name = replace_song_name(file.name)
    file.rename(new_name)


def replace_album_name(name: str) -> str:
    return re.sub(r'(?P<year>(?:20|19)\d{2}) - .+ - (?P<name>.+)',
                  r'\g<year> \g<name>',
                  name)


def replace_song_name(name: str) -> str:
    return re.sub(r'(?P<year>(?:20|19)\d{2}) - .+ - (?P<name>.+)',
                  r'\g<year> \g<name>',
                  name)
