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

wife.penup()
wife.goto(0,-40)
wife.pendown()
wife.circle(200)



smiles = turtle.Turtle()    
smiles.penup()
smiles.goto(-75,150)
smiles.pendown()
smiles.circle(10)     #eye one

smiles.penup()
smiles.goto(75,150)
smiles.pendown()
smiles.circle(10)     #eye two

smiles.penup()
smiles.goto(0,0)
smiles.pendown()
smiles.circle(100,90)   #right smile

smiles.penup()           
smiles.setheading(180) # <-- look West
smiles.goto(0,0)
smiles.pendown()
smiles.circle(-100,90)

exit = input()
smiles.done()
