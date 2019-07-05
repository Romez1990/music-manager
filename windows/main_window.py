from PyQt5.QtWidgets import QMainWindow, QFileDialog, QMessageBox
from windows.main_window_ui import Ui_main_window
from windows.settings_window import SettingsWindow
from lyrics.processor import Processor, Mode
import os


class MainWindow(QMainWindow, Ui_main_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
        
        self.path_line_edit.textChanged.connect(self.check_existence)
        self.browse_push_button.clicked.connect(self.browse)
        self.start_push_button.clicked.connect(self.start)
        self.start_action.setShortcut('Return')
        self.start_action.triggered.connect(self.start)
        self.settings_action.setShortcut('Ctrl+Alt+S')
        self.settings_action.triggered.connect(self.settings_action_triggered)
        self.quit_action.setShortcuts(['Ctrl+Q', 'Ctrl+F4'])
        self.quit_action.triggered.connect(self.close)
    
    def check_existence(self):
        path = self.path_line_edit.text()
        exists = os.path.exists(path)
        self.start_push_button.setEnabled(exists)
    
    def browse(self):
        path = QFileDialog.getExistingDirectory(self, 'Open directory to process', self.path_line_edit.text())
        self.path_line_edit.setText(path)
    
    def start(self):
        path = self.path_line_edit.text()
        mode = Mode(self.mode_combo_box.currentIndex())
        flags = {}
        error_handlers = {
            'lyrics_not_found': self.lyrics_not_found
        }
        processor = Processor(flags, error_handlers)
        processor.process(path, mode)
        del processor
        QMessageBox.information(self, 'Complete', 'Processing complete')
    
    def lyrics_not_found(self, artist, album, title):
        print('Sorry, lyrics for this song not found:')
        print(f'\tArtist: {artist}')
        print(f'\tAlbum: {album}')
        print(f'\tTitle: {title}')
        print('Find the page for this song in the browser')
        os.system('pause')
    
    def settings_action_triggered(self):
        settings_window = SettingsWindow(self)
        settings_window.exec_()
