using System.Collections.Generic;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Cancel;
using Tavisca.Connector.Hotels.Model.Cancel.Validator;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Platform.Common.Profiling;

namespace Tavisca.Connector.Hotels.Tourico.Cancel.Validation
{
    public class CancelRequestValidator : RequestValidator
    {
        private IHotelMetadata _metadataProvider;

        public CancelRequestValidator(IHotelMetadata metadataProvider) : base (metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }

        public List<Info> Validate(CancelRequest request)
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
