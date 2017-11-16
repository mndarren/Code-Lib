/* Macros */
#define TRUE 1
#define INFOSIZE 200
#define IDSIZE 9
#define FILE_NAME "gasData.dat"

/*New structure */
struct mpg_info{
    char  id[IDSIZE];
	int   odometer;
	float gallons;
 };
typedef struct mpg_info Item;

/*functions*/
float computeMpg(Item gasData[],char param[],int count);
int initial(Item gasData[]);
void insertionSort(Item a[],int low,int high);
void exchange(Item *a,Item *b);               /*sorting tool*/
void Itemcpy(Item *x,Item *y);
