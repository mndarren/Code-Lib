/*@author Zhao Xie
  @date 12/18/2013
  @file Partition.h*/
  
#ifndef PARTITION
#define PARTITION

class Partition
{
  public:
    static const int N = 20;
	Partition();
	int uf_find(int x);
	void uf_union(int x,int y);
  private:
    int p[N];
};
#include "Partition.cpp"
#endif