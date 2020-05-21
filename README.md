Core
=====================

Core is a service locator, object factory and a collection of basic services to ease the development of C# applications or games.

This package will follow a [semantic versioning](http://semver.org/).

Any feedback is welcome !


See also
=====================

- [Core Math](https://github.com/JuDelCo/CoreMath) - Linear algebra math library, also 2D/3D physics and IK
- [Core ECS](https://github.com/JuDelCo/CoreECS) - Deterministic lightweight ECS framework


Install
=====================

If you are using Unity, update the dependencies in the ```/Packages/manifest.json``` file in your project folder with:

```
	"com.judelco.core": "https://github.com/JuDelCo/Core.git#v1.12.0",
```

otherwise, use this package as it is in native C# applications, it will work just fine.


Contents
=====================

#### Services

- ```LogService```
    * Handles all logging across all services and your code.
    * **Note**: This service is internally automatically registered for you.
- ```EventBusService```
    * Useful event bus to communicate your objects in a decoupled way.
- ```DataService```
    * Useful service to store a reference of all data in the application.
    * Handles individual multiple data types in lists and unique data types in shared mode.
- ```CoroutineService```
    * Handles your coroutines state in plain C#, no MonoBehaviours used.
- ```TaskService```
    * Allows you to run tasks that need to be updated over time.
	* Exposes several helper functions that returns promises.

#### Services (Unity3D)

- ```LogUnityService```
    * Redirects all logs to Unity console (critical in Unity environments).
- ```UnityService```
    * Exposes several Unity engine events and ticks Coroutine and Task services.
- ```DataServiceUnityExtensions```
    * Add some helper methods to handle ScriptableObjects in DataService.

#### Classes

- ```Observable```
    * Generic type that handles action subscribers to value changes of any type.
- ```ObjectPool```
    * Simple generic object pool using a stack internally.

#### Classes (Unity3D)

- ```MonoBehaviourActor```
    * Handles methods for EventBus and Data services and for Observables.

#### Helpers

- ```IEnumerable```
    * Map, Filter and Reduce alias methods using original LINQ methods.
- ```Promise```
    * Promise class to defer actions until after a previous condition is resolved.
- ```MonoBehaviour```
    * MonoBehaviour extension methods for EventBus and getting common properties.
- ```String```
    * Extension methods for creating hashes based on the string.


Documentation (Unity3D)
=====================

It's recomended to create a static class anywhere in your project to register all your services automatically.

```csharp
using UnityEngine;
using Ju;

public static class Bootstrap
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Init()
	{
		// Generic services

		Services.RegisterService<IEventBusService, EventBusService>();
		Services.RegisterService<ITaskService, TaskService>();
		Services.RegisterService<ICoroutineService, CoroutineService>();
		Services.RegisterService<IDataService, DataService>();

		// Unity related services

		Services.RegisterService<ILogUnityService, LogUnityService>();
		Services.RegisterService<IUnityService, UnityService>();

		// Register your services here
	}
}
```

You can name the class and the method to whatever you want.


Documentation (Generic)
=====================

You can also create a static class where you name each service you use frequently as syntactic sugar:

```csharp
using Ju;

public static class Core
{
	public static T Get<T>() => Services.Get<T>();
	public static T Get<T>(string id) => Services.Get<T>(id);
	public static void Fire<T>(T msg) => Services.Get<IEventBusService>().Fire(msg);
	public static ILogService Log => Services.Get<ILogService>();
	public static IEventBusService Event => Services.Get<IEventBusService>();
	public static ITaskService Task => Services.Get<ITaskService>();
	public static ICoroutineService Coroutine => Services.Get<ICoroutineService>();
	public static IDataService Data => Services.Get<IDataService>();
	//public static IUnityService Unity => Services.Get<IUnityService>();

	// ... add more shorthands as you need
}

// Now you can use the static class Core to access your services or custom shorthand methods !
```

... and since ```Log``` service usage is very common, you can even use a wrapper to write less code when logging:

```csharp
public static class Log
{
	public static void Debug(string msg, params object[] args) => Core.Log.Debug(msg, args);
	public static void Info(string msg, params object[] args) => Core.Log.Info(msg, args);
	public static void Notice(string msg, params object[] args) => Core.Log.Notice(msg, args);
	public static void Warning(string msg, params object[] args) => Core.Log.Warning(msg, args);
	public static void Error(string msg, params object[] args) => Core.Log.Error(msg, args);
}

// Now you can use Log.Debug("test") anywhere in your code !
```

Create your service classes and make sure they inherit from ```IService``` interface.

```csharp
using Ju;

public class CustomService : IService
{
	public void Setup()
	{
		// Here you can resolve the service dependencies.
	}

	public void Start()
	{
		// Here you can initialize your service.
	}

	public void CustomMethod()
	{
	}
}
```

Then, at some point in the entry point of your program, register your services using:

```csharp
// Register a lazy service, it will initialize when used the first time only.
Services.RegisterLazyService<CustomService>();

// Register service and initialize them right away.
Services.RegisterService<CustomService>();
```

**NOTE**: If you are using Unity3D, use the tip in the Unity3D documentation above.

You can also register a object factory using:

```csharp
// A new object of type CustomClass will be created in each call.
// You can pass any function or lambda to build your new object.
Services.RegisterFactory<CustomClass>(() => new CustomClass());
```

Before the program finalizes, you should dispose the services using:

```csharp
// Dispose all services, also the event "OnApplicationQuit" will fire.
Services.Dispose();
```


The MIT License (MIT)
=====================

Copyright © 2016-2020 Juan Delgado (JuDelCo)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
