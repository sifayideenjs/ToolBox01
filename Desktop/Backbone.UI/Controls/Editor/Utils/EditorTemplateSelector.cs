///=======================================================================================
/// Copyright                            : Copyright (c) 2013 
/// Company Name                         : TRANE
/// Application Name                     : TRACE 800
/// File Name                            : EditorTemplateSelector.cs
/// Author Name                          : Vallarasu Sambathkumar
/// Created Date                         : 4/2/2013 6:11:39 PM
/// Description                          : Defines a template selector for known types for PropertyEditor.
///=======================================================================================

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Backbone.Common.UI.Controls
{
	/// <summary>
	/// Defines a template selector for known types for PropertyEditor.
	/// </summary>
	public class EditorTemplateSelector : DataTemplateSelector 
	{
		public DataTemplate ReadOnlyPresenter { get; set; }

		public DataTemplate EnumsEditor { get; set; }

		public DataTemplate BooleanEditor { get; set; }

		public DataTemplate TextEditor { get; set; }

		public override System.Windows.DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
		{
			var editor = container as PropertyEditor;

			//if(editor == null) return ReadOnlyPresenter;

			var pinfo = item as PropertyDescriptor;

			if (pinfo == null || pinfo.IsReadOnly) return ReadOnlyPresenter;

			var type = pinfo.PropertyType;

			if (type == typeof(Boolean))
				return BooleanEditor;

			if (type.IsEnum)
				return EnumsEditor;

			if (type == typeof(string)
				|| type == typeof(double)
				|| type == typeof(int)
				|| type == typeof(decimal)
				|| type == typeof(float))
				return TextEditor;

			return ReadOnlyPresenter;
		}
	}
}
