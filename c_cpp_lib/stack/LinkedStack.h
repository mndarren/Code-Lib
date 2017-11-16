//Created by Zhao Xie
/**@file LinkedStack.h*/
#ifndef _LINKED_STACK
#define _LINKED_STACK
#include "StackInterface.h"
#include "PrecondViolatedExcep.h"
template <class ItemType>


template <class ItemType>
class LinkedStack : public StackInterface<ItemType>
{
  public:
    //Constructor & destructor
	LinkedStack();
	LinkedStack(const LinkedStack<ItemType>& aStack);
	virtual ~LinkedStack();
	//Stack functions
	bool push(const ItemType& entry);
	bool pop();
	bool isEmpty() const {return top==NULL;}
	ItemType peek() const throw(PrecondViolatedExcep);
    int size() const {return depth;}
  private:
    //Data members
	struct Node
	{
	  ItemType data;
	  Node* next;
	};
	Node* top;
	int depth;
	Node* get_node(const ItemType& entry, Node* nextP);
};
#include "LinkedStack.cpp"
#endif