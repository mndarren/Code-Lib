"""Created sample TK application to keep windows awake
"""

import tkinter as tk
import ctypes
import sys


def display_on():
    global root
    print("Always On")
    ctypes.windll.kernel32.SetThreadExecutionState(0x80000002)
    # hide the window
    root.withdraw()


def display_reset():
    ctypes.windll.kernel32.SetThreadExecutionState(0x80000000)
    sys.exit(0)


# auto click the button
def invoke_display_on():
    slogan.invoke()
    root.after(5000, invoke_display_on)


root = tk.Tk()
root.geometry("200x60")
root.title("Display App")
frame = tk.Frame(root)
frame.pack()

button = tk.Button(frame, text="Quit", fg="red", command=display_reset)
button.pack(side=tk.LEFT)
slogan = tk.Button(frame, text="Always ON", command=display_on)
slogan.pack(side=tk.LEFT)
root.after(2000, invoke_display_on)

root.mainloop()
