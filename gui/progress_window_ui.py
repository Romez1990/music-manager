# -*- coding: utf-8 -*-

# Form implementation generated from reading ui file 'progress_window.ui'
#
# Created by: PyQt5 UI code generator 5.12.1
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets


class Ui_progress_window(object):
    def setupUi(self, progress_window):
        progress_window.setObjectName("progress_window")
        progress_window.resize(506, 350)
        self.centralwidget = QtWidgets.QWidget(progress_window)
        self.centralwidget.setObjectName("centralwidget")
        self.verticalLayout = QtWidgets.QVBoxLayout(self.centralwidget)
        self.verticalLayout.setObjectName("verticalLayout")
        self.vertical_layout_2 = QtWidgets.QVBoxLayout()
        self.vertical_layout_2.setSpacing(12)
        self.vertical_layout_2.setObjectName("vertical_layout_2")
        spacerItem = QtWidgets.QSpacerItem(486, 84, QtWidgets.QSizePolicy.Minimum, QtWidgets.QSizePolicy.Expanding)
        self.vertical_layout_2.addItem(spacerItem)
        self.description_label = QtWidgets.QLabel(self.centralwidget)
        font = QtGui.QFont()
        font.setPointSize(11)
        self.description_label.setFont(font)
        self.description_label.setStyleSheet("")
        self.description_label.setTextFormat(QtCore.Qt.AutoText)
        self.description_label.setWordWrap(True)
        self.description_label.setObjectName("description_label")
        self.vertical_layout_2.addWidget(self.description_label)
        self.progress_bar = QtWidgets.QProgressBar(self.centralwidget)
        self.progress_bar.setEnabled(True)
        font = QtGui.QFont()
        font.setPointSize(11)
        self.progress_bar.setFont(font)
        self.progress_bar.setProperty("value", 0)
        self.progress_bar.setTextVisible(False)
        self.progress_bar.setObjectName("progress_bar")
        self.vertical_layout_2.addWidget(self.progress_bar)
        spacerItem1 = QtWidgets.QSpacerItem(20, 40, QtWidgets.QSizePolicy.Minimum, QtWidgets.QSizePolicy.Expanding)
        self.vertical_layout_2.addItem(spacerItem1)
        self.verticalLayout.addLayout(self.vertical_layout_2)
        self.horizontal_layout_1 = QtWidgets.QHBoxLayout()
        self.horizontal_layout_1.setSpacing(11)
        self.horizontal_layout_1.setObjectName("horizontal_layout_1")
        spacerItem2 = QtWidgets.QSpacerItem(40, 20, QtWidgets.QSizePolicy.Expanding, QtWidgets.QSizePolicy.Minimum)
        self.horizontal_layout_1.addItem(spacerItem2)
        self.finish_push_button = QtWidgets.QPushButton(self.centralwidget)
        self.finish_push_button.setEnabled(False)
        font = QtGui.QFont()
        font.setPointSize(11)
        self.finish_push_button.setFont(font)
        self.finish_push_button.setObjectName("finish_push_button")
        self.horizontal_layout_1.addWidget(self.finish_push_button)
        self.cancel_push_button = QtWidgets.QPushButton(self.centralwidget)
        font = QtGui.QFont()
        font.setPointSize(11)
        self.cancel_push_button.setFont(font)
        self.cancel_push_button.setObjectName("cancel_push_button")
        self.horizontal_layout_1.addWidget(self.cancel_push_button)
        self.verticalLayout.addLayout(self.horizontal_layout_1)
        progress_window.setCentralWidget(self.centralwidget)
        
        self.retranslateUi(progress_window)
        QtCore.QMetaObject.connectSlotsByName(progress_window)
    
    def retranslateUi(self, progress_window):
        _translate = QtCore.QCoreApplication.translate
        progress_window.setWindowTitle(_translate("progress_window", "MainWindow"))
        self.description_label.setText(_translate("progress_window", "Artist: {artist}\n"
                                                                     "Album: {album}\n"
                                                                     "Title: {title}\n"
                                                                     "{percent}% proceseed"))
        self.finish_push_button.setText(_translate("progress_window", "Finish"))
        self.cancel_push_button.setText(_translate("progress_window", "Cancel"))
