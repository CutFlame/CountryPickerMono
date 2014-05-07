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
			countryPicker.Setup ();
			countryPicker.SetSelectedLocale (NSLocale.CurrentLocale);
			countryPicker.DidSelectCountry += HandleDidSelectCountry;
		}

		void HandleDidSelectCountry (object sender, EventArgs e)
		{
			nameLabel.Text = countryPicker.SelectedCountryName;
			codeLabel.Text = countryPicker.SelectedCountryCode;
			var locale = countryPicker.SelectedLocale;
			Console.WriteLine ("[CountryCode: {0}, LanguageCode: {1}, Identifier: {2}]", locale.CountryCode, locale.LanguageCode, locale.Identifier);
		}

		protected override void Dispose (bool disposing)
		{
			if(disposing)
			{
				if(countryPicker!=null)
				{
					countryPicker.DidSelectCountry -= HandleDidSelectCountry;
				}
			}
			base.Dispose (disposing);
		}
	}
}

