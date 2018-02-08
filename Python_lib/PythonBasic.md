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
    ```
