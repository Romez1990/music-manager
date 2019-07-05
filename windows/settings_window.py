from PyQt5.QtWidgets import QDialog
from windows.settings_window_ui import Ui_settings_window
from configuration import Config


class SettingsWindow(QDialog, Ui_settings_window):
    def __init__(self, parent=None, *args, **kwargs):
        QDialog.__init__(self)
        self.setupUi(self)
        
        self.config = Config()
        self.driver_line_edit.setText(self.config.get('browser', 'driver'))
        self.minimize_check_box.setChecked(self.config.getboolean('browser', 'minimize'))
        self.api_token_line_edit.setText(self.config.get('genius', 'api_token'))
        
        def set_driver():
            self.config.set_option('browser', 'driver', self.driver_line_edit.text())
        
        self.driver_line_edit.editingFinished.connect(set_driver)
        
        def set_minimize():
            self.config.set_option('browser', 'minimize', self.minimize_check_box.isChecked())
        
        self.minimize_check_box.stateChanged.connect(set_minimize)
        
        def set_api_token():
            self.config.set_option('genius', 'api_token', self.api_token_line_edit.text())
        
        self.api_token_line_edit.editingFinished.connect(set_api_token)
        
        self.ok_push_button.clicked.connect(self.save_and_close)
        self.cancel_push_button.clicked.connect(self.close)
    
    def save_and_close(self):
        self.config.save()
        self.close()
