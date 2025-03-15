// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
 
namespace Manul.Toolbar
{
	[CreateAssetMenu(fileName = "Manul Toolbar Button List Set", menuName = "Manul Tools/Manul Toolbar Button List Set")]
	public class ManulToolbarButtonListSet : ScriptableObject
	{
		public string intPrefName;
		public float popupWidth = 100f;
		public List<ManulToolbarButtonListSetEntry> listEntries;
	}
}

#endif
