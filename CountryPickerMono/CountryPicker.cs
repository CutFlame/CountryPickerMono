using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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

		string[] _countryNames;
		string[] _countryCodes;
		Dictionary<string, string> _countryNamesByCode;
		Dictionary<string, string> _countryCodesByName;

		public string[] CountryNames
		{
			get
			{
				if (_countryNames == null)
				{
					_countryNames = CountryNamesByCode.Values.ToArray ();
				}
				return _countryNames;
			}
		}

		public string[] CountryCodes
		{
			get
			{
				if (_countryCodes == null)
				{
					_countryCodes = CountryCodesByName.Values.ToArray ();
				}
				return _countryCodes;
			}
		}

		Dictionary<string, string> CountryNamesByCode
		{
			get
			{
				if (_countryNamesByCode == null)
				{
					_countryNamesByCode = new Dictionary<string, string> ();
					foreach (var code in NSLocale.ISOCountryCodes)
					{
						var countryName = NSLocale.CurrentLocale.GetCountryCodeDisplayName (code);
						if (countryName != null)
						{
							_countryNamesByCode.Add (code, countryName);
						}
					}
				}
				return _countryNamesByCode;
			}
		}

		Dictionary<string, string> CountryCodesByName
		{
			get
			{
				if (_countryCodesByName == null)
				{
					_countryCodesByName = new Dictionary<string, string> ();
					var countryNamesByCode = CountryNamesByCode;
					foreach (var code in countryNamesByCode.Keys)
					{
						_countryCodesByName.Add (countryNamesByCode [code], code);
					}
				}
				return _countryCodesByName;
			}
		}

		public string SelectedCountryCode
		{
			get
			{
				int index = base.SelectedRowInComponent (0);
				return CountryCodes [index];
			}
		}

		public string SelectedCountryName
		{
			get
			{
				int index = SelectedRowInComponent (0);
				return CountryNames [index];
			}
		}

		public NSLocale SelectedLocale
		{
			get
			{
				var countryCode = SelectedCountryCode;
				if (countryCode != null)
				{
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
			Setup ();
		}

		void Setup()
		{
			var model = new CountryPickerModel(this);
			model.DidSelectCountry = CallDidSelectCountry;
			base.Source = model;
		}

		public void SetSelectedCountryCode(string countryCode, bool animated = false)
		{
			int index = Array.IndexOf (CountryCodes, countryCode);
			if (index >= 0)
			{
				base.Select (index, 0, animated);
			}
		}

		public void SetSelectedCountryName(string countryName, bool animated = false)
		{
			int index = Array.IndexOf (CountryNames, countryName);
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

		class CountryPickerModel : UIPickerViewModel
		{
			public Action<string, string> DidSelectCountry;

			readonly CountryPicker _picker;

			public CountryPickerModel (CountryPicker countryPicker)
			{
				_picker = countryPicker;
			}

			public override int GetComponentCount (UIPickerView picker)
			{
				return 1;
			}

			public override int GetRowsInComponent (UIPickerView picker, int component)
			{
				return _picker.CountryCodes.Length;
			}
			
			public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
			{
				if (view == null)
				{
					view = new UIView (new RectangleF (0, 0, 280, 30));
					var label = new UILabel (new RectangleF (35, 3, 245, 24));
					label.BackgroundColor = UIColor.Clear;
					label.Tag = 1;
					view.AddSubview (label);

					var flagView = new UIImageView (new RectangleF (3, 3, 24, 24));
					flagView.ContentMode = UIViewContentMode.ScaleAspectFit;
					flagView.Tag = 2;
					view.AddSubview (flagView);
				}

				((UILabel)view.ViewWithTag (1)).Text = _picker.CountryNames[row];
				((UIImageView)view.ViewWithTag (2)).Image = FlagForCountryCode(_picker.CountryCodes[row]);
				return view;
			}

			public override void Selected (UIPickerView picker, int row, int component)
			{
				var handler = DidSelectCountry;
				if (handler != null)
				{
					handler (_picker.CountryNames [row], _picker.CountryCodes [row]);
				}
			}

			UIImage FlagForCountryCode(string code)
			{
				return UIImage.FromFile (FlagFileNameForCountryCode (code));
			}

			string FlagFileNameForCountryCode(string code)
			{
				return "Flags/" + code + ".png";
			}
		}

	}
}

