from flask import Flask, render_template, request, escape, session
from flask import copy_current_request_context
from vsearch import search4letters
from threading import Thread
# import mysql.connector
from DBcm import UseDatabase, ConnectionError, CredentialsError, SQLError
from checker import check_logged_in

app = Flask(__name__)

dbconfig = {'host': '127.0.0.1',
            'user': 'vsearch',
            'password': 'vsearchPa33!',
            'database': 'vsearchlogDB', }
# def log_request(req: 'flask_request', res: str) -> None:
#     with open('vsearch.log','a') as log:
#       # print(str(dir(req)), res, file=log)
#       # print(req.form, file=log, end='|')
#       # print(req.remote_addr, file=log, end='|')
#       # print(req.user_agent, file=log, end='|')
#       # print(res, file=log)
#       print(req.form, req.remote_addr, req.user_agent, res, file=log, sep='|')



@app.route('/search4', methods=['POST'])
def do_search() -> 'html':

    @copy_current_request_context
    def log_request(req: 'flask_request', res: str) -> None:
        """Log details of the web request and the results."""
        raise Exception("something awful just happened.")
        with UseDatabase(dbconfig) as cursor:
            _SQL = """insert into log
                  (phrase, letters, ip, browser_string, results)
                  values
                  (%s, %s, %s, %s, %s)"""
            cursor.execute(_SQL, (req.form['phrase'],
                          req.form['letters'],
                          req.remote_addr,
                          req.user_agent.browser,
                          res, ))

    phrase = request.form['phrase']
    letters = request.form['letters']
    title = 'Here are your results:'
    results = str(search4letters(phrase, letters))
    try:
      t = Thread(target = log_request, args=(request, results))
      t.start()
    except Exception as err:
      print('**** Logging failed with this error: ', str(err))
    return render_template('results.html',
                           the_title=title,
                           the_phrase=phrase,
                           the_letters=letters,
                           the_results=results,)


@app.route('/entry')
@app.route('/')
def entry_page() -> 'html':
    return render_template('entry.html',
                           the_title='Welcome to search4letters web!',)


@app.route('/login')
def do_login() -> str:
  session['logged_in'] = True
  return 'You are now logged in.'


@app.route('/logout')
def do_logout() -> str:
    session.pop('logged_in')
    return 'You are now logged out.'


@app.route('/viewlog')
@check_logged_in
def view_the_log() -> 'html':
    """Display the contents of the log file as a HTML table."""
    # contents = []
    # with open('vsearch.log') as log:
    #     for line in log:
    #         contents.append([])
    #         for item in line.split('|'):
    #             contents[-1].append(escape(item))
    # titles = ('Form Data', 'Remote_addr', 'User_agent', 'Results')
    try:
      with UseDatabase(dbconfig) as cursor:
        _SQL = """select phrase, letters, ip, browser_string, results from log"""
        cursor.execute(_SQL)
        contents = cursor.fetchall()
        titles = ('Phrase', 'Letters', 'Remote_addr', 'User_agent', 'Results')
        return render_template('viewlog.html',
                              the_title='View Log',
                              the_row_titles=titles,
                              the_data=contents,)
    except ConnectionError as err:
      print('Is your database switched on? Error:', str(err))
    except CredentialsError as err:
      print('User-id/Password issues. Error:', str(err))
    except SQLError as err:
      print('Is your query correct? Error:', str(err))
    except Exception as err:
      print('something went wrong: ', str(err))


app.secret_key = 'ThisIsAVerySecreyKeyNotBrokenByAnybody..'

if __name__ == '__main__':
    app.run(debug=True)
