from cli.argparser import parse_args
from cli.error_handlers import error_handlers
from processing_system.processor import Processor
from processing_system.flags import Flag
import os

args = parse_args()
path = os.getcwd() if args.path is None else os.path.join(os.getcwd(), args.path)
flags = {Flag.lyrics}

processor = Processor(error_handlers)
processor.process(path, args.mode, flags)
