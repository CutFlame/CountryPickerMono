using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace CountryPickerMono
{
	
	public class CountryPickerSource : UIPickerViewModel
	{
		public Action<string, string> DidSelectCountry;

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
				var label = new UILabel (new RectangleF (35, 3, 245, 24));
				label.BackgroundColor = UIColor.Clear;
				label.Tag = 1;
				view.AddSubview (label);

				var flagView = new UIImageView (new RectangleF (3, 3, 24, 24));
				flagView.ContentMode = UIViewContentMode.ScaleAspectFit;
				flagView.Tag = 2;
				view.AddSubview (flagView);
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
				handler (_model.CountryNames [row], _model.CountryCodes [row]);
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
