import java.net.*;
import java.io.*;

public class UDPClient{
        public static void main(String argv[])
        {
                byte[] SendBuf=new byte[10];
                byte[] RecvBuf=new byte[1024];
                String host = "localhost",
                        tmp;
				int port = 11401,
                        index;
                DatagramSocket socket = null;
                BufferedReader input = null;
                DatagramPacket request,
                                reply;
                InetAddress address;

        try {
                socket = new DatagramSocket();
                address = InetAddress.getByName(host);
                input = new BufferedReader(new InputStreamReader(System.in));
                System.out.print("Enter a Temp in F: ");
                tmp=input.readLine();
                tmp.getBytes(0,tmp.length(),SendBuf,0);
                request = new DatagramPacket(SendBuf, tmp.length(), address, port);
                socket.send(request);
                reply = new DatagramPacket(RecvBuf, RecvBuf.length);
                socket.receive(reply);      
                tmp = new String(reply.getData());
                index = tmp.indexOf('.');
                System.out.println("\n Temp in C: " + tmp.substring(0,index+2));
                socket.close();
                        }
                        catch (IOException ioe)
                        { System.err.println("IO error: " + ioe); }
        }
}




