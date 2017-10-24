#temporary change or touch file content
echo grant temporary right
chmod u+rw myPass.txt
read -p 'UserName:' uservar
read -sp 'Password:' passvar
echo $uservar, $passvar >> myPass.txt 
chmod -rwx myPass.txt
logger myPass.txt modified by `id`
echo
echo Thankyou $uservar, we now have your login details