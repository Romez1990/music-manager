from config import ConfigParser, init_config
from processor import Processor
from cli import main, handlers

init_config(ConfigParser)

processor = Processor(handlers)
main(processor)
