// SPDX-License-Identifier: MIT
// Copyright (c) 2016-2025 Juan Delgado (@JuDelCo)

#if UNITY_2019_3_OR_NEWER

using System;
using System.Collections.Specialized;
using Ju.Handlers;
using Ju.Hjson;
using UnityEngine;

public static class JsonArrayUnityExtensions
{
	public static void Subscribe(this JsonArray array, Behaviour behaviour, Action<NotifyCollectionChangedEventArgs> action, bool alwaysActive = false)
	{
		array.Subscribe(new BehaviourLinkHandler(behaviour, alwaysActive), action);
	}
}

#endif
