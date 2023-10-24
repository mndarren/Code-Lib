"""
This code does not work since computer not accept 0x00001000
"""
import ctypes

ctypes.windll.kernel32.SetThreadExecutionState(0x80000002)
MB_SYSTEMMODAL = 0x00001000
MB_OK = 0
MB_OKCANCEL = 1
MB_YESNOCANCEL = 3
MB_YESNO = 4

IDOK = 1
IDCANCEL = 2
IDABORT = 3
IDYES = 6
IDNO = 7
ctypes.windll.user32.MessageBoxW(None, "Hit OK to close the program, "
                                       "otherwise leave it open to prevent the screen saver from coming on.",
                                 MB_OK)
ctypes.windll.kernel32.SetThreadExecutionState(0x80000000)
