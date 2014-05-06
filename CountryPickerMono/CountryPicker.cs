using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using System.Linq;
using System.Drawing;

namespace CountryPickerMono
{
	[Register("CountryPicker")]
	public class CountryPicker : UIPickerView
	{
		public Action<string, string> DidSelectCountry;

		string[] _countryNames;

		public string[] CountryNames()
		{
			if(_countryNames == null)
			{
				_countryNames = CountryNamesByCode ().Values.Cast<string>().ToArray ();
			}
			return _countryNames;
		}

		string[] _countryCodes;

		public string[] CountryCodes()
		{
			if(_countryCodes==null)
			{
				_countryCodes = CountryCodesByName ().Values.Cast<string> ().ToArray ();
			}
			return _countryCodes;
		}

		NSDictionary _countryNamesByCode;

		public NSDictionary CountryNamesByCode()
		{
			if (_countryNamesByCode == null)
			{
				NSMutableDictionary namesByCode = new NSMutableDictionary ();
				foreach (var code in NSLocale.ISOCountryCodes)
				{
					var identifier = NSLocale.LocaleIdentifierFromComponents (NSDictionary.FromObjectAndKey (NSObject.FromObject (code), NSObject.FromObject ("NSLocalCountryCode")));
					var countryName = NSLocale.CurrentLocale.GetIdentifierDisplayName (identifier);
					if (countryName != null)
					{
						namesByCode.Add (NSObject.FromObject (code), NSObject.FromObject (countryName));
					}
				}
				_countryNamesByCode = (NSDictionary)namesByCode.Copy ();
			}
			return _countryNamesByCode;
		}

		NSDictionary _countryCodesByName;

		public NSDictionary CountryCodesByName()
		{
			if(_countryCodesByName==null)
			{
				NSDictionary countryNamesByCode = CountryNamesByCode ();
				NSMutableDictionary codesByName = new NSMutableDictionary ();
				foreach(var code in countryNamesByCode)
				{
					codesByName.Add (NSObject.FromObject (countryNamesByCode[NSObject.FromObject (code)]), NSObject.FromObject (code));
				}
				_countryCodesByName = (NSDictionary)codesByName.Copy ();
			}
			return _countryCodesByName;
		}

		void Setup()
		{
			var model = new CountryPickerModel(this);
			model.DidSelectCountry = CallDidSelectCountry;
			base.Source = model;
		}

		public CountryPicker (IntPtr handle) : base(handle)
		{
			Setup ();
		}

		public void SetSelectedCountryCode(string countryCode, bool animated = false)
		{
			int index = Array.IndexOf (CountryCodes (), countryCode);
			if(index >=0)
			{
				base.Select (index, 0, animated);
			}
		}

		public string SelectedCountryCode()
		{
			int index = base.SelectedRowInComponent (0);
			return CountryCodes () [index];
		}

		public void SetSelectedCountryName(string countryName, bool animated = false)
		{
			int index = Array.IndexOf (CountryNames (), countryName);
			if(index>=0)
			{
				Select (index, 0, animated);
			}
		}

		public string SelectedCountryName()
		{
			int index = SelectedRowInComponent (0);
			return CountryNames () [index];
		}

		public void SetSelectedLocale(NSLocale locale, bool animated = false)
		{
			SetSelectedCountryCode (locale.CountryCode, animated);
		}

		public NSLocale SelectedLocale()
		{
			var countryCode = SelectedCountryCode ();
			if(countryCode!=null)
			{
				var identifier = NSLocale.LocaleIdentifierFromComponents (NSDictionary.FromObjectAndKey (NSObject.FromObject (countryCode), NSObject.FromObject ("NSLocalCountryCode")));
				return NSLocale.FromLocaleIdentifier (identifier);
			}
			return null;
		}

		void CallDidSelectCountry(string name, string code)
		{
			var handler = DidSelectCountry;
			if(handler!=null)
			{
				handler (name, code);
			}
		}

		class CountryPickerModel : UIPickerViewModel
		{
			public Action<string, string> DidSelectCountry;

			CountryPicker _picker;

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
				return _picker.CountryCodes ().Length;
			}
			
			public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
			{
				if (view == null)
				{
					view = new UIView (new RectangleF (0, 0, 280, 30));
					UILabel label = new UILabel (new RectangleF (35, 3, 245, 24));
					label.BackgroundColor = UIColor.Clear;
					label.Tag = 1;
					view.AddSubview (label);

					UIImageView flagView = new UIImageView (new RectangleF (3, 3, 24, 24));
					flagView.ContentMode = UIViewContentMode.ScaleAspectFit;
					flagView.Tag = 2;
					view.AddSubview (flagView);
				}

				((UILabel)view.ViewWithTag (1)).Text = _picker.CountryNames ()[row];
				((UIImageView)view.ViewWithTag (2)).Image = UIImage.FromFile (_picker.CountryCodes ()[row]);
				return view;
			}

			public override void Selected (UIPickerView picker, int row, int component)
			{
				var handler = DidSelectCountry;
				if(handler!=null)
				{
					handler (_picker.CountryNames () [row], _picker.CountryCodes () [row]);
				}
			}
		}

	}
}

