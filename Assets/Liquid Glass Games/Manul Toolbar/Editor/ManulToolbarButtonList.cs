// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
 
namespace Manul.Toolbar
{
	[CreateAssetMenu(fileName = "Manul Toolbar Button List", menuName = "Manul Tools/Manul Toolbar Button List")]
	public class ManulToolbarButtonList : ScriptableObject
	{
		public List<ManulToolbarEntry> buttons;
	}
}

#endif
