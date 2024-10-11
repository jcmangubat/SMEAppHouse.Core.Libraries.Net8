namespace SMEAppHouse.Core.CodeKits.Geo;

public sealed class GeoKit
{
    private static readonly List<CountryInfo> _countryInfos = [];
    public static List<CountryInfo> CountryInfos
    {
        get
        {
            if (_countryInfos == null || _countryInfos.Count == 0)
            {
                #region add the countries
                _countryInfos.Add(new CountryInfo(1, "AED", "United Arab Emirates", "Dirhams"));
                _countryInfos.Add(new CountryInfo(2, "AFN", "Afghanistan", "Afghanis"));
                _countryInfos.Add(new CountryInfo(3, "ALL", "Albania", "Leke"));
                _countryInfos.Add(new CountryInfo(4, "AMD", "Armenia", "Drams"));
                _countryInfos.Add(new CountryInfo(5, "ANG", "Netherlands Antilles", "Guilders (also called Florins)"));
                _countryInfos.Add(new CountryInfo(6, "AOA", "Angola", "Kwanza"));
                _countryInfos.Add(new CountryInfo(7, "ARS", "Argentina", "Pesos"));
                _countryInfos.Add(new CountryInfo(8, "AUD", "Australia", "Dollars"));
                _countryInfos.Add(new CountryInfo(9, "AWG", "Aruba", "Guilders (also called Florins)"));
                _countryInfos.Add(new CountryInfo(10, "AZN", "Azerbaijan", "New Manats"));
                _countryInfos.Add(new CountryInfo(11, "BAM", "Bosnia and Herzegovina", "Convertible Marka"));
                _countryInfos.Add(new CountryInfo(12, "BBD", "Barbados", "Dollars"));
                _countryInfos.Add(new CountryInfo(13, "BDT", "Bangladesh", "Taka"));
                _countryInfos.Add(new CountryInfo(14, "BGN", "Bulgaria", "Leva"));
                _countryInfos.Add(new CountryInfo(15, "BHD", "Bahrain", "Dinars"));
                _countryInfos.Add(new CountryInfo(16, "BIF", "Burundi", "Francs"));
                _countryInfos.Add(new CountryInfo(17, "BMD", "Bermuda", "Dollars"));
                _countryInfos.Add(new CountryInfo(18, "BND", "Brunei Darussalam", "Dollars"));
                _countryInfos.Add(new CountryInfo(19, "BOB", "Bolivia", "Bolivianos"));
                _countryInfos.Add(new CountryInfo(20, "BRL", "Brazil", "Brazil Real"));
                _countryInfos.Add(new CountryInfo(21, "BSD", "Bahamas", "Dollars"));
                _countryInfos.Add(new CountryInfo(22, "BTN", "Bhutan", "Ngultrum"));
                _countryInfos.Add(new CountryInfo(23, "BWP", "Botswana", "Pulas"));
                _countryInfos.Add(new CountryInfo(24, "BYR", "Belarus", "Rubles"));
                _countryInfos.Add(new CountryInfo(25, "BZD", "Belize", "Dollars"));
                _countryInfos.Add(new CountryInfo(26, "CAD", "Canada", "Dollars"));
                _countryInfos.Add(new CountryInfo(27, "CDF", "Congo/Kinshasa", "Congolese Francs"));
                _countryInfos.Add(new CountryInfo(28, "CHF", "Switzerland", "Francs"));
                _countryInfos.Add(new CountryInfo(29, "CLP", "Chile", "Pesos"));
                _countryInfos.Add(new CountryInfo(30, "CNY", "China", "Yuan Renminbi"));
                _countryInfos.Add(new CountryInfo(31, "COP", "Colombia", "Pesos"));
                _countryInfos.Add(new CountryInfo(32, "CRC", "Costa Rica", "Colones"));
                _countryInfos.Add(new CountryInfo(33, "CUP", "Cuba", "Pesos"));
                _countryInfos.Add(new CountryInfo(34, "CVE", "Cape Verde", "Escudos"));
                _countryInfos.Add(new CountryInfo(35, "CYP", "Cyprus", "Pounds (expires 2008-Jan-31)"));
                _countryInfos.Add(new CountryInfo(36, "CZK", "Czech Republic", "Koruny"));
                _countryInfos.Add(new CountryInfo(37, "DJF", "Djibouti", "Francs"));
                _countryInfos.Add(new CountryInfo(38, "DKK", "Denmark", "Kroner"));
                _countryInfos.Add(new CountryInfo(39, "DOP", "Dominican Republic", "Pesos"));
                _countryInfos.Add(new CountryInfo(40, "DZD", "Algeria", "Algeria Dinars"));
                _countryInfos.Add(new CountryInfo(41, "EEK", "Estonia", "Krooni"));
                _countryInfos.Add(new CountryInfo(42, "EGP", "Egypt", "Pounds"));
                _countryInfos.Add(new CountryInfo(43, "ERN", "Eritrea", "Nakfa"));
                _countryInfos.Add(new CountryInfo(44, "ETB", "Ethiopia", "Birr"));
                _countryInfos.Add(new CountryInfo(45, "EUR", "Euro Member Countries", "Euro"));
                _countryInfos.Add(new CountryInfo(46, "FJD", "Fiji", "Dollars"));
                _countryInfos.Add(new CountryInfo(47, "FKP", "Falkland Islands (Malvinas)", "Pounds"));
                _countryInfos.Add(new CountryInfo(48, "GBP", "United Kingdom", "Pounds"));
                _countryInfos.Add(new CountryInfo(49, "GEL", "Georgia", "Lari"));
                _countryInfos.Add(new CountryInfo(50, "GGP", "Guernsey", "Pounds"));
                _countryInfos.Add(new CountryInfo(51, "GHS", "Ghana", "Cedis"));
                _countryInfos.Add(new CountryInfo(52, "GIP", "Gibraltar", "Pounds"));
                _countryInfos.Add(new CountryInfo(53, "GMD", "Gambia", "Dalasi"));
                _countryInfos.Add(new CountryInfo(54, "GNF", "Guinea", "Francs"));
                _countryInfos.Add(new CountryInfo(55, "GTQ", "Guatemala", "Quetzales"));
                _countryInfos.Add(new CountryInfo(56, "GYD", "Guyana", "Dollars"));
                _countryInfos.Add(new CountryInfo(57, "HKD", "Hong Kong", "Dollars"));
                _countryInfos.Add(new CountryInfo(58, "HNL", "Honduras", "Lempiras"));
                _countryInfos.Add(new CountryInfo(59, "HRK", "Croatia", "Kuna"));
                _countryInfos.Add(new CountryInfo(60, "HTG", "Haiti", "Gourdes"));
                _countryInfos.Add(new CountryInfo(61, "HUF", "Hungary", "Forint"));
                _countryInfos.Add(new CountryInfo(62, "IDR", "Indonesia", "Rupiahs"));
                _countryInfos.Add(new CountryInfo(63, "ILS", "Israel", "New Shekels"));
                _countryInfos.Add(new CountryInfo(64, "IMP", "Isle of Man", "Pounds"));
                _countryInfos.Add(new CountryInfo(65, "INR", "India", "Rupees"));
                _countryInfos.Add(new CountryInfo(66, "IQD", "Iraq", "Dinars"));
                _countryInfos.Add(new CountryInfo(67, "IRR", "Iran", "Rials"));
                _countryInfos.Add(new CountryInfo(68, "ISK", "Iceland", "Kronur"));
                _countryInfos.Add(new CountryInfo(69, "JEP", "Jersey", "Pounds"));
                _countryInfos.Add(new CountryInfo(70, "JMD", "Jamaica", "Dollars"));
                _countryInfos.Add(new CountryInfo(71, "JOD", "Jordan", "Dinars"));
                _countryInfos.Add(new CountryInfo(72, "JPY", "Japan", "Yen"));
                _countryInfos.Add(new CountryInfo(73, "KES", "Kenya", "Shillings"));
                _countryInfos.Add(new CountryInfo(74, "KGS", "Kyrgyzstan", "Soms"));
                _countryInfos.Add(new CountryInfo(75, "KHR", "Cambodia", "Riels"));
                _countryInfos.Add(new CountryInfo(76, "KMF", "Comoros", "Francs"));
                _countryInfos.Add(new CountryInfo(77, "KPW", "Korea (North)", "Won"));
                _countryInfos.Add(new CountryInfo(78, "KRW", "Korea (South)", "Won"));
                _countryInfos.Add(new CountryInfo(79, "KWD", "Kuwait", "Dinars"));
                _countryInfos.Add(new CountryInfo(80, "KYD", "Cayman Islands", "Dollars"));
                _countryInfos.Add(new CountryInfo(81, "KZT", "Kazakhstan", "Tenge"));
                _countryInfos.Add(new CountryInfo(82, "LAK", "Laos", "Kips"));
                _countryInfos.Add(new CountryInfo(83, "LBP", "Lebanon", "Pounds"));
                _countryInfos.Add(new CountryInfo(84, "LKR", "Sri Lanka", "Rupees"));
                _countryInfos.Add(new CountryInfo(85, "LRD", "Liberia", "Dollars"));
                _countryInfos.Add(new CountryInfo(86, "LSL", "Lesotho", "Maloti"));
                _countryInfos.Add(new CountryInfo(87, "LTL", "Lithuania", "Litai"));
                _countryInfos.Add(new CountryInfo(88, "LVL", "Latvia", "Lati"));
                _countryInfos.Add(new CountryInfo(89, "LYD", "Libya", "Dinars"));
                _countryInfos.Add(new CountryInfo(90, "MAD", "Morocco", "Dirhams"));
                _countryInfos.Add(new CountryInfo(91, "MDL", "Moldova", "Lei"));
                _countryInfos.Add(new CountryInfo(92, "MGA", "Madagascar", "Ariary"));
                _countryInfos.Add(new CountryInfo(93, "MKD", "Macedonia", "Denars"));
                _countryInfos.Add(new CountryInfo(94, "MMK", "Myanmar (Burma)", "Kyats"));
                _countryInfos.Add(new CountryInfo(95, "MNT", "Mongolia", "Tugriks"));
                _countryInfos.Add(new CountryInfo(96, "MOP", "Macau", "Patacas"));
                _countryInfos.Add(new CountryInfo(97, "MRO", "Mauritania", "Ouguiyas"));
                _countryInfos.Add(new CountryInfo(98, "MTL", "Malta", "Liri (expires 2008-Jan-31)"));
                _countryInfos.Add(new CountryInfo(99, "MUR", "Mauritius", "Rupees"));
                _countryInfos.Add(new CountryInfo(100, "MVR", "Maldives (Maldive Islands)", "Rufiyaa"));
                _countryInfos.Add(new CountryInfo(101, "MWK", "Malawi", "Kwachas"));
                _countryInfos.Add(new CountryInfo(102, "MXN", "Mexico", "Pesos"));
                _countryInfos.Add(new CountryInfo(103, "MYR", "Malaysia", "Ringgits"));
                _countryInfos.Add(new CountryInfo(104, "MZN", "Mozambique", "Meticais"));
                _countryInfos.Add(new CountryInfo(105, "NAD", "Namibia", "Dollars"));
                _countryInfos.Add(new CountryInfo(106, "NGN", "Nigeria", "Nairas"));
                _countryInfos.Add(new CountryInfo(107, "NIO", "Nicaragua", "Cordobas"));
                _countryInfos.Add(new CountryInfo(108, "NOK", "Norway", "Krone"));
                _countryInfos.Add(new CountryInfo(109, "NPR", "Nepal", "Nepal Rupees"));
                _countryInfos.Add(new CountryInfo(110, "NZD", "New Zealand", "Dollars"));
                _countryInfos.Add(new CountryInfo(111, "OMR", "Oman", "Rials"));
                _countryInfos.Add(new CountryInfo(112, "PAB", "Panama", "Balboa"));
                _countryInfos.Add(new CountryInfo(113, "PEN", "Peru", "Nuevos Soles"));
                _countryInfos.Add(new CountryInfo(114, "PGK", "Papua New Guinea", "Kina"));
                _countryInfos.Add(new CountryInfo(115, "PHP", "Philippines", "Pesos"));
                _countryInfos.Add(new CountryInfo(116, "PKR", "Pakistan", "Rupees"));
                _countryInfos.Add(new CountryInfo(117, "PLN", "Poland", "Zlotych"));
                _countryInfos.Add(new CountryInfo(118, "PYG", "Paraguay", "Guarani"));
                _countryInfos.Add(new CountryInfo(119, "QAR", "Qatar", "Rials"));
                _countryInfos.Add(new CountryInfo(120, "RON", "Romania", "New Lei"));
                _countryInfos.Add(new CountryInfo(121, "RSD", "Serbia", "Dinars"));
                _countryInfos.Add(new CountryInfo(122, "RUB", "Russia", "Rubles"));
                _countryInfos.Add(new CountryInfo(123, "RWF", "Rwanda", "Rwanda Francs"));
                _countryInfos.Add(new CountryInfo(124, "SAR", "Saudi Arabia", "Riyals"));
                _countryInfos.Add(new CountryInfo(125, "SBD", "Solomon Islands", "Dollars"));
                _countryInfos.Add(new CountryInfo(126, "SCR", "Seychelles", "Rupees"));
                _countryInfos.Add(new CountryInfo(127, "SDG", "Sudan", "Pounds"));
                _countryInfos.Add(new CountryInfo(128, "SEK", "Sweden", "Kronor"));
                _countryInfos.Add(new CountryInfo(129, "SGD", "Singapore", "Dollars"));
                _countryInfos.Add(new CountryInfo(130, "SHP", "Saint Helena", "Pounds"));
                _countryInfos.Add(new CountryInfo(131, "SLL", "Sierra Leone", "Leones"));
                _countryInfos.Add(new CountryInfo(132, "SOS", "Somalia", "Shillings"));
                _countryInfos.Add(new CountryInfo(133, "SPL", "Seborga", "Luigini"));
                _countryInfos.Add(new CountryInfo(134, "SRD", "Suriname", "Dollars"));
                _countryInfos.Add(new CountryInfo(135, "STD", "São Tome and Principe", "Dobras"));
                _countryInfos.Add(new CountryInfo(136, "SVC", "El Salvador", "Colones"));
                _countryInfos.Add(new CountryInfo(137, "SYP", "Syria", "Pounds"));
                _countryInfos.Add(new CountryInfo(138, "SZL", "Swaziland", "Emalangeni"));
                _countryInfos.Add(new CountryInfo(139, "THB", "Thailand", "Baht"));
                _countryInfos.Add(new CountryInfo(140, "TJS", "Tajikistan", "Somoni"));
                _countryInfos.Add(new CountryInfo(141, "TMM", "Turkmenistan", "Manats"));
                _countryInfos.Add(new CountryInfo(142, "TND", "Tunisia", "Dinars"));
                _countryInfos.Add(new CountryInfo(143, "TOP", "Tonga", "Pa'anga"));
                _countryInfos.Add(new CountryInfo(144, "TRY", "Turkey", "New Lira"));
                _countryInfos.Add(new CountryInfo(145, "TTD", "Trinidad and Tobago", "Dollars"));
                _countryInfos.Add(new CountryInfo(146, "TVD", "Tuvalu", "Tuvalu Dollars"));
                _countryInfos.Add(new CountryInfo(147, "TWD", "Taiwan", "New Dollars"));
                _countryInfos.Add(new CountryInfo(148, "TZS", "Tanzania", "Shillings"));
                _countryInfos.Add(new CountryInfo(149, "UAH", "Ukraine", "Hryvnia"));
                _countryInfos.Add(new CountryInfo(150, "UGX", "Uganda", "Shillings"));
                _countryInfos.Add(new CountryInfo(151, "USD", "United States of America", "Dollars"));
                _countryInfos.Add(new CountryInfo(152, "UYU", "Uruguay", "Pesos"));
                _countryInfos.Add(new CountryInfo(153, "UZS", "Uzbekistan", "Sums"));
                _countryInfos.Add(new CountryInfo(154, "VEB", "Venezuela", "Bolivares (expires 2008-Jun-30)"));
                _countryInfos.Add(new CountryInfo(155, "VEF", "Venezuela", "Bolivares Fuertes"));
                _countryInfos.Add(new CountryInfo(156, "VND", "Viet Nam", "Dong"));
                _countryInfos.Add(new CountryInfo(157, "VUV", "Vanuatu", "Vatu"));
                _countryInfos.Add(new CountryInfo(158, "WST", "Samoa", "Tala"));
                _countryInfos.Add(new CountryInfo(159, "XAF", "Communauté Financière Africaine BEAC", "Francs"));
                _countryInfos.Add(new CountryInfo(160, "XAG", "Silver", "Ounces"));
                _countryInfos.Add(new CountryInfo(161, "XAU", "Gold", "Ounces"));
                _countryInfos.Add(new CountryInfo(162, "XCD", "East Caribbean", "Dollars"));
                _countryInfos.Add(new CountryInfo(163, "XDR", "International Monetary Fund (IMF)", "Special Drawing Rights"));
                _countryInfos.Add(new CountryInfo(164, "XOF", "Communauté Financière Africaine BCEAO", "Francs"));
                _countryInfos.Add(new CountryInfo(165, "XPD", "Palladium", "Ounces"));
                _countryInfos.Add(new CountryInfo(166, "XPF", "Comptoirs Français du Pacifique", "Francs"));
                _countryInfos.Add(new CountryInfo(167, "XPT", "Platinum", "Ounces"));
                _countryInfos.Add(new CountryInfo(168, "YER", "Yemen", "Rials"));
                _countryInfos.Add(new CountryInfo(169, "ZAR", "South Africa", "Rand"));
                _countryInfos.Add(new CountryInfo(170, "ZMK", "Zambia", "Kwacha"));
                _countryInfos.Add(new CountryInfo(171, "ZWD", "Zimbabwe", "Zimbabwe Dollars"));
                #endregion add the countries
            }
            return _countryInfos;
        }
    }

}

public class CountryInfo(int id, string code, string name, string currency)
{
    public int Id { get; private set; } = id;
    public string Code { get; private set; } = code;
    public string Name { get; private set; } = name;
    public string Currency { get; private set; } = currency;
}
