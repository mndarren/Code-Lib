# Performance Focus
========================================
1. spot
2. Union operator
   ```
   >>> a = set('abcde')
   >>> l = ['f', 'g']
   >>> a |= set(l)
   >>> a
   set(['a', 'c', 'b', 'e', 'd', 'g', 'f'])
   ```
