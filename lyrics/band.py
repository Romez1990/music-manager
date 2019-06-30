from lyrics.browser import Browser
from lyrics.directory_iterator import iterate
from lyrics import album
import os


def process(dir, browser=None):
    browser = Browser.exists(browser)
    iterate(browser, dir, handler)


def handler(browser, path):
    if os.path.isdir(path):
        album.process(path, browser)
