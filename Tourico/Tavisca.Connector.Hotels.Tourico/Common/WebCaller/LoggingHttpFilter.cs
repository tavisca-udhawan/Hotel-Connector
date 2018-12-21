using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Connector.Hotels.ErrorHandling.Exceptions;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Logging;
using Tavisca.Platform.Common.Models;

namespace Tavisca.Connector.Hotels.Tourico.Common.WebCaller
{
    public class LoggingHttpFilter : HttpFilter
    {
        public LoggingHttpFilter(bool enableSanitization = false)
        {
            _enableSanitization = enableSanitization;
        }
        private readonly bool _enableSanitization;

        public async override Task<HttpResponse> ApplyAsync(HttpRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            HttpResponse httpResponse = null;
            Exception fault = null;
            string transactionId = Guid.NewGuid().ToString();
            var watch = Stopwatch.StartNew();
            try
            {
                httpResponse = await base.ApplyAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                object apiName;
                request.LogData.TryGetValue(SupplierConstants.Keys.Api, out apiName);
                var svcEx = new SupplierException(FaultCodes.ServiceCommunication, ErrorMessages.ServiceCommunication(apiName.ToString()), HttpStatusCode.InternalServerError, ex);
                fault = svcEx;
                throw svcEx;
            }
            finally
            {
                watch.Stop();
                var faultPolicy = request.FaultPolicy ?? DefaultFaultPolicy.Instance;
                var isSuccessful = fault != null ? false : await faultPolicy.IsFaultedAsync(request, httpResponse);
                var apiLog = CreateApiLog(request, httpResponse, isSuccessful, watch.Elapsed, transactionId);
                await Logger.WriteLogAsync(apiLog);
                if (fault != null)
                {
                    var exceptionLog = CreateExceptionLog(fault, transactionId);
                    await Logger.WriteLogAsync(exceptionLog);
                }
            }
            return httpResponse;
        }

        private ILog CreateApiLog(HttpRequest request, HttpResponse response, bool isSuccessful, TimeSpan timeTaken, string transactionId)
        {
            var apiLog = new ApiLog
            {
                ApplicationName = string.IsNullOrWhiteSpace(CallContext.Current?.ApplicationName) ? SupplierConstants.ApplicationName : CallContext.Current.ApplicationName,
                CorrelationId = CallContext.Current?.CorrelationId,
                StackId = CallContext.Current?.StackId,
                TenantId = CallContext.Current?.TenantId,
                ApplicationTransactionId = CallContext.Current?.TransactionId,
                ClientIp = CallContext.Current?.IpAddress,
                TimeTakenInMs = timeTaken.TotalMilliseconds,
                Url = request.Uri.AbsoluteUri,
                IsSuccessful = isSuccessful
            };
            string txId;
            if (response != null && response.Headers.TryGetValue(HeaderNames.TransactionId, out txId))
                apiLog.TransactionId = txId;
            else
                apiLog.TransactionId = transactionId;
            if (!string.IsNullOrWhiteSpace(CallContext.Current?.SessionId))
                apiLog.SetValue(SupplierConstants.Keys.SessionId, CallContext.Current?.SessionId);
            apiLog.SetValue(SupplierConstants.Keys.HttpMethod, request.Method);
            if (!string.IsNullOrWhiteSpace(CallContext.Current?.SessionId))
                apiLog.SetValue(SupplierConstants.Keys.SessionId, CallContext.Current?.SessionId);
            if (!string.IsNullOrEmpty(CallContext.Current?.UserToken))
                apiLog.SetValue(SupplierConstants.Keys.UserIdentifier, CallContext.Current?.UserToken);

            SetLogFieldsFromRequest(request, ref apiLog);
            SetLogFieldsFromResponse(response, ref apiLog);

            if (_enableSanitization)
                apiLog.EnableSanitization();

            return apiLog;
        }

        private void SetLogFieldsFromRequest(HttpRequest request, ref ApiLog apiLog)
        {
            if (request?.Payload != null)
                apiLog.Request = new Payload(request.Payload);

            if (request?.Headers != null)
                foreach (var header in request.Headers)
                    apiLog.RequestHeaders[header.Key] = header.Value;

            if (request?.LogData != null)
                foreach (var logData in request.LogData)
                    apiLog.TrySetValue(logData.Key, logData.Value);
        }

        private void SetLogFieldsFromResponse(HttpResponse response, ref ApiLog apiLog)
        {
            if (response != null)
            {
                apiLog.SetValue(SupplierConstants.Keys.HttpStatusCode, (int)response.Status);

                if (response.Payload != null)
                    apiLog.Response = new Payload(response.Payload);

                if (response.Headers != null)
                    foreach (var header in response.Headers)
                        apiLog.ResponseHeaders[header.Key] = header.Value;

                if (response.LogData != null)
                    foreach (var logData in response.LogData)
                        apiLog.TrySetValue(logData.Key, logData.Value);
            }
        }

        private ILog CreateExceptionLog(Exception ex, string transactionId)
        {
            var exceptionLog = new ExceptionLog(ex)
            {
                ApplicationName = string.IsNullOrWhiteSpace(CallContext.Current?.ApplicationName) ? SupplierConstants.ApplicationName : CallContext.Current.ApplicationName,
                CorrelationId = CallContext.Current?.CorrelationId,
                StackId = CallContext.Current?.StackId,
                TenantId = CallContext.Current?.TenantId,
                ApplicationTransactionId = CallContext.Current?.TransactionId
            };
            exceptionLog.SetValue(SupplierConstants.Keys.SessionId, CallContext.Current?.SessionId);
            exceptionLog.SetValue(SupplierConstants.Keys.TransactionId, transactionId);
            return exceptionLog;
        }
    }
}
