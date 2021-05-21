// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using System.Collections.Generic;
using Ju.Data.Conversion;
using Ju.Extensions;
using Ju.Handlers;

namespace Ju.Data
{
	internal interface IJData
	{
		Type GetDataType();
		object GetRawData();
		void SetRawData(object data);
	}

	public class JData<T> : JNode, IJData
	{
		private static readonly JNode[] emptyCollection = { };
		private List<JDataHandleActionPair<JData<T>>> actionsValue = null;
		private List<JDataHandleActionPair<JData<T>>> cachedActionsValue = null;
		private T defaultValue;
		private T value;

		public JData(T value = default(T))
		{
			this.value = value;
			this.defaultValue = default(T);
		}

		public JData(T value, T defaultValue = default(T))
		{
			this.value = value;
			this.defaultValue = defaultValue;
		}

		public override void Reset()
		{
			Value = defaultValue;
		}

		public override int GetSubscriberCount()
		{
			if (actionsValue != null)
			{
				return (actionsValue.Count + base.GetSubscriberCount());
			}

			return base.GetSubscriberCount();
		}

		public override JNode Clone()
		{
			return new JData<T>(value, defaultValue);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposedValue)
			{
				return;
			}

			if (disposing)
			{
				base.Dispose(disposing);

				if (actionsValue != null)
				{
					actionsValue.Clear();
				}

				if (defaultValue is IDisposable disposableDefaultValue)
				{
					disposableDefaultValue.Dispose();
				}

				if (value is IDisposable disposableValue)
				{
					disposableValue.Dispose();
				}
			}

			disposedValue = true;
		}

		public override IEnumerator<JNode> GetEnumerator()
		{
			yield return null;
		}

		public override IEnumerable<JNode> Children
		{
			get => emptyCollection;
		}

		public void Subscribe(ILinkHandler handle, Action<JData<T>> action)
		{
			if (actionsValue == null)
			{
				actionsValue = new List<JDataHandleActionPair<JData<T>>>();
			}

			actionsValue.Add(new JDataHandleActionPair<JData<T>>(handle, action));
		}

		public Type GetDataType()
		{
			return typeof(T);
		}

		public object GetRawData()
		{
			return value;
		}

		public void SetRawData(object rawData)
		{
			if (rawData is T data)
			{
				Value = data;
			}
			else if (typeof(T).IsEnum && rawData.GetType() == typeof(string))
			{
				var type = typeof(T);
				var dataStr = (string)rawData;
				var found = false;

				foreach (var enumValue in Enum.GetNames(type))
				{
					if (enumValue.Equals(dataStr, StringComparison.CurrentCultureIgnoreCase))
					{
						found = true;
						Value = (T)Enum.Parse(type, dataStr, true);
						break;
					}
				}

				if (!found)
				{
					throw new Exception($"Invalid conversion the Enum of type {type.GetFriendlyName()} from the string {dataStr}.");
				}
			}
			else
			{
				var conversionType = new ConversionType(rawData.GetType(), typeof(T));

				if (DataTypeConverter.HasConverter(conversionType))
				{
					try
					{
						var newValue = (T)(DataTypeConverter.GetConverter(conversionType)(rawData));
						Value = newValue;
					}
					catch
					{
						throw new Exception($"Failed to convert from type {rawData.GetType().GetFriendlyName()} to the type {typeof(T).GetFriendlyName()} of the JData node.");
					}
				}
				else
				{
					throw new Exception($"No type converter found for a raw data of type {rawData.GetType().GetFriendlyName()} to the type {typeof(T).GetFriendlyName()} of the JData node.");
				}
			}
		}

		private void TriggerValue()
		{
			if (actionsValue == null || actionsValue.Count == 0)
			{
				return;
			}

			if (cachedActionsValue == null)
			{
				cachedActionsValue = new List<JDataHandleActionPair<JData<T>>>();
			}

			cachedActionsValue.AddRange(actionsValue);

			for (int i = 0; i < cachedActionsValue.Count; ++i)
			{
				if (callStackCounter > 0)
				{
					throw new Exception("A JData callback has modified the same value that triggered it.");
				}

				var handle = cachedActionsValue[i].handle;

				if (!handle.IsActive)
				{
					if (handle.IsDestroyed)
					{
						actionsValue.Remove(cachedActionsValue[i]);
					}

					continue;
				}

				++callStackCounter;

				cachedActionsValue[i].action(this);

				--callStackCounter;
			}

			cachedActionsValue.Clear();
		}

		public T DefaultValue
		{
			get => defaultValue;
			set
			{
				defaultValue = value;
			}
		}

		public T Value
		{
			get => value;
			set
			{
				if (!EqualityComparer<T>.Default.Equals(this.value, value))
				{
					this.value = value;
					TriggerValue();
					Trigger(this);
				}
			}
		}

		public T ValueSilent
		{
			get => value;
			set
			{
				this.value = value;
			}
		}

		public override string ToString()
		{
			return $"\"{value}\"";
		}

		public override string ToString(int maxDepth)
		{
			return ToString();
		}
	}
}
