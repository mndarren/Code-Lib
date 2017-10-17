//CSCI591 (301) Sec1 <Zhao Xie>
//Project5 Due:Oct 17
/**Stack.h*/
#ifndef STACK_H
#define STACK_H

#include <iostream>
#include <cstdlib>
using namespace std;

class Stack
{
  public:
     typedef char Item;
     static const int MAX = 20;
     //constructor
	 Stack(){ used=0;}     //Inline
	 //modification functions
	 void push(const Item& entry);
	 Item pop();
	 //constant functions
	 int length() const { return used;} //Inline
	 bool empty() const {return used == 0;} //Inline
	 Item peek() const;
  private:
     Item data[MAX];
	 int used;
};
#include "Stack.cpp"
#endif