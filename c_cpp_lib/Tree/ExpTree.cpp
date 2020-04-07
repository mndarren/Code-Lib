/*@author Zhao Xie
  @date 12/17/2013
  @file ExpTree.cpp*/
  #include "ExpTree.h"
  void ExpTree::destroy(Node* r)
  {
    if(r!=NULL)
	{
	  destroy(r->left);
	  destroy(r->right);
	  delete r;
	}
  }
  ExpTree::Node* ExpTree::help_build
                 (istream& in_s)
{
  Node* temp;
  char ch;
  in_s >>ch;
  if (isdigit(ch))
  {
    temp = new Node;
	temp->oprd = ch-'0';
	temp->optr = 'a';
	temp->left = NULL;
	temp->right = NULL;
  }
  else //'('
  {
    temp = new Node;
	temp->left = help_build(in_s);
	in_s >> temp->optr;
	temp->right = help_build(in_s);
	in_s >>ch; //')'
  }
  return temp;
}
  void ExpTree::infix(Node* r)
 {
   if (r->left == NULL)
     cout <<r->oprd<<' ';
   else
   {
     cout<<'('<<' ';
	 infix(r->left);
	 cout<<r->optr<<' ';
	 infix(r->right);
	 cout<<')';
   }
 }
int ExpTree::compute(Node* r)				 
{
  if(r->left->optr=='a'&&
     r->right->optr=='a')
	 r->oprd = eval(r->left->oprd,r->optr,
	                r->right->oprd);
  else if(r->left->oprd=='a')
    r->oprd = eval(r->left->oprd,r->optr,
	               compute(r->right));
  else if(r->right->oprd=='a')
    r->oprd = eval(compute(r->left),r->optr,
	               r->right->oprd);
  else
    r->oprd = eval(compute(r->left),r->optr,
	               compute(r->right));
  return r->oprd;
}
int ExpTree::eval(int left,char op,int right)
{
  switch(op)
  {
    case '+':return left+right;
	case '-':return left-right;
	case '*':return left*right;
	case '/':return left/right;
  }
}