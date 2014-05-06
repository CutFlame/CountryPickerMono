using System;
using MonoTouch;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;

namespace CountryPickerMono
{
	[Register("CountryPicker")]
	public class CountryPicker : UIPickerView
	{
		public Action<string, string> DidSelectCountry;

		public string SelectedCountryCode
		{
			get
			{
				int index = base.SelectedRowInComponent (0);
				return _model.CountryCodes [index];
			}
		}

		public string SelectedCountryName
		{
			get
			{
				int index = SelectedRowInComponent (0);
				return _model.CountryNames [index];
			}
		}

		public NSLocale SelectedLocale
		{
			get
			{
				var countryCode = SelectedCountryCode;
				if (countryCode != null)
				{
					//kCFLocaleCountryCodeKey
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

		CountrySelectionModel _model;

		public CountryPicker (IntPtr handle) : base(handle)
		{
		}

		public void Setup(CountrySelectionModel model)
		{
			_model = model;
			var source = new CountryPickerSource(model);
			source.DidSelectCountry = CallDidSelectCountry;
			base.Source = source;
		}

		public void SetSelectedCountryCode(string countryCode, bool animated = false)
		{
			int index = Array.IndexOf (_model.CountryCodes, countryCode);
			if (index >= 0)
			{
				base.Select (index, 0, animated);
			}
		}

		public void SetSelectedCountryName(string countryName, bool animated = false)
		{
			int index = Array.IndexOf (_model.CountryNames, countryName);
			if (index >= 0)
			{
				Select (index, 0, animated);
			}
		}

		public void SetSelectedLocale(NSLocale locale, bool animated = false)
		{
			SetSelectedCountryCode (locale.CountryCode, animated);
		}

		void CallDidSelectCountry(string name, string code)
		{
			var handler = DidSelectCountry;
			if (handler != null)
			{
				handler (name, code);
			}
		}

	}
}

