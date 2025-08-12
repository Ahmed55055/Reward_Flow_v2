# Database Schema Design Decision
---
Date: 10-8-2025

###  Problem
---

We need a faster way to handle foreign keys in Database and be able to handle millions of records with minimal space needed.

### Decision
---

The decision is to add new id as int primary key and the Guid/ UUID is still exsits in the database as indexed id for public use

The new id is the clustered indexed as it will be used for internal db use and backend if needed.

this decision has been taken after the [[07 Use N-gram To Fuzzy Search Employees Data]] decision. due to the potential millions of records carrying foreign key to the user table

### Consequences/Implications
---

- Reduce foreign keys space by 75% from 16 bytes of Unique identifier to 4 bytes int
	  
- Adds different id could cause security vulnerability if published publicly 
	  
- Adds 4 extra bytes to each user and 16 extra bytes for the Unique identifier index + 4 bytes as reference to the clustered index
	  
- may be confusing to new comer developing the system witch id to use