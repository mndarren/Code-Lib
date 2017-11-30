##! Bro IDS project Date: 4/24/2017, XSS.bro
## Run bro using the command:
## /opt/bro/bin/bro -i eth1 sqlix.bro -C
## "-C" is for dealing with invalid checksum
## Need more work on the pattern

@load base/frameworks/notice
@load base/protocols/ssh
@load base/protocols/http

module HTTP;
export {
 	redef enum Notice::Type += {
 	XSS_URI_Injection_Attack,
 	XSS_Post_Injection_Attack,
 	};

 	## URL message input
 	type UMessage: record
 	{
 		text: string; ##< The actual URL body
 	};

 	const match_xss_uri = /[<>]/ &redef;
 	const match_xss_uri1 = /[<>]/ &redef;
 	const match_xss_body = /[\%3C\%3E]/ &redef;
 	global ascore:count &redef;
 	global http_body:string &redef;

 	redef record Info += {
 		## Variable names extracted from all cookies.
 		post_vars: vector of string &optional &log;
 	};
}

### parse body
function parse_body(data: string) : UMessage
{
 	local msg: UMessage;
 	local array = split_string(data, /comments=/);
 	for( i in array)
 	{
 		local val = array[i];
 		msg$text = val;
 	}
 	if( i == 2)
	 {
 		return msg;
 	}
 	else
 	{
 		msg$text = "";
 		return msg;
 	}
}

## Parse URI
function parse_uri(data: string) : UMessage
{
 	local msg: UMessage;
	local array = split_string(data, /name=/);
 	for ( i in array )
 	{
 		local val = array[i];
 		msg$text = val;
 	}

 	if(i == 2)
 	{
 		return msg; # returns msg
 	}
 	else
 	{
 		msg$text = "";
 		return msg;
 	}
}

event http_entity_data(c: connection, is_orig: bool, length: count, data: string) &priority=5
{
 	local msg:UMessage;
 	ascore = 1;
 	#if(c$http$first_chunk)
 	#{
 		http_body = data;
 		## GET XSS IN REQUEST BODY
 		msg = parse_body(http_body);
 		if(|msg$text| > 10)
 			++ascore;
 		if(match_xss_body in msg$text)
 		{
 			++ascore;
 			if(match_xss_uri1 in msg$text)
 				++ascore;
 		}
 		if ( ascore >= 3)
		{
 			print fmt("%s, An XSS injection caught: %s is the attacker!  ascore = %s",
                                strftime("%Y/%M/%d %H:%m:%s", network_time()), c$id$orig_h, ascore);
 			NOTICE([$note=XSS_Post_Injection_Attack,
 			$conn=c,
 			$msg=fmt("XSS Attack from %s to destination: %s with Attack string %s and post data %s",
					c$id$orig_h, c$id$resp_h, c$http$uri, http_body)]);
 		}
 	#}
}

event http_request(c: connection, method: string, original_URI: string,
 					unescaped_URI: string, version: string) &priority=3
{
 	local msg:UMessage;
 	local body:UMessage;
 	ascore = 1;
 	# GET XSS IN HTTP REQUEST HEADER
	msg = parse_uri(c$http$uri);
 	# Test for string length
 	if ( |msg$text| > 10)
 		++ascore;
 	if(match_xss_uri in msg$text)
 	{
 		++ascore;
 		if(match_xss_uri1 in msg$text)
 			++ascore;
 	}
 	if ( ascore >= 3)
 	{
 		NOTICE([$note=XSS_URI_Injection_Attack,
 		$conn=c,
 		$msg=fmt("XSS Attack from %s to destination: %s with Attack string %s", c$id$orig_h, c$id$resp_h,
				c$http$uri)]);
 	}
}
