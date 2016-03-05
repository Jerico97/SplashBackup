using Rocket.API;

namespace Config
{
    public class BackupCFG : IRocketPluginConfiguration
    {
        public int BackUpMin = 30;
        public int MaxFilesSizeMB = 25;

        public void LoadDefaults()
        {
            BackUpMin = 30;
            MaxFilesSizeMB = 25;
        }
    }
}
