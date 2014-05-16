using System;
using MonoTouch;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace CountryPickerMono
{
	/// <summary>
	/// Picker showing country names and their flags
	/// </summary>
	[Register("CountryPicker")]
	public class CountryPicker : UIPickerView
	{
		/// <summary>
		/// Event fired when a country is selected
		/// </summary>
		public event EventHandler DidSelectCountry;

		CountrySelectionModel _model;

		/// <summary>
		/// Gets the selected country code.
		/// </summary>
		public string SelectedCountryCode
		{
			get
			{
				int index = base.SelectedRowInComponent (0);
				return GetValueAtIndexOrDefault (_model.CountryCodes, index);
			}
		}

		/// <summary>
		/// Gets the name of the selected country.
		/// </summary>
		public string SelectedCountryName
		{
			get
			{
				int index = base.SelectedRowInComponent (0);
				return GetValueAtIndexOrDefault(_model.CountryNames, index);
			}
		}

		/// <summary>
		/// Gets the locale of the selected country
		/// </summary>
		public NSLocale SelectedLocale
		{
			get
			{
				var countryCode = SelectedCountryCode;
				if (countryCode != null)
				{
					//get the library constant for NSLocaleCountryCode which is "kCFLocaleCountryCodeKey"
					IntPtr handle = Dlfcn.dlopen (Constants.CoreFoundationLibrary, 0);
					string countryCodeKey = Dlfcn.GetStringConstant (handle, "NSLocaleCountryCode");
					Dlfcn.dlclose (handle);

					var components = NSDictionary.FromObjectsAndKeys (new object[]{ countryCode }, new string[]{ countryCodeKey });
					string identifier = NSLocale.LocaleIdentifierFromComponents (components);
					components.Dispose ();

					return NSLocale.FromLocaleIdentifier (identifier);
				}
				return null;
			}
		}

		public CountryPicker (IntPtr handle) : base(handle)
		{
		}

		/// <summary>
		/// Setup the <paramref name="countryCodes"/> to be displayed in the picker.
		/// null will show all NSlocale.ISOCountryCodes
		/// </summary>
		public void Setup(string[] countryCodes = null)
		{
			if(countryCodes == null)
			{
				countryCodes = NSLocale.ISOCountryCodes;
			}
			Setup (new CountrySelectionModel (countryCodes, NSLocale.CurrentLocale.GetCountryCodeDisplayName));
		}

		/// <summary>
		/// Setup the values to be displayed in the picker
		/// </summary>
		public void Setup(CountrySelectionModel model)
		{
			_model = model;
			var source = new CountryPickerSource(model);
			source.DidSelectCountry = CallDidSelectCountry;
			base.Source = source;
		}

		/// <summary>
		/// Sets the selected country code.
		/// </summary>
		public void SetSelectedCountryCode(string countryCode, bool animated = false)
		{
			int index = Array.IndexOf (_model.CountryCodes, countryCode);
			if (index >= 0)
			{
				base.Select (index, 0, animated);
			}
		}

		/// <summary>
		/// Sets the name of the selected country.
		/// </summary>
		public void SetSelectedCountryName(string countryName, bool animated = false)
		{
			int index = Array.IndexOf (_model.CountryNames, countryName);
			if (index >= 0)
			{
				base.Select (index, 0, animated);
			}
		}

		/// <summary>
		/// Sets the selected country to match the given locale.
		/// </summary>
		public void SetSelectedLocale(NSLocale locale, bool animated = false)
		{
			SetSelectedCountryCode (locale.CountryCode, animated);
		}

		void CallDidSelectCountry()
		{
			var eventHandler = DidSelectCountry;
			if (eventHandler == null)
			{
				return;
			}
			foreach(var handler in eventHandler.GetInvocationList ())
			{
				if (handler != null)
				{
					handler.DynamicInvoke (this, EventArgs.Empty);
				}
			}
		}

		static T GetValueAtIndexOrDefault<T> (T[] array, int index, T defaultValue = null) where T : class
		{
			if(0 <= index && index < array.Length)
			{
				return array [index];
			}
			return defaultValue;
		}

	}
}

