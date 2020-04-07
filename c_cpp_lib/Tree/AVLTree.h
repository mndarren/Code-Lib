/*@class CSCI591 Fall 2013
 *@project 10 Due: 12/12/2013
 *@author Zhao Xie
 *@file AVLTree.h*/
 #include "Inventory.h"
 #ifndef AVLTREE_H
 #define AVLTREE_H
 
 class AVLTree
 {
   private:
	 struct Node
     {
       Inventory invent;
	   Node* left;
	   Node* right;
     };
     Node* root;
	 float value;
	 void rotate_right(Node* &r);
	 void rotate_left(Node* &r);
	 int height(Node* r);
	 Node* get_node(const Inventory &invent1,Node* left1,Node* right1);
	 void destroy(Node* r);
	 int r_count(Node* r) const;
	 void r_insert(Node* &t,Inventory invent);
	 void help_remove(Node* &t,int number);
	 void remove_node(Node* &t);
	 void print(ostream& out_a,Node* p);
	 Node* copy( Node* r);
	 float r_value(Node* r);
   public:
     //constructor & destructor
	 AVLTree(){root = NULL;value=0.0;}
	 AVLTree(AVLTree &avl){root = copy(avl.root); }
	 ~AVLTree(){destroy(root);}
	 //modification functions
	 void insert(Inventory invent);
	 void remove(int number) {help_remove(root,number);}
	 void set_quantity(int quantity,int target);
	 //constant methods
	 int get_quantity(int target);
	 void print_node(ostream& out,int target) const;
	 int length() const{return r_count(root);}
	 float getValue(){r_value(root);return value;}
	 //friend method
	 friend ostream& operator<< (ostream& out_a, AVLTree& a);
};
#include "AVLTree.cpp"
#endif
