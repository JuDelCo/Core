using System;
using System.Collections;
using System.Collections.Generic;

namespace Ju
{
	internal class CoroutineHandlePair
	{
		public ILinkHandler handle;
		public IEnumerator coroutine;

		public CoroutineHandlePair(ILinkHandler handle, IEnumerator coroutine)
		{
			this.handle = handle;
			this.coroutine = coroutine;
		}
	}

	public class CoroutineService : ICoroutineService
	{
		public event LogMessageEvent OnLogDebug = delegate { };
		public event LogMessageEvent OnLogInfo = delegate { };
		public event LogMessageEvent OnLogNotice = delegate { };
		public event LogMessageEvent OnLogWarning = delegate { };
		public event LogMessageEvent OnLogError = delegate { };

		private List<CoroutineHandlePair> coroutines = new List<CoroutineHandlePair>();
		private List<CoroutineHandlePair> coroutinesRunner = new List<CoroutineHandlePair>();

		public void Setup()
		{
		}

		public void Start()
		{
		}

		public void Tick()
		{
			lock (coroutines)
			{
				if (coroutines.Count > 0)
				{
					for (int i = 0; i < coroutines.Count; ++i)
					{
						coroutinesRunner.Add(coroutines[i]);
					}

					coroutines.Clear();
				}
			}

			if (coroutinesRunner.Count > 0)
			{
				for (int i = (coroutinesRunner.Count - 1); i >= 0; --i)
				{
					if (coroutinesRunner[i].handle.IsDestroyed)
					{
						coroutinesRunner.RemoveAt(i);
						continue;
					}
					else if (!coroutinesRunner[i].handle.IsActive)
					{
						continue;
					}

					if (System.Diagnostics.Debugger.IsAttached)
					{
						if (TickCoroutine(coroutinesRunner[i].coroutine))
						{
							coroutinesRunner.RemoveAt(i);
						}
					}
					else
					{
						try
						{
							if (TickCoroutine(coroutinesRunner[i].coroutine))
							{
								coroutinesRunner.RemoveAt(i);
							}
						}
						catch (Exception e)
						{
							OnLogError("Uncaught Exception: {0}", e.Message);
						}
					}
				}

				if (coroutinesRunner.Count > 0)
				{
					for (int i = 0; i < coroutinesRunner.Count; ++i)
					{
						coroutines.Add(coroutinesRunner[i]);
					}

					coroutinesRunner.Clear();
				}
			}
		}

		private bool TickCoroutine(IEnumerator routine)
		{
			var isFinished = false;

			if (routine.Current is Coroutine)
			{
				if (!((Coroutine)routine.Current).KeepWaiting)
				{
					if (!routine.MoveNext())
					{
						isFinished = true;
					}
				}
			}
			else if (routine.Current is YieldInstruction)
			{
				if (!((YieldInstruction)routine.Current).MoveNext())
				{
					if (!routine.MoveNext())
					{
						isFinished = true;
					}
				}
			}
			else
			{
				if (!routine.MoveNext())
				{
					isFinished = true;
				}
			}

			return isFinished;
		}

		public Coroutine StartCoroutine(ILinkHandler handle, IEnumerator routine)
		{
			var coroutine = new Coroutine(routine);

			lock (coroutines)
			{
				coroutines.Add(new CoroutineHandlePair(handle, coroutine));
			}

			return coroutine;
		}
	}
}
