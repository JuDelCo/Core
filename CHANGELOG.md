
# Changelog

## [1.37.0] - 2021-05-24

### Added

- Add csproj file to allow compiling and using the library for native NET projects.

### Fixed

- Remove all NET Reflection usage.

### Improved

- Remove usage of NET Reflection (Activator.CreateInstance) in ServiceContainer and CacheService classes.
- Improve Hjson coding convention.
- Update VSCode settings.json file to handle generated folders by dotnet compiler.
- Update .editorconfig to include generated folders by dotnet compiler.
- Update .gitignore to exclude generated folders by dotnet compiler.

## [1.36.0] - 2021-05-21

### Added

- Add maxDepth parameter to the ToHjson/ToJson methods for JNode.
- Add maxDepth parameter to the ToString method for JNode.
- Add generic JList<T> class.
- Add new GetFriendlyName extension method for Type.

### Improved

- Separate all IService, State and Behaviour extensions in different files.
- Improve type naming in all log messages.
- Update README.

## [1.35.0] - 2021-05-17

### Added

- Add new Data feature (JNode, JDict, JList, JData, JRef classes).
- Add extensions methods for the new Data feature.
- Add customized Hjson/Json library.
- Add Hjson/Json extension methods.
- Add new Cast class to convert the type of arbitrary data without throwing exceptions.
- Add new DataTypeConverter class to convert the type of arbitrary data.
- Add several new constructors to Clock, Timer and FrameTimer classes.
- Add new Reset method to ITimer interface.
- Add new extension methods for IDictionary.

### Changed

- Rename DataService to CacheService.
- Rename some extension methods static classes.
- Move observable extension methods to Ju.Observables namespace.
- Move GamepadController class to the Ju.Input namespace.
- Move KeyboardController class to the Ju.Input namespace.
- Move MouseController class to the Ju.Input namespace.
- Move InputAction class to the Ju.Input namespace.
- Move InputPlayer class to the Ju.Input namespace.

### Fixed

- Fix LogUnityService error when logging messages with curly braces.
- Fix Observable Value set property.

## [1.34.0] - 2021-04-12

### Fixed

- Fix InputService load service error.
- Fix EventBus stack overflow bug when a subscriber action exception is raised and other cases.
- Fix Input delta mouse precision when using both providers from Unity.
- Fix Unity service callback bug when trying to close the app.
- Fix Stop method in timers.

### Improved

- Add new internal service cache for all internal service usage.

[1.37.0]: https://github.com/JuDelCo/Core/compare/v1.36.0...v1.37.0
[1.36.0]: https://github.com/JuDelCo/Core/compare/v1.35.0...v1.36.0
[1.35.0]: https://github.com/JuDelCo/Core/compare/v1.34.0...v1.35.0
[1.34.0]: https://github.com/JuDelCo/Core/compare/v1.33.0...v1.34.0
