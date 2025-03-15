// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Manul.Toolbar.Examples
{
	public static class ExampleStaticClass
	{
		static void LMB_None()
		{
			Debug.Log("The button was clicked by LEFT mouse button. No additional key was held.");
		}

		static void RMB_None()
		{
			Debug.Log("The button was clicked by RIGHT mouse button. No additional key was held.");
		}

		static void MMB_None()
		{
			Debug.Log("The button was clicked by MIDDLE mouse button. No additional key was held.");
		}

		static void LMB_Ctrl()
		{
			Debug.Log("The button was clicked by LEFT mouse button while holding the CTRL key.");
		}

		static void RMB_Ctrl()
		{
			Debug.Log("The button was clicked by RIGHT mouse button while holding the CTRL key.");
		}

		static void MMB_Ctrl()
		{
			Debug.Log("The button was clicked by MIDDLE mouse button while holding the CTRL key.");
		}

		static void LMB_Shift()
		{
			Debug.Log("The button was clicked by LEFT mouse button while holding the SHIFT key.");
		}

		static void RMB_Shift()
		{
			Debug.Log("The button was clicked by RIGHT mouse button while holding the SHIFT key.");
		}

		static void MMB_Shift()
		{
			Debug.Log("The button was clicked by MIDDLE mouse button while holding the SHIFT key.");
		}

		static void LMB_Alt()
		{
			Debug.Log("The button was clicked by LEFT mouse button while holding the ALT key.");
		}

		static void RMB_Alt()
		{
			Debug.Log("The button was clicked by RIGHT mouse button while holding the ALT key.");
		}

		static void MMB_Alt()
		{
			Debug.Log("The button was clicked by MIDDLE mouse button while holding the ALT key.");
		}

		static void CheckExampleEditorPref()
		{
			Debug.Log("Example Editor Pref is: " + EditorPrefs.GetBool("ExampleTogglePref"));
		}

		static void CheckOptionAPref()
		{
			Debug.Log("Example Editor Int Pref named 'PopupExampleA' is: " + EditorPrefs.GetInt("PopupExampleA"));
		}

		static void CheckOptionBPref()
		{
			Debug.Log("Example Editor Int Pref named 'PopupExampleB' is: " + EditorPrefs.GetInt("PopupExampleB"));
		} 
	}
}

#endif
