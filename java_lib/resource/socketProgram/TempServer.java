/***************************************************************************
 * Program: TempServer.java                                                 *
 * Type   : Connection Oriented (TCP)                                       *
 * Author : Abdullah Alhamamah						    	    			*
 * Comment: This server program accept three connection from a client,get   *
 *          temp in fahrenheit convert it to celsius then send it back to   *
 *          client					    									*
 ***************************************************************************/

import java.io.*;
import java.net.*;

public class TempServer
{
	public static void main(String argv[]) throws Exception
   	{
    	String F,                		/* Temp in fahrenheit */
               C;                		/* Temp in celsius */
      	double temp;             		/* Temporary variable */
      	int counter = 1,		  		/* Counter for number of conections */
            port    = 12411,	  			/* Port number */
          	backlog = 5; 		  		/* Number of connections */
      	BufferedReader   in = null; 	/* Create an input stream & initialize it */
      	DataOutputStream out= null; 	/* Create an output stream & initialize it */
      	ServerSocket socket = null; 	/* Create a server socket & initialize it */
      	Socket    conSocket = null; 	/* Create a client socket& initialize it */

		try
      	{    	/* Create a socket, bind it to port */
        		socket = new ServerSocket(port,backlog);
      	} catch (IOException e)
			{
    	    	System.out.println("Failed to listen on port:" + port);
    	   		System.exit(-1);
			}

		while(counter <= 3)
      	{
        	System.out.println("Waiting for connection... \n");

        	try
	  		{   /* Accept a connection from a client */
            	conSocket = socket.accept();
            	/* If can't accept connection, throw an exception and  print an error message */
       		} catch (IOException e)
	   			{
            		System.out.println("Failed to accept connection on port:" + port);
    				System.exit(-1);
	   			}
	   		try
	   		{
	      		System.out.println("Connection " + counter + " recived from: "
                                                 + conSocket.getInetAddress().getHostName());
            	/* Connect the input stream to the socket */
	      		in = new BufferedReader(new InputStreamReader(conSocket.getInputStream()));
				/* Connect the output stream to the socket */
	      		out=  new DataOutputStream(conSocket.getOutputStream());
	      		/* If can't get the I/O stream, throw an exception and  print an error message */
        	} catch(IOException e)
	   			{
            		System.out.println("I/O error when connecting to client: " + conSocket.getInetAddress().getHostName());
    				System.exit(-1);
	   			}


         	/* Read temp from client */
	   		F = in.readLine();
	   		System.out.println("Temp in F: " + F);
         	/* Convert temp from fahrenheit to celsius */
         	temp = 0.55 * (Double.parseDouble(F)-32.0);
         	/* Convert tempreture to a string */
	   		C=Double.toString(temp);
	   		/* Send temp in celsius to client */
	   		out.writeBytes(C + '\n');
         	/* Increase counter by 1 */
         	++counter;
         	/* Flush the output stream */
         	out.flush();
      	}/* End of while loop */

      	/* Close the server's socket */
      	socket.close();
      	/* Close the input buffer */
		in.close();
		/* Close the output stream */
		out.close();
   }
}
