# Entity-Relationship-Model
=========================================
0. Goal:  to arrange the data in such a form that it is consistent, 
		  non-redundant and supports operations for data manipulation.
1. Data Nomalization Rules
	```
	1) Eliminate Repeating groups (1NF, first Normal Form, no repeating groups of data)
		An attribute (column) of a table cannot hold multiple values.
	2) Eliminate Redundant data (2NF, non-key attributes fully dependent on its PK)
		No non-prime attribute is dependent on the proper subset of any candidate key of table.
	3) Eliminate Columns Not Dependent on Key (3NF, directly dependent on PK)
		No Transitive functional dependency of non-prime attribute.
		"Each attribute must be a fact about the key, the whole key, and nothing but the key."
	4) Isolate Independent Multiple Relationships
	5) Isolate Semantically Related Multiple Relationships
	```
2. Terms
	```
	ANSI --American National Standard Institute
	OLAP --Online Analytical Processing
	OLTP --Online Transactional Processing
	POC  --Proof of Concept
	```