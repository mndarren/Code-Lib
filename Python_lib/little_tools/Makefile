mount_all: mount_people_orgs mount_u_drive mount_products
	echo "Done for all!"

mount_people_orgs:
	sudo mount -t cifs //sfcprint.sfsi.stearnsbank.net/datamodule ~/mnt/sfcprint -o domain=sfsi,username=darrenx,sec=ntlm,vers=1.0 && \
	echo "Data mounted!"

mount_u_drive:
	sudo mount -t cifs //sfcfile.sfsi.stearnsbank.net/data/users/DarrenX ~/mnt/DarrenX -o domain=sfsi,username=darrenx,sec=ntlm,vers=1.0 && \
	echo "U drive mounted!"

mount_products:
	sudo mount -t cifs //efdnfs.sfsi.stearnsbank.net/info ~/mnt/efdfile -o domain=sfsi,username=juliao,sec=ntlm,vers=1.0 \
	echo "Product mounted!"
