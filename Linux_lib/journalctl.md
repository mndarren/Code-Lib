# Linux Commands
===========================  
1. journalctl Common CMD
```
# LOG level: emerg, alert, crit, err, warning, notice, info, debug (01234567)
# journal can be configged in /etc/systemd/journald.conf
# all journal in /var/log/journal
sudo systemctl restart systemd-journald
# To view your default limits
sudo journalctl -u systemd-journald

# Manually clean up archieved logs
journalctl --vacuum-size=2G
journalctl --vacuum-time=1years
journalctl --vacuum-files=10

# show all, including unprintable
journalctl --all
# limit number of lines
journalctl -n 15
# display journal log in real-time
journalctl -f
# display kernel logs
journalctl -k
# List boot
journalctl --list-boot
journalctl -b 92746927c11f4b66baf19ac0097c8943
journalctl -b # current boot
journalctl -b -1 # previous boot
# with time period (today, tomorrow)(1 day ago)
journalctl --since "yesterday"
journalctl --since "2 hours ago"
journalctl --since "2018-08-30 14:10:10" --until "2018-09-02 12:05:50"
journalctl --since -1h15min  # 1 hour 15 minutes in the past
journalctl --until +3h30min  # 3 hours 30 minutes in the future
journalctl --since 09:00 --until "1 hour ago"
# newest first
journalctl -r
journalctl --reverse
# no display warning or info
journalctl -q
journalctl --quiet
# help
journalctl --help
# Specify keyword
journalctl -r | grep GnuPG
# Specify priority
journalctl -p warning
journalctl -p err -b
journalctl -p 3 
# verbose output (short, json, json-prety, cat)
journalctl -o verbose
journalctl --no-full  # output truncated
journalctl -a         # show all
journalctl --no-pager # no less format
# Systemd Service
journalctl -u ssh
# Specify PID
journalctl _PID=1234
# Specify user
id -u www-data
journalctl _UID=33 --since today
# Specify group ID
journalctl -F _GID
journalctl _GID=1000
# filter by a path
journalctl /usr/bin/bash

EXAMPLES
# view last few logs (extra end)
journalctl -xe
# Error level, extra current boot
journalctl -p 3 -xb
# warning to info level, current session
journalctl -p 4..6 -b0
# log disk usage
journalctl --disk-usage
```
