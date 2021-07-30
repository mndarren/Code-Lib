#!/bin/bash
export DISPLAY=:0.0

if [ $# -eq 0 ]; then
  echo usage: $(basename $0) "on|off|status"
  exit 1
fi

if [ $1 = "off" ]; then
  echo -en "Turning monitor off..."
  xset dpms force off
  echo -en "done.\nCheck:"
  xset -q|grep "Monitor is"
elif [ $1 = "on" ]; then
  echo -en "Turning monitor on..."
  xset dpms force on
  echo -en "done.\nCheck:"
  xset -q|grep "Monitor is"
elif [ $1 = "status" ]; then
  xset -q|sed -ne 's/^[ ]*Monitor is //p'
else
  echo usage: $(basename $0) "on|off|status"
fi

#Save this script in something like /usr/bin, give it a name (like switch_dpms) and make it executable with chmod 664 /usr/bin/switch_dpm.
#
#Now all you need to do is add it to a cron job. So open your crontab file with:
#
#crontab -e
#and add this at the bottom:
#
#@reboot /usr/bin/switch_dpms off
#Every reboot it will turn dpms to off and you can also turn it on from commandline by doing /usr/bin/switch_dpms on or check its status with /usr/bin/switch_dpms status.
