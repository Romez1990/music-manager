from dotenv import load_dotenv
import sys
from PyQt5.QtWidgets import QApplication
from windows.main_window import MainWindow

load_dotenv()
app = QApplication(sys.argv)
main_window = MainWindow()
main_window.show()
sys.exit(app.exec_())
