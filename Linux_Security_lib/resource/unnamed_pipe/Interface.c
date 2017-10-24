#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <errno.h>
#include <string.h>
#include "Communication.h"

/*CSCI311 project3 Zhao Xie
 *This program is an interface, which will
 *accept commands from the console and send
 *them to the Server for execution. After
 *executing the command, the Server will
 *send the output to the Interface for display
 *on the console.
 */
 
int 
main(int argc,char *argv[]){
   int toChild[2], toParent[2], status, n;
   pid_t pid, return_pid;
   char line[INFOSIZE],str1[10],str2[10];
   
   if (pipe(toChild) < 0 || pipe(toParent) < 0)
       printf("pipe error: %d/n",errno);
   printf("Input command:\n");
    if((pid = fork())<0){
	     printf("fork error: %d\n",errno);
	}else if(pid>0){				      /*parent*/
		 close(toChild[0]);
		 close(toParent[1]); 
	}else{							     /*child*/
		 close(toChild[1]);
		 close(toParent[0]);
		 sprintf(str1,"%d",toChild[0]);
		 sprintf(str2,"%d",toParent[1]);
         if (execl("./Server", "Server",str1,str2, NULL) < 0)
           printf("execl error: %d/n",errno);
    }

	while (TRUE)
	{
	    if(fgets(line, INFOSIZE, stdin) == NULL){
		    printf("fgets error\n");
			printf("Input command(exit to quit):\n");
		}else{
		    n = strlen(line); 
			if (write(toChild[1], line, n) != n)
              printf("write error to pipe: %d/n",errno);
		}
		    if ((n = read(toParent[0], line, INFOSIZE)) < 0)
              printf("read error from pipe: %d/n",errno);
			line[n] = 0;                    /* null terminate */
			printf("response: %s",line);
			if(strcmp(line,"Server complete.\n") == 0){
			    printf("Interface: child process (%d) completed.\n",pid);
				break;
			}
		printf("Input command:\n");
	}
	/*checking the status of child*/
	waitpid(pid,&status,WNOHANG);
	printf("Interface: child process exit status = %d.\n",status);
	printf("Interface: Complete.\n");
	exit(0);
}
