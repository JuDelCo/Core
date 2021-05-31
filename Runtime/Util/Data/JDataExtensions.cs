// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2021 Juan Delgado (@JuDelCo)

using System;
using Ju.Data;
using Ju.Handlers;

public static class JDataExtensions
{
	public static Action<T> Bind<T>(this JData<T> jData, ILinkHandler handle, Action<JData<T>> action)
	{
		jData.Subscribe(handle, action);

		return (value) =>
		{
			if (jData != null && !jData.IsDisposed())
			{
				jData.Value = value;
			}
		};
	}

	public static Action<TRemote> Bind<T, TRemote>(this JData<T> jData, ILinkHandler handle, Action<JData<T>> action, Func<TRemote, T> converter)
	{
		jData.Subscribe(handle, action);

		return (value) =>
		{
			if (jData != null && !jData.IsDisposed())
			{
				jData.Value = converter(value);
			}
		};
	}
}
