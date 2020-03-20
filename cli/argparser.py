from argparse import ArgumentParser, Namespace

from processor import Mode


def parse_args() -> Namespace:
    parser = ArgumentParser(description='Find lyrics for the songs')

    parser.add_argument('path',
                        type=str, metavar='path', nargs='?', default='',
                        help='Path to the folder')

    mode = parser.add_mutually_exclusive_group()
    mode.add_argument('-a', '--album',
                      action='store_const', dest='mode', const=Mode.album,
                      help='Find lyrics for the album folder')
    mode.add_argument('-b', '--band',
                      action='store_const',
                      dest='mode', const=Mode.band,
                      help='Find lyrics for the band folder')
    mode.add_argument('-c', '--compilation',
                      action='store_const', dest='mode', const=Mode.compilation,
                      help='Find lyrics for the compilation folder')
    parser.set_defaults(mode=Mode.album)

    return parser.parse_args()
