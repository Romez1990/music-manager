from PyQt5.QtWidgets import QMainWindow, QFileDialog
from gui.main_window_ui import Ui_main_window
from gui.progress_window import ProgressWindow
from gui.settings_window import SettingsWindow
from gui.finding_lyrics_modal import FindingLyricsModal
from processing_system.processor import Processor
from processing_system.mode import Mode
from processing_system.flags import Flag
from threading import Thread
import os


class MainWindow(QMainWindow, Ui_main_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
        self.connect_signals()
        self.set_shortcuts()
        error_handlers = {
            'lyrics_not_found': self.lyrics_not_found
        }
        self.processor = Processor(error_handlers)
        self.processor.on_progress = self.progress
        self.processor.on_song_change = self.song_change
        self.processor.on_complete = self.complete
        self.progress_window = ProgressWindow(self)
        self.progress_window.on_cancel = self.cancel
    
    def connect_signals(self):
        self.path_line_edit.textChanged.connect(self.check_existence)
        self.browse_push_button.clicked.connect(self.browse)
        self.start_push_button.clicked.connect(self.start)
        self.start_action.triggered.connect(self.start)
        self.settings_action.triggered.connect(self.settings_action_triggered)
        self.quit_action.triggered.connect(self.close)
    
    def set_shortcuts(self):
        self.start_action.setShortcut('Return')
        self.settings_action.setShortcut('Ctrl+Alt+S')
        self.quit_action.setShortcuts(['Ctrl+Q', 'Ctrl+F4'])
    
    def check_existence(self):
        path = self.path_line_edit.text()
        exists = os.path.exists(path)
        self.start_push_button.setEnabled(exists)
        return exists
    
    def browse(self):
        path = QFileDialog.getExistingDirectory(self, 'Open directory to process', self.path_line_edit.text())
        self.path_line_edit.setText(path)
    
    def start(self):
        if not self.check_existence():
            return
        
        self.hide()
        self.progress_window.change_progress(0)
        self.progress_window.change_song({
            'artist': '',
            'album':  '',
            'title':  '',
        })
        self.progress_window.show()
        path = self.path_line_edit.text()
        mode = Mode(self.mode_combo_box.currentIndex())
        flags = {Flag.lyrics}
        Thread(target=self.processor.process, args=(path, mode, flags)).start()
    
    def progress(self, progress):
        value = round(progress * 100)
        self.progress_window.change_progress(value)
    
    def song_change(self, song):
        self.progress_window.change_song(song)
    
    def complete(self):
        self.progress_window.hide()
        self.show()
    
    def cancel(self):
        raise NotImplementedError
    
    def lyrics_not_found(self, artist, album, title):
        finding_lyrics_modal = FindingLyricsModal(self, artist, album, title)
        res = finding_lyrics_modal.exec_()
        return bool(res)
    
    def settings_action_triggered(self):
        settings_window = SettingsWindow(self)
        settings_window.exec_()
