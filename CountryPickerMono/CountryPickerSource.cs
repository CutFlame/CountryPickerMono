using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CountryPickerMono
{
	/// <summary>
	/// Model (Source) for a UIPickerView to display countries and their flags
	/// </summary>
	public class CountryPickerSource : UIPickerViewModel
	{
		/// <summary>
		/// Action fired when a country is selected
		/// </summary>
		public Action DidSelectCountry;

		readonly CountrySelectionModel _model;

		public CountryPickerSource (CountrySelectionModel model)
		{
			_model = model;
		}

		public override int GetComponentCount (UIPickerView picker)
		{
			return 1;
		}

		public override int GetRowsInComponent (UIPickerView picker, int component)
		{
			return _model.CountryCodes.Length;
		}

		public override UIView GetView (UIPickerView picker, int row, int component, UIView view)
		{
			if (view == null)
			{
				view = new UIView (new RectangleF (0, 0, 280, 30));
				var label = new UILabel ();
				label.BackgroundColor = UIColor.Clear;
				label.Tag = 1;
				view.AddSubview (label);

				var flagView = new UIImageView ();
				flagView.ContentMode = UIViewContentMode.ScaleAspectFit;
				flagView.Tag = 2;
				view.AddSubview (flagView);

				view.AddConstraints (ConstraintsForFlagAndLabel (label, flagView));
			}

			((UILabel)view.ViewWithTag (1)).Text = GetValueAtIndexOrDefault (_model.CountryNames, row);
			var countryCodeOrBlank = GetValueAtIndexOrDefault (_model.CountryCodes, row, "Blank");
			((UIImageView)view.ViewWithTag (2)).Image = FlagForCountryCode (countryCodeOrBlank);
			return view;
		}

		NSLayoutConstraint[] ConstraintsForFlagAndLabel (UILabel label, UIImageView flagView)
		{
			label.TranslatesAutoresizingMaskIntoConstraints = false;
			flagView.TranslatesAutoresizingMaskIntoConstraints = false;

			var dictOfViews = NSDictionary.FromObjectsAndKeys
			(
				new object[] { label, flagView, },
				new string[] { "label", "flagView", }
			);
			var dictOfMetrics = NSDictionary.FromObjectsAndKeys
			(
				new object[] { 245, 24, 24, 3, },
				new string[] { "labelWidth", "flagHeight", "flagWidth", "veritcalPadding", }
			);

			var constraints = new List<NSLayoutConstraint> ();
			constraints.AddRange (NSLayoutConstraint.FromVisualFormat ("H:|-[flagView(flagWidth)]-[label]-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));
			constraints.AddRange (NSLayoutConstraint.FromVisualFormat ("V:|-(>=veritcalPadding)-[flagView(flagHeight)]-(>=veritcalPadding)-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));
			constraints.AddRange (NSLayoutConstraint.FromVisualFormat ("V:|-(>=veritcalPadding)-[label(flagHeight)]-(>=veritcalPadding)-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));

			dictOfMetrics.Dispose ();
			dictOfViews.Dispose ();
			return constraints.ToArray ();
		}

		UIImage FlagForCountryCode(string code)
		{
			return UIImage.FromFile (FlagFileNameForCountryCode (code));
		}

		string FlagFileNameForCountryCode(string code)
		{
			return "Flags/" + code + ".png";
		}

		public override void Selected (UIPickerView picker, int row, int component)
		{
			var handler = DidSelectCountry;
			if (handler != null)
			{
				handler ();
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
