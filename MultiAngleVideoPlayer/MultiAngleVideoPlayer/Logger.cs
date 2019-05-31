using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Windows.Storage;

namespace MultiAngleVideoPlayer
{
    public class Logger
    {
        public static StorageFolder logsFolder;
        public static StorageFile logFile;
        public static bool log_yes = true;

        public static async void CreateLog(string participantNum)
        {
            try
            {
                logFile = await DownloadsFolder.CreateFileAsync("logFileP" + participantNum + ".txt", Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private static async void LogAsync(String logMessage)
        {
            String toWrite = System.DateTime.Now.TimeOfDay.ToString().Split('.')[0] + "," + logMessage + "\n";

            try
            {
                await FileIO.AppendTextAsync(logFile, toWrite);
            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        public static void Log(String logMessage)
        {
            if (log_yes)
            {
                Action act = delegate () { LogAsync(logMessage); };
                Task.Run(act);
            }
        }

    }
}
