# -*- coding: utf-8 -*-

# Form implementation generated from reading gui file 'finding_lyrics_modal.gui'
#
# Created by: PyQt5 UI code generator 5.12.2
#
# WARNING! All changes made in this file will be lost!

from PyQt5 import QtCore, QtGui, QtWidgets


class Ui_finding_lyrics_modal(object):
    def setupUi(self, finding_lyrics_modal):
        finding_lyrics_modal.setObjectName("finding_lyrics_modal")
        finding_lyrics_modal.resize(362, 161)
        finding_lyrics_modal.setModal(True)
        self.button_box = QtWidgets.QDialogButtonBox(finding_lyrics_modal)
        self.button_box.setGeometry(QtCore.QRect(10, 122, 341, 32))
        font = QtGui.QFont()
        font.setKerning(True)
        self.button_box.setFont(font)
        self.button_box.setOrientation(QtCore.Qt.Horizontal)
        self.button_box.setStandardButtons(QtWidgets.QDialogButtonBox.Cancel | QtWidgets.QDialogButtonBox.Ok)
        self.button_box.setObjectName("button_box")
        self.message_label = QtWidgets.QLabel(finding_lyrics_modal)
        self.message_label.setGeometry(QtCore.QRect(10, 10, 381, 111))
        font = QtGui.QFont()
        font.setPointSize(11)
        self.message_label.setFont(font)
        self.message_label.setAlignment(QtCore.Qt.AlignLeading | QtCore.Qt.AlignLeft | QtCore.Qt.AlignTop)
        self.message_label.setWordWrap(True)
        self.message_label.setObjectName("message_label")
        
        self.retranslateUi(finding_lyrics_modal)
        self.button_box.accepted.connect(finding_lyrics_modal.accept)
        self.button_box.rejected.connect(finding_lyrics_modal.reject)
        QtCore.QMetaObject.connectSlotsByName(finding_lyrics_modal)
    
    def retranslateUi(self, finding_lyrics_modal):
        _translate = QtCore.QCoreApplication.translate
        finding_lyrics_modal.setWindowTitle(_translate("finding_lyrics_modal", "Dialog"))
        self.message_label.setText(_translate("finding_lyrics_modal", "We can not find the lyrics for this song:\n"
                                                                      "    Artist:\n"
                                                                      "    Title:\n"
                                                                      "Find the page for this song in the browser\n"
                                                                      "and press OK once you have found\n"
                                                                      "or press Cancel to do not find lytics for this song"))
