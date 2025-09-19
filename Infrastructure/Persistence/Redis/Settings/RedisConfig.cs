namespace Firebase_Auth.Infrastructure.Persistence.Redis.Settings;
  public class RedisSettings
    {
        public string ConnectionString { get; set; } = "localhost:6379";
        public string InstanceName { get; set; } = "MY_APP";
        public int Database { get; set; } = 0;
        public int ConnectTimeout { get; set; } = 5000;
        public int SyncTimeout { get; set; } = 5000;
        public bool AbortOnConnectFail { get; set; } = false;
    }