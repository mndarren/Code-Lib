//CSCI591(301) Sec.1 <Zhao Xie>
//@date 11/03/2013
/**@file PLinkedQueue.h*/
#ifndef PLINKED_QUEUE_H
#define PLINKED_QUEUE_H
#include <iostream>
using namespace std;
template <class ItemType>
class PLinkedQueue;

template <class ItemType>
ostream& operator<< (ostream& out_q,
                    const PLinkedQueue<ItemType>& q);
					 
template <class ItemType>
class PLinkedQueue
{
  public:
    //constructor & destructor
	PLinkedQueue(){front=NULL;count=0;}
	PLinkedQueue(const PLinkedQueue<ItemType>& q);
	virtual ~PLinkedQueue();
	//member functions
	void enqueue(ItemType& entry,int pri);
	ItemType dequeue();
	bool empty() const {return count == 0;}
	int size() const {return count;}
	//friend function
	friend ostream& operator<< <ItemType>
	           (ostream& out_q, const PLinkedQueue<ItemType>& q);	
  private:
    struct Node
	{
	  ItemType data;
	  int priority;
	  Node* next;
	};
	Node* front;
	int count;
	Node* get_node(ItemType entry, int pri,Node* link);
};
#include "PLinkedQueue.template"
#endif