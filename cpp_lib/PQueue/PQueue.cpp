/*@auther Zhao Xie
  @date 12/18/2013
  @file PQueue.cpp*/

template <class T>
void PQueue<T>::enqueue(T entry)
{
  if(count<CAPACITY)
  {
    data[count]=entry;
	reheap_up(count);
	++count;
  }
}
template <class T>
T PQueue<T>::dequeue()
{
  T temp;
  if(count>0)
  {
    temp = data[0];
	data[0] = data[--count];
	reheap_down(0);
	return temp;
  }
}
template <class T>
void PQueue<T>::exchange(T &x,T &y)
{
  T temp;
  temp = x;
  x = y;
  y = temp;
}
template <class T>
void PQueue<T>::reheap_up(int child)
{
  int parent = (child-1)/2;
  while(child>0&&data[child]>data[parent])
  {
    exchange(data[child],data[parent]);
	child = parent;
	parent = (child-1)/2;
  }
}
template <class T>
void PQueue<T>::reheap_down(int parent)
{
  int max_child;
  bool done = false;
  while(2*parent+1 < count&&!done)
  {
    max_child = 2*parent+1;
	if(max_child+1<=count &&
	   data[max_child+1]>data[max_child])
	   max_child++;
	if(data[max_child]>data[parent])
	{
	  exchange(data[parent],data[max_child]);
	  parent = max_child;
	}
	else
	  done = true;
  }
}