# Python Basic
===========================
1. Don't mix tabs & spaces!
2. Everything is an object!
3. OOP is optional.
4. Key should be initialized!
5. set() -- union(), difference(), intersection()
6. list() -- append(), remove(), extend([]), pop(), insert(), copy(), letters(start,stop,step)
7. io -- print(), ''.join(), getcwd(), os.getenv('HOME'), os.environ
8. datetime -- time.sleep(), datetime.today().year, datetime.isoformat(datetime.today()),time.strftiem("%A %P")
9. sys -- sys.platform
10. random -- str(random.randint(1,60))
11. html -- html.escape(), html.unescape()
12. enum & json
13. argument: position assignment & keyword assignment
14. module 3 location: current directory, site-package, std lib
15. create module 3 steps
    ```
    create a distribution description (setup.py, README.txt),
    generate distributed file, 
    Package Installer for Python(PIP)
    ```

16. argument
    ```
    by value -- immutable value (tuple, str, int) Assignment operation will create new object
    by reference -- mutable value (list, dict, set)
    ```
17. HTTP status
    ```
    100-199  info msg
    200-299  success msg
    300-399  redirection
    400-499  client error
    500-599  server error
    ```
 18. Context Manager & Decorator
    ```
    Context Manager will be a class with 3 dunder functions: __init__, __enter__ and __exit__.
    Decorator will be a function whose argument is a function and return is a function.
        i) Decorator is a functon;
        ii) A decorator takes the decorated function as arg;
        iii) A decorator returns a new function;
        iv) A decorator maintains the decorated function's signature.
    ```
19. Great arguments
    ```
    *args: will pass into a list of arguments;
    **kwargs: will pass into a dict of arguments.
    ```
20. Web is stateless. Don't store your webapp state in global variables.
21. Common Error Handling
    ```
    i) DB connection fails;
    ii) App is subjected to an attack;
    iii) Take a long time to execute operation;
    iv) Function calls fail.
    ```
22. The best things in Python
    ```
    i) context manager (creating your own with statement)
    ii) decorator (creating your own decorator)
    iii) comprehension (listcomp, dictcomp and setcomp)
         more_dests = [dest.title() for dest in flights.values()]
         just_freeports = {convert2ampm(k): v.title()
                            for k, v in flights.items()
                            if v == 'FREEPORT'}

        [] for listcomp, {} for dictcomp and setcomp, () for generator
        generator gets better performance than listcomp when big data
        generator function
    ```
23. virtualenv
    ```
    1) $sudo -H pip3 install virtualenv     #install pip3
    2) $virtualenv -p python3 venv          #--system-site-packages, --site-packages
    3) $source venv/bin/activate                                  #activate virtualenv
    4) $deactivate                                       #deactivate virtualenv
    5) $rm -rf somewhere/virtualenvs/<project-name>      #delete virtualenv
    ```
