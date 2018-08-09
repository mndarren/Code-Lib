# draw a shape
import turtle
# ted = turtle.Turtle()
# ted.pencolor("blue")
# ted.forward(50)
# ted.right(90)
# ted.forward(50)
# ted.right(90)
# ted.forward(50)
# ted.right(90)
# ted.forward(50)
# ted.right(90)
# exit = input()
# ted.done()

wife = turtle.Turtle()
wife.pencolor("red")
wife.left(90)

for i in range(0, 360):
    wife.forward(2)
    wife.right(1)

exit = input()
wife.done()