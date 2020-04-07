/*@class CSCI591 Fall 2013
 *@project 10 Due: 12/12/2013
 *@author Zhao Xie
 *@file AVLTree.cpp*/
#include "AVLTree.h"

/*private function for copy constructor*/
AVLTree::Node* AVLTree::copy( Node* r)
{
  if(r==NULL)
    return NULL;
  else
    return get_node(r->invent,copy(r->left),copy(r->right));
}
//private function to be called by destructor
void AVLTree::destroy(Node* r)
{
  if(r!=NULL)
  {
    destroy(r->left);
	destroy(r->right);
	delete r;
  }
}
//right rotation
void AVLTree::rotate_right(Node* &r)
{
  Node* temp;
  temp = r->left;
  r->left = temp->right;
  temp->right = r;
  r=temp;
}
//left rotation
void AVLTree::rotate_left(Node* &r)
{
  Node* temp;
  temp = r->right;
  r->right = temp->left;
  temp->left = r;
  r=temp;
}
//get a new node
AVLTree::Node* AVLTree::get_node(const Inventory &invent1,
                                Node* left1,Node* right1)
{
  Node *temp = new Node;
  temp->invent=invent1;
  temp->left = left1;
  temp->right = right1;
  return temp;
}
/*int max(int n1,int n2)
{
  if(n1>n2) return n1;
  else return n2;
}*/
//compute the height of the tree
int AVLTree::height(Node* r)
{
  if (r == NULL)
    return 0;
  else
    return 1+max(height(r->left),height(r->right));
}
/*private function to insert a new node*/
void AVLTree::r_insert(Node* &t,Inventory invent1)
{
  if (t== NULL)
  {
	t= get_node(invent1,NULL,NULL); 
  }
  else if(invent1.getNumber()<t->invent.getNumber())
    r_insert(t->left,invent1);
  else
    r_insert(t->right,invent1);
}
void AVLTree::insert(Inventory invent)	  
{
  r_insert(root,invent);
  int value = height(root->left)-height(root->right);
  if(value==2)
  {
    if(height(root->left->left)-height(root->left->right)==-1)
	  rotate_left(root->left);
	rotate_right(root);
  }
  else if(value==-2)
  {
    if(height(root->right->left)-height(root->right->right)==1)
	  rotate_right(root->right);
	rotate_left(root);
  }
}
/*private function to be called by length()*/
int AVLTree::r_count(Node* r) const
{
  if (r==NULL)
    return 0;
  else
    return r_count(r->left)+1
	       +r_count(r->right);
}
float AVLTree::r_value(Node* p)
{
  if(p!=NULL)
  {
	r_value(p->left);
	value += p->invent.value();
	r_value(p->right);
	return value;
  }
}
/*private function to be called by get_count()*/
void AVLTree::print_node(ostream& out,int target) const
{
  Node* p = root;
  while(1)
  {
    if(p == NULL)
	  return;
	else if(target == p->invent.getNumber())
	{
     p->invent.print(out);
	  return;
	}
    else if(target<p->invent.getNumber())
      p = p->left;
	else
	  p = p->right;
  }
}
int AVLTree::get_quantity(int target)
{
  Node* p = root;
  while(1)
  {
    if(p == NULL)
	  return 0;
	else if(target == p->invent.getNumber())
	  return p->invent.getQuantity();
    else if(target<p->invent.getNumber())
      p = p->left;
	else
	  p = p->right;
  }
}
void AVLTree::set_quantity(int quantity,int target)
{
  Node* p = root;
  while(1)
  {
    if(target == p->invent.getNumber())
	 { p->invent.setQuantity(quantity);return;}
    else if(target<p->invent.getNumber())
      p = p->left;
	else
	  p = p->right;
  }
}
/*private function to be called by remove()*/
void AVLTree::help_remove(Node* &t,int target)
{
  if(t->invent.getNumber() == target)
    remove_node(t);
  else if(target<t->invent.getNumber())
    help_remove(t->left,target);
  else
    help_remove(t->right,target);
}
/*private function to be called by help_remove()*/
void AVLTree::remove_node(Node* &t)
{
  Node* pl,*back;
  if(t->left == NULL && t->right == NULL)
    {delete t; t=NULL;}
  else if(t->left == NULL)
  {
    pl = t;
	t= t->right;
	delete pl;
  }
  else if(t->right == NULL)
  {
    pl = t;
	t = t->left;
	delete pl;
  }
  else
  {
    back = t;
	pl = t->right;
	while (pl->left!=NULL)
	{
	  back = pl;
	  pl = pl->left;
	}
	t->invent=pl->invent;
	if(back==t)
	  remove_node(back->right);
	else
	  remove_node(back->left);
  }
}
/*private function to do print items*/
void AVLTree::print(ostream& out_a,Node* p)
{
  if(p!=NULL)
  {
    print(out_a,p->left);
	out_a <<p->invent<<endl;
	print(out_a,p->right);
  }
}
ostream& operator<< (ostream& out_a, AVLTree& a)
{
  a.print(out_a,a.root);
  return out_a;
}