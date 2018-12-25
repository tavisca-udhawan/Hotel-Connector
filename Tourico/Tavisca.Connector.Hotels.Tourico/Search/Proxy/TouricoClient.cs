using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Connector.Hotels.Model.Common;
using Tavisca.Connector.Hotels.Tourico.Common;
using Tavisca.Platform.Common.Logging;
using static Tavisca.Connector.Hotels.Tourico.Common.SupplierConstants;
namespace Tavisca.Connector.Hotels.Tourico.Search.Proxy
{
    public class TouricoClient
    {
        private readonly string _url;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _version;
        private readonly string _culture;
       // private readonly ExceptionMapper _exceptionMapper;

        public TouricoClient(SupplierConfiguration configurations)
        {
            _url = configurations.Url;
            _userName = configurations.UserName;
            _password = configurations.Password;
           // _version = Constants.APIVersion;
            _culture = configurations.Culture;
    //  _exceptionMapper = exceptionMapper;
        }
        
        public SearchResult GetHotels(SearchHotelsByIdRequest1 request)
        {
            var watch = new Stopwatch();
            object supplierResponse = null;
            StatusOptions status = StatusOptions.Failure;
            try
            {
                if (request != null)
                {
                    watch.Start();
                    var response = ExecuteCall(request);
                    supplierResponse = response;
                    status = StatusOptions.Success;
                    return response;
                }
            }
          
            catch (TimeoutException timeoutException)
            {
                supplierResponse = timeoutException;
                throw new SupplierTimeoutException(FaultCodes.TimeOut, FaultMessages.TimeOut, HttpStatusCode.GatewayTimeout, timeoutException);
            }
            catch (FaultException faultException)
            {
                supplierResponse = faultException;
                //_exceptionMapper.RaiseSupplierException(faultException, supplier);
            }
            catch (Exception exception)
            {
                supplierResponse = exception;
               // _exceptionMapper.RaiseSupplierException(exception, supplier);
            }

            finally
            {
                watch.Stop();
             // var logRequest = new Payload(ByteXmlHelper.ToByteArrayUsingXmlSerialization(request));
            // var logResponse = new Payload(ByteXmlHelper.ToByteArrayUsingXmlSerialization(supplierResponse));
               // ConnectorLogger.WriteApiLogAsync(logRequest, logResponse, SupplierConstants.SearchConstants.Api, SupplierConstants.SearchConstants.Verb, SupplierConstants.SearchConstants.MethodName,
               //     status, watch.ElapsedMilliseconds, _url, 0, null, GetRequestHeaders(), null, null, null, supplier.Id, supplier.Name);
            }
            return null;
        }

        private SearchResult ExecuteCall(SearchHotelsByIdRequest1 request)
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            var address = new EndpointAddress(_url);
            var service = new HotelFlowClient(binding, address);
            return service.SearchHotelsById(GetAuthenticationHeaders(),request.request, new[] { new Feature { name = "TaxBreakdown", value = "true" } });
        }
        private AuthenticationHeader GetAuthenticationHeaders()
        {
           return new AuthenticationHeader
            {
                LoginName = _userName,
                Password = _password,
                Version = _version,
                Culture = Culture.en_US
            };
        }        
    }
}
