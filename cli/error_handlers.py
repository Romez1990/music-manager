from os import system


def lyrics_not_found(artist, album, title):
    print(f'''We can not find the lyrics for this song:
\tArtist: {artist}
\tAlbum: {album}
\tTitle: {title}
Find the page for this song in the browser
and press OK once you have found
or press Cancel to do not find lyrics for this song''')
    system('pause')
    return True


error_handlers = {
    'lyrics_not_found': lyrics_not_found
}
