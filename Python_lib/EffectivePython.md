# Effective Python
==========================================
1. Take advantage of Each Block in try/except/else/finally
2. Prefer Exceptions to Returning None
```
def divide(a, b):
	try:
		return a/b
	except ZeroDivisionError as e:
		raise ValueError('Invalid inputs') from e
```
3. How closure interact with variable scope
```
def sort_priority(values: List, group: Set):
	def helper(x):
		if x in group:
			return (0, x)
		return (1, x)
	values.sort(key=helper)

class Sorter:
	def __init__(self, group):
		self.group = group
		self.found = False

	def __call__(self, x):
		if x in self.group:
			self.found = True
			return (0, x)
		return (1, x)

sorter = Sorter(group)
numbers.sort(key=sorter)
assert sorter.found is True
```
4. Be Defensive When Iterating Over Arguments
```
def normalize(numbers):
	total = sum(numbers)
	result = []
	for value in numbers:
		percent = 100 * value/total
		result.append(percent)
	return result

class ReadVisits:
	def __init__(self, data_path):
		self.data_path = data_path

	def __iter_(self):
		with open(self.data_path) as f:
			for line in f:
				yield int(line)

visits = ReadVisits(path)
percentages = normalize(visits)
print(percentages)
```
5. 