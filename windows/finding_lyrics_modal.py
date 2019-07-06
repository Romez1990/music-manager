from PyQt5.QtWidgets import QDialog
from windows.finding_lyrics_modal_ui import Ui_finding_lyrics_modal


class FindingLyricsModal(QDialog, Ui_finding_lyrics_modal):
    def __init__(self, parent=None, *args, **kwargs):
        QDialog.__init__(self)
        self.setupUi(self)
        self.connect_signals()
        self.message_label.setText(
            f'''We can not find the lyrics for this song:
    Artist: {args[0]}
    Album: {args[1]}
    Title: {args[2]}
Find the page for this song in the browser
and press OK once you have found
or press Cancel to do not find lyrics for this song''')
    
    def connect_signals(self):
        self.button_box.accepted.connect(self.accept)
        self.button_box.rejected.connect(self.reject)
