///=======================================================================================
/// Copyright                            : Copyright (c) 2013 
/// Company Name                         : TRANE
/// Application Name                     : TRACE 800
/// File Name                            : SharedDictionaryManager.cs
/// Author Name                          : Vallarasu Sambathkumar
/// Created Date                         : 4/2/2013 6:11:39 PM
/// Description                          : Defines static helper for sharing and accessing resources.
///=======================================================================================

using System.Windows;

namespace Backbone.Common.UI.Controls
{
	/// <summary>
	/// Defines static helper for sharing and accessing resources.
	/// </summary>
	internal static class SharedDictionaryManager
	{
		internal static ResourceDictionary SharedDictionary
		{
			get
			{
				if (_sharedDictionary == null)
				{
					System.Uri resourceLocater =
						new System.Uri("/Common.UI;component/Themes/Generic.xaml",
										System.UriKind.Relative);

					_sharedDictionary =
						(ResourceDictionary)Application.LoadComponent(resourceLocater);
				}

				return _sharedDictionary;
			}
		}

		private static ResourceDictionary _sharedDictionary;
	}
}
