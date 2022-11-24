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
```
2. .tmux.conf
```
# Change C-b to C-g as prefix
set -g prefix C-g
unbind-key C-b
bind-key C-g send-prefix

# 0 is too far from ` ;)
set -g base-index 1

# https://github.com/tmux/tmux/wiki/Clipboard
# https://superuser.com/questions/1336762/how-do-i-copy-paste-from-the-system-clipboard-in-tmux-in-xterm-on-linux/1336764
set -g mouse on
set -s set-clipboard external

# Make esacpe work the way I would expect, eliminate latin character
# translation.
set -sg escape-time 0

set-option -g update-environment "DISPLAY SSH_AUTH_SOCK SSH_ASKPASS SSH_AGENT_PID SSH_CONNECTION WINDOWID XAUTHORITY"

set -g default-terminal screen-256color
set -g history-limit 10000

# Adapted from https://gist.github.com/spicycode/1229612
# Use Alt-arrow keys without prefix key to switch panes
bind -n M-Left select-pane -L
bind -n M-Right select-pane -R
bind -n M-Up select-pane -U
bind -n M-Down select-pane -D

# Shift arrow to switch windows
bind -n S-Left  previous-window
bind -n S-Right next-window

bind-key J resize-pane -D 5
bind-key K resize-pane -U 5
bind-key H resize-pane -L 5
bind-key L resize-pane -R 5

bind-key -n M-j resize-pane -D
bind-key -n M-k resize-pane -U
bind-key -n M-h resize-pane -L
bind-key -n M-l resize-pane -R

bind-key Z resize-pane -Z

# Max window
set-window-option -g aggressive-resize on
```