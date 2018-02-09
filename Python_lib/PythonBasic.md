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
    2) $virtualenv -p python3 venv          #create a virtualenv
    3) $source venv/bin/activate                                  #activate virtualenv
    4) $deactivate                                       #deactivate virtualenv
    5) $rm -rf somewhere/virtualenvs/<project-name>      #delete virtualenv
    ```
24. collections, itertools & functools
    ```
    collections (OrderedDict, Counter, ChainMap)
    itertools (product, permutations, combinations, combinations_with_replacement)
        product('ABCD', repeat=2)
        permutations('ABCD', 2)
        combinations('ABCD', 2)
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
    ```