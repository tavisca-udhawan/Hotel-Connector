namespace Tavisca.Connector.Hotels.Tourico.Common
{
    public static class SupplierConstants
    {
        public static readonly string ApplicationName = "connector-hotels";

        public static class SupplierConfig
        {
            public const string SearchURL = "searchUrl";
            public const string RoomRatesURL = "roomRatesUrl";
            public const string RateRulesURL = "rateRulesUrl";
            public const string BookURL = "bookUrl";
            public const string RetrieveURL = "retrieveUrl";
            public const string CancelURL = "cancelUrl";
            public const string BookingEmail = "bookingEmail";
            public const string UserId = "userId";
            public const string Password = "password";
            public const string SupplierTimeOut = "supplierTimeOut";
            public const string UserName = "userName";
            public const string Url = "url";
            public const string Culture = "culture";
            public const string IsPublished = "isPublish";
        }

        public static class Keys
        {
            public static readonly string Api = "api";
            public static readonly string Verb = "verb";
            public static readonly string MethodName = "method_name";
            public static readonly string ConnectorId = "connector_id";
            public static readonly string SupplierId = "supplier_id";
            public static readonly string SupplierName = "supplier_name";
            public static readonly string IsTestEnvironment = "is_test_environment";
            public static readonly string HttpStatusCode = "http_status_code";
            public static readonly string HttpMethod = "http_method";
            public static readonly string TransactionId = "oski-transactionId";
            public static readonly string SessionId = "session_id";
            public static readonly string UserIdentifier = "user_identifier";
        }

        public static class Logging
        {
            public static readonly string SupplierErrorCode = "supplier_error_code";
            public static readonly string SupplierErrorMessage = "supplier_error_message";
            public static readonly string ErrorCode = "error_code";
            public static readonly string ErrorMessage = "error_message";
            public static readonly string IsUnMapped = "is_unmapped";
            public static readonly string SessionId = "session_id";
        }

        public static class CommonConstants
        {
            public const string AcceptEncoding = "accept-encoding";
            public const string AcceptEncodingValue = "gzip, deflate";
        }

        public static class SearchConstants
        {
            // ConnectorCode: change Api and verb value as per the connector
            public const string Api = "Tourico{supplier_api_version}";
            public const string Verb = "{supplier_api_callType}"; //example: roomAvailability
            public const string MethodName = "search";
        }

        public static class RoomRatesConstants
        {
            // ConnectorCode: change Api and verb value as per the connector
            public const string Api = "Tourico{supplier_api_version}";
            public const string Verb = "{supplier_api_callType}"; //example: roomAvailability
            public const string MethodName = "roomRates";
        }

        public static class RateRulesConstants
        {
            // ConnectorCode: change Api and verb value as per the connector
            public const string Api = "Tourico{supplier_api_version}";
            public const string Verb = "{supplier_api_callType}"; //example: roomAvailability
            public const string MethodName = "rateRules";
        }

        public static class BookConstants
        {
            // ConnectorCode: change Api and verb value as per the connector
            public const string Api = "Tourico{supplier_api_version}";
            public const string Verb = "{supplier_api_callType}"; //example: roomAvailability
            public const string MethodName = "book";
        }

        public static class CancelConstants
        {
            // ConnectorCode: change Api and verb value as per the connector
            public const string Api = "Tourico{supplier_api_version}";
            public const string Verb = "{supplier_api_callType}"; //example: roomAvailability
            public const string MethodName = "cancel";
        }

        public static class RetrieveConstants
        {
            // ConnectorCode: change Api value as per the connector
            public const string Api = "bookings.getBookingDetails?";
            public const string Verb = "retrieve";
            public const string MethodName = "connector_retrieve_supplierlog";
            public const string TraceLogMethodName = "connector_retrieve_tracelog";
        }
    }
}
