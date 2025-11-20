# Reward Flow v2 - Project Context Analysis

## Project Overview

**Reward Flow v2** is an employee reward management system designed for educational institutions (universities/colleges). The system manages employee compensation calculations based on teaching sessions, handles employee data with advanced search capabilities, and provides user authentication with role-based access control. The project follows a rapid MVP development approach while maintaining scalability for future enhancements.

## Architecture

### Core Architecture Pattern
- **Vertical Slices Architecture**: Adopted instead of traditional Clean Architecture or DDD for faster MVP development
- **Self-Contained Operations Pattern**: Each business operation handles its own persistence automatically, eliminating manual save coordination
- **Namespace-based Domain Separation**: Three main domains (Employees, Rewards, User) with internal entity access restrictions

### System Layers & Data Flow
```
API Endpoints → Business Logic (Vertical Slices) → Data Access → SQL Server Database
     ↓
JWT Authentication → Authorization → Domain Services → Entity Framework Core
```

### Key Architectural Decisions
1. **Single Project Structure**: All components in one project with namespace separation
2. **Unified Database Schema**: Single database with multiple DbContexts for domain separation
3. **DbContext Factory Pattern**: Used for error recovery and connection management
4. **Cross-Slice Communication**: Shared Employee Lookup Service for inter-domain operations

## Technologies & Tools

### Core Framework
- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API development
- **Entity Framework Core 9.0.7** - ORM with SQL Server provider

### Database & Storage
- **SQL Server** - Primary database (LocalDB for development)
- **Entity Framework Migrations** - Database schema management
- **ON DELETE CASCADE** - Automatic cleanup of related records

### Authentication & Security
- **JWT Bearer Authentication** - Token-based authentication
- **AES Encryption Service** - Data encryption for sensitive fields
- **Role-based Authorization** - User roles (Admin, User) with plan-based access

### Validation & Error Handling
- **FluentValidation 12.0.0** - Request validation
- **FluentResults.Extensions.AspNetCore** - Result pattern implementation
- **Global Exception Handler** - Centralized error handling

### Search & Performance
- **N-gram Tokenization** - Advanced fuzzy search for employee names
- **xxHash.NET** - High-performance hashing for tokenization
- **Bulk Operations** - Optimized database operations for large datasets

### API Documentation
- **Scalar.AspNetCore** - Modern API documentation and testing interface
- **OpenAPI/Swagger** - API specification and documentation

## Core Features

### Employee Management
- **CRUD Operations**: Create, read, update, delete employees
- **Advanced Search**: Fuzzy name search using N-gram tokenization with Levenshtein distance
- **Bulk Import**: Efficient bulk employee data insertion
- **Data Encryption**: Sensitive fields (National Number, Account Number) encrypted at rest
- **Organizational Structure**: Faculty, Department, Job Title hierarchies

### Reward System
- **Session-based Rewards**: Calculate rewards based on teaching sessions
- **Flexible Calculation**: Configurable percentage-based reward calculation
- **Employee-Subject Assignment**: Assign employees to subjects with session tracking
- **Automatic Total Updates**: Real-time reward total recalculation
- **Business Rules Engine**: Configurable validation rules for reward calculations

### User Management & Authentication
- **JWT Authentication**: Secure token-based authentication with refresh tokens
- **User Registration/Login**: Complete authentication flow
- **Role-based Access**: Admin and User roles with different permissions
- **Plan Management**: Free and premium plan support
- **Profile Management**: User profile with picture support

### Data Integrity & Performance
- **Transaction Management**: Atomic operations with proper rollback
- **Optimistic Concurrency**: Prevent data conflicts in multi-user scenarios
- **Connection Pooling**: Efficient database connection management
- **Lazy Loading**: Optimized data loading strategies

## Design Decisions

### 1. Vertical Slices Over Layered Architecture
**Rationale**: Enables faster MVP development with loosely coupled features that can scale independently without requiring complete architectural overhaul.

### 2. Self-Contained Operations Pattern
**Rationale**: Eliminates transaction management complexity and tight coupling by having each business method handle its own persistence automatically.

### 3. N-gram Tokenization for Employee Search
**Rationale**: Provides superior fuzzy search capabilities compared to traditional LIKE queries, essential for handling Arabic names and typos in employee data.

### 4. Unified Database with Multiple DbContexts
**Rationale**: Maintains data consistency while providing domain separation and enabling independent scaling of different business areas.

### 5. AES Encryption for Sensitive Data
**Rationale**: Ensures compliance with data protection requirements while maintaining query performance through interceptor-based encryption/decryption.

## Dependencies and Integrations

### External Dependencies
- **Microsoft.EntityFrameworkCore.SqlServer** - Database connectivity
- **Microsoft.AspNetCore.Authentication.JwtBearer** - JWT authentication
- **System.IdentityModel.Tokens.Jwt** - JWT token handling
- **FluentValidation** - Input validation
- **xxHash.NET** - High-performance hashing

### Internal Service Dependencies
- **IEmployeeLookupService** - Cross-domain employee data access
- **ITokenizer** - N-gram tokenization service
- **IAesEncryptionService** - Data encryption/decryption
- **IUserIdRetrievalService** - User context management
- **ISessionRewardCalculator** - Reward calculation logic

### Database Integrations
- **Multiple DbContexts**: UserDbContext, EmployeeDbContext, RewardDbContext
- **Entity Relationships**: Complex relationships with cascade deletes
- **Migration Management**: Separate migration paths for different contexts

## Known Limitations or Potential Improvements

### Current Limitations
1. **Single Database Dependency**: No distributed database support, potential single point of failure
2. **Limited Scalability**: Vertical slices may require refactoring to Clean Architecture for complex scenarios
3. **Manual PDF Generation**: PDF and Excel export features not yet implemented
4. **Basic Error Logging**: Limited observability and monitoring capabilities
5. **Hardcoded Business Rules**: Reward calculation rules are not externally configurable

### Scalability Concerns
1. **N-gram Token Storage**: May require optimization for very large employee datasets
2. **Synchronous Operations**: Some operations could benefit from async processing
3. **Memory Usage**: Bulk operations may consume significant memory for large datasets

### Security Improvements Needed
1. **Rate Limiting**: No API rate limiting implemented
2. **Audit Logging**: Limited audit trail for sensitive operations
3. **Input Sanitization**: Additional XSS and injection protection needed
4. **Encryption Key Management**: Static encryption keys should be externalized

### Performance Optimizations
1. **Caching Strategy**: No caching layer for frequently accessed data
2. **Database Indexing**: May need additional indexes for complex queries
3. **Connection Pooling**: Could be optimized for high-concurrency scenarios

## Suggested Questions to Ask Next

### Architecture & Design
- "How should we implement caching for employee lookup operations?"
- "What's the strategy for migrating to microservices if needed?"
- "How can we make business rules externally configurable?"

### Security & Compliance
- "How should we implement comprehensive audit logging?"
- "What's the plan for encryption key rotation and management?"
- "How can we add rate limiting and DDoS protection?"

### Performance & Scalability
- "What's the expected load and how should we optimize for it?"
- "How can we implement background processing for heavy operations?"
- "What monitoring and alerting should be implemented?"

### Feature Development
- "How should PDF generation and reporting be implemented?"
- "What additional reward calculation types are needed?"
- "How can we add real-time notifications for reward updates?"

### Data Management
- "What's the data retention and archival strategy?"
- "How should we handle data migration from legacy systems?"
- "What backup and disaster recovery procedures are needed?"

### Integration & APIs
- "What external systems need to integrate with this API?"
- "How should we implement webhook notifications?"
- "What's the plan for API versioning and backward compatibility?"

---

*This analysis provides a comprehensive overview of the Reward Flow v2 system architecture, design decisions, and potential areas for improvement. Use this context to understand the system's current state and guide future development decisions.*