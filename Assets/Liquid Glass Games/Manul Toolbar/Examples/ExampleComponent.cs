// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

using UnityEngine;

#if UNITY_EDITOR

namespace Manul.Toolbar.Examples
{
	public class ExampleComponent : MonoBehaviour
	{
		void MethodFromComponent()
		{
			Debug.Log("This is method ivoked using the 'Component Method' action type.");
		}
	}
}

#endif
