### 7 Python tricks
=================================================
1. enumerate
```
for i, city in enumerate(cities):
	print(i, city)
```
2.  2 lists
```
for x, y in zip(x_list, y_list):
	print(x, y)
```
3. swap 2 variables
```
x, y = y, x
```
4. get value from dict
```
age = ages.get('John', 'Unknown')
```
5. for else
```
for letter in haystack:
	if needle == letter:
		print('Found!')
		break
else:
	print('Not found!')
```
6. with statement
```
with open('sth.txt') as f:
	for line in f:
		print(line)
```
7. try except else
```
try:
	print(int('x'))
except:
	print('Failed')
else:
	print('Success!')
```
