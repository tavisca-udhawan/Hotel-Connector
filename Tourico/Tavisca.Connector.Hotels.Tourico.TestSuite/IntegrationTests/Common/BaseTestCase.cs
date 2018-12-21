using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Host;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.Retrieve;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Translators;
using Xunit;
using static Tavisca.Connector.Hotels.Host.Constants;
using static Tavisca.Connector.Hotels.Tourico.TestSuite.TestSuiteConstants;
using static Tavisca.Connector.Hotels.Common.Constants;

using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;
namespace Tavisca.Connector.Hotels.Tourico.TestSuite.IntegrationTests
{
    public class BaseTestCase
    {
        protected readonly TestServer _server;
        protected readonly HotelMetadata _hotelMetadata;
        protected readonly JsonSerializerSettings _globalSerializerSettings;       

        public BaseTestCase()
        {
            _server = new TestServer(new WebHostBuilder()
            .UseStartup<Startup>());
            IServiceProvider serviceProvider = _server.Host.Services;            
            var metadata = serviceProvider.GetService(typeof(IHotelMetadata)) as IHotelMetadata;    
            _hotelMetadata = Task.Run(async () => await metadata.GetMetadataAsync()).Result;            
            var serializerSettings = serviceProvider.GetService(typeof(ITranslatorOptions)) as ITranslatorOptions;
            _globalSerializerSettings = serializerSettings.GetSerializerSettings();
        }

        protected async Task<string> GetRateRulesResponse(RateRulesRequest rateRulesRequest)
        {
            var requestJson = JsonConvert.SerializeObject(rateRulesRequest, _globalSerializerSettings);
            var responseString = await GetHttpClientResponse(requestJson, WebApiRoute.RateRulesRoute);
            return responseString;
        }

        protected async Task<string> GetBookResponse(BookRequest bookRequest)
        {
            var requestJson = JsonConvert.SerializeObject(bookRequest, _globalSerializerSettings);
            var responseString = await GetHttpClientResponse(requestJson, WebApiRoute.BookRoute);
            return responseString;
        }

        protected async Task<string> GetSearchResponse(ConnectorSearch.SearchRequest searchRequest)
        {
            var requestJson = JsonConvert.SerializeObject(searchRequest, _globalSerializerSettings);
            var responseString = await GetHttpClientResponse(requestJson, WebApiRoute.SearchRoute);
            return responseString;
        }

        protected async Task<string> GetRoomRatesResponse(Request roomRatesRequest)
        {
            var requestJson = JsonConvert.SerializeObject(roomRatesRequest, _globalSerializerSettings);
            var responseString = await GetHttpClientResponse(requestJson, WebApiRoute.RoomRatesRoute);
            return responseString;
        }

        protected async Task<string> GetRetrieveResponse(RetrieveRequest retrieveRequest)
        {
            var requestJson = JsonConvert.SerializeObject(retrieveRequest, _globalSerializerSettings);
            var responseString = await GetHttpClientResponse(requestJson, WebApiRoute.RetrieveRoute);
            return responseString;
        }

        protected async Task<string> GetHttpClientResponse(string requestJson, string requestUri)
        {
            var stringContent = new StringContent(requestJson, Encoding.GetEncoding(0), HeadersValues.MediaType);

            HttpClient client = _server.CreateClient();
            client.DefaultRequestHeaders.Add(Headers.CorrelationId, HeadersValues.CorrelationIdValue);
            client.DefaultRequestHeaders.Add(Headers.Culture, HeadersValues.CultureValue);
            client.DefaultRequestHeaders.Add(Headers.TenantId, HeadersValues.TenantIdValue);
            client.DefaultRequestHeaders.Add(Headers.UserToken, HeadersValues.UserTokenValue);

            var response = await client.PostAsync(requestUri, stringContent);
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

        protected static void CheckIsTestPassed(ErrorInfo errorInfo, string errorCode, List<Info> infoList)
        {
            if (errorInfo?.Code == errorCode)
            {
                foreach (var info in infoList)
                {
                    var code = info.Code;
                    if (errorInfo.Info.Any(responseInfo => responseInfo.Code == code))
                        Assert.True(true);
                    else
                        Assert.True(false);
                }
            }
        }

        protected static bool CheckWarnings(List<Warning> warnings, string warningCode)
        {
            var isTestPassed = false;
            if (warnings == null) return false;

            if (warnings.Any(warning => warning.Code == warningCode))
                isTestPassed = true;

            return isTestPassed;
        }

    }
}
