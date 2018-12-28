using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Model.Common;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.UnitTest
{
    [ExcludeFromCodeCoverage]
    public class MockRequestCreator
    {
        public static ConnectorSearch.SearchRequest CreateRequest(Supplier supplier = null)
        {
            if (supplier == null)
                supplier = GetSupplier();
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithInvalidStayDuration()
        {
            var criteria = new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(90), new List<string>
                                                                                                  {
                                                                                                      "1234745",
                                                                                                      "1988",
                                                                                                      "8146"
                                                                                                  }, GetOccupancy());
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), criteria, GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithNoAdults()
        {
            var occupancy = new List<RequestOccupancy>
                   {
                       new RequestOccupancy(0, null)
                   };

            var criteria = new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "1234745",
                                                                                                      "1988",
                                                                                                      "8146"
                                                                                                  }, occupancy);

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), criteria, GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }
        public static ConnectorSearch.SearchRequest CreateRequestWithMissingUrlInConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("userName", "abc"));
            configurations.Add(new Configuration("password", "111111"));
            configurations.Add(new Configuration("version", "7"));

            var supplier = new Supplier("Tourico", "Tourico", true, configurations,
                new SupplierSpecificOption("en-US", null, null, null, null));

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithMissingUserNameInConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "http://demo-hotelws.touricoholidays.com/HotelFlow.svc/bas"));
            configurations.Add(new Configuration("password", "111111"));
            configurations.Add(new Configuration("version", "7"));

            var supplier = new Supplier("Tourico", "Tourico", true, configurations,
                new SupplierSpecificOption("en-US", null, null, null, null));

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithMissingPwdInConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "host8408"));
            configurations.Add(new Configuration("userName", "abc"));
            configurations.Add(new Configuration("version", "7"));

            var supplier = new Supplier("Tourico", "Tourico", true, configurations,
                new SupplierSpecificOption("en-US", null, null, null, null));

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithMissingPwdAndVersionInConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "host8408"));
            configurations.Add(new Configuration("userName", "abc"));

            var supplier = new Supplier("Tourico", "Tourico", true, configurations,
                new SupplierSpecificOption("en-US", null, null, null, null));

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithMissingVersionInConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "host8408"));
            configurations.Add(new Configuration("userName", "abc"));
            configurations.Add(new Configuration("password", "111111"));

            var supplier = new Supplier("Tourico", "Tourico", true, configurations,
                new SupplierSpecificOption("en-US", null, null, null, null));

            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), supplier, null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithNullOptionalFields()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), GetSupplier(), null,
                null);

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithInvalidHotelIds()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteriaWithInvalidHotelIds(), GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }


        public static ConnectorSearch.SearchRequest CreateRequestWithIncorrectHotelIds()
        {
            var criteria = new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "1234"
                                                                                                  }, GetOccupancy());
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), criteria, GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithBothValidAndInvalidHotelIds()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteriaWithBothValidAndInvalidHotelIds(), GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }

        private static Criteria GetCriteriaWithBothValidAndInvalidHotelIds()
        {
            return new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "123", "124", "abc"
                                                                                                  }, GetOccupancy());
        }

        private static Criteria GetCriteriaWithInvalidHotelIds()
        {
            return new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "abc"
                                                                                                  }, GetOccupancy());
        }

        public static ConnectorSearch.SearchRequest CreateMultiroomRequest()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetMultiroomCriteria(), GetSupplier(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithEmptyCulture()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), GetSupplierWithEmptyCulture(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithWithInvalidConfigurations()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), GetSupplierWithInvalidConfigurations(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static ConnectorSearch.SearchRequest CreateRequestWithWithInvalidPwdInConfigurations()
        {
            return new ConnectorSearch.SearchRequest(Guid.NewGuid().ToString(), GetCriteria(), GetSupplierWithInvalidPwdInConfigurations(), null,
                new List<OptionalField> { OptionalField.All });

        }

        public static Supplier GetSupplierWithInvalidPwdInConfigurations()
        {
            return new Supplier("Tourico", "Tourico", true, GetInvalidPassword(),
                new SupplierSpecificOption("en-US", null, null, null, null));

        }

        private static Criteria GetMultiroomCriteria()
        {
            return new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "1234745",
                                                                                                      "1988",
                                                                                                      "8146"
                                                                                                  }, GetMultiRoomOccupancy());
        }

        private static List<RequestOccupancy> GetMultiRoomOccupancy()
        {
            return new List<RequestOccupancy>
                   {
                       new RequestOccupancy(1, new List<int> { 5 }),
                       new RequestOccupancy(2, new List<int> { 6 })
                   };
        }

        public static Supplier GetSupplier()
        {
            return new Supplier("Tourico", "Tourico", true, GetConfigurations(),
                new SupplierSpecificOption("en-US", null, null, null, null));

        }

        public static Supplier GetSupplierWithInvalidConfigurations()
        {
            return new Supplier("Tourico", "Tourico", true, GetInvalidSupplierConfigurations(),
                new SupplierSpecificOption("en-US", null, null, null, null));

        }

        public static Supplier GetSupplierWithEmptyCulture()
        {
            return new Supplier("Tourico", "Tourico", true, GetConfigurations(),
                new SupplierSpecificOption("en-US", null, null, null, null));

        }

        internal static List<Configuration> GetInvalidSupplierConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "abcd"));
            configurations.Add(new Configuration("userName", "host8408"));
            configurations.Add(new Configuration("password", "111111"));
            configurations.Add(new Configuration("version", "7"));
            return configurations;
        }

        internal static List<Configuration> GetInvalidPassword()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "http://demo-hotelws.touricoholidays.com/HotelFlow.svc/bas"));
            configurations.Add(new Configuration("userName", "host8408"));
            configurations.Add(new Configuration("password", "787"));
            configurations.Add(new Configuration("version", "7"));
            return configurations;
        }

        private static List<Configuration> GetConfigurations()
        {
            var configurations = new List<Configuration>();
            configurations.Add(new Configuration("url", "http://demo-hotelws.touricoholidays.com/HotelFlow.svc/bas"));
            configurations.Add(new Configuration("userName", "host8408"));
            configurations.Add(new Configuration("password", "111111"));
            configurations.Add(new Configuration("version", "7"));
            return configurations;
        }

        private static Criteria GetCriteria()
        {
            return new Criteria(DateTime.Now.AddDays(30), DateTime.Now.AddDays(35), new List<string>
                                                                                                  {
                                                                                                      "1234745",
                                                                                                      "1988",
                                                                                                      "8146"
                                                                                                  }, GetOccupancy());
        }

        private static List<RequestOccupancy> GetOccupancy()
        {
            return new List<RequestOccupancy>
                   {
                       new RequestOccupancy(1, null),
                       new RequestOccupancy(2, new List<int> { 3 })
                   };
        }
    }
}
