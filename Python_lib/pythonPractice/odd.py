from datetime import datetime
from os import getcwd
import sys, os, time, html, random

odds = [1, 3,5,7,9,11,13,15,17,19,21,23,25,27,29,31,33,35,37,39,41,43,45,47,49,51,53,55,57,59]

right_this_minute = datetime.today().minute

if right_this_minute in odds:
    print("This minute seems a little odd. " + str(right_this_minute))
else:
    print("Not an odd minute")
print("where I am is: " + getcwd())
print("platform: " + sys.platform)
print("environ: " + str(os.environ))
print("HOME: " + os.getenv('HOME'))
print("this year: " + str(datetime.today().year))
print("today: " + datetime.isoformat(datetime.today()))
print("Time: " + time.strftime("%A %p"))

print("html: " + html.escape("<script>darren</script>"))
print("html: " + html.unescape("&lt;script&gt;darren&lt;/script&gt;"))

for num in range(5):
    time.sleep(1)
    print(num)
    print("random: " + str(random.randint(1,60)))

phrase = "Don't panic"
plist = list(phrase)
print(phrase)
print(plist)

new_phrase = ''.join(plist)
print(plist)
print(new_phrase)

vowls = ['a', 'e', 'i', 'o','u']
# word = input("input a word Please: ")
word = 'asdfouop'
found = []
for letter in word:
    if letter in vowls:
        if letter not in found:
            found.append(letter)
for vowel in found:
    print(vowel)

plist.extend([plist.pop(),plist.pop()])
new_phrase = ''.join(plist)
print(new_phrase)
new_phrase = ''.join(plist)

plist.append('ZX')
new_phrase = ''.join(plist)
print(new_phrase)

plist.remove('ZX')
new_phrase = ''.join(plist)
print(new_phrase)

third = plist.copy()
plist.insert(0,plist.pop())
new_phrase = ''.join(plist)
print(new_phrase)
new_phrase = ''.join(third)
print(new_phrase)

plist.insert(len(plist)-1, plist.pop(0))
new_phrase = ''.join(plist)
print(new_phrase)

print(plist[3:])
print(plist[:10])
print(plist[::2])
print(plist[0:3])
print(plist[-6:])
print(plist[::-1])
