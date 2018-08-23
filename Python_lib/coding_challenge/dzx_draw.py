'''
Author: Darren Zhao Xie
Date: 8/12/2018
Purpose: Drawing tool from Turtle will be use to simiplify drawing picture by Turtle
Module: dzx_draw.py
'''

import turtle #import turtle so we can use it
wn = turtle.Screen() #set the screen to wn
bob = turtle.Turtle() #and set bob as our turtle

#functions to allow users to customize the face
def main(color, sp, hide):
    #the Main function
    wn.bgcolor(color) #sets the background color
    bob.speed(sp) #sets bob's speed fastest = 0, fast = 10, normal = 6, slow = 3, slowest = 1
    if hide == True:
        #if hide = true hide our turtle 
        bob.hideturtle()
    #endif
    
def points(x,y):
    #Function to bring pen up and down while going to a point
    bob.penup() #brings pen up
    bob.goto(x,y) #makes turtle go to set point
    bob.pendown() #puts pen down
    
def circles(color, size, angle):
    #Function to create circles and colors them
    bob.fillcolor(color) #set fill color
    bob.begin_fill() #start filling
    if angle == 360: #if the angle is a full circle
        bob.circle(size) #make circle with set size
    else:
        bob.circle(size, angle) #create circle with set size and angle
    bob.end_fill() #finish filling       
# Start Main Code Ending the Functions    

main('black', 0, True)

points(0,-225)
circles('white',250,360)

points(-70,70)
circles('blue',50,360)

points(70,70)
circles('blue',50,360)

bob.right(70)
points(-80,-50)
circles('red',100,135)

exit = input()
bob.done()