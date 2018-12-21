using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tavisca.Connector.Hotels.WebAPI;
using Tavisca.Platform.Common;
using Tavisca.Platform.Common.Logging;
using Tavisca.Platform.Common.ExceptionManagement;
using StructureMap;
using Tavisca.Connector.Hotels.Translators;
using Microsoft.Extensions.Configuration;
using Tavisca.Platform.Common.ApplicationEventBus;
using Tavisca.Connector.Hotels.Common;
using Microsoft.Practices.ServiceLocation;
using System.ServiceModel;
using SoapCore;

namespace Tavisca.Connector.Hotels.Host
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private readonly string _environmentName;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", env.EnvironmentName);
            _environmentName = env.EnvironmentName;
        }


        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddMemoryCache();

            services.AddMvc()
                .AddControllersAsServices()
                .AddJsonOptions(opt => opt.SerializerSettings.ContractResolver = new TranslatorFactory().Build("1.0").ContractResolver);

            var serviceProvider = ConfigureIoC(services);
            Logger.Initialize(serviceProvider.GetRequiredService<ILogWriterFactory>());
            ExceptionPolicy.Configure(serviceProvider.GetRequiredService<IErrorHandler>());

            //register config change event handler in application bus
            var bus = serviceProvider.GetRequiredService<IApplicationEventBus>();
            bus.Register("config-update", new Tavisca.Common.Plugins.Configuration.ConfigurationObserver(new Tavisca.Common.Plugins.Configuration.ConsulConfigurationStore(), new Tavisca.Common.Plugins.Aws.ParameterStoreProvider()));
            return serviceProvider;
        }

        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            var container = new Container(c =>
            {
                c.AddRegistry<ComponentRegistry>();

            });
            container.Configure(config =>
            {
                config.Populate(services);
            });

            var serviceLocator = new StructureMapServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
            var serviceProvider = container.GetInstance<IServiceProvider>();
            return container.GetInstance<IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.ConfigureStartup();
            //app.UseSoapEndpoint<ISampleService>("/Service.svc", new BasicHttpBinding(), SoapSerializer.DataContractSerializer);
            app.UseMvc();
        }
    }
}
