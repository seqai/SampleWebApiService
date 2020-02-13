namespace SampleWebApiService.Infrastructure.Configuration
{
    public class PersistenceConfiguration
    {
        public string ConnectionString { get; set; }
        public bool AutoMigrate { get; set; }
    }
}
