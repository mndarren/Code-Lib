//ExpTreeTest.cpp
#include "ExpTree.h"
using namespace std;

int main()
{
  ExpTree exp;
  exp.build(cin);
  exp.print();
  cout<<"\nThe result is: "<<exp.result()<<endl;
  return 0;
}