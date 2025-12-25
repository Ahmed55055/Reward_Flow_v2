# 15. Use Hashed Columns for Duplicate Validation with Encrypted Data

Date: 2024-12-25

## Context

The Employee entity stores sensitive data (NationalNumber and AccountNumber) using encryption with salt for security reasons. However, this creates a challenge for:

1. **Duplicate Validation**: We need to prevent duplicate NationalNumber and AccountNumber entries during employee insertion
2. **Search Operations**: We need to search employees by NationalNumber efficiently
3. **Security Requirements**: We cannot compromise on encryption security by switching to deterministic encryption or removing salt

The current implementation in `CreateEmployee.cs` performs duplicate validation by comparing encrypted values directly, but this approach has limitations:
- Salt-based encryption produces different encrypted values for the same input
- Database queries cannot efficiently match encrypted values
- Search operations become complex and inefficient

## Decision

We will introduce two additional columns to the Employee entity to store one-way hashed values:

- `NationalNumberHash`: SHA-256 hash of the NationalNumber
- `AccountNumberHash`: SHA-256 hash of the AccountNumber (nullable)

These hashed columns will be used exclusively for:
1. Duplicate validation during insertion/updates (per user scope)
2. Search operations by NationalNumber
3. Database indexing for performance

**Important**: Duplicates are allowed across different users but not within the same user's data. The unique constraints will be composite: (CreatedBy, NationalNumberHash) and (CreatedBy, AccountNumberHash).

## Consequences

### Positive

- **Security Maintained**: Original encrypted data remains secure with salt-based encryption
- **Efficient Validation**: Hash-based duplicate checking is fast and reliable
- **Database Performance**: Unique indexes on hash columns provide O(1) lookup performance
- **Search Capability**: NationalNumber search becomes efficient using hash comparison
- **No Pattern Detection**: One-way hashing prevents pattern analysis of sensitive data

### Negative

- **Storage Overhead**: Additional 64-128 bytes per employee record
- **Hash Collisions**: Theoretical risk of SHA-256 collisions (extremely low probability)
- **Migration Complexity**: Existing data needs hash generation during migration
- **Code Complexity**: Additional hash generation logic in CRUD operations

### Neutral

- **Consistency**: Hash generation must be consistent across all operations
- **Maintenance**: Hash columns must be updated whenever source data changes
