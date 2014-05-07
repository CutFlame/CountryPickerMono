using System;
using System.Collections.Generic;
using System.Linq;

namespace CountryPickerMono
{
	/// <summary>
	/// Platform agnostic model for storing country codes and generating country names
	/// </summary>
	public class CountrySelectionModel
	{
		/// <summary>
		/// Gets the country names.
		/// </summary>
		public string[] CountryNames { get; private set; }

		/// <summary>
		/// Gets the country codes.
		/// </summary>
		public string[] CountryCodes { get; private set; }

		/// <summary>
		/// Gets the country names by country code.
		/// </summary>
		public Dictionary<string, string> CountryNamesByCode { get; private set; }

		/// <summary>
		/// Gets the country codes by country name.
		/// </summary>
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
