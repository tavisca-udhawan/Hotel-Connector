using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tavisca.Connector.Hotels.Tourico.Common.Proxy
{
    public class SupplierProxy
    {
        public class SupplierSearchRq {
            public List<string>HotelIdInfo { get; set; }
            public DateTime CheckIn { get; set; }
            public DateTime CheckOut { get; set; }
            public int AdultNum { get; set; }
            public int ChildNum { get; set; }
            public List<int> ChildAge { get; set; }
            public int MaxPrice { get; set; }
            public int StarLevel { get; set; }
            public string AvailableOnly { get; set; }
            public string Features { get; set; }


        }
        public class SupplierSearchRs { public List<string> Itineraries { get; set; } }
        public class SupplierRoomRatesRq { }
        public class SupplierRoomRatesRs { }
        public class SupplierRateRulesRq { }
        public class SupplierRateRulesRs { }
        public class SupplierBookingRq { }
        public class SupplierBookingRs { }
        public class SupplierCancelRq { }
        public class SupplierCancelRs { }
        public class SupplierRetrieveRq { }
        public class SupplierRetrieveRs { }

        public class ErrorTypes {
            public List<ErrorType> Errors { get; set; }
        }

        public class ErrorType
        {
            public string Text { get; }
            public string Code { get; }
        }
    }
}
