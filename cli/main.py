from pathlib import Path

from processor import ProcessorInterface, Flag
from .argparser import parse_args
from .handlers import handlers


def main(processor: ProcessorInterface):
    args = parse_args()
    path = Path.cwd() / args.path
    flags = {Flag.lyrics}

    processor.handlers = handlers
    processor.scan(path, args.mode)
    processor.process(flags)
