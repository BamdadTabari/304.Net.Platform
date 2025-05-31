# 304 – Generic CQRS Vertical Slice Base Project for .NET

Welcome to **304**, a modular and extensible base project for building modern .NET applications using the **CQRS (Command Query Responsibility Segregation)** pattern with a **Vertical Slice Architecture**.

🎯 Designed for clarity, scalability, and testability  
💡 Built using **generic handlers**, **shared abstractions**, and **clean slice separation**  
🧪 Includes **generic tests** with a working sample: a simple blog system

---

## 🔧 Technologies & Patterns

- **.NET 10**
- **CQRS Pattern** with MediatR
- **Vertical Slice Architecture**
- **Minimal Dependencies**
- **Generic Command/Query/Response Handlers**
- **FluentValidation**
- **Entity Framework Core**
- **xUnit + Moq** for testing

---

## 🚀 Features

- ✅ Generic command, query, and response structure
- ✅ DRY principles via reusable handler base classes
- ✅ Clean vertical slice folders per use-case
- ✅ A simple blog sample demonstrating real usage
- ✅ Generic unit/integration test base with examples
- ✅ Clear and extensible project structure for growing apps

---

🧠 Why Vertical Slice Architecture?
Instead of organizing by technical concerns (Controllers, Services, etc.), Vertical Slice breaks features into isolated, end-to-end units of functionality. This means each feature lives in its own folder and includes:

Command or Query

Handler

Validation

Response

Tests

🔁 Easy to extend
📦 Minimal coupling
🔍 Better separation of concerns

----


🧪 Testing
304 includes a reusable test foundation with:

Generic test helpers

In-memory database setup

Sample test cases for blog operations

----

git clone https://github.com/yourusername/304.git
cd 304
dotnet restore
dotnet run

----

🛠 Customize for Your Project
Replace the Blog slice with your own features. Follow the same structure to:

Define Command/Query

Create a Handler inheriting from generic base

Add validators if needed

Write tests using the provided base helpers

🤝 Contributions
Feel free to fork and adapt this project. PRs are welcome!

