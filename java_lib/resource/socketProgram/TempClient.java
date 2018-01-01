/**************************************************************************
 * This client program connect to a server, send temp in fahrenheit then  *
 * get it converted to celsius and displayed it on screen.                *
 *************************************************************************/
import java.io.*;
import java.net.*;

public class TempClient{

   public static void main(String argv[])  throws Exception{
       String F,               	/* Temp in fahrenheit */
	          C,        		/* Temp in celsius */
	       host = "127.0.0.1";	/* Server address */
	int    port = 12411,		/* Port number */
	       index;			/* Position of decimal point */
	BufferedReader   UserInput = null;
	BufferedReader   in        = null;
	DataOutputStream out       = null;
	Socket           socket    = null;
	try{
		/* Create a socket, connect it to server on the specified port */	
	     socket= new Socket(InetAddress.getByName(host), port);
	     System.out.println("Connecting to: " + socket.getInetAddress().getHostName());
	} catch (UnknownHostException e) {
	     System.err.println("Unknown host: " + host);
	     System.exit(1);
	}
	try	{    /* Get an input stream from the socket */	 
	     in  = new BufferedReader(new InputStreamReader(socket.getInputStream()));
	     /* Get an output stream to the socket */
	     out = new DataOutputStream(socket.getOutputStream());
	     /* Get a standard input from the keyboard */
  	     UserInput = new BufferedReader(new InputStreamReader(System.in));
 	} catch (IOException e) {
		System.err.println("I/O error on port: " + host);
        System.exit(1);
	}
	System.out.print("Enter a Tempreture in F: ");
        /* Read temp in fahrenheit from the input stream */
	F = UserInput.readLine();
	/* Send the temp to server */  
	out.writeBytes(F  + '\n' );
	/* Get temp in celsius from server */       
	C  = in.readLine();
	/* Find the position of the decimal point in the string */
	index = C.indexOf('.');
        /* Print the temp in two decimal places */
	System.out.println("Tempreture in C: " + C.substring(0,index+2));

	/* Close the client's socket */        
	socket.close();

    
   }

}




