using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Rocket.API;
using Rocket.Unturned;
using Rocket.Core;
using UnityEngine;
using Rocket.Unturned.Plugins;
using SDG;
using Rocket.Core.Plugins;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using System.IO;
using SDG.Unturned;
using System.Timers;
using Rocket.Unturned.Player;
using Rocket.Unturned.Items;
using Config;

//C:\Users\Jerico\Desktop\Unturned Test Server\Unturned\Servers\TestServer2016\Rocket\

namespace SplashBackup
{
    public class backup : RocketPlugin<BackupCFG>
    {
        private DateTime lastCalled = DateTime.Now;
        public DateTime? lastmessage = DateTime.Now;

        public static backup Instance;

        protected override void Load()
        {
            Instance = this;
            string BackUpFolder = System.Environment.CurrentDirectory + @"\Plugins\SplashBackup\BackUps\";
            if (!System.IO.Directory.Exists(BackUpFolder)) System.IO.Directory.CreateDirectory(BackUpFolder);

            Dobackup();

        }

        public static void Dobackup()
        {
            string Patch = (System.Environment.CurrentDirectory);
            int StringLenght = Patch.Length - 6;
            string ServerPatch = Patch.Substring(0, StringLenght);
            string BackUpFolder = System.Environment.CurrentDirectory + @"\Plugins\SplashBackup\BackUps\";
            string ServerConfig = ServerPatch + @"Level\" + Provider.map + @"\";

            //new Thread(() =>
            //{
                //DoBackUp:
            string FileTime = Convert.ToString(DateTime.Now.ToFileTime());
            string MoveTargetFolder = BackUpFolder + FileTime + @"\";
            System.IO.Directory.CreateDirectory(MoveTargetFolder);

            FileInfo BarricadesInfo = new FileInfo(ServerConfig + "Barricades.dat");
            FileInfo ObjectsInfo = new FileInfo(ServerConfig + "Objects.dat");
            FileInfo StructuresInfo = new FileInfo(ServerConfig + "Structures.dat");
            Exception TooLargeFile = new Exception("WARNING! Your level's file is too large! Please load latest good file from plugin folder.");

            if ( (BarricadesInfo.Length/1024/1024) > Instance.Configuration.Instance.MaxFilesSizeMB)
            {
                Logger.LogException(TooLargeFile);
            }
            if ((ObjectsInfo.Length / 1024 / 1024) > Instance.Configuration.Instance.MaxFilesSizeMB)
            {
                Logger.LogException(TooLargeFile);
            }
            if ((StructuresInfo.Length / 1024 / 1024) > Instance.Configuration.Instance.MaxFilesSizeMB)
            {
                Logger.LogException(TooLargeFile);
            }

            System.IO.File.Copy(ServerConfig + "Barricades.dat", MoveTargetFolder + "Barricades.dat");
            System.IO.File.Copy(ServerConfig + "Objects.dat", MoveTargetFolder + "Objects.dat");
            System.IO.File.Copy(ServerConfig + "Structures.dat", MoveTargetFolder + "Structures.dat");
            Logger.LogWarning("");
            Logger.LogWarning("Backup created :)");
            Logger.LogWarning("Structures size: " + StructuresInfo.Length / 1024 + "kb");
            Logger.LogWarning("Objects size   : " + ObjectsInfo.Length / 1024 + "kb");
            Logger.LogWarning("Barricades size: " + BarricadesInfo.Length / 1024 + "kb");
            //Thread.Sleep(Instance.Configuration.Instance.BackUpMin * 60000);
            //goto DoBackUp;
            //}).Start();
        }

        void FixedUpdate()
        {
            if ((DateTime.Now - lastmessage.Value).TotalSeconds > Instance.Configuration.Instance.BackUpMin * 60)
            {
                lastmessage = DateTime.Now;
                Dobackup();
            }

        }


    }

}
