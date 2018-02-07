from flask import Flask, session
from checker import check_logged_in

app = Flask(__name__)

app.secret_key = 'YouWillNeverGuess'


@app.route('/')
def hello() -> str:
	return 'Hello from the simple webapp.'

@app.route('/page1')
@check_logged_in
def page1() -> str:
	return 'This is page 1.'

@app.route('/page2')
@check_logged_in
def page2() -> str:
	return 'This is page 2.'


@app.route('/page3')
@check_logged_in
def page3() -> str:
	return 'This is page 3.'


@app.route('/setuser/<user>')
def setuser(user: str) -> str:
    session['user'] = user
    return 'User value set to: ' + session['user']


@app.route('/getuser')
def getuser() -> str:
    return 'User value is currently set to: ' + session['user']


@app.route('/login')
def do_login() -> str:
	session['logged_in'] = True
	return 'You are now logged in.'


@app.route('/logout')
def do_logout() -> str:
    session.pop('logged_in')
    return 'You are now logged out.'


# @app.route('/status')
# def do_login() -> str:
#     if 'logged_in' in session:
#         return 'You are currently logged in.'
#     return 'You are NOT logged in.'


if __name__ == '__main__':
    app.run(debug=True)
