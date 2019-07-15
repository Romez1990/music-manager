from PyQt5.QtWidgets import QMainWindow
from gui.progress_window_ui import Ui_progress_window


class ProgressWindow(QMainWindow, Ui_progress_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
        self.connect_signals()
        self.template = self.description_label.text()
        self.values = {
            'artist':  '',
            'album':   '',
            'title':   '',
            'percent': 0
        }
        self.on_cancel = None
    
    def connect_signals(self):
        self.cancel_push_button.clicked.connect(self.cancel)
    
    def __update_description(self):
        self.description_label.setText(self.template.format(**self.values))
    
    def change_song(self, values):
        self.values.update(values)
        self.__update_description()
    
    def change_progress(self, value):
        self.values['percent'] = value
        self.progress_bar.setValue(self.values['percent'])
        self.__update_description()
    
    def cancel(self):
        self.on_cancel()
