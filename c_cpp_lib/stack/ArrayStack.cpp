//CSCI591 (301) Sec1 <Zhao Xie>
//Project5 Due:Oct 17
/**Stack.cpp*/
#include "Stack.h"

void Stack::push(const Item& entry)
{
  if (used < MAX)
  {
    data[used] = entry;
    ++used;
  }
}
Stack::Item Stack::pop()
{
  if (!empty())
  {
    --used;
    return data[used];
  }
}

Stack::Item Stack::peek() const
{
  if (!empty())
    return data[used-1];
}