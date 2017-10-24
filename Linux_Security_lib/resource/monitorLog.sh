echo This is a script file to perform stateful inspection for sql injections
mount -t tmpfs -o size=512m tmpfs /mnt/ramdisk
sudo cat -n mysqllog > /mnt/ramdisk/mysqllog2
cat /mnt/ramdisk/mysqllog2 | grep "%27+or+27%"
rm /mnt/ramdisk/mysqllog2
unmount /mnt/ramdisk
echo end of script