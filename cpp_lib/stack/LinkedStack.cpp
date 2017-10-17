//Created by Zhao Xie
/**@file LinkedStack.cpp*/
#include "LinkedStack.h"

template <class ItemType>
LinkedStack<ItemType>::LinkedStack():top(NULL), depth(0)
{}

template <class ItemType>
LinkedStack<ItemType>::
LinkedStack(const LinkedStack<ItemType>& aStack)
{
  Node* oldP = aStack->top;
  if (oldP == NULL)
     top = NULL;
  else
  {
    top = get_node(oldP->data,NULL);
	Node* newP = top;
	while(oldP != NULL)
	{
	  oldP = oldP->next;
	  newP->next = get_node(oldP->data,oldP->next);
	  newP = newP->next;
	}
  }
}
template <class ItemType>
typename LinkedStack<ItemType>::Node* LinkedStack<ItemType>::
get_node(const ItemType& entry, Node* nextP)
{
  Node* temp = new Node;
  temp->data = entry;
  temp->next = nextP;
  return temp;
}
template <class ItemType>
LinkedStack<ItemType>::~LinkedStack()
{
  while(!isEmpty())
    pop();
}
template <class ItemType>
bool LinkedStack<ItemType>::
push(const ItemType& entry)
{
  top = get_node(entry,top);
  ++depth;
  return true;
}
template <class ItemType>
bool LinkedStack<ItemType>::pop()
{
  bool result = false;
  if(!isEmpty())
  {
    Node* temp = top;
	top = top->next;
	delete temp;
	temp = NULL;
	result = true;
	--depth;
  }
  return result;
}
template <class ItemType>
ItemType LinkedStack<ItemType>
::peek() const throw (PrecondViolatedExcep)
{
  if (isEmpty())
  {
    string message = "peek() called with empty stack";
	throw PrecondViolateExcep(message);
  }
  return top->data;
}
