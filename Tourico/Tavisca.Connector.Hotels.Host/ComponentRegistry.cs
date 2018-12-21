using StructureMap;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Connector.Hotels.Translators;
using Tavisca.Connector.Hotels.Model.Search;
using Tavisca.Connector.Hotels.Model.RoomRates;
using Tavisca.Connector.Hotels.Model.RateRules;
using Tavisca.Connector.Hotels.Model.Book;
using Tavisca.Connector.Hotels.Model.Cancel;
using Tavisca.Connector.Hotels.Model.Retrieve;
using Tavisca.Connector.Hotels.Tourico.Search;
using Tavisca.Connector.Hotels.Tourico.RoomRates;
using Tavisca.Connector.Hotels.Tourico.RateRules;
using Tavisca.Connector.Hotels.Tourico.Book;
using Tavisca.Connector.Hotels.Tourico.Cancel;
using Tavisca.Connector.Hotels.Tourico.Retrieve;
using Tavisca.Platform.Common.ExceptionManagement;
using Tavisca.Platform.Common.Configurations;
using Tavisca.Platform.Common.Logging;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Serialization;
using Tavisca.Platform.Common.MemoryStreamPool;
using Tavisca.Platform.Common.StateManagement;
using Tavisca.Connector.Hotels.Common;
using Tavisca.Platform.Common.SessionStore;
using Tavisca.Common.Plugins.SessionStore;
using Tavisca.Platform.Common.ApplicationEventBus;
using Tavisca.Connector.Hotels.Model.Metadata;
using Tavisca.Connector.Hotels.Common.Serialization;

namespace Tavisca.Connector.Hotels.Host
{
    public class ComponentRegistry : Registry
    {
        public ComponentRegistry()
        {
            //Consul
            //ForSingletonOf<IConfigurationProvider>().Use<ConsulConfigurationProvider>();
            ForSingletonOf<IConfigurationProvider>().Use(c => new Tavisca.Common.Plugins.Configuration.ConfigurationProvider(Constants.Application));

            //logging framework
            For<ILogSink>().Use<Tavisca.Common.Plugins.Aws.FirehoseSink>().Named(Constants.Logging.Firehose);
            For<ILogSink>().Use<Tavisca.Common.Plugins.Redis.RedisSink>().Named(Constants.Logging.Redis);
            For<ILogWriterFactory>().Use<LogWriterFactory>();

            //state management
            For<Tavisca.Common.Plugins.Aerospike.IStateProviderSettings>().Use<Tavisca.Common.Plugins.Aerospike.AerospikeSettingsProvider>();
            For<ISessionStore>().Use<SessionStore>();
            For<ISessionProviderFactory>().Use<Tavisca.Common.Plugins.Aerospike.AerospikeSessionProviderFactory>();

            ForSingletonOf<Tavisca.Common.Plugins.Aerospike.IAerospikeClientFactory>().Use<Tavisca.Connector.Hotels.Common.AerospikeClientFactory>();
            ForSingletonOf<IMemoryStreamPool>().Use<Tavisca.Common.Plugins.RecyclableStreamPool.RecyclableStreamPool>();

            //ForSingletonOf<IStateProvider>().Use<SessionStateProvider>();
            ForSingletonOf<IStateProvider>().Use<Tavisca.Common.Plugins.Aerospike.AerospikeObjectStateProvider>();

            //ApplicationEventBus
            For<IApplicationEventBus>().Use<Tavisca.Common.Plugins.Configuration.ApplicationEventBus>().SelectConstructor(() => new Tavisca.Common.Plugins.Configuration.ApplicationEventBus());

            For<IHotelMetadata>().Use<ServiceBasedMetadata>();
            For<ISerializerFactory>().Use<SerializerFactory>();
            For<ISerializer>().Use<Tavisca.Connector.Hotels.Common.Serialization.NewtonSoftJsonSerializer>();
            For<BaseSerializerFactory>().Use<TranslatorFactory>();
            For<ITranslatorOptions>().Use<TranslatorSerializerSettings>();

            For<IErrorHandler>().Use<ErrorHandler>();
            ForConcreteType<CallContextCreator>().Configure.Ctor<string>("applicationName").Is("ConnectorShell").Ctor<string>("applicationShortName").Is("connector_shell");

            For<IHttpConnector>().Use<WebRequestConnector>();
            For<IHotelSearch>().Use<HotelSearch>();
            For<IHotelBook>().Use<HotelBook>();
            For<IHotelRoomRates>().Use<HotelRoomRates>();
            For<IHotelRateRules>().Use<HotelRateRules>();
            For<IHotelCancel>().Use<HotelCancel>();
            For<IRetrieve>().Use<HotelRetrieve>();
        }
    }
}