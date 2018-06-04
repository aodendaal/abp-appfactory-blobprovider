using Abp.AppFactory.BlobProvider.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Abp.AppFactory.BlobProvider
{
    public class BlobProviderModule : AbpModule
    {
        private readonly IHostingEnvironment env;
        private IConfigurationRoot _appConfiguration;

        public BlobProviderModule(IHostingEnvironment env)
        {
            this.env = env;
        }

        public override void PreInitialize()
        {
            IocManager.Register<BlobConfiguration>();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _appConfiguration = builder.Build();

            var config = new BlobConfiguration
            {
                DefaultEndpointsProtocol = _appConfiguration["BlobProvider:DefaultEndpointsProtocol"],
                AccountName = _appConfiguration["BlobProvider:AccountName"],
                AccountKey = _appConfiguration["BlobProvider:AccountKey"]
            };

            Configuration.Modules.BlobConfiguration().DefaultEndpointsProtocol = config.DefaultEndpointsProtocol;
            Configuration.Modules.BlobConfiguration().AccountName = config.AccountName;
            Configuration.Modules.BlobConfiguration().AccountKey = config.AccountKey;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(BlobProviderModule).GetAssembly());
        }
    }
}