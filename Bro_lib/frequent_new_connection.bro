##! New connections, 
##  Zhao Xie, IA612

@load base/frameworks/notice
@load base/protocols/conn

global source_ip: table[addr] of time;

export {
   redef enum Notice::Type += {
      New_Outgoing_Connection
   };
}

event new_connection (c: connection) {
  if(c$id$orig_h != 192.168.192.129 && c$id$resp_h == 192.168.192.129){
   if (c$id$orig_h !in source_ip) {
      print fmt ("%s: New connection from %s %s to %s %s ",
                strftime("%Y/%M/%d %H:%m:%S", network_time()),
                c$id$orig_h, c$id$orig_p, c$id$resp_h, c$id$resp_p);
      source_ip[c$id$orig_h] = c$start_time;
   } else {
      local diff: interval;
      diff = current_time() - source_ip[c$id$orig_h];
      source_ip[c$id$orig_h] = c$start_time;
      print fmt("%s: Old IP connection %s %s > %s %s, time interval = %s",
               strftime("%Y/%M/%d %H:%m:%s", network_time()),
               c$id$orig_h, c$id$orig_p, c$id$resp_h, c$id$resp_p, diff);
   }
   NOTICE([$note = New_Outgoing_Connection, $conn = c]);
  }
}
