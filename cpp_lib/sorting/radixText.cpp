//Radix Sort for strings
#include <iostream>
using namespace std;
#include "Sort.h"
void write(string a[],int n)
{
  for (int i=0;i<n;i++)
     cout <<' '<<a[i];
}
/*void radixS(string a[],int low,int high,int radix,int digits)
{
  LinkedQueue<int> q[radix];
  int factor = 1;
  int digit;
  int index;
  for (int k=0;k<digits;++k)
  {
    for (int i=low;i<=high;++i)
	{
	  digit = (a[i]/factor)%radix;
	  (q[digit]).enqueue(a[i]);
	}
	index = low;
	for (int d=0;d<radix;++d)
	  while(!(q[d]).empty())
	    a[index++] = (q[d]).dequeue();
	factor = factor*radix;
  }
}*/
int main()
{
  int n = 25;
  string a[] = {"xie","chen","mo","zhao","luo","cheng","xie","hai","lei","zi","mo","pan","sheng","ti","ai","dong","you",
                "sun","wei","song","xiang","wen","song","jing","xia"};
  write(a,n);
  //quickSort(a,0,n-1);
  insertionSort(a,0,n-1);
  //mergeSort(a,0,n-1);
  //bubbleSort(a,0,n-1);
  //selectionSort(a,0,n-1);
  //radixS(a,0,n-1,26,5);
  cout<<endl;
  write(a,n);cout<<endl;
  return 0;
}