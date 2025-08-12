# **Private User ID Retrieval Decision**  
---

**Date:** 12-8-2025

## **Problem**  
---

Database relations introduce tidy coupling in the application level witch is making development harder so we need to find an approach to reduce application coupling  

example of tidy coupling introduced
1. updating a column has a foreign key in another table this is night mare and need to update multiple files in sequence to work probably.
		
2. Delete user scenario: when deleting a user there is always related tables like employees and all related employees should be deleted, when new related table is introduced the employee deletion logic need to be updated violating Open closed principle

## **Decision**  
---

After going throw various solutions some was very good for scalable distributed system but this is a small system and wont be ever distributed.

And the most suited solution for this system is to keep the relations and Use `ON DELETE CASCADE` as it is the sweet spot between data integrity and consistency, and making deleting much easier and scalable if added new tables


## **Consequences/Implications**
---

- - Strong referential integrity enforced by the database.
    
- Simplified application logic for delete operations.
    
- Future tables can be linked without changing delete code (OCP preserved).
    
- Tight coupling at the DB level, acceptable for single-DB lifetime.
    
- No application-layer hooks during cascade deletes unless added manually.
    
- Large cascade deletes may impact performance if data volume is high.
    
- Risk of accidental mass deletion — may require delete safeguards.