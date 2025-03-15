// Copyright (c) 2024 Liquid Glass Studios. All rights reserved.

#if UNITY_EDITOR
 
using UnityEngine;
using UnityEditor;

namespace Manul.Toolbar
{
	public enum Message
	{
		None,
		NoSettingsAsset,
		NoButtonListAsset,
		NoButtonListSetAsset,
		NoIndexInButtonListSetAsset,
		NoButtonListAssetInButtonListSetAsset,
		EmptyPrefFieldForSet,
		TwoOrMoreSetingsFiles,
		CantFindGameObject,
		CantFindFolder,	
		CantFindMethod,
		CantFindClass,
		NoObjectForButton,
		NoTypeOnScene,
		MoreThanOneTypeOnScene,
		NoComponent
	}

	public static class ManulToolbarMessages
	{
		static string messagePrefix = "[MANUL Inspector message] ";

		public static void ShowMessage(Message type, MessageType messageType, string[] infos)
		{
			string messageText = messagePrefix;

			switch (type)
			{
				case Message.NoSettingsAsset:
					messageText += "Manul Toolbar Settings asset cannot be found in the Resources folder.";
					Debug.LogWarning(messageText); 
					return;
			} 

			if (!ManulToolbar.settings.settings.showConsoleMessages) return;

			switch (type)
			{
				case Message.None:
					messageText += "None";
					break;

				case Message.NoButtonListAsset:
					messageText += "Manul Toolbar Button List asset for " + infos[0] + " toolbar side is missing! Please add this asset to the 'Override " + infos[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset for " + infos[0] + " toolbar side is missing! Please add this asset to the 'Override " + infos[1] + " Side' field in the Manul Toolbar Settings.";
					break;

				case Message.NoIndexInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field does not have an entry with index: " + infos[1];
					break;

				case Message.NoButtonListAssetInButtonListSetAsset:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field has an entry with index " + infos[1] + ", but this entry does not have the Manul Toolbar Button List asset.";
					break;

				case Message.EmptyPrefFieldForSet:
					messageText += "Manul Toolbar Button List Set asset in the 'Override " + infos[0] + " Side' field has an empty 'Int Pref Name' field.";
					break;

				case Message.CantFindFolder:
					messageText += "Cannot find a folder with the path: '" + infos[0] + "' for the button named '" + infos[1] + "'.";
					break;

				case Message.CantFindGameObject:
					messageText += "Cannot find GameObject with name '" + infos[0] + "' in the current opened scene in the button named '" + infos[1] + "'.";
					break;

				case Message.NoObjectForButton:
					messageText += "Object field in the '" + infos[0] + "' action in the button named '" + infos[1] + "' is empty.";
					break;

				case Message.NoTypeOnScene:
					messageText += "No object of type '" + infos[0] + "' was found in the current scene so the method '" + infos[3] + "' (from action '" + infos[2] + "' in the button named '" + infos[1] + "') cannot be invoked.";
					break;

				case Message.MoreThanOneTypeOnScene:
					messageText += "Two or more objects of the type '" + infos[0] + "' were found in the current scene so the method '" + infos[3] + "' (from action '" + infos[2] + "' in the button named '" + infos[1] + "') cannot be invoked. " +
						"Make sure that there is exactly one object of the given type.";
					break;

				case Message.CantFindClass:
					messageText += "Cannot find a class named '" + infos[0] + "' for the action '" + infos[2] + "' in the button named '" + infos[1] + "'. Make sure you typed the class name with the namespace (for example: 'Manul.Toolbar.Examples.ExampleComponent').";
					break;

				case Message.CantFindMethod:
					messageText += "Cannot find a method '" + infos[3] + "' in the class named '" + infos[0] + "' for the action '" + infos[2] + "' in the button named '" + infos[1] + "'.";
					break;

				case Message.TwoOrMoreSetingsFiles:
					messageText +=
						"You have two or more Manul Toolbar Settings assets named 'Manul Toolbar', but they are located in different 'Resources' folders. " +
						"There must be exactly one Manul Toolbar Settings asset named 'Manul Toolbar' in exactly one 'Resource' folder, " +
						"otherwise you will encounter problems while using the Manul Toolbar tool.";
					break;
			}

			switch (messageType)
			{
				case MessageType.Info: Debug.Log(messageText); break;
				case MessageType.Warning: Debug.LogWarning(messageText); break;
				case MessageType.Error: Debug.LogError(messageText); break;
			}
		}
	}
}

#endif


 