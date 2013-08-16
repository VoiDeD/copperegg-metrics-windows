CopperEgg Metrics (Windows)
---

Windows service designed to collect custom metrics for CopperEgg monitoring.

This project is still heavily WIP, so your mileage may vary!


## Metrics Collected

- MySQL
  - Connected and running threads
  - Current, total, and peak connections
  - Queries
  - Slow queries
  - INSERT, SELECT, and UPDATE commands
  - Uptime
  - Full table scan joins
  - Data sent/received
- .NET
  - Exceptions thrown
  - Interop: Params/returns marshalled
  - Memory: # GC handles
  - Memory: Allocated bytes/sec
  - Memory: Large object heap size
  - Networking: bytes sent/received
  - Networking: Connections established


## Requirements

- .NET 4.5


## Dependencies

- Json.NET

When building with Visual Studio, dependencies should be resolved via NuGet.
