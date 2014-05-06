// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace CountryPickerMono
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		MonoTouch.UIKit.UILabel codeLabel { get; set; }

		[Outlet]
		CountryPickerMono.CountryPicker countryPicker { get; set; }

		[Outlet]
		MonoTouch.UIKit.UILabel nameLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (codeLabel != null) {
				codeLabel.Dispose ();
				codeLabel = null;
			}

			if (nameLabel != null) {
				nameLabel.Dispose ();
				nameLabel = null;
			}

			if (countryPicker != null) {
				countryPicker.Dispose ();
				countryPicker = null;
			}
		}
	}
}
