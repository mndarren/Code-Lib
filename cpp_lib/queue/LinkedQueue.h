//CSCI591(301) Sec.1 <Zhao Xie>
//project7 Due: 11/12/2013
/**@file LinkedQueue.h*/
#ifndef LINKED_QUEUE_H
#define LINKED_QUEUE_H
#include <iostream>
using namespace std;
template <class ItemType>
class LinkedQueue;

template <class ItemType>
ostream& operator<< (ostream& out_q,
                    const LinkedQueue<ItemType>& q);
					 
template <class ItemType>
class LinkedQueue
{
  public:
    //constructor & destructor
	LinkedQueue(){front=NULL;rear=NULL;count=0;}
	LinkedQueue(const LinkedQueue<ItemType>& q);
	virtual ~LinkedQueue();
	//member functions
	void enqueue(ItemType& entry);
	ItemType dequeue();
	bool empty() const {return count == 0;}
	int size() const {return count;}
	//friend function
	friend ostream& operator<< <ItemType>
	           (ostream& out_q, const LinkedQueue<ItemType>& q);	
  private:
    struct Node
	{
	  ItemType data;
	  Node* next;
	};
	Node* front;
	Node* rear;
	int count;
	Node* get_node(ItemType entry, Node* link);
};
#include "LinkedQueue.template"
#endif