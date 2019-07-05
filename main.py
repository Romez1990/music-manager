import sys
from PyQt5.QtWidgets import QApplication
from windows.main_window import MainWindow

app = QApplication(sys.argv)
main_window = MainWindow()
main_window.show()
sys.exit(app.exec_())
