# Database Employee Search By Name Decision
---
Date: 8-8-2025

###  Problem
---

We need to determine an effective way to fuzzy search employees by name ***Get Similar Employees By Name*** and calculate the trades off of each approach

The choices is Levenshtein vs n-gram indexing

### Decision
---

Based on the two approaches trades off as discussed in [[Levenshtein Distance And N-gram Analysis]] and the level of data is stored and the needed search speed **500ms** maximum for the response we decided to use n-gram as the main fuzzy search approach

### Consequences/Implications
---

- Higher memory usage per user, **3.36Mb** 
- extremely faster matching avoiding full table scan or run an algorithm into each record
- 