/*@author Zhao Xie <11/02/2013>
**@file Sort.cpp*/
//quickSort
const int MIN_SIZE = 10;
template <class ItemType>
void partition(ItemType a[], int low,int high,
               ItemType pivot,int &i,int &j)
{
  int lastS1 = low-1;
  int firstU = low;
  int firstS3 = high+1;
  while(firstU<firstS3)
    if (a[firstU]<pivot)
	  exchange(a[firstU++],a[++lastS1]);
	else if (a[firstU]==pivot)
	  ++firstU;
	else
	  exchange(a[firstU],a[--firstS3]);
  i = lastS1;
  j = firstS3;
}
template <class ItemType>
void quickSort(ItemType a[],int low,int high)
{ 
  if (high-low+1 < MIN_SIZE)
     insertionSort(a,low,high);
  else
  {
    ItemType pivot=a[low];
	int i,j;
	partition(a,low,high,pivot,i,j);
	quickSort(a,low,i);
	quickSort(a,j,high);
  }
}
//littleTools
template <class ItemType>
void exchange(ItemType& a,ItemType& b)
{
  ItemType temp;
  temp = a;
  a = b;
  b = temp;
}
//insertionSort
template <class ItemType>
void insertionSort(ItemType a[],int low,int high)
{ 
  for(int i=low; i<high;++i)
  {
    for(int j=i+1; j>low; --j)
	  if (a[j-1]>a[j])
	    exchange(a[j-1],a[j]);
	  else
	    break;
  }
}
//mergeSort
template <class ItemType>
void merge(ItemType a[],int low,int mid,int high)
{
  ItemType b[high+1]; //local copy
  int i1 = low;
  int i2 = mid+1;
  int index = low;
  //copy the values into b[]
  for (int i=low;i<=high;++i)
    b[i] = a[i];
	
  while(i1<=mid&&i2<=high)
  {
	  if (b[i1]<b[i2])
	    a[index++] = b[i1++];
	  else
	    a[index++] = b[i2++];
  }
  while(i1<=mid)
    a[index++] = b[i1++];
  while(i2<=high)
    a[index++] = b[i2++];
}
template <class ItemType>
void mergeSort(ItemType a[],int low,int high)
{ 
  if(low<high)
  {
    int mid = low + (high-low)/2;
	mergeSort(a,low,mid);
    mergeSort(a,mid+1,high);
    merge(a,low,mid,high);
  }
}
//bubbleSort
template <class ItemType>
void bubbleSort(ItemType a[],int low,int high)
{
  bool sorted = false;
  for(int i=low+1,k=1;!sorted&&i<=high;i++,k++)
  {
    sorted = true;
	for (int j=low;j<=high-k;j++)
	{
	  if(a[j]>a[j+1])
	  {
    	exchange(a[j],a[j+1]);
		sorted = false;
      }//end if
	}//end for
  }//end for
}
//selectionSort
template <class ItemType>
void selectionSort(ItemType a[],int low,int high)
{
  for(int last = high;last>=low+1;last--)
  {
    int largest = findIndexofLargest(a,low,last);
	exchange(a[largest],a[last]);
  }
}
template <class ItemType>
int findIndexofLargest(ItemType a[],int low,int high)
{
  int index = low;
  for (int i=low+1;i<=high;i++)
    if (a[index]<a[i])
	  index = i;
  return index;
}
//radixSort
template <class ItemType>
void radixSort(ItemType a[],int low,int high)
{
  int radix = 10;
  //compute digits
  int digits = 1;
  int largest = a[low];
  for (int i=low+1;i<=high;i++)
    if (largest<a[i])
	  largest = a[i];
  int flag = 1;
  while(flag)
  {
    largest = largest/radix;
	if (largest != 0)
	  digits++;
	else
	  flag = 0;
  }
  radixS(a,low,high,radix,digits);
}
template <class ItemType>
void radixS(ItemType a[],int low,int high,int radix,int digits)
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
}
//heapSort
template <class ItemType>
void heapSort(ItemType a[],int n)
{
  for (int i=(n-2)/2; i>=0;--i)
    walk_down(a,i,n-1);
  for(int i=n-1;i>0;--i)
  {
    exchange(a[0],a[i]);
	walk_down(a,0,i-1);
  }
}
template <class ItemType>
void walk_down(ItemType a[],int parent,int last)
{
  int max_child;
  bool done=false;
  while(2*parent+1<=last && !done)
  {
    max_child=2*parent+1;
	if(a[max_child+1]>a[max_child])
	  ++max_child;
	if(max_child<last&&a[max_child]>a[parent])
	{
	  exchange(a[max_child],a[parent]);
	  parent = max_child;
	}
	else
	  done=true;
  }
}