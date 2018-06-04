using Abp.Configuration.Startup;

namespace Abp.AppFactory.BlobProvider.Configuration
{
    public static class BlobConfigurationExtentions
    {
        public static BlobConfiguration BlobConfiguration(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<BlobConfiguration>();
        }
    }
}