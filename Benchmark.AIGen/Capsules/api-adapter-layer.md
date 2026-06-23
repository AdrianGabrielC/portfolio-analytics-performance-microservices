# API Adapter Layer Rules

Benchmark.Api is an adapter layer.

Allowed:
- Accept HTTP requests.
- Bind DTOs.
- Map DTOs to command/query options.
- Create command/query objects.
- Send through IMediator.
- Return HTTP responses.

Forbidden:
- Business logic.
- Repository access.
- DbContext access.
- Infrastructure access.
- Domain entity creation.
- Handler invocation directly.

Required flow:
Request DTO
-> CommandOptions or QueryOptions
-> Command or Query
-> IMediator.Send
-> ActionResult