using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Root
{
	public class UserLocationIP : IUserLocation
	{
		private class CountryInfoIP
		{
			public string ipAddress = string.Empty;

			public string countryCode = string.Empty;

			public string countryName = string.Empty;

			public string region = string.Empty;
		}

		private const string REQUEST_INFO_URL = "http://api.dtamobile.com/helper/get_country";

		private MonoBehaviour m_Coroutiner;

		private IJsonService m_Jsoner;

		private CountryInfoIP m_Info;

		private Dictionary<string, Dictionary<string, string>> m_RegionDB;

		private const string COUNTRY_REGION_DB = "{\r\n\t\t\t'gdpr': {\r\n\t\t        'ch': 'switzerland',\r\n\t\t        'ee': 'estonia',\r\n\t\t        'cz': 'czech republic',\r\n\t\t        'at': 'austria',\r\n\t\t        'ie': 'ireland',\r\n\t\t        'gr': 'greece',\r\n\t\t        'nl': 'netherlands',\r\n\t\t        'pt': 'portugal',\r\n\t\t        'lv': 'latvia',\r\n\t\t        'lt': 'lithuania',\r\n\t\t        'lu': 'luxembourg',\r\n\t\t        'es': 'spain',\r\n\t\t        'it': 'italy',\r\n\t\t        'ro': 'romania',\r\n\t\t        'pl': 'poland',\r\n\t\t        'be': 'belgium',\r\n\t\t        'fr': 'france',\r\n\t\t        'bg': 'bulgaria',\r\n\t\t        'dk': 'denmark',\r\n\t\t        'hr': 'croatia',\r\n\t\t        'de': 'germany',\r\n\t\t        'hu': 'hungary',\r\n\t\t        'fi': 'finland',\r\n\t\t        'cy': 'cyprus',\r\n\t\t        'sk': 'slovakia',\r\n\t\t        'mt': 'malta',\r\n\t\t        'si': 'slovenia',\r\n\t\t        'se': 'sweden',\r\n\t\t        'gb': 'united kingdom'\r\n\t        },\r\n\t        'europe': {\r\n\t\t        'va': 'vatican city',\r\n\t\t        'ch': 'switzerland',\r\n\t\t        'ad': 'andorra',\r\n\t\t        'ee': 'estonia',\r\n\t\t        'is': 'iceland',\r\n\t\t        'am': 'armenia',\r\n\t\t        'al': 'albania',\r\n\t\t        'cz': 'czech republic',\r\n\t\t        'ge': 'georgia',\r\n\t\t        'at': 'austria',\r\n\t\t        'ie': 'ireland',\r\n\t\t        'gi': 'gibraltar',\r\n\t\t        'gr': 'greece',\r\n\t\t        'nl': 'netherlands',\r\n\t\t        'pt': 'portugal',\r\n\t\t        'no': 'norway',\r\n\t\t        'lv': 'latvia',\r\n\t\t        'lt': 'lithuania',\r\n\t\t        'lu': 'luxembourg',\r\n\t\t        'es': 'spain',\r\n\t\t        'it': 'italy',\r\n\t\t        'ro': 'romania',\r\n\t\t        'pl': 'poland',\r\n\t\t        'be': 'belgium',\r\n\t\t        'fr': 'france',\r\n\t\t        'bg': 'bulgaria',\r\n\t\t        'dk': 'denmark',\r\n\t\t        'hr': 'croatia',\r\n\t\t        'de': 'germany',\r\n\t\t        'hu': 'hungary',\r\n\t\t        'ba': 'bosnia/herzegovina',\r\n\t\t        'fi': 'finland',\r\n\t\t        'by': 'belarus',\r\n\t\t        'fo': 'faeroe islands',\r\n\t\t        'mc': 'monaco',\r\n\t\t        'cy': 'cyprus',\r\n\t\t        'mk': 'macedonia',\r\n\t\t        'sk': 'slovakia',\r\n\t\t        'mt': 'malta',\r\n\t\t        'si': 'slovenia',\r\n\t\t        'sm': 'san marino',\r\n\t\t        'se': 'sweden',\r\n\t\t        'gb': 'united kingdom'\r\n\t        },\r\n\t        'oceania': {\r\n\t\t        'ck': 'cook islands',\r\n\t\t        'pw': 'palau',\r\n\t\t        'tv': 'tuvalu',\r\n\t\t        'na': 'nauru',\r\n\t\t        'ki': 'kiribati',\r\n\t\t        'mh': 'marshall islands',\r\n\t\t        'nu': 'niue',\r\n\t\t        'to': 'tonga',\r\n\t\t        'nz': 'new zealand',\r\n\t\t        'au': 'australia',\r\n\t\t        'vu': 'vanuatu',\r\n\t\t        'sb': 'solomon islands',\r\n\t\t        'ws': 'samoa',\r\n\t\t        'fj': 'fiji',\r\n\t\t        'fm': 'micronesia'\r\n\t        },\r\n\t        'africa': {\r\n\t\t        'gw': 'guinea-bissau',\r\n\t\t        'zm': 'zambia',\r\n\t\t        'ci': 'ivory coast',\r\n\t\t        'eh': 'western sahara',\r\n\t\t        'gq': 'equatorial guinea',\r\n\t\t        'eg': 'egypt',\r\n\t\t        'cg': 'congo',\r\n\t\t        'cf': 'central african republic',\r\n\t\t        'ao': 'angola',\r\n\t\t        'ga': 'gabon',\r\n\t\t        'et': 'ethiopia',\r\n\t\t        'gn': 'guinea',\r\n\t\t        'gm': 'gambia',\r\n\t\t        'zw': 'zimbabwe',\r\n\t\t        'cv': 'cape verde',\r\n\t\t        'gh': 'ghana',\r\n\t\t        'rw': 'rwanda',\r\n\t\t        'tz': 'tanzania',\r\n\t\t        'cm': 'cameroon',\r\n\t\t        'na': 'namibia',\r\n\t\t        'ne': 'niger',\r\n\t\t        'ng': 'nigeria',\r\n\t\t        'tn': 'tunisia',\r\n\t\t        'lr': 'liberia',\r\n\t\t        'ls': 'lesotho',\r\n\t\t        'tg': 'togo',\r\n\t\t        'td': 'chad',\r\n\t\t        'er': 'eritrea',\r\n\t\t        'ly': 'libya',\r\n\t\t        'bf': 'burkina faso',\r\n\t\t        'dj': 'djibouti',\r\n\t\t        'sl': 'sierra leone',\r\n\t\t        'bi': 'burundi',\r\n\t\t        'bj': 'benin',\r\n\t\t        'za': 'south africa',\r\n\t\t        'bw': 'botswana',\r\n\t\t        'dz': 'algeria',\r\n\t\t        'sz': 'swaziland',\r\n\t\t        'mg': 'madagascar',\r\n\t\t        'ma': 'morocco',\r\n\t\t        'ke': 'kenya',\r\n\t\t        'ml': 'mali',\r\n\t\t        'km': 'comoros',\r\n\t\t        'st': 'sao tome and principe',\r\n\t\t        'mu': 'mauritius',\r\n\t\t        'mw': 'malawi',\r\n\t\t        'so': 'somalia',\r\n\t\t        'sn': 'senegal',\r\n\t\t        'mr': 'mauritania',\r\n\t\t        'sc': 'seychelles',\r\n\t\t        'ug': 'uganda',\r\n\t\t        'sd': 'sudan',\r\n\t\t        'mz': 'mozambique'\r\n\t        },\r\n\t        'asia': {\r\n\t\t        'mn': 'mongolia',\r\n\t\t        'cn': 'china',\r\n\t\t        'af': 'afghanistan',\r\n\t\t        'am': 'armenia',\r\n\t\t        'vn': 'vietnam',\r\n\t\t        'ge': 'georgia',\r\n\t\t        'in': 'india',\r\n\t\t        'az': 'azerbaijan',\r\n\t\t        'id': 'indonesia',\r\n\t\t        'ru': 'russia',\r\n\t\t        'la': 'laos',\r\n\t\t        'tw': 'taiwan',\r\n\t\t        'tr': 'turkey',\r\n\t\t        'lk': 'sri lanka',\r\n\t\t        'tm': 'turkmenistan',\r\n\t\t        'tj': 'tajikistan',\r\n\t\t        'pg': 'papua new guinea',\r\n\t\t        'th': 'thailand',\r\n\t\t        'np': 'nepal',\r\n\t\t        'pk': 'pakistan',\r\n\t\t        'ph': 'philippines',\r\n\t\t        'bd': 'bangladesh',\r\n\t\t        'ua': 'ukraine',\r\n\t\t        'bn': 'brunei',\r\n\t\t        'jp': 'japan',\r\n\t\t        'bt': 'bhutan',\r\n\t\t        'hk': 'hong kong',\r\n\t\t        'kg': 'kyrgyzstan',\r\n\t\t        'uz': 'uzbekistan',\r\n\t\t        'mm': 'burma (myanmar)',\r\n\t\t        'sg': 'singapore',\r\n\t\t        'mo': 'macau',\r\n\t\t        'kh': 'cambodia',\r\n\t\t        'kr': 'korea',\r\n\t\t        'mv': 'maldives',\r\n\t\t        'kz': 'kazakhstan',\r\n\t\t        'my': 'malaysia'\r\n\t        },\r\n\t        'north america': {\r\n\t\t        'gt': 'guatemala',\r\n\t\t        'ag': 'antigua and barbuda',\r\n\t\t        'vg': 'british virgin islands (uk)',\r\n\t\t        'ai': 'anguilla (uk)',\r\n\t\t        'vi': 'virgin island',\r\n\t\t        'ca': 'canada',\r\n\t\t        'gd': 'grenada',\r\n\t\t        'aw': 'aruba (netherlands)',\r\n\t\t        'cr': 'costa rica',\r\n\t\t        'cu': 'cuba',\r\n\t\t        'pr': 'puerto rico (us)',\r\n\t\t        'ni': 'nicaragua',\r\n\t\t        'tt': 'trinidad and tobago',\r\n\t\t        'gp': 'guadeloupe (france)',\r\n\t\t        'pa': 'panama',\r\n\t\t        'do': 'dominican republic',\r\n\t\t        'dm': 'dominica',\r\n\t\t        'bb': 'barbados',\r\n\t\t        'ht': 'haiti',\r\n\t\t        'jm': 'jamaica',\r\n\t\t        'hn': 'honduras',\r\n\t\t        'bs': 'bahamas, the',\r\n\t\t        'bz': 'belize',\r\n\t\t        'sx': 'saint kitts and nevis',\r\n\t\t        'sv': 'el salvador',\r\n\t\t        'us': 'united states',\r\n\t\t        'mq': 'martinique (france)',\r\n\t\t        'ms': 'monsterrat (uk)',\r\n\t\t        'ky': 'cayman islands (uk)',\r\n\t\t        'mx': 'mexico'\r\n\t        },\r\n\t        'south america': {\r\n\t\t        'gd': 'south georgia',\r\n\t\t        'py': 'paraguay',\r\n\t\t        'co': 'colombia',\r\n\t\t        've': 'venezuela',\r\n\t\t        'cl': 'chile',\r\n\t\t        'sr': 'suriname',\r\n\t\t        'bo': 'bolivia',\r\n\t\t        'ec': 'ecuador',\r\n\t\t        'gf': 'french guiana',\r\n\t\t        'ar': 'argentina',\r\n\t\t        'gy': 'guyana',\r\n\t\t        'br': 'brazil',\r\n\t\t        'pe': 'peru',\r\n\t\t        'uy': 'uruguay',\r\n\t\t        'fk': 'falkland islands'\r\n\t        },\r\n\t        'middle east': {\r\n\t\t        'om': 'oman',\r\n\t\t        'lb': 'lebanon',\r\n\t\t        'iq': 'iraq',\r\n\t\t        'ye': 'yemen',\r\n\t\t        'ir': 'iran',\r\n\t\t        'bh': 'bahrain',\r\n\t\t        'sy': 'syria',\r\n\t\t        'qa': 'qatar',\r\n\t\t        'jo': 'jordan',\r\n\t\t        'kw': 'kuwait',\r\n\t\t        'il': 'israel',\r\n\t\t        'ae': 'united arab emirates',\r\n\t\t        'sa': 'saudi arabia'\r\n\t        }\r\n        }";

		public UserLocationIP(MonoBehaviour coroutiner, IJsonService jsoner)
		{
			m_Coroutiner = coroutiner;
			m_Jsoner = jsoner;
			m_RegionDB = m_Jsoner.FromJson<Dictionary<string, Dictionary<string, string>>>("{\r\n\t\t\t'gdpr': {\r\n\t\t        'ch': 'switzerland',\r\n\t\t        'ee': 'estonia',\r\n\t\t        'cz': 'czech republic',\r\n\t\t        'at': 'austria',\r\n\t\t        'ie': 'ireland',\r\n\t\t        'gr': 'greece',\r\n\t\t        'nl': 'netherlands',\r\n\t\t        'pt': 'portugal',\r\n\t\t        'lv': 'latvia',\r\n\t\t        'lt': 'lithuania',\r\n\t\t        'lu': 'luxembourg',\r\n\t\t        'es': 'spain',\r\n\t\t        'it': 'italy',\r\n\t\t        'ro': 'romania',\r\n\t\t        'pl': 'poland',\r\n\t\t        'be': 'belgium',\r\n\t\t        'fr': 'france',\r\n\t\t        'bg': 'bulgaria',\r\n\t\t        'dk': 'denmark',\r\n\t\t        'hr': 'croatia',\r\n\t\t        'de': 'germany',\r\n\t\t        'hu': 'hungary',\r\n\t\t        'fi': 'finland',\r\n\t\t        'cy': 'cyprus',\r\n\t\t        'sk': 'slovakia',\r\n\t\t        'mt': 'malta',\r\n\t\t        'si': 'slovenia',\r\n\t\t        'se': 'sweden',\r\n\t\t        'gb': 'united kingdom'\r\n\t        },\r\n\t        'europe': {\r\n\t\t        'va': 'vatican city',\r\n\t\t        'ch': 'switzerland',\r\n\t\t        'ad': 'andorra',\r\n\t\t        'ee': 'estonia',\r\n\t\t        'is': 'iceland',\r\n\t\t        'am': 'armenia',\r\n\t\t        'al': 'albania',\r\n\t\t        'cz': 'czech republic',\r\n\t\t        'ge': 'georgia',\r\n\t\t        'at': 'austria',\r\n\t\t        'ie': 'ireland',\r\n\t\t        'gi': 'gibraltar',\r\n\t\t        'gr': 'greece',\r\n\t\t        'nl': 'netherlands',\r\n\t\t        'pt': 'portugal',\r\n\t\t        'no': 'norway',\r\n\t\t        'lv': 'latvia',\r\n\t\t        'lt': 'lithuania',\r\n\t\t        'lu': 'luxembourg',\r\n\t\t        'es': 'spain',\r\n\t\t        'it': 'italy',\r\n\t\t        'ro': 'romania',\r\n\t\t        'pl': 'poland',\r\n\t\t        'be': 'belgium',\r\n\t\t        'fr': 'france',\r\n\t\t        'bg': 'bulgaria',\r\n\t\t        'dk': 'denmark',\r\n\t\t        'hr': 'croatia',\r\n\t\t        'de': 'germany',\r\n\t\t        'hu': 'hungary',\r\n\t\t        'ba': 'bosnia/herzegovina',\r\n\t\t        'fi': 'finland',\r\n\t\t        'by': 'belarus',\r\n\t\t        'fo': 'faeroe islands',\r\n\t\t        'mc': 'monaco',\r\n\t\t        'cy': 'cyprus',\r\n\t\t        'mk': 'macedonia',\r\n\t\t        'sk': 'slovakia',\r\n\t\t        'mt': 'malta',\r\n\t\t        'si': 'slovenia',\r\n\t\t        'sm': 'san marino',\r\n\t\t        'se': 'sweden',\r\n\t\t        'gb': 'united kingdom'\r\n\t        },\r\n\t        'oceania': {\r\n\t\t        'ck': 'cook islands',\r\n\t\t        'pw': 'palau',\r\n\t\t        'tv': 'tuvalu',\r\n\t\t        'na': 'nauru',\r\n\t\t        'ki': 'kiribati',\r\n\t\t        'mh': 'marshall islands',\r\n\t\t        'nu': 'niue',\r\n\t\t        'to': 'tonga',\r\n\t\t        'nz': 'new zealand',\r\n\t\t        'au': 'australia',\r\n\t\t        'vu': 'vanuatu',\r\n\t\t        'sb': 'solomon islands',\r\n\t\t        'ws': 'samoa',\r\n\t\t        'fj': 'fiji',\r\n\t\t        'fm': 'micronesia'\r\n\t        },\r\n\t        'africa': {\r\n\t\t        'gw': 'guinea-bissau',\r\n\t\t        'zm': 'zambia',\r\n\t\t        'ci': 'ivory coast',\r\n\t\t        'eh': 'western sahara',\r\n\t\t        'gq': 'equatorial guinea',\r\n\t\t        'eg': 'egypt',\r\n\t\t        'cg': 'congo',\r\n\t\t        'cf': 'central african republic',\r\n\t\t        'ao': 'angola',\r\n\t\t        'ga': 'gabon',\r\n\t\t        'et': 'ethiopia',\r\n\t\t        'gn': 'guinea',\r\n\t\t        'gm': 'gambia',\r\n\t\t        'zw': 'zimbabwe',\r\n\t\t        'cv': 'cape verde',\r\n\t\t        'gh': 'ghana',\r\n\t\t        'rw': 'rwanda',\r\n\t\t        'tz': 'tanzania',\r\n\t\t        'cm': 'cameroon',\r\n\t\t        'na': 'namibia',\r\n\t\t        'ne': 'niger',\r\n\t\t        'ng': 'nigeria',\r\n\t\t        'tn': 'tunisia',\r\n\t\t        'lr': 'liberia',\r\n\t\t        'ls': 'lesotho',\r\n\t\t        'tg': 'togo',\r\n\t\t        'td': 'chad',\r\n\t\t        'er': 'eritrea',\r\n\t\t        'ly': 'libya',\r\n\t\t        'bf': 'burkina faso',\r\n\t\t        'dj': 'djibouti',\r\n\t\t        'sl': 'sierra leone',\r\n\t\t        'bi': 'burundi',\r\n\t\t        'bj': 'benin',\r\n\t\t        'za': 'south africa',\r\n\t\t        'bw': 'botswana',\r\n\t\t        'dz': 'algeria',\r\n\t\t        'sz': 'swaziland',\r\n\t\t        'mg': 'madagascar',\r\n\t\t        'ma': 'morocco',\r\n\t\t        'ke': 'kenya',\r\n\t\t        'ml': 'mali',\r\n\t\t        'km': 'comoros',\r\n\t\t        'st': 'sao tome and principe',\r\n\t\t        'mu': 'mauritius',\r\n\t\t        'mw': 'malawi',\r\n\t\t        'so': 'somalia',\r\n\t\t        'sn': 'senegal',\r\n\t\t        'mr': 'mauritania',\r\n\t\t        'sc': 'seychelles',\r\n\t\t        'ug': 'uganda',\r\n\t\t        'sd': 'sudan',\r\n\t\t        'mz': 'mozambique'\r\n\t        },\r\n\t        'asia': {\r\n\t\t        'mn': 'mongolia',\r\n\t\t        'cn': 'china',\r\n\t\t        'af': 'afghanistan',\r\n\t\t        'am': 'armenia',\r\n\t\t        'vn': 'vietnam',\r\n\t\t        'ge': 'georgia',\r\n\t\t        'in': 'india',\r\n\t\t        'az': 'azerbaijan',\r\n\t\t        'id': 'indonesia',\r\n\t\t        'ru': 'russia',\r\n\t\t        'la': 'laos',\r\n\t\t        'tw': 'taiwan',\r\n\t\t        'tr': 'turkey',\r\n\t\t        'lk': 'sri lanka',\r\n\t\t        'tm': 'turkmenistan',\r\n\t\t        'tj': 'tajikistan',\r\n\t\t        'pg': 'papua new guinea',\r\n\t\t        'th': 'thailand',\r\n\t\t        'np': 'nepal',\r\n\t\t        'pk': 'pakistan',\r\n\t\t        'ph': 'philippines',\r\n\t\t        'bd': 'bangladesh',\r\n\t\t        'ua': 'ukraine',\r\n\t\t        'bn': 'brunei',\r\n\t\t        'jp': 'japan',\r\n\t\t        'bt': 'bhutan',\r\n\t\t        'hk': 'hong kong',\r\n\t\t        'kg': 'kyrgyzstan',\r\n\t\t        'uz': 'uzbekistan',\r\n\t\t        'mm': 'burma (myanmar)',\r\n\t\t        'sg': 'singapore',\r\n\t\t        'mo': 'macau',\r\n\t\t        'kh': 'cambodia',\r\n\t\t        'kr': 'korea',\r\n\t\t        'mv': 'maldives',\r\n\t\t        'kz': 'kazakhstan',\r\n\t\t        'my': 'malaysia'\r\n\t        },\r\n\t        'north america': {\r\n\t\t        'gt': 'guatemala',\r\n\t\t        'ag': 'antigua and barbuda',\r\n\t\t        'vg': 'british virgin islands (uk)',\r\n\t\t        'ai': 'anguilla (uk)',\r\n\t\t        'vi': 'virgin island',\r\n\t\t        'ca': 'canada',\r\n\t\t        'gd': 'grenada',\r\n\t\t        'aw': 'aruba (netherlands)',\r\n\t\t        'cr': 'costa rica',\r\n\t\t        'cu': 'cuba',\r\n\t\t        'pr': 'puerto rico (us)',\r\n\t\t        'ni': 'nicaragua',\r\n\t\t        'tt': 'trinidad and tobago',\r\n\t\t        'gp': 'guadeloupe (france)',\r\n\t\t        'pa': 'panama',\r\n\t\t        'do': 'dominican republic',\r\n\t\t        'dm': 'dominica',\r\n\t\t        'bb': 'barbados',\r\n\t\t        'ht': 'haiti',\r\n\t\t        'jm': 'jamaica',\r\n\t\t        'hn': 'honduras',\r\n\t\t        'bs': 'bahamas, the',\r\n\t\t        'bz': 'belize',\r\n\t\t        'sx': 'saint kitts and nevis',\r\n\t\t        'sv': 'el salvador',\r\n\t\t        'us': 'united states',\r\n\t\t        'mq': 'martinique (france)',\r\n\t\t        'ms': 'monsterrat (uk)',\r\n\t\t        'ky': 'cayman islands (uk)',\r\n\t\t        'mx': 'mexico'\r\n\t        },\r\n\t        'south america': {\r\n\t\t        'gd': 'south georgia',\r\n\t\t        'py': 'paraguay',\r\n\t\t        'co': 'colombia',\r\n\t\t        've': 'venezuela',\r\n\t\t        'cl': 'chile',\r\n\t\t        'sr': 'suriname',\r\n\t\t        'bo': 'bolivia',\r\n\t\t        'ec': 'ecuador',\r\n\t\t        'gf': 'french guiana',\r\n\t\t        'ar': 'argentina',\r\n\t\t        'gy': 'guyana',\r\n\t\t        'br': 'brazil',\r\n\t\t        'pe': 'peru',\r\n\t\t        'uy': 'uruguay',\r\n\t\t        'fk': 'falkland islands'\r\n\t        },\r\n\t        'middle east': {\r\n\t\t        'om': 'oman',\r\n\t\t        'lb': 'lebanon',\r\n\t\t        'iq': 'iraq',\r\n\t\t        'ye': 'yemen',\r\n\t\t        'ir': 'iran',\r\n\t\t        'bh': 'bahrain',\r\n\t\t        'sy': 'syria',\r\n\t\t        'qa': 'qatar',\r\n\t\t        'jo': 'jordan',\r\n\t\t        'kw': 'kuwait',\r\n\t\t        'il': 'israel',\r\n\t\t        'ae': 'united arab emirates',\r\n\t\t        'sa': 'saudi arabia'\r\n\t        }\r\n        }");
			m_Info = new CountryInfoIP();
		}

		public void RequestInfo(Action<bool> callback)
		{
			m_Coroutiner.StartCoroutine(IERequestInfo(callback));
		}

		private IEnumerator IERequestInfo(Action<bool> callback)
		{
			WWW www = new WWW("http://api.dtamobile.com/helper/get_country");
			yield return www;
			if (string.IsNullOrEmpty(www.error))
			{
				CountryInfoIP countryInfoIP = m_Jsoner.FromJson<CountryInfoIP>(www.text);
				if (countryInfoIP != null)
				{
					m_Info = countryInfoIP;
					callback(obj: true);
				}
				else
				{
					callback(obj: false);
				}
			}
			else
			{
				UnityEngine.Debug.Log("Error get location info: " + www.error);
				callback(obj: false);
			}
		}

		public string GetAddressInfo()
		{
			return m_Info.ipAddress;
		}

		public string GetCountryCode()
		{
			return m_Info.countryCode;
		}

		public string GetCountryName()
		{
			return m_Info.countryName;
		}

		public string GetRegion()
		{
			if (m_Info != null && !string.IsNullOrEmpty(m_Info.region))
			{
				return m_Info.region;
			}
			string countryCode = GetCountryCode();
			if (string.IsNullOrEmpty(countryCode))
			{
				return string.Empty;
			}
			countryCode = countryCode.ToLower();
			foreach (string key in m_RegionDB.Keys)
			{
				if (m_RegionDB[key].ContainsKey(countryCode))
				{
					return key;
				}
			}
			return string.Empty;
		}

		public bool IsRegionGDPR()
		{
			return GetRegion() == "gdpr";
		}
	}
}
