# Database Schema Design Decision
---
Date: 6-8-2025

###  Problem
---

We need an approach to minimize database manual definition and make Entity framework handle it all including foreign keys following Decision [[05 Use Unified Database schema]]. to

### Decision
---

We will override decision [[03 Use Namespaces per Subdomain with Internal Entity Access]] and make an exception for **DbContexts**

**The new rules is**

1. DbContext definition can access any other entity by reference.
2. Entities won't having navigation prosperities.
3. The namespace separation rule still applies for every thing exception DbContext configuration

### Consequences/Implications
---

- Consistency in database definition avoiding inconsistent migration with database definition by avoiding manual setup
	  
- Allows adding foreign Keys easely to follow Decision [[05 Use Unified Database schema]]
	  
- Keeps code restrictions for loosing coupling of the project as mentioned in [[03 Use Namespaces per Subdomain with Internal Entity Access]], [[04 User Vertical Slices Architecture]]