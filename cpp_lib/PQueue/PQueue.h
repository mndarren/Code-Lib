/*@auther Zhao Xie
  @date 12/18/2013
  @file PQueue.h*/
  
#ifndef PQUEUE_H
#define PQUEUE_H

template <class T>
class PQueue
{
  public:
    static const int CAPACITY=20;
	PQueue(){count=0;}
	void enqueue(T entry);
	T dequeue();
	int size(){return count;}
	bool empty(){return count==0;}
  private:
    T data[CAPACITY];
	int count;
	void exchange(T &x,T &y);
	void reheap_up(int child);
	void reheap_down(int parent);
};
#include "PQueue.cpp"
#endif