"""
This code does not work because mouse moving not stop screen lock in computer
"""
import time
import random
import pyautogui


sleep_time = 60  # 2 minutes
one_day_times = int(86400/sleep_time)  # 24 hours


def auto_move_mouse(days):
    pyautogui.FAILSAFE = False
    for z in range(0, days*one_day_times):
        x = random.randint(0, 1500)
        y = random.randint(0, 1500)
        pyautogui.moveTo(x, y)

        localtime = time.localtime()
        result = time.strftime("%I:M:%S %p", localtime)

        print("Moved at " + str(result) + " (" + str(x) + ", " + str(y) + ")")
        time.sleep(sleep_time)


if __name__ == '__main__':
    auto_move_mouse(7)
