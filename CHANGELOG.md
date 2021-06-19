
# Changelog

## [1.39.0] - 2021-06-19

### Added

- Add Unity Hot Reloading compatibility.
- Add Unity custom property drawers for Color and Color32 structs.
- Add Unity custom property drawers for TimeSince and TimeUntil structs.
- Add new extension methods WithRed, WithGreen, WithBlue and WithAlpha to Color and Color32 structs.
- Add new SerializableDictionary classes.
- Add new SerializableType struct.

### Changed

- BehaviourLinkHandlers will now return true in IsDestroyed property if alwaysActive is false and the Behaviour is disabled.
- Remove redundant extension methods for FSM class.
- Move SimpleFSM to FSM folder.

### Fixed

- Fix StopCurrentEventPropagation method for EventBus service.

### Improved

- Rewrited SimpleFSM class.
- Move UnityQuit and UnityQuitRequested events to native Core.
- Make all Color structs serializable.
- Make Span, TimeSince, TimeUntil structs serializable.

## [1.38.0] - 2021-05-31

### Added

- Add new TimeSince, TimeSinceUnscaled, TimeUntil and TimeUntilUnscaled structs.
- Add new string extension method to remove invisible characters.
- Add new string extension method to sanitize paths removing path/file invalid characters.
- Add new extension methods for Int values (IsWithin and IsBetween).
- Add new Base64 extension methods for string.
- Add new MD5 extension methods for string and byte arrays.
- Add new Clamp01 extension method to Color.
- Add new Cast method to get a formatted size string.
- Add new Cast methods to get a formatted time from int or long values.
- Add new constructor to ObjectPool with desired capacity.
- Add new Swap method to JDict and JList classes.
- Add new JNode related extension methods for IService, State and Behaviour.
- Add new helper GetDataClass methods to initialize class based JData nodes in JDict nodes.
- Add new JNodeLinkHandler class.

### Changed

- Change State default Condition return value to false.
- Remove default clamping to Color struct to allow HDR colors.

### Fixed

- Fix Unity Input Manager service provider error on MacOS.
- Fix JData Bind methods when node ref is null.
- Fix JNode missing events on Detach.

### Improved

- Add IEquatable interface to all structs.
- JNode subscribe events now have event type info (Add, ValueChange, Move, Remove, Clear).
- Remove the need to use the Value property when using JData nodes as values.
- Improve usage of generic JList nodes on foreach iterations or when using the indexer operator.
- Reduced (or removed in some scenarios) the GC allocations when using JNodes.

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

[1.39.0]: https://github.com/JuDelCo/Core/compare/v1.38.0...v1.39.0
[1.38.0]: https://github.com/JuDelCo/Core/compare/v1.37.0...v1.38.0
[1.37.0]: https://github.com/JuDelCo/Core/compare/v1.36.0...v1.37.0
[1.36.0]: https://github.com/JuDelCo/Core/compare/v1.35.0...v1.36.0
[1.35.0]: https://github.com/JuDelCo/Core/compare/v1.34.0...v1.35.0
[1.34.0]: https://github.com/JuDelCo/Core/compare/v1.33.0...v1.34.0
