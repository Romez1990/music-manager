from PyQt5.QtWidgets import QMainWindow, QFileDialog, QMessageBox
from windows.main_window_ui import Ui_main_window
from lyrics import album, band, compilation
import os


class MainWindow(QMainWindow, Ui_main_window):
    def __init__(self, parent=None, *args, **kwargs):
        QMainWindow.__init__(self)
        self.setupUi(self)
        
        self.path_line_edit.textChanged.connect(self.check_existence)
        self.browse_push_button.clicked.connect(self.browse)
        self.start_push_button.clicked.connect(self.start)
    
    def check_existence(self):
        path = self.path_line_edit.text()
        exists = os.path.exists(path)
        self.start_push_button.setEnabled(exists)
    
    def browse(self):
        path = QFileDialog.getExistingDirectory(self, 'Open directory to process', self.path_line_edit.text())
        self.path_line_edit.setText(path)
    
    def start(self):
        current_mode = None
        modes = [('album', album.process), ('band', band.process), ('compilation', compilation.process)]
        for mode in modes:
            if mode[0] == self.mode_combo_box.currentText().lower():
                current_mode = mode[1]
                break
        path = self.path_line_edit.text()
        current_mode(path)
        QMessageBox.information(self, 'Complete', 'Processing complete')
