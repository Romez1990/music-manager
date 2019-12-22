from cli import main, handlers
from processor import MainProcessor

processor = MainProcessor(handlers)
main(processor)
