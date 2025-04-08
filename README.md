JuCore
=====================

Service locator with lots of useful services and util classes to ease the development of C# apps and games.

Any feedback is welcome !


See also
=====================

- [JuCore Math](https://github.com/JuDelCo/CoreMath) - Linear algebra math library, also 2D/3D physics, noise functions and IK
- [JuCore ECS](https://github.com/JuDelCo/CoreECS) - Deterministic lightweight ECS framework


Install
=====================

The Wiki explains [**how to install**](https://github.com/JuDelCo/Core/wiki/Usage.Getting-Started) the library for your specific case. Below is a quick summary:

- For **Unity**, update the dependencies in the ```/Packages/manifest.json``` file in your project folder by adding:

```json
	"com.judelco.core": "https://github.com/JuDelCo/Core.git#v1.49.0",
```

- For native **.NET projects**, **Godot**, etc... run the following command in the console of your .NET project to add the package:

```bash
	dotnet add package JuDelCo.Lib.Core
```


Documentation
=====================

Go to the Wiki to learn how to [**install**](https://github.com/JuDelCo/Core/wiki/Usage.Getting-Started) and [**use**](https://github.com/JuDelCo/Core/wiki) this framework. The API is documented there too.

https://github.com/JuDelCo/Core/wiki


Contents
=====================

#### Install
- [Getting Started](https://github.com/JuDelCo/Core/wiki/Usage.Getting-Started)
- [Unity Projects](https://github.com/JuDelCo/Core/wiki/Usage.Unity-Projects)
- [Native C# Projects](https://github.com/JuDelCo/Core/wiki/Usage.Native-CSharp-Projects)

#### Manual
- [Service Container](https://github.com/JuDelCo/Core/wiki/Usage.Service-Container)
- [Custom Services](https://github.com/JuDelCo/Core/wiki/Usage.Custom-Services)
- [Logging](https://github.com/JuDelCo/Core/wiki/Usage.Logging)
- [TimeUpdate Events](https://github.com/JuDelCo/Core/wiki/Usage.TimeUpdate-Events)
- [LinkHandlers](https://github.com/JuDelCo/Core/wiki/Usage.LinkHandlers)

#### Services
- [Cache](https://github.com/JuDelCo/Core/wiki/API.Service.Cache)
- [Coroutine](https://github.com/JuDelCo/Core/wiki/API.Service.Coroutine)
- [EventBus](https://github.com/JuDelCo/Core/wiki/API.Service.EventBus)
- [Input](https://github.com/JuDelCo/Core/wiki/API.Service.Input)
- [Task](https://github.com/JuDelCo/Core/wiki/API.Service.Task)
- [Time](https://github.com/JuDelCo/Core/wiki/API.Service.Time)

#### Util
- [Extensions](https://github.com/JuDelCo/Core/wiki/API.Util.Extensions)
- [Clocks](https://github.com/JuDelCo/Core/wiki/API.Util.Clocks)
- [Color](https://github.com/JuDelCo/Core/wiki/API.Util.Color)
- [Conversion](https://github.com/JuDelCo/Core/wiki/API.Util.Conversion)
- [Crypt](https://github.com/JuDelCo/Core/wiki/API.Util.Crypt)
- [Data](https://github.com/JuDelCo/Core/wiki/API.Util.Data)
- [FSM](https://github.com/JuDelCo/Core/wiki/API.Util.FSM)
- [Gradient](https://github.com/JuDelCo/Core/wiki/API.Util.Gradient)
- [Hjson/Json](https://github.com/JuDelCo/Core/wiki/API.Util.Hjson)
- [ObjectPool](https://github.com/JuDelCo/Core/wiki/API.Util.ObjectPool)
- [Observables](https://github.com/JuDelCo/Core/wiki/API.Util.Observables)
- [Promises](https://github.com/JuDelCo/Core/wiki/API.Util.Promises)
- [Random](https://github.com/JuDelCo/Core/wiki/API.Util.Random)
- [Span](https://github.com/JuDelCo/Core/wiki/API.Util.Span)
- [Timers](https://github.com/JuDelCo/Core/wiki/API.Util.Timers)

#### Unity3D Services
- [Cache](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.Cache)
- [Input](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.Input)
- [Log](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.Log)
- [PrefabPool](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.PrefabPool)
- [Time](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.Time)
- [Unity](https://github.com/JuDelCo/Core/wiki/API.Unity.Service.Unity)

#### Unity3D Integrations
- [Behaviour Extensions](https://github.com/JuDelCo/Core/wiki/API.Unity.Behaviour-Extensions)
- [Util Extensions](https://github.com/JuDelCo/Core/wiki/API.Unity.Util-Extensions)


The MIT License (MIT)
=====================

Copyright © 2016-2025 Juan Delgado (@JuDelCo)

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
