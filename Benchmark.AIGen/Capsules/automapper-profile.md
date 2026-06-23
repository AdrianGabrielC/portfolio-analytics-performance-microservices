# AutoMapper Profile Rules

Each request DTO must have an explicit mapping to the corresponding CommandOptions or QueryOptions type.

Profiles:
- Must inherit from AutoMapper.Profile.
- Must be sealed.
- Must live under Benchmark.Api.Profiles.
- Must contain only mapping configuration.