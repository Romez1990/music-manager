from PyQt5.QtWidgets import QDialog
from windows.settings_window_ui import Ui_settings_window


class SettingsWindow(QDialog, Ui_settings_window):
    def __init__(self, parent=None, *args, **kwargs):
        QDialog.__init__(self)
        self.setupUi(self)
