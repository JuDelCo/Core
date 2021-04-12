
# Changelog

## [1.34.0] - 2021-04-12

### Fixed

- Fix InputService load service error.
- Fix EventBus stack overflow bug when a subscriber action exception is raised and other cases.
- Fix Input delta mouse precision when using both providers from Unity.
- Fix Unity service callback bug when trying to close the app.
- Fix Stop method in timers.

### Improved

- Add new internal service cache for all internal service usage.

[1.34.0]: https://github.com/JuDelCo/Core/compare/v1.33.0...v1.34.0
