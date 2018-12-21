namespace Tavisca.Connector.Hotels.Host
{
    public static class Constants
    {
        public const string Application = "connector_hotels";

        public static class WebApiRoute
        {
            public const string BaseRoute = "connector/Tourico/hotels/v1.0";
            public const string SearchRoute = "search";
            public const string RoomRatesRoute = "getroomrates";
            public const string RateRulesRoute = "getraterules";
            public const string BookRoute = "book";
            public const string CancelRoute = "cancel";
            public const string RetrieveRoute = "retrieve";
            public const string ConfigsSpecRoute = "getconfigurationspec";
            public const string MetaDataRoute = "getmetadata";
            public const string HealthCheckRoute = "healthcheck";
        }

        public static class Logging
        {
            public const string Firehose = "Firehose";
            public const string Redis = "Redis";
        }
    }
}
