from functools import wraps

def logit(logfile='out.log'):
    def logging_decorator(func):
        @wraps(func)
        def wrapped_function(*args, **kwargs):
            log_string = func.__name__ + " was called"
            print(log_string)
            # Open the logfile and append
            with open(logfile, 'a') as opened_file:
                # Now we log to the specified logfile
                opened_file.write(log_string + '\n')
        return wrapped_function
    return logging_decorator

@logit()
def myfunc1():
    pass

myfunc1()
# Output: myfunc1 was called
# A file called out.log now exists, with the above string

@logit(logfile='func2.log')
def myfunc2():
    pass

myfunc2()
# Output: myfunc2 was called
# A file called func2.log now exists, with the above string


#class way
# class logit(object):
#     def __init__(self, logfile='out.log'):
#         self.logfile = logfile

#     def __call__(self, func):
#         log_string = func.__name__ + " was called"
#         print(log_string)
#         # Open the logfile and append
#         with open(self.logfile, 'a') as opened_file:
#             # Now we log to the specified logfile
#             opened_file.write(log_string + '\n')
#         # Now, send a notification
#         self.notify()

#     def notify(self):
#         # logit only logs, no more
#         pass
# @logit()
# def myfunc1():
#     pass

# class email_logit(logit):
#     '''
#     A logit implementation for sending emails to admins
#     when the function is called.
#     '''
#     def __init__(self, email='admin@myproject.com', *args, **kwargs):
#         self.email = email
#         super(email_logit, self).__init__(*args, **kwargs)

#     def notify(self):
#         # Send an email to self.email
#         # Will not be implemented here
#         pass