// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR

using UnityEditor;

namespace Manul.Toolbar
{	
	public class ManulToolbarMenuItems
	{
		/// Upper Bar

		[MenuItem("Tools/Manul Toolbar/Open Manul Toolbar Settings")]
		private static void Toolbar_OpenManulToolbarSettings()
		{
			if (ManulToolbar.settings == null) return;

#if UNITY_2021_1_OR_NEWER
			EditorUtility.OpenPropertyEditor(ManulToolbar.settings);
#else
			// Selection.activeObject = settings;
#endif
		}

		[MenuItem("Tools/Manul Toolbar/Select Manul Toolbar Settings")]
		private static void Toolbar_SelectManulToolbarSettings()
		{
			if (ManulToolbar.settings == null) return;

			Selection.activeObject = ManulToolbar.settings;
		}

		/// Left Side

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Open Asset")]
		private static void CreateButtonLeft_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Select Asset")]
		private static void CreateButtonLeft_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Open and Select Asset")]
		private static void CreateButtonLeft_OpenSelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenAsset, true);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Show in Explorer")]
		private static void CreateButtonLeft_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Show Properties")]
		private static void CreateButtonLeft_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Left Side/Open Folder")]
		private static void CreateButtonLeft_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Left, ToolbarButtonType.OpenFolder);
		}

		/// Right Side

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Open Asset")]
		private static void CreateButtonRight_OpenAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenAsset);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Select Asset")]
		private static void CreateButtonRight_SelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.SelectAsset);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Open and Select Asset")]
		private static void CreateButtonRight_OpenSelectAsset(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenAsset, true);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Show in Explorer")]
		private static void CreateButtonRight_ShowInExplorers(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.ShowAssetInExplorer);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Show Properties")]
		private static void CreateButtonRight_ShowProperties(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.PropertiesWindow);
		}

		[MenuItem("Assets/Manul Toolbar/Create Button on Right Side/Open Folder")]
		private static void CreateButtonRight_OpenAssetByPath(MenuCommand menuCommand)
		{
			ManulToolbar.CreateButton(ToolbarEntrySideType.Right, ToolbarButtonType.OpenFolder);
		}
	}
}

#endif