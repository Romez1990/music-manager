from typing import Callable

from filesystem import Directory


class Action:
    def __init__(self, handlers: Callable):
        self.handlers = handlers

    def perform(self, filesystem: Directory) -> None:
        raise NotImplementedError
