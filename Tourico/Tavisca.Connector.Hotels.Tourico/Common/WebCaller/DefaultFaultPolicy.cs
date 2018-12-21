using System.Threading.Tasks;
using Tavisca.Platform.Common;

namespace Tavisca.Connector.Hotels.Tourico.Common.WebCaller
{
    public class DefaultFaultPolicy : IFaultPolicy
    {
        public static IFaultPolicy Instance = new DefaultFaultPolicy();

        public Task<bool> IsFaultedAsync(HttpRequest req, HttpResponse res)
        {
            var statusCode = (int)(res?.Status);
            return Task.FromResult(statusCode / 100 == 2);
        }
    }
}
