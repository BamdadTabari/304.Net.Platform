# Testing Layer Documentation

This document provides an overview and usage guide for the helper classes and methods used in unit testing the application’s handlers.

---

## Overview

To simplify and standardize unit testing, this project includes several helper classes that cover common test scenarios such as Create, Edit, Delete, GetAll, GetBySlug, and Pagination operations. These helpers manage mocks, setup, and assertions to reduce boilerplate code in tests.

---

## Helper Classes and Their Responsibilities

### 1. `MockUnitOfWorkFactory`

- Creates a mock `IUnitOfWork` instance with injected or default repository mocks.
- Automatically mocks the `CommitAsync` method to simulate transaction commit.
- Facilitates consistent setup for repository dependencies across tests.

---

### 2. `TestExpressionEvaluator`

- Provides utilities for comparing Expression<Func<TEntity, bool>> in tests.
- Supports checking if two expressions are logically equivalent by their string representation.
- Extracts and compares values from expression trees, e.g., matching a slug value.

---

### 3. `DeleteHandlerTestHelper`

- Contains test helpers for Delete operations:
  - `TestDelete`: Tests successful deletion including verifying `Remove` and `CommitAsync` calls.
  - `TestDeleteNotFound`: Tests behavior when the entity to delete is not found (should return failure).
- Automates mock setups and common assertions related to delete handlers.

---

### 4. `EditHandlerTestHelper`

- Provides reusable test methods for Edit operations:
  - `TestEditSuccess`: Verifies entity retrieval, update logic, commit success, and expected property changes.
  - `TestEditNotFound`: Validates the handler’s response when the entity to edit does not exist.
  - `TestEditCommitFail`: Checks handler behavior when the commit operation fails.
- Reduces repetitive mock and assert code for editing scenarios.

---

### 5. `GetAllHandlerTestHelper`

- Assists testing “Get All” handlers:
  - `TestHandle_Success`: Tests successful retrieval of entity lists with mocked repository data.
  - `TestHandle_FailOnException`: Verifies handler’s error response when repository throws an exception.
- Ensures handlers correctly return data or handle errors.

---

### 6. `GetBySlugHandlerTestHelper`

- Contains helpers for GetBySlug query handlers:
  - `TestGetBySlug_Success`: Tests fetching a single entity by slug including optional include parameters.
  - `TestGetBySlug_NotFound`: Checks behavior when the entity is not found.
- Simplifies tests involving slug-based queries.

---

### 7. `GetPaginatedHandlerTestHelper`

- Helps test paginated list retrieval handlers:
  - `TestPaginated_Success`: Verifies that paginated results are returned correctly with expected count and includes.
- Streamlines testing of pagination logic.

---

## Usage

- Inject the required mocks and handlers using the provided factory methods.
- Use the helper methods to perform common assertions and mock setups.
- Customize mock behavior and assertions using optional parameters and callbacks.
- Focus on test logic instead of boilerplate code.

---

## Benefits

- Consistent and maintainable unit tests.
- Reduced duplication in test code.
- Clear separation of setup, execution, and assertion logic.
- Easier onboarding for new developers writing tests.

---

If you have questions or want to extend the helpers for new scenarios, please refer to the code comments or contact the development team.

