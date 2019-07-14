from PyQt5.QtWidgets import QMainWindow
from gui.progress_window_ui import Ui_progress_window


class ProgressWindow(QMainWindow, Ui_progress_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
