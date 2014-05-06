using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CountryPickerMono
{
	public partial class ViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone
		{
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}

		public ViewController ()
			: base ("ViewController", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			countryPicker.Setup (new CountrySelectionModel (NSLocale.ISOCountryCodes, NSLocale.CurrentLocale.GetCountryCodeDisplayName));
			countryPicker.DidSelectCountry = HandleDidSelectCountry;
		}

		void HandleDidSelectCountry (string name, string code)
		{
			nameLabel.Text = name;
			codeLabel.Text = code;
			var locale = countryPicker.SelectedLocale;
			Console.WriteLine ("[CountryCode: {0}, LanguageCode: {1}, Identifier: {2}]", locale.CountryCode, locale.LanguageCode, locale.Identifier);
		}
	}
}

