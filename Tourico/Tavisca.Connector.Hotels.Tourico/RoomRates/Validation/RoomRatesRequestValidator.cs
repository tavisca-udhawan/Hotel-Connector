using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Platform.Common.Profiling;
using Tavisca.Connector.Hotels.Model.RoomRates.Validator;
using Tavisca.Connector.Hotels.Model.Common;

namespace Tavisca.Connector.Hotels.Tourico.RoomRates.Validation
{
    public class RoomRatesRequestValidator : RequestValidator
    {
        private IHotelMetadata _metadataProvider;

        public RoomRatesRequestValidator(IHotelMetadata metadataProvider) : base (metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }

        public List<Info> Validate(Request request)
        {
            List<Info> infos;
            using (var profileScope = new ProfileContext("Tourico-requestValidation", false))
            {
                infos = base.ValidateRequest(request);
                //Add Supplier specific Validation Here
                //infos.AddRange(ValidateConfigurations(request.Supplier.Configurations));
            }
            return infos;
        }

        //public List<Info> ValidateConfigurations(List<Configuration> configurations)
        //{
        //    var infos = new List<Info>();
        //    var configurationSpec = _metadataProvider.GetConfigurationSpecAsync().GetAwaiter().GetResult();
        //    infos.AddRange(base.ValidateMandatoryAndAddMissingOptionalConfigurations(configurations, configurationSpec));
        //    return infos;
        //}
    }
}