24. collections, itertools & functools
    ```
    collections (deque, namedtuple, enum.Enum, defaultdict, OrderedDict, Counter, ChainMap)
    	defaultdict --without needing to check if key is present or not;
    		some_dict = {}
			some_dict['colours']['favourite'] = "yellow"
			# Raises KeyError: 'colours'
			import collections
			tree = lambda: collections.defaultdict(tree)
			some_dict = tree()
			some_dict['colours']['favourite'] = "yellow"
			# Works fine
			import json
			print(json.dumps(some_dict))
			# Output: {"colours": {"favourite": "yellow"}}
		OrderedDict --keep entries sorted
			from collections import OrderedDict
			colours = OrderedDict([("Red", 198), ("Green", 170), ("Blue", 160)])
			for key, value in colours.items():
			    print(key, value)
		Count  --count items
			from collections import Counter
			colours = (
			    ('Yasoob', 'Yellow'),
			    ('Ali', 'Blue'),
			    ('Arham', 'Green'),
			    ('Ali', 'Black'),
			    ('Yasoob', 'Red'),
			    ('Ahmed', 'Silver'),
			)
			favs = Counter(name for name, colour in colours)
			print(favs)
			# Output: Counter({
			#    'Yasoob': 2,
			#    'Ali': 2,
			#    'Arham': 1,
			#    'Ahmed': 1
			# })
			with open('filename', 'rb') as f:
			    line_count = Counter(f)
			print(line_count)
		deque  --Is kinda a queue
			d = deque(range(5))
			print(len(d))
			# Output: 5
			d.popleft()
			# Output: 0
			d.pop()
			# Output: 4
			print(d)
			# Output: deque([1, 2, 3])
			d = deque(maxlen=30)
			d = deque([1,2,3,4,5])
			d.extendleft([0])
			d.extend([6,7,8])
			print(d)
			# Output: deque([0, 1, 2, 3, 4, 5, 6, 7, 8])
		namedtuple --immutable dict, self-document
			from collections import namedtuple
			Animal = namedtuple('Animal', 'name age type')
			perry = Animal(name="perry", age=31, type="cat")
			print(perry)
			# Output: Animal(name='perry', age=31, type='cat')
			print(perry.name)
			# Output: 'perry'
			print(perry._asdict())  #convert it into a dict
			# Output: OrderedDict([('name', 'Perry'), ('age', 31), ...
		enum.Enum
			from collections import namedtuple
			from enum import Enum
			class Species(Enum):
			    cat = 1
			    dog = 2
			    horse = 3
			    aardvark = 4
			    butterfly = 5
			    owl = 6
			    platypus = 7
			    dragon = 8
			    unicorn = 9
			    # The list goes on and on...
			    # But we don't really care about age, so we can use an alias.
			    kitten = 1
			    puppy = 2
			Animal = namedtuple('Animal', 'name age type')
			perry = Animal(name="Perry", age=31, type=Species.cat)
			drogon = Animal(name="Drogon", age=4, type=Species.dragon)
			tom = Animal(name="Tom", age=75, type=Species.cat)
			charlie = Animal(name="Charlie", age=2, type=Species.kitten)
    itertools (product, permutations, combinations, combinations_with_replacement)
        product('ABCD', repeat=2) #with AA
        permutations('ABCD', 2)  #no AA format
        combinations('ABCD', 2)  #no AA
        combinations_with_replacement('ABCD', 2)
    functools (partial)
    ```
25. Running code concurrently
    ```
    threading
    multiprocessing
    asyncio
    concurrent.futures
    ```
26. new keywords: async & await
27. Test: doctest (very useful), unittest (complained), py.test (programmers like it)
28. Debug, SQL & code checker (pdb, sqlalchemy, pylint)
29. Underscore meaning
    ```
    1) store the last expression in interpreter
    2) ignore value
    3) private var, method or class (not really private)
    4) single_trailing_underscore_ for avoiding keyword conflict
    5) mangling method. e.g. __method()   =>  _ClassName__method()
    6) magic method e.g.  __init__()  __slots__ = ['name', 'identifier']
    7) As internationalization (i18n) or Localization (l10n) e.g. Django frameword, gettext()
    8) digital separator e.g.
        dec_base = 1_000_000
        bin_base = 0b_1111_0000
        hex_base = 0x_1234_abcd
    ```
