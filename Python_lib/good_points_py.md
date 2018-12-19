# Good Points Python
==========================================
1. De-dup List[Dict] 
```
[dict(t) for t in {tuple(d.items()) for d in rel_lsit}]
```
