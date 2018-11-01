///=======================================================================================
/// Copyright                            : Copyright (c) 2013 
/// Company Name                         : TRANE
/// Application Name                     : Backbone 800
/// File Name                            : EnumToValueConverter.cs
/// Author Name                          : Vallarasu Sambathkumar
/// Created Date                         : 4/2/2013 6:11:39 PM
/// Description                          : Helps convert enumeration into the values specified in enumration.
///=======================================================================================

using System;
using System.Windows.Data;

namespace Backbone.Common.UI.Converters
{
	/// <summary>
	/// Helps convert enumeration into the values specified in enumeration.
	/// </summary>
	public class EnumToValueConverter : IValueConverter
	{
	    private Type _type;

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			_type = value as Type;

			if (_type == null || !_type.IsEnum) return value;

			return Enum.GetValues(_type);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
