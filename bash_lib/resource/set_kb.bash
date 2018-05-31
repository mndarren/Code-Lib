# Set keyboard layout in xrdp sessions 
cd /etc/xrdp 
test=$(setxkbmap -query | awk -F":" '/layout/ {print $2}') 
echo "your current keyboard layout is.." $test
setxkbmap -layout $test 
sudo cp /etc/xrdp/km-0409.ini /etc/xrdp/km-0409.ini.bak 
sudo xrdp-genkeymap km-0409.ini