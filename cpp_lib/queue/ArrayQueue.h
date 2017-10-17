//CSCI591(301) Sec1 <Zhao Xie>
//Project 6 Due:10/29/2013
/**@file ArrayQueue.h*/
#ifndef ARRAY_QUEUE_H
#define ARRAY_QUEUE_H
#include <iostream>
using namespace std;

template <class ItemType>
class ArrayQueue;

template <class ItemType>
ostream& operator << ( ostream& out_q,
                   ArrayQueue<ItemType> &q );

template <class ItemType>
class ArrayQueue
{
  public:
    static const int MAX = 100;
	//constructor
	ArrayQueue();
	//member functions
	void enqueue(ItemType entry);
	ItemType dequeue();
	int size() const { return count;}
	bool empty() const {return count==0;}
	//friend function
	friend ostream& operator<< <ItemType>
	                (ostream& out_q, ArrayQueue<ItemType> &q);	
  private:
    ItemType data[MAX];
	int front;
	int rear;
	int count;
	int next_index(int i)
	{ return (i+1)%MAX;}
};
#include "ArrayQueue.cpp"
#endif