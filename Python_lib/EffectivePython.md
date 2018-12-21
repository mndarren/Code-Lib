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
5. *arg as arguments (2 problems)
```
(1) using the * operator with a generator may cause your program to run out of memory and crash.
(2) Adding new positional parameters to functions that accept *args can introduce hard-to-find bugs.
```
6. Always specify optional arguments using the keyword names and never pass them as positional arguments.
7. Use None and Docstrings to specify dynamic Default arguments
```
Default arguments are only evaluated once.
```
8. Keyword-only arguments
```
def safe_division_c(number, divisor, *, ignore_overflow=False, ignore_zero_division=False)

# calling
safe_division_c(1.0, 0, True, False)  # Error
safe_division_c(1.0, 0, ignore_zero_division=True)  # OK
```
9. Accept function instead of class
```
def log_missing():
	print('Key added')
	return 0
current = {'green': 12, 'blue': 3}
increments = [
	('red', 5),
	('blue', 17),
	('orange', 9)
]
# stateless function
results = defaultdict(log_missing, current)
for k, amount in increments:
	results[k] += amount

################################################
# stateful function
def increment_with_report(current, increment):
	added_count = 0

	def missing():
		nonlocal added_count  # stateful closure
		added_count += 1
		return 0

	result = defaultdict(missing, current)
	for k, amount in increments:
		result[k] += amount
	return result, added_count
#################################################
# statefule class
class CountMissing:
	def __init__(self):
		self.added = 0

	def __call__(self):
		self.added += 1
		return 0

counter = CountMissing()
result = defaultdict(counter, current)  # relies on __call__
for k, amount in increments:
	result[k] += amount

```