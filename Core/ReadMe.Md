# 📦 Core Layer Documentation (.NET 10 + CQRS + MediatR + Vertical Slice)

This document describes the structure and purpose of the **Core** layer in a clean, modular .NET 10 project following **CQRS**, **MediatR**, and **Vertical Slice Architecture** using **generic components**.

---

## 🎯 Purpose

The `Core` layer serves as the foundation of the application, offering **shared logic**, **enums**, **pagination**, **authentication helpers**, and **extensions** used across features and layers (like Application, API, etc).

---

## 📁 Folder Structure

Core
│
├── Base
│ ├── EF # Base entities and interfaces (e.g. IBaseEntity)
│ └── Auth # Auth constants (claim keys, secret key access)
│
├── Enums # Shared enums like SortByEnum
│
├── ExtensionAndSort # IQueryable sorting extensions
│
├── JWT # JWT helper, parser, and token service classes
│
├── Pagination # PaginationFilter, PaginatedList, and helpers
│
└── Utils # General purpose utility classes

markdown
Copy
Edit

---

## 🧩 Key Components

### ✅ Base.EF

- `IBaseEntity`: A base entity interface containing common properties like:
  - `id`
  - `name`, `slug`
  - `created_at`, `updated_at`
- Used in generic sorting and filtering mechanisms.

---

### ✅ Enums

- `SortByEnum`: Enum defining common sort options such as:
  - `Name`
  - `Created_At`
  - `Updated_At`

Used for dynamic sorting in queries.

---

### ✅ ExtensionAndSort

- `ApplySort<T>`: Extension method for `IQueryable<T>` to apply sorting dynamically using `SortByEnum`.
- Generic and compatible with `IBaseEntity`.

---

### ✅ JWT Components

#### 🔐 JwtHelper

- `CreateJwtAccessToken` / `CreateJwtRefreshToken`
- `Validate` token signature and expiry
- `GetPayload` and `GetUsername` from token

#### 🔍 JwtParser

- `ParseClaimsFromJwt(string token)`: Extracts claims from JWT payload.
- Supports role extraction as single string or JSON array.

#### 🔄 JwtTokenService

- `GenerateToken(User, List<string> roles)`
- `GenerateRefreshToken()`
- `GetPrincipalFromExpiredToken(string token)`
- `GetTokenExpiryMinutes(string token)`
- Central service for issuing and validating tokens.

---

### ✅ Pagination

#### 📄 PaginationFilter

- Base filter with:
  - `Page`, `PageSize`
  - `SortByEnum`
- Enforces valid limits (`MaxPageSize = 200`)

#### 🔍 DefaultPaginationFilter

- Extended filter with:
  - `SearchTerm` (string)
  - `BoolFilter` (bool?)
  - `Min`, `Max` (double?)
  - `SortBy`

Useful for grid/table views in front-end.

#### 📦 PaginatedList<T>

- Standard paginated result wrapper:
  - `Page`, `PageSize`, `TotalCount`
  - `CurrentPage`, `TotalPages`
  - `Data: List<T>`
  - `HasNext`, `HasPrevious`

Used in responses of list-based APIs.

---

## 📐 Vertical Slice Integration

The `Core` layer enables each feature slice (vertical slice) to work independently with shared utilities:

- **JWT** utilities for secure access across features
- **Pagination** for any list/query feature
- **Sorting** with generic support for entities
- **Enums** for standardized client-server interactions

It avoids duplication and encourages reuse across Command and Query handlers.

---

## ⚙️ Dependencies

- `.NET 10`
- `MediatR`
- `System.IdentityModel.Tokens.Jwt`
- `System.Text.Json`
- `LINQ`

No dependency on infrastructure or database layers.

---

## ✅ Benefits

- High reusability
- Clean separation of concerns
- Easily testable components
- Ready for feature-first (slice-based) development
- Strong typing with shared contracts

---

## 🧠 Developer Notes

- Only add to `Core` what is truly **shared** and **generic**.
- Keep this layer **infrastructure-agnostic**.
- Avoid placing any service that depends on external resources here.

---

## 📎 License

This documentation is free to use in any .NET 10/CQRS-based projects.

---
