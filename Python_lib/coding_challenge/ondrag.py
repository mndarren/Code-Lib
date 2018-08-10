
from turtle import Turtle
 
t=Turtle()
t.screen.bgcolor("black")
t.color("blue")
 
def goto(x,y):
    t.goto(x,y)
 
t.ondrag(goto)
t.done()
