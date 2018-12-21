using System;
using System.Collections.Generic;
using System.Text;
using Tavisca.Connector.Hotels.Model.Retrieve;

namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Retrieve
{
    public class MockRetrieveRequest : BaseMockRequest
    {
        public static RetrieveRequest BasicRetrieve_Request()
        {
            string supplierConfirmationNum = "123";
            return new RetrieveRequest(supplierConfirmationNum, CreateSupplierObject());
        }

        public static RetrieveRequest RetrieveByDate_Request()
        {
            //As the retrieve response is mocked, we have defined date range accordingly.
            DateTime fromDate = Convert.ToDateTime("2017-01-01");
            DateTime toDate = Convert.ToDateTime("2017-12-31");
            BookingDuration bookingDuration = new BookingDuration(fromDate, toDate);
            return new RetrieveRequest(bookingDuration, CreateSupplierObject());
        }

        public static RetrieveRequest CriteriaMissing_Request()
        {
            return new RetrieveRequest(string.Empty, CreateSupplierObject());
        }

        public static RetrieveRequest FromDateGreaterThanToDate_Request()
        {
            //As the retrieve response is mocked, we have defined date range accordingly.
            DateTime fromDate = Convert.ToDateTime("2017-08-28");
            DateTime toDate = Convert.ToDateTime("2017-06-28");
            BookingDuration bookingDuration = new BookingDuration(fromDate, toDate);
            return new RetrieveRequest(bookingDuration, CreateSupplierObject());
        }
    }
}
