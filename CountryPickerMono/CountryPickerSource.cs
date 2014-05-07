using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace CountryPickerMono
{
	
	public class CountryPickerSource : UIPickerViewModel
	{
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

				label.TranslatesAutoresizingMaskIntoConstraints = false;
				flagView.TranslatesAutoresizingMaskIntoConstraints = false;
				var dictOfViews = NSDictionary.FromObjectsAndKeys (new object[]{ label, flagView }, new string[]{ "label", "flagView" });
				var dictOfMetrics = NSDictionary.FromObjectsAndKeys (new object[]{ 245, 24, 24, 3, }, new string[]{ "labelWidth", "flagHeight", "flagWidth", "veritcalPadding", });
				view.AddConstraints (NSLayoutConstraint.FromVisualFormat ("H:|-[flagView(flagWidth)]-[label]-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));
				view.AddConstraints (NSLayoutConstraint.FromVisualFormat ("V:|-(>=veritcalPadding)-[flagView(flagHeight)]-(>=veritcalPadding)-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));
				view.AddConstraints (NSLayoutConstraint.FromVisualFormat ("V:|-(>=veritcalPadding)-[label(flagHeight)]-(>=veritcalPadding)-|", NSLayoutFormatOptions.DirectionLeadingToTrailing, dictOfMetrics, dictOfViews));
				dictOfMetrics.Dispose ();
				dictOfViews.Dispose ();
			}

			((UILabel)view.ViewWithTag (1)).Text = _model.CountryNames[row];
			((UIImageView)view.ViewWithTag (2)).Image = FlagForCountryCode(_model.CountryCodes[row]);
			return view;
		}

		public override void Selected (UIPickerView picker, int row, int component)
		{
			var handler = DidSelectCountry;
			if (handler != null)
			{
				handler ();
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
