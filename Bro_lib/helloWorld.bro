function check_ip(s: subnet, ips: set[addr]): bool
{
	for ( ip in ips )
	{
		if(ip in s){
			print ip, " is in subnet";
		}else{
			print "IP address <",ip,"> is not in subnet";
		}
	}
	return 1;
}


event bro_init() 
{
	local ips: set[addr] = {192.168.1.1, 192.168.1.2, 192.168.1.3, 192.168.1.4, 192.168.2.1};
    local s: subnet = 192.168.1.0/24;
    
	# Function calls.
	if(check_ip(s, ips)){
		print "Task completed successfully!";
	}else{
		print "Task failed!";
	}
	
}