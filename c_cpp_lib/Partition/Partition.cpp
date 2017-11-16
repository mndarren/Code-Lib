/*@author Zhao Xie
  @date 12/18/2013
  @file partition.cpp*/
  
Partition::Partition()
{
  for(int i=0;i<N;++i)
     p[i] = -1;
}
int Partition::uf_find(int x)
{
  int temp = x;
  while(p[temp]>=0)
     temp=p[temp];
  return temp;
}
void Partition::uf_union(int x,int y)
{
  int root,new_child;
  root = uf_find(x);
  new_child = uf_find(y);
  p[new_child] = root;
}
//avoid worst case 1. weight balancing
/*void Partition::uf_union(int x,int y)
{
  int xset = uf_find(x);
  int yset = uf_find(y);
  if (p[xset]<p[yset])
  {
    p[xset] += p[yset];
	p[yset] = xset;
  }
  else
  {
    p[yset] += p[xset];
	p[xset] = yset;
  }
}
//2. path compression
int Partition::uf_find(int x)
{
  int temp=x,t,root;
  while(p[temp]>=0)
    temp = p[temp];
  root = temp;
  temp = x;
  while(p[temp]>=0)
  {
    t=temp;
	temp = p[temp];
	p[t] = root;
  }
  return root;
}*/