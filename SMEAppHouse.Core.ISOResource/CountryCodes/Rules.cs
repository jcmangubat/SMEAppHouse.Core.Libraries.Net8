using System;
using System.Collections.Generic;
using System.Text;

namespace SMEAppHouse.Core.ISOResource.CountryCodes
{
    public class Rules
    {
        public enum Continent { Af, An, As, Empty, Eu, Na, Oc, Sa };

        public enum DevelopedDevelopingCountries { Developed, Developing, Empty };

        public enum GlobalCode { Empty, True };

        public enum GlobalName { Empty, World };

        public enum IntermediateRegionName { Caribbean, CentralAmerica, ChannelIslands, EasternAfrica, Empty, MiddleAfrica, SouthAmerica, SouthernAfrica, WesternAfrica };

        public enum Iso4217CurrencyMinorUnitEnum { Empty, The22 };

        public enum LandLockedDevelopingCountriesLldc { Empty, X };

        public enum RegionName { Africa, Americas, Asia, Empty, Europe, Oceania };
    }
}
