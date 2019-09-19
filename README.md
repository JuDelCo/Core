Core
=====================

Core is a service locator, object factory and a collection of basic services to ease the development of C# applications or games.

This package will follow a [semantic versioning](http://semver.org/).

Any feedback is welcome !


See also
=====================

- [Core Unity](https://github.com/JuDelCo/CoreUnity) - Core services extension for Unity3D


Documentation
=====================

#### Services

- LogService
    * Handles all logging across all services and your code.
    * **Note**: The LogService is internally automatically registered for you.
- EventBusService
    * Useful event bus to communicate your objects in a decoupled way.
- DataService
    * Useful service to store a reference of all data in the application.
    * Handles individual multiple data types in lists and unique data types in shared mode.

#### Classes

- Observable
    * Generic type that handles action subscribers to value changes of any type.

#### Usage

First, create your service classes and make sure they inherit from IService interface.

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

Copyright © 2016-2019 Juan Delgado (JuDelCo)

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
