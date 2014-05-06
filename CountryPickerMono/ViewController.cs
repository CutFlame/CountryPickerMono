
using System;
using System.Drawing;

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

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			countryPicker.DidSelectCountry = HandleDidSelectCountry;
			
			// Perform any additional setup after loading the view, typically from a nib.
		}

		void HandleDidSelectCountry (string name, string code)
		{
			nameLabel.Text = name;
			codeLabel.Text = code;
		}
	}
}