30. Python Tips
    ```
    1) __slots__ will reduce RAM waste 40-50% (dict -> set)
    2) Pdb importance
        $python -m pdb my_script.py   #small program
        pdb.set_trace()               #inside script
        c -- continue
        w -- show context
        a -- arguments
        s -- step inside function
        n -- next to have function reached or return
    3) Generator: generate values on the fly, not store in memory
        generator function will not return value, yield it.
        generator requires fewer resources. e.g.
        def fibon(n):
            a = b = 1
            for i in range(n):
                yield a
                a, b = b, a + b
        for x in fibon(1000000):
            print(x)
        my_string = "Yasoob"
        my_iter = iter(my_string)
        print(next(my_iter))
        # Output: 'Y'
    4) Map, Filter & Reduce
        Map e.g.
       		items = [1, 2, 3, 4, 5]
       		squared = list(map(`lambda x: x**2, items`))
       		def multiply(x):
       		    return (`x*x`)
       		def add(x):
       		    return (x+x)
       		funcs = [multiply, add]
       		for i in range(5):
       		    value = list(map(lambda x: x(i), funcs))
       		    print(value)
       		# Output:
       		# [0, 0]
       		# [1, 2]
       		# [4, 4]
       		# [9, 6]
       		# [16, 8]
       	Filter e.g.
       		number_list = range(-5, 5)
			less_than_zero = list(filter(lambda x: x < 0, number_list))
			print(less_than_zero)
			# Output: [-5, -4, -3, -2, -1]
		Reduce e.g.
			product = 1
			list = [1, 2, 3, 4]
			for num in list:
			    product = product * num
			# product = 24
			from functools import reduce
			product = reduce((lambda x, y: x * y), [1, 2, 3, 4])
			# Output: 24
    5) set Data Structure (difference, intersection)
    	some_list = ['a', 'b', 'c', 'b', 'd', 'm', 'n', 'n']
		duplicates = set([x for x in some_list if some_list.count(x) > 1])
		print(duplicates)
		# Output: set(['b', 'n'])
	6) Ternary Operators e.g.
		condition = True
		print(2 if condition else 1/0)
		#Output is 2
		print((1/0, 2)[condition])
		#ZeroDivisionError is raised
	7) Decorators (functions which modify the functionality of another function)
		Point: if great() is a function, and print(great) will print out the code of great;
											 print(great()) will execute the great() and print out the return.
		Where to use: check authorization, Logging, 
			from functools import wraps
			def logit(func):
			    @wraps(func)
			    def with_logging`(*args, **kwargs)`:
			        print(func.__name__ + " was called")
			        return func`(*args, **kwargs)`
			    return with_logging
	8) global, usually not to be used
	9) Enumerate
		for counter, value in enumerate(some_list):
    		print(counter, value)
    	my_list = ['apple', 'banana', 'grapes', 'pear']
		counter_list = list(enumerate(my_list, 1))
		print(counter_list)
		# Output: [(1, 'apple'), (2, 'banana'), (3, 'grapes'), (4, 'pear')]
	10) Object introspection
		dir
			my_list = [1, 2, 3]
			dir(my_list)
			# Output: ['__add__', '__class__', '__contains__', '__delattr__', '__delitem__',
			# '__delslice__', '__doc__', '__eq__', '__format__', '__ge__', '__getattribute__',
			# '__getitem__', '__getslice__', '__gt__', '__hash__', '__iadd__', '__imul__',
			# '__init__', '__iter__', '__le__', '__len__', '__lt__', '__mul__', '__ne__',
			# '__new__', '__reduce__', '__reduce_ex__', '__repr__', '__reversed__', '__rmul__',
			# '__setattr__', '__setitem__', '__setslice__', '__sizeof__', '__str__',
			# '__subclasshook__', 'append', 'count', 'extend', 'index', 'insert', 'pop',
			# 'remove', 'reverse', 'sort']
		type & id
			name = "Yasoob"
			print(id(name))
			# Output: 139972439030304
		inspect
			import inspect
			print(inspect.getmembers(str))
			# Output: [('__add__', <slot wrapper '__add__' of ... ...>)]
	11) try/except/else/finally
		try:
		    print('I am sure no exception is going to occur!')
		except Exception:
		    print('exception')
		else:
		    # any code that should only run if no exception occurs in the try,
		    # but for which exceptions should NOT be caught
		    print('This would only run if no exception occurs. And an error here '
		          'would NOT be caught.')
		finally:
		    print('This would be printed in every case.')
	12) Lambda
		add = lambda x, y: x + y
		print(add(3, 5))
		# Output: 8
		a = [(1, 2), (4, 1), (9, 10), (13, -3)]
		a.sort(key=lambda x: x[1])
		print(a)
		# Output: [(13, -3), (4, 1), (1, 2), (9, 10)]
		##parallel sorting of lists ??
		data = zip(list1, list2)
		data.sort()
		list1, list2 = map(lambda t: list(t), zip`(*data)`)
	13) One-Liners
		Simple Web Server
			# Python 2
			python -m SimpleHTTPServer
			# Python 3
			python -m http.server
		Json PPrint $cat file.json | python -m json.tool
		python -m cProfile my_script.py  #Profiling a script
		python -c "import csv,json;print json.dumps(list(csv.reader(open('csv_file.csv')))  #csv to json
		print(list(itertools.chain`(*a_list)`))   #List Flattening
		##one-line constructor
		class A(object):
    		def __init__(self, a, b, c, d, e, f):
    		    self.__dict__.update({k: v for k, v in locals().items() if k != 'self'})
    14) for/else loop
    	for n in range(2, 10):
   			for x in range(2, n):
   			    if n % x == 0:
   			        print`( n, 'equals', x, '*', n/x)`
   			        break
   			else:
   			    # loop fell through without finding a factor
   			    print(n, 'is a prime number')
   	15) 
    ```
 31. Python Security (SAST & DAST)
    ```
    1) Static code analysis tools (PEP, unittest, pylint and bandit)
    2) DASTProxy from eBay
    3) Sqreen monitoring your app in real-time
    ```