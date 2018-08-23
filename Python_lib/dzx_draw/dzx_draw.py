"""
Author: Darren Zhao Xie
Date: 8/12/2018
Purpose: Drawing tool from Turtle will be use to simiplify drawing picture by Turtle
Module: dzx_draw.py
"""
import turtle


class DrawTool:
    """
    Create some tools for basic drawing.
    """

    @staticmethod
    def setup(bob: turtle.Turtle, speed=6, hide=False):
        bob.speed(speed)  # sets bob's speed fastest = 0, fast = 10, normal = 6, slow = 3, slowest = 1
        bob.hideturtle() if hide is True else False

    @staticmethod
    def point(bob: turtle.Turtle, x, y):
        """jump to a point"""
        bob.penup()
        bob.goto(x, y)
        bob.pendown()

    @staticmethod
    def filled_circle(bob: turtle.Turtle, color, size, angle):
        """Create a filled up circle"""
        bob.fillcolor(color)  # set fill color
        bob.begin_fill()  # start filling
        if angle == 360:  # if the angle is a full circle
            bob.circle(size)  # make circle with set size
        else:
            bob.circle(size, angle)  # create circle with set size and angle
        bob.end_fill()  # finish filling
