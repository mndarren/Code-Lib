//CSCI591(301) Project9
//Zhao Xie (Due 12/3/2013)
/**@file Trie.h*/

#ifndef _TRIE
#define _TRIE

#include <iostream>
#include <iomanip>
#include "LinkedStack.h"
using namespace std;

class Trie
{ 
  private:
    struct Node
	{
	  int count; 
	  Node *children[26];
	};
	Node *root;
	//private method
	void destroy(Node* r);
	int r_count(Node* r) const;
	void r_insert(Node* &t,char w[],int pos,int& count);
	void help_remove(Node* &r,char w[],int pos);
	void print(ostream& ,Node* ,LinkedStack<char>&);
	Node* copy( Node* r);
	//void r_get_count(Node*,int[],int) const;
  public:
    //default constructor
	Trie() {root = NULL;}
	//copy constructor
	Trie(const Trie& con)
	{root = copy(con.root);}
	//destructor
	~Trie() {destroy(root);}
	//modification methods
	void insert(char w[], int count) {r_insert(root,w,0,count);}
	void remove(char w[]) {help_remove(root,w,0);}
	//constant methods
	int get_count(char w[]) const;//{return r_get_count(root,w,0);}
	int length() const{return r_count(root);}
	//friend method
	friend ostream& operator<< (ostream& out_c, Trie c);
};
#include "Trie.cpp"
#endif
