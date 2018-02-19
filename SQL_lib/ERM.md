# Entity-Relationship-Model
=========================================
0. Goal:  to arrange the data in such a form that it is consistent, 
		  non-redundant and supports operations for data manipulation.
1. Data Nomalization Rules
	```
	1) Eliminate Repeating groups (1NF, no repeating groups of data)
	2) Eliminate Redundant data (2NF, non-key attributes fully dependent on its PK)
	3) Eliminate Columns Not Dependent on Key (3NF, directly dependent on PK)
		"Each attribute must be a fact about the key, the whole key, and nothing but the key."
	4) Isolate Independent Multiple Relationships
	5) Isolate Semantically Related Multiple Relationships
	```
2. 