#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <errno.h>
#include <string.h>
#include "Communication.h"

/*CSCI311 project3 Zhao Xie
 *This is the Server.c program, which will
 *be executed by Interface.c program to call
 *execv() system call. This program is charge
 *of execution of command.
 */

 int 
 main(int argc,char *argv[])
 {
    Item gasData[INFOSIZE];
	char line[INFOSIZE], command[IDSIZE], param[IDSIZE];
	int count,n,index,hasComma;
	int readfd = atoi(argv[1]), writefd = atoi(argv[2]);
	float result;
	
	count = initial(gasData);   		   /*initialize the gasData */
	/*Command execution */
	while (TRUE){
		n = read(readfd, line, INFOSIZE);
        line[n] = 0;    				    /* null terminate */
	    hasComma = 0;
		for(index=0;index<n;index++)        /*check if line has a comma*/
		    if(line[index] == ',')
			    hasComma = 1;
		if(hasComma)
		    sscanf(line, "%[^','],%s", command, param);
        else
		    sscanf(line,"%s",command);
		if(strcmp(command,"mpg")==0) {
           result = computeMpg(gasData,param,count);
		   sprintf(line, "Average MPG = %f \n", result);
	    }else if(strcmp(command,"list")==0){
		   Item array[IDSIZE];
		   int i=0;   							/* a counter */
		   char temp[INFOSIZE];
		   line[0] = '\n';
		   line[1] = '\0';
		   for(index=0;index<count;index++){
	          if(strcmp(gasData[index].id,param)==0){
				 Itemcpy(&array[i],&gasData[index]);     /*copy data from gasData to array*/
				 i++;
			    }
	       }
		   insertionSort(array,0,i-1);                 /*sorting the list by the key odometer*/
		   for(index=0;index<i;index++){
		      sprintf(temp,"	odometer = %d,	gallons = %f\n",
	                  array[index].odometer,array[index].gallons);
			  strcat(line,temp);
		   }
	    }else if(strcmp(command,"exit")==0){
	           sprintf(line,"Server complete.\n");
		       n = strlen(line);
			   if( write(writefd, line, n) != n)
     	          printf("write error2/n");
			   exit(0);
	    }
	    else{
	            sprintf(line,"invalid command.\n");
		}
		command[0] = '\0';
		param[0] = '\0';
		n = strlen(line);
		if( write(writefd, line, n) != n)
     	     printf("write error2/n");
		line[0] = '\0';                   /*clear line*/
	}
 }
 int initial(Item gasData[])
 {
    char line[INFOSIZE],*ch;
	FILE *in_file;
	int count = 0,n;
	
	/* Initializing data array */
	in_file = fopen(FILE_NAME, "r");
	if(in_file == NULL){
	   printf("Can not open %s\n", FILE_NAME);
	   exit(8);
	}
	while(TRUE){
	   /*ch = fgets(line, sizeof(line), in_file);
	   if(ch == NULL)
	      break;
	   sscanf(line, "%s %d %f",gasData[count].id,
	          &(gasData[count].odometer),&(gasData[count].gallons));*/
	    fscanf(in_file,"%s %d %f",gasData[count].id,
	          &(gasData[count].odometer),&(gasData[count].gallons));
	    if(feof(in_file))
		    break;
		sprintf(line,"element = %d:	id = %s,	odometer = %d,	gallons = %f \n",
	          count,gasData[count].id,gasData[count].odometer,gasData[count].gallons);
	    n = strlen(line);
        if (write(STDOUT_FILENO, line, n) != n)
     	    printf("write error000/n");
	    count++;
	}
	if(fclose(in_file) != 0)
	   printf("fclose error.\n");
	return(count);
 }
 float computeMpg(Item gasData[],char param[],int count)
 {
    int i,largest = 0,smallest;
	float sum_gallons = 0.0,mpg;
	for(i=0;i<count;i++){
	   if(strcmp(gasData[i].id,param)==0){
		  if(gasData[i].gallons == 0.0)
		     smallest = gasData[i].odometer;
		  if(gasData[i].odometer>largest)
		     largest = gasData[i].odometer;
		  sum_gallons += gasData[i].gallons;
		}
	}
	return (largest-smallest)/sum_gallons;
 }
void insertionSort(Item a[],int low,int high)
{ 
  for(int i=low; i<high;++i)
  {
    for(int j=i+1; j>low; --j)
	  if (a[j-1].odometer>a[j].odometer)
	    exchange(&a[j-1],&a[j]);
	  else
	    break;
  }
}
void exchange(Item *a,Item *b)
{
  Item temp;
  Itemcpy(&temp,&(*a));
  Itemcpy(&(*a),&(*b));
  Itemcpy(&(*b),&temp);
}
 void Itemcpy(Item *x,Item *y)
 {
    strcpy((*x).id,(*y).id);
	(*x).odometer = (*y).odometer;
	(*x).gallons = (*y).gallons;
 }
