using System.Collections.Generic;
using Tavisca.Connector.Hotels.ErrorHandling;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Book.Validator;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Platform.Common.Profiling;

namespace Tavisca.Connector.Hotels.Tourico.Book.Validation
{
    public class BookRequestValidator : RequestValidator
    {
        private IHotelMetadata _metadataProvider;

        public BookRequestValidator(IHotelMetadata metadataProvider) : base (metadataProvider, false)
        {
            _metadataProvider = metadataProvider;
        }

        public List<Info> Validate(BookRequest request)
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
