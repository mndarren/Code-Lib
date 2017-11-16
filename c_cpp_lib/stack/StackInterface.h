//Created by Zhao Xie
//Basic ADT
/**@file StackInterface.h*/
#ifndef _STACK_INTERFACE
#define _STACK_INTERFACE

template<class ItemType>
class StackInterface
{
  public:
    /**Whether the stack is empty.
	 @return True if empty, otherwise false.*/
	 virtual bool isEmpty() const = 0;
	 /**Add a entry to the stack.
	 @post if successful, the entry is at the top of the stack.
	 @param entry The new Item to be added.
	 @return True if successful, otherwise false.*/
	 virtual bool push(const ItemType& entry) = 0;
	 /**Remove the top item from the stack.
	 @post If successful, the top item is removed.
	 @return True if successful, otherwise false.*/
	 virtual bool pop() = 0;
	 /**Look at the top item of the stack.
	 @pre The stack is not empty.
	 @post The value of top item of the stack is returned.
	 @return The top item value.*/
	 virtual ItemType peek() const = 0;
};
#endif