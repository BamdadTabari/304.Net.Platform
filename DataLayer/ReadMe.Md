# Data Layer Documentation

## Overview

The Data Layer is responsible for managing data access and manipulation in the application. It abstracts the interaction with the database using Entity Framework Core and provides generic repository patterns to handle CRUD operations, queries, and pagination efficiently.

This layer exposes a generic repository interface and its implementation to enable reusability and maintainability across different entity types.

---

## Repository Interface: `IRepository<TEntity>`

`IRepository<TEntity>` is a generic interface defining common data access methods. It works with any entity type implementing the `IBaseEntity` interface.

### Key Methods:

- **Existence Checks**
  - `Task<bool> ExistsAsync()`: Checks if any records exist.
  - `Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate)`: Checks if any records satisfy the given condition.
  - `Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)`: Async existence check with condition.

- **Count**
  - `int Count()`: Counts all records.
  - `int Count(Expression<Func<TEntity, bool>> predicate)`: Counts records matching a condition.

- **Add/Update/Delete**
  - Synchronous methods:
    - `void Add(TEntity entity)`
    - `void AddRange(IEnumerable<TEntity> entities)`
    - `void Remove(TEntity entity)`
    - `void RemoveRange(IEnumerable<TEntity> entities)`
    - `void Update(TEntity entity)`
    - `void UpdateRange(IEnumerable<TEntity> entities)`
  - Asynchronous methods:
    - `Task AddAsync(TEntity entity)`
    - `Task AddRangeAsync(IEnumerable<TEntity> entities)`
    - `Task<TEntity> AddAsyncReturnId(TEntity entity)`

- **Find Methods**
  - `Task<TEntity?> FindSingle(Expression<Func<TEntity, bool>> predicate, params string[]? includes)`
  - `Task<TEntity?> FindFirst(Expression<Func<TEntity, bool>> predicate, params string[]? includes)`
  - `List<TEntity> FindList(Expression<Func<TEntity, bool>> predicate, params string[]? includes)`
  - `List<TEntity> FindList(params string[]? includes)`
  - AsNoTracking versions for optimized read-only queries:
    - `Task<TEntity?> FindSingleAsNoTracking(...)`
    - `Task<TEntity?> FindFirstAsNoTracking(...)`
    - `List<TEntity> FindListAsNoTracking(...)`

- **Pagination**
  - `Task<PaginatedList<TEntity>> GetPagedResultAsync(DefaultPaginationFilter filter, Expression<Func<TEntity, bool>>? predicate, params string[]? includes)`
  - Supports filtering, sorting, and pagination for large datasets.

---

## Repository Implementation: `Repository<TEntity>`

The concrete class `Repository<TEntity>` implements `IRepository<TEntity>` using EF Core's `DbContext`:

- **DbContext Usage**  
  The repository works on `DbContext.Set<TEntity>()` to query and modify data.

- **Include Support**  
  The repository supports eager loading through string-based navigation properties passed as `includes`.

- **Sync and Async Support**  
  Both synchronous and asynchronous methods are provided, with async preferred for database calls.

- **Pagination and Sorting**  
  The repository supports generic filtering and sorting based on `DefaultPaginationFilter` and works seamlessly with entities implementing `IBaseEntity`.

- **NoTracking Queries**  
  Methods with `AsNoTracking()` optimize read-only queries by disabling EF Core change tracking.

---

## Usage Example

```csharp
public class ProductService
{
    private readonly IRepository<Product> _repository;

    public ProductService(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<List<Product>> GetActiveProductsAsync()
    {
        return await _repository.FindListAsNoTracking(p => p.IsActive);
    }

    public async Task<PaginatedList<Product>> GetPagedProductsAsync(DefaultPaginationFilter filter)
    {
        return await _repository.GetPagedResultAsync(filter, p => p.IsActive);
    }

    public async Task AddProductAsync(Product product)
    {
        await _repository.AddAsync(product);
        // Remember to save changes in DbContext outside repository if needed
    }
}
