import os


def iterate(browser, dir, callback):
    for filename in sorted(os.listdir(dir)):
        path = os.path.join(dir, filename)
        callback(browser, path)
