/*@author Zhao Xie
  @date 12/17/2013
  @file ExpTree.h*/
  #include <iostream>
  using namespace std;
  #ifndef EXPTREE
  #define EXPTREE

  class ExpTree
 {
    private:
	   struct Node
	   {
	     int oprd;
		 char optr;
		 Node* left;
		 Node* right;
	   };
	   Node* root;
	   void destroy(Node* r);
	   Node* help_build(istream& in_s);
	   void infix(Node* r);
	   int compute(Node* r);
	   int eval(int, char,int);
	public:
	   ExpTree(){root = NULL;}
	   ~ExpTree(){destroy(root);}
	   void build(istream& in_s){root = help_build(in_s);}
	   void print(){infix(root);}
	   int result(){return compute(root);}
 };
 #include "ExpTree.cpp"
 #endif