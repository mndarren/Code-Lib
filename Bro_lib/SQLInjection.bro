##! Bro IDS project, Date: 4/24/2017, SQLInjection.bro
## Run bro using the command:
## /opt/bro/bin/bro -i eth1 sqlicode.bro -C
## "-C" is for dealing with invalid checksum
## Need more work on the pattern

@load base/frameworks/notice
@load base/protocols/ssh
@load base/protocols/http
@load base/frameworks/sumstats

module HTTP;
export {
    redef enum Notice::Type += {
        SQL_URI_Injection_Attack,
        SQL_Post_Injection_Attack,
    };
    
     ## URL message input
    type UMessage: record{
        text: string; ##< The actual URL body
    };
    
    const match_sql_uri = /[']/ &redef;
    const match_sql_uri1 = /[']/ &redef;
    const match_sql_uri2 = /[0-9]/ &redef;
    const match_sql_body = /[\%27]/ &redef;
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
    local array = split_string(data, /password=/);
    
    for( i in array){
        local val = array[i];
        msg$text = val;
    }
    if( i == 2){
        return msg;
    }
     else
     {
     msg$text = "";
     return msg;
     }
}

## Parse URI
function parse_uri(data: string) : UMessage{

    local msg: UMessage;
    local array = split_string(data, /name=/);
    for ( i in array ){
        local val = array[i];
        msg$text = val;
    }

    if(i == 2){
        return msg; # returns msg
    }
    else{
        msg$text = "";
        return msg;
    }
}

event http_entity_data(c: connection, is_orig: bool, length: count, data: string) &priority=5 {
    local msg:UMessage;
    ascore = 1;
    
    http_body = data;
    ## GET SQL IN REQUEST BODY
    msg = parse_body(http_body);
    if(|msg$text| > 10)
        ++ascore;
    if(match_sql_body in msg$text){
        ++ascore;
        if(match_sql_uri1 in msg$text)
            ++ascore;
    }
    if ( ascore >= 3){
        print fmt("%s, An SQL injection caught: %s is the attacker!  ascore = %s",
                                strftime("%Y/%M/%d %H:%m:%s", network_time()), c$id$orig_h, ascore);
        NOTICE([$note=SQL_Post_Injection_Attack,
        $conn=c,
        $msg=fmt("SQL Attack from %s to destination: %s with Attack string %s and post data %s",
        c$id$orig_h, c$id$resp_h, c$http$uri, http_body)]);
    }
}


event http_request(c: connection, method: string, original_URI: string, unescaped_URI: string, version: string) &priority=3{
    local msg:UMessage;
    local body:UMessage;
    ascore = 1;
    # GET SQL IN HTTP REQUEST HEADER
    msg = parse_uri(c$http$uri);
    # Test for string length
    if (|msg$text| > 2)
        ++ascore;

        if(match_sql_uri in msg$text){
            ++ascore;
            if(match_sql_uri1 in msg$text)
                ++ascore;
        }
        if(match_sql_uri2 in msg$text && |msg$text| > 2){
            ++ascore;
        }
        if ( ascore >= 3){
            NOTICE([$note=SQL_URI_Injection_Attack,
            $conn=c,
            $msg=fmt("SQL Attack from %s to destination: %s with Attack string %s", c$id$orig_h, c$id$resp_h,
            c$http$uri)]);
        }
}
