#temporary change or touch file content
echo grant temporary right
chmod u+r myPass.txt
head myPass.txt
chmod -rwx myPass.txt
logger Darren read the file myPass.txt
logger `id`
echo take away temporary right