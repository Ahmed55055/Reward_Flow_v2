# Use Namespaces per Subdomain with Internal Entity Access
---
**Date:** 4-8-2025

## **Problem**  
---
We need to define a scalable structure for implementing subdomains that balances modularity, maintainability, and the reality of entity relationships.

## **Decision**  
---
We will implement each subdomain as a separate namespace (module), where each namespace contains its own **DbContext** and a group of **tightly coupled entities**. This approach follows the principles of **evolutionary architecture**, informed by a previous system that failed to scale due to overly centralized design and the inability to deliver a focused MVP.

but instead of using a single context for the whole application or a strict one-entity-per-context design, we group related domain entities that are conceptually and relationally coupled within the same namespace and context. For example:

- **User Namespace**: includes `User`, `UserRole`, `Plan`
    
- **Employee Namespace**: includes `Employee`, `JobTitle`, `Department`, `EmployeeStatus`, etc.
    

Entities within a namespace can reference each other (e.g., via navigation properties or foreign keys), but **namespaces remain isolated**—no direct access to entities across namespaces is allowed.
If there is foreign key between **DbSets** in different namespace, will be by table name instead of object reference

This is a pragmatic compromise between theoretical DDD boundaries and practical system design, tailored to support scaling subdomains incrementally while respecting domain cohesion.

## **Consequences**
---

- Logical cohesion is preserved by grouping related entities based on real-world domain and database coupling.
    
- Namespace-based boundaries promote modularity without premature fragmentation.
    
- Each subdomain is internally consistent and can evolve independently.
    
- ORM mapping is simplified by aligning closely with natural data relationships.
    
- Future extraction to microservices or separate projects is facilitated by namespace isolation.
    
- Some trade-offs exist in theoretical purity, but these are deliberate and acceptable for maintainability and clarity.
    
- Developers must ensure strict boundary discipline to prevent leakage between namespaces.