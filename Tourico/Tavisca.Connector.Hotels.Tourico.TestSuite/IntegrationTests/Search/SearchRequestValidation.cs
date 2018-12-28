using System;
using System.Collections.Generic;
using System.Text;
using Tavisca.Connector.Hotels.Model.Common;
using Xunit;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests.Search
{
    public class SearchRequestValidation
    {
       [Fact]
       public void ValidateConfigurations()
        {
            var configurations = ValidationTestCases.GetConfigurations();
        }
    }
}
