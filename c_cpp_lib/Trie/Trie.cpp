//CSCI591(301) Project9
//Zhao Xie (Due 12/3/2013)
/**@file Trie.cpp*/
#include "Trie.h"

/*private function for copy constructor*/
Trie::Node* Trie::copy( Node* r)
{
  if(r==NULL)
    return NULL;
  else
  {
    Node* temp;
	temp = new Node;
	temp->count = r->count;
	for(int i=0;i<26;i++)
	  temp->children[i]=copy(r->children[i]);
	return temp;
  }
}
//private function to be called by destructor
void Trie::destroy(Node* r)
{
  if(r!=NULL)
  {
    for(int i=0; i<26;i++)
	  destroy(r->children[i]);
	delete r;
  }
}
/*private function to insert a new node*/
void Trie::r_insert(Node* &t,char w[],int pos,int& count)
{
  if (t== NULL)
  {
	t = new Node;
    t->count = 0;
    for(int i=0;i<26;++i)
      t->children[i]=NULL;
  }
  if(w[pos]=='\0')
    t->count = count;
  else
    r_insert(t->children[w[pos]-'A'],w,pos+1,count);
}

int Trie::r_count(Node* r) const
{
  int count=0;
  if (r==NULL)
    return 0;
  if(r->count)
    count=1;
  for(int i=0;i<26;++i)
    count += r_count(r->children[i]);
  return count;
}
/*private function to be called by get_count()*/
/*int Trie::r_get_count(Node* r, char w[]
                               ,int pos) const
{
  if(r==NULL)
    return 0;
  else if(w[pos]=='\0')
    return r->count;
  else
    return r_get_count(r->children[w[pos]-'A'],
	                    w,pos+1);
}*/

int Trie::get_count(char w[]) const
{
  Node* p=root;
  int pos=0;
  while(p!=NULL&&w[pos]!='\0')
    p=p->children[w[pos++]-'A'];
  if(p==NULL)
    return 0;
  else
    return p->count;
}

/*private function to be called by remove()*/
void Trie::help_remove(Node* &r,char w[],int pos)
{
  if (w[pos]=='\0')
    r->count = 0;
  else
    help_remove(r->children[w[pos]-'A'],w,pos+1);
}

/*private function to do print items*/
void Trie::print(ostream& out_c,Node* r,
                         LinkedStack<char>& s)
{
  if(r!=NULL)
  {
    if(r->count)
	{
 	   s.st_print(out_c);
	   out_c <<"  "<<r->count<<endl;
	}
	for(int i=0;i<26;++i)
	{
	    s.push(i+'A');
	    print(out_c,r->children[i],s);
	    s.pop();
	}
  }
}
ostream& operator<< (ostream& out_c, Trie c)
{
  LinkedStack<char> s;
  c.print(out_c,c.root,s);
  return out_c;
}
