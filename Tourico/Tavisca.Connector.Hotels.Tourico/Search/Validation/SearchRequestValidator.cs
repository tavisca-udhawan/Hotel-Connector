using System.Collections.Generic;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Model.Search.Validator;
using Tavisca.Platform.Common.Profiling;
using ConnectorSearch = Tavisca.Connector.Hotels.Model.Search;

namespace Tavisca.Connector.Hotels.Tourico.Search.Validation
{
    public class SearchRequestValidator : RequestValidator
    {
        private IHotelMetadata _metadataProvider;

        public SearchRequestValidator(IHotelMetadata metadataProvider) : base (metadataProvider)
        {
            _metadataProvider = metadataProvider;
        }

        public List<Info> Validate(ConnectorSearch.SearchRequest request)
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
