# 2. Use One Project

**Date:** 4-8-2025

## **Context**
---
This is the second attempt at building this system, this time following  **evolutionary architecture** methodology. The previous project version attempted to predefine a large-scale architecture from the beginning. However, it failed to deliver an MVP in a timely manner and could not scale individual components independently, making iteration and adaptation difficult. This experience motivated a more incremental and modular approach.

## **Problem**  
---
We need to determine the most suitable solution structure for the initial phase of our project.

## **Decision**  
---
We will employ a single project structure for production code, along with two separate test projects for integration and unit tests.

## **Consequences**
---
- The straightforward organization enables team members to quickly familiarize themselves with the codebase.
    
- By simplifying the solution architecture at the outset, the team can prioritize delivering new features over addressing architectural concerns.
    
- This approach allows us to make architectural pattern decisions for each module as needed, responding to real requirements instead of preemptively committing to specific patterns.
    
- If not managed properly, the single project structure could become cluttered and disorganized, necessitating timely extraction into separate projects.
    
- As the project grows, managing and maintaining a single project may become increasingly complex, potentially requiring a transition to a more modular structure.