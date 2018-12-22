using Tavisca.Connector.Hotels.Model.Common;

namespace Tavisca.Connector.Hotels.Tourico.Common
{
    public class SupplierConfiguration
    {

        public bool IsPublished { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string SearchUrl { get; set; }
        public string RoomRatesUrl { get; set; }
        public string RetrieveUrl { get; set; }
        public string RateRulesUrl { get; set; }
        public string BookingUrl { get; set; }
        public string CancelUrl { get; set; }
        public string BookingEmailId { get; set; }
        public int TimeOutInSeconds { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string Culture { get; set; }
        

    }


    internal static class SupplierConfigurationTranslator
    {
        internal static SupplierConfiguration GetConfigurations(this Supplier supplier)
        {
            ///Remove NotImplementedException and add supplier specificc configuration
            ///Please check below sample for same.
            //throw new System.NotImplementedException();

            var supplierConfiguration = new SupplierConfiguration
            {
                SearchUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.SearchURL),
                RoomRatesUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.RoomRatesURL),
                RateRulesUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.RateRulesURL),
                RetrieveUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.RetrieveURL),
                BookingUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.BookURL),
                CancelUrl = GetUrl(supplier, SupplierConstants.SupplierConfig.CancelURL),
                BookingEmailId = supplier[SupplierConstants.SupplierConfig.BookingEmail],
                TimeOutInSeconds = supplier[SupplierConstants.SupplierConfig.SupplierTimeOut] != null ? int.Parse(supplier[SupplierConstants.SupplierConfig.SupplierTimeOut]) : 180,
                Url = GetUrl(supplier, SupplierConstants.SupplierConfig.Url),
                UserName=supplier[SupplierConstants.SupplierConfig.UserName],
                Password=supplier[SupplierConstants.SupplierConfig.Password],
                UserId=supplier[SupplierConstants.SupplierConfig.UserId],
                Culture=supplier[SupplierConstants.SupplierConfig.Culture],
              //  IsPublished= bool.Parse(supplier[SupplierConstants.SupplierConfig.IsPublished])

            };
            return supplierConfiguration;
        }

        private static string GetUrl(Supplier supplier, string key)
        {
            if (!string.IsNullOrWhiteSpace(key) && !string.IsNullOrWhiteSpace(supplier[key]))
                return supplier[key];
            return EnvironmentUrl.GetStaticUrl(supplier.IsTestEnvironment, key);
        }
    }

    internal static class EnvironmentUrl
    {
        //<Critical - ToDo>Please supplier test and prod URLs here.
        //In case there is no url in resuest.configuration then url should be picked from here on the basis of isTestEnv

        private static readonly string testSearchUrl = "";
        private static readonly string prodSearchUrl = "";
               
        private static readonly string testRoomRatesUrl = "";
        private static readonly string prodRoomRatesUrl = "";
               
        private static readonly string testRateRulesUrl = "";
        private static readonly string prodRateRulesUrl = "";
               
        private static readonly string testBookingUrl = "";
        private static readonly string prodBookingUrl = "";
               
        private static readonly string testCancelUrl = "";
        private static readonly string prodCancelUrl = "";

        private static readonly string testRetrieveUrl = "";
        private static readonly string prodRetrieveUrl = "";

        private static readonly string testUrl = "http://demo-hotelws.touricoholidays.com/HotelFlow.svc/bas";
        private static readonly string prodUrl = "";

        internal static string GetStaticUrl(bool isTestEnv, string verb)
        {
            switch (verb)
            {
                case SupplierConstants.SupplierConfig.SearchURL:
                    return isTestEnv ? testSearchUrl : prodSearchUrl;
                case SupplierConstants.SupplierConfig.RoomRatesURL:
                    return isTestEnv ? testRoomRatesUrl : prodRoomRatesUrl;
                case SupplierConstants.SupplierConfig.RateRulesURL:
                    return isTestEnv ? testRateRulesUrl : prodRateRulesUrl;
                case SupplierConstants.SupplierConfig.BookURL:
                    return isTestEnv ? testBookingUrl : prodBookingUrl;
                case SupplierConstants.SupplierConfig.CancelURL:
                    return isTestEnv ? testCancelUrl : prodCancelUrl;
                case SupplierConstants.SupplierConfig.RetrieveURL:
                    return isTestEnv ? testRetrieveUrl : prodRetrieveUrl;
                case SupplierConstants.SupplierConfig.Url:
                    return isTestEnv ? testUrl : prodUrl;
                default:
                    return null;
            }
        }
    }
}
