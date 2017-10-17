//CSCI591(301) Sec1 <Zhao Xie>
//Project 6 Due:10/29/2013
/**@file ArrayQueue.cpp*/

template <class ItemType>
ArrayQueue<ItemType>::ArrayQueue()
{
  front = 0;
  rear = MAX-1;
  count = 0;
}
/*Add a new entry on the rear of the queue.
*@pre entry is >=1;*/
template <class ItemType>
void ArrayQueue<ItemType>::enqueue(ItemType entry)
{
  if (count<MAX)
  {
    rear = next_index(rear);
	data[rear] = entry;
	++count;
  }
}
/*Remove a item from the queue.
*@return a value of ItemType.*/
template <class ItemType>
ItemType ArrayQueue<ItemType>::dequeue()
{
  ItemType it;
  if (count>0)
  {
    it = data[front];
	front = next_index(front);
	--count;
	return it;
  }
}
/*Display the items of the queue.
*@pre q is an object of ArrayQueue.*/
template <class ItemType>
ostream& operator<< (ostream& out_q,
                     ArrayQueue<ItemType> &q)
{
  ItemType it;
  int n = q.size();
  for (int i=0;i<n;++i)
  {
    it = q.dequeue();
	out_q << it <<' ';
	q.enqueue(it);
  }
  return out_q;
}