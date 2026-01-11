# Rate Limiter Service

## Project Overview

This project implements a lightweight, in-memory rate limiting service using **ASP.NET Core (.NET 10)**.  
The service determines whether an incoming request from a specific identifier (for example, a `userId` or API key) should be **allowed or rejected** based on configurable rate-limiting rules.

---

## Setup and Execution

### Prerequisites
- .NET 10 SDK
- Visual Studio 2022 or later

### Running the Service
1. Open the solution in Visual Studio
2. Set `RateLimiter.Api` as the startup project
3. Press **F5** or click **Run**

The Swagger UI will be available at: https://localhost:5503/swagger/index.html

### Running Tests
```bash
dotnet test
```

---

## API Documentation

### POST `/check`

#### Request
```json
{
  "identifier": "user-123"
}
```

#### Responses
- **200 OK** - Request is within the allowed rate limit  
- **429 Too Many Requests** - Rate limit exceeded  
- **400 Bad Request** - Identifier is missing or invalid  

---

## Design Decisions & Trade-offs

### Rate Limiting Algorithm

The service uses a **Fixed Window Counter** approach implemented with time-based buckets.
Each incoming request increments a counter associated with a unique identifier and the
current fixed time window.

#### Why this algorithm?
- Simple and efficient with constant-time operations
- Low memory overhead compared to sliding window logs
- Easy to adapt to distributed stores such as Redis using atomic `INCR` and `EXPIRE`
- Suitable for in-memory and demonstration use cases

---

### Code Structure

The application is structured into clear, well-defined layers:

- **Controllers** - Handle HTTP concerns only
- **Services** - Contain rate-limiting business logic
- **Stores** - Abstract the underlying storage mechanism
- **Options** - Define configurable rate-limiting rules

This separation of concerns ensures the system is **testable, maintainable, and extensible**.

---

### Configuration Strategy

Rate-limiting rules are defined in `appsettings.json` and consumed via `IOptionsMonitor<T>`.  
This allows rate-limit values to be updated **at runtime without restarting the service**, improving operational flexibility.

---

### Storage Abstraction

The rate limiter depends on an `IRateLimitStore` interface.  
The current implementation uses an **in-memory, thread-safe store** based on `ConcurrentDictionary`.

Because the service logic depends only on the abstraction, the store can be replaced with **Redis** with minimal changes to the codebase.

---

### Testing Approach

Unit tests focus on the **core rate-limiting logic** rather than controller behavior.  
External dependencies such as `IOptionsMonitor<T>` are mocked using **Moq**, ensuring that tests are:

- Fast
- Deterministic
- Isolated from infrastructure concerns

---

### Distributed Systems Considerations

In a distributed environment with multiple service instances behind a load balancer, the in-memory store would be replaced with a centralized data store such as **Redis**.  
Redis atomic operations would ensure consistent rate-limiting behavior across all instances.

---

## Future Improvements

Given more time, the following enhancements could be added:

- Support for per-user or per-group rate-limiting rules
- Automatic cleanup of expired counters
- Exposing metrics for allowed and rejected requests
- Containerization using Docker
