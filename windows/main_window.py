from PyQt5.QtWidgets import QMainWindow
from windows.main_window_ui import Ui_main_window


class MainWindow(QMainWindow, Ui_main_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
