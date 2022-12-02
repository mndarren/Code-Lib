# Linux Commands
===========================  
1. Tmux Common CMD
```
# Create Session
tmux
tmux new
tmux new-session
tmux new -s session_name

# Detach/Attach Session
Ctrl+B+D
exit
tmux attach -t session_name

# View Session
tmux ls
tmux list-sessions
Ctrl+B+S

# Rename Session
tmux rename-session -t old_name new_name

# Kill Session
tmux kill-server
tmux kill-session -t session_name

# Create/Close Windows
Ctrl+B+C
Ctrl+B+&
Ctrl+B+,  # Rename current window

# Select Window
Ctrl+B+window_id
Ctrl+B+N
Ctrl+B+P

# Create/Close Toggle Panes
Ctrl+B+%
Ctrl+B+"
Ctrl+B+X
Ctrl+B+arrow

# Use Mouse
:set -g mouse on
:set -g mouse off

tmux -Version
tmux show -s set-clipboard

# system config @ /etc/tmux.conf
# user config @ .tmux.conf

# To get list of global options
# To get the list of window options
# To get the list of server options
tmux show-options -g
tmux show-options -w
tmux show-options -s
```
