using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryPickerMono
{
	public class CountrySelectionModel
	{
		public string[] CountryNames { get; private set; }

		public string[] CountryCodes { get; private set; }

		public Dictionary<string, string> CountryNamesByCode { get; private set; }

		public Dictionary<string, string> CountryCodesByName { get; private set; }

		public CountrySelectionModel(string[] countryCodes, Func<string, string> convertCodeToName)
		{
			Setup (countryCodes, convertCodeToName);
		}

		void Setup (string[] countryCodes, Func<string, string> convertCodeToName)
		{
			CountryNamesByCode = new Dictionary<string, string> ();
			CountryCodesByName = new Dictionary<string, string> ();

			//NSLocale.ISOCountryCodes
			foreach (var code in countryCodes)
			{
				if (!string.IsNullOrWhiteSpace (code))
				{
					//NSLocale.CurrentLocale.GetCountryCodeDisplayName
					string countryName = convertCodeToName (code);
					if (!string.IsNullOrWhiteSpace (countryName))
					{
						CountryNamesByCode.Add (code, countryName);
						CountryCodesByName.Add (countryName, code);
					}
				}
			}
			CountryCodes = CountryCodesByName.Values.ToArray ();
			CountryNames = CountryNamesByCode.Values.ToArray ();
		}
	}
}
