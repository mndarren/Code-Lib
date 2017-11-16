//Sorting Checking
#include <iostream>
#include "Sort.h"
using namespace std;

void write(int a[],int n)
{
  for (int i=0;i<n;i++)
     cout <<' '<<a[i];
}
int main()
{
  int n = 12;
  int a[] = {9,8,7,3,6,10,23,11,19,14,5,6};
  write(a,n);
  //quickSort(a,0,n-1);
  //insertionSort(a,0,n-1);
  //mergeSort(a,0,n-1);
  //bubbleSort(a,0,n-1);
  //selectionSort(a,0,n-1);
  //radixSort(a,0,n-1);
  heapSor(a,12);
  cout<<endl;
  write(a,n);cout<<endl;
  return 0;
}