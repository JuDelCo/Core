JuCore
=====================

JuCore is a service locator with lots of useful services to ease the development of C# applications and games.

Any feedback is welcome !


See also
=====================

- [JuCore Math](https://github.com/JuDelCo/CoreMath) - Linear algebra math library, also 2D/3D physics, noise functions and IK
- [JuCore ECS](https://github.com/JuDelCo/CoreECS) - Deterministic lightweight ECS framework


Documentation
=====================

Go to the Wiki to learn how to **install** and **use** this framework. The API is documented there too.

https://github.com/JuDelCo/Core/wiki


Contents
=====================

#### Core
- [Service Container](https://github.com/JuDelCo/Core/wiki/Usage.Service-Container)
- [Service Interfaces](https://github.com/JuDelCo/Core/wiki/Usage.Services)
- [TimeUpdate Events](https://github.com/JuDelCo/Core/wiki/Usage.TimeUpdate-Events)
- [LinkHandlers](https://github.com/JuDelCo/Core/wiki/Usage.LinkHandlers)

#### Services

- [EventBus](https://github.com/JuDelCo/Core/wiki/API.Service.EventBus)
- [Time](https://github.com/JuDelCo/Core/wiki/API.Service.Time)
- [Input](https://github.com/JuDelCo/Core/wiki/API.Service.Input)
- [Task](https://github.com/JuDelCo/Core/wiki/API.Service.Task)
- [Coroutine](https://github.com/JuDelCo/Core/wiki/API.Service.Coroutine)
- [Data](https://github.com/JuDelCo/Core/wiki/API.Service.Data)
- [Extensions](https://github.com/JuDelCo/Core/wiki/API.Service.Extensions)

#### Util

- [Color](https://github.com/JuDelCo/Core/wiki/API.Util.Color)
	- Color
	- Color32
- [Finite State Machine](https://github.com/JuDelCo/Core/wiki/API.Util.FSM)
	- FSM
	- SimpleFSM
- [ObjectPool](https://github.com/JuDelCo/Core/wiki/API.Util.ObjectPool)
- [Observables](https://github.com/JuDelCo/Core/wiki/API.Util.Observables)
- [Promises](https://github.com/JuDelCo/Core/wiki/API.Util.Promises)
- [Random](https://github.com/JuDelCo/Core/wiki/API.Util.Random)
- [Time](https://github.com/JuDelCo/Core/wiki/API.Util.Time)
	- Span
	- Clock
	- ClockPrecise
	- Timer
	- FrameClock
	- FrameTimer
- [Extensions](https://github.com/JuDelCo/Core/wiki/API.Util.Extensions)
	- IEnumerable
	- ICollection
	- IDictionary
	- String
	- Float
	- Bool

#### Unity3D

- [Services](https://github.com/JuDelCo/Core/wiki/API.Unity.Services)
	- LogUnity
	- InputUnity
	- PrefabPool
	- Unity
	- Service Extensions
- [Extensions](https://github.com/JuDelCo/Core/wiki/API.Unity.Util-Extensions)
	- Behaviour
	- LayerMask
	- Color
	- Color32
	- Observable
	- Random


The MIT License (MIT)
=====================

Copyright © 2016-2021 Juan Delgado (@JuDelCo)

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
