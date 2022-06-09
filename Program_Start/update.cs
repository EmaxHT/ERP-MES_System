using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program_Start
{
    class update
    {
        public static List<string> getUpdateInfo(string downloadsURL, string versionFile, string resourceDownloadFolder, int startLine)
        {
            bool updateChecked = false;

            if (!Directory.Exists(resourceDownloadFolder))
            {
                Directory.CreateDirectory(resourceDownloadFolder);
            }

            updateChecked = webdata.downloadFromWeb(downloadsURL, versionFile, resourceDownloadFolder);

            if (updateChecked)
            {
                return populateInfoFromWeb(versionFile, resourceDownloadFolder, startLine);
            }
            else
            {
                return null;
            }
        }

        public static void installUpdateNow(string downloadsURL, string filename, string downloadTo, bool unzip)
        {
            bool downloadSuccess = webdata.downloadFromWeb(downloadsURL, filename, downloadTo);

            if (unzip)
            {
                unZip(downloadTo + filename, downloadTo);
            }
        }

        public static void installUpdateRestart(string downloadsURL, string filename, string destinationFolder, string processToEnd, string postProcess, string startupCommand, string updater)
        {
            string cmdLn = "";

            cmdLn += "|downloadFile|" + filename;
            cmdLn += "|URL|" + downloadsURL;
            cmdLn += "|destinationFolder|" + destinationFolder;
            cmdLn += "|processToEnd|" + processToEnd;
            cmdLn += "|postProcessFile|" + postProcess;
            cmdLn += "|command|" + @" / " + startupCommand;

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = updater;
            startInfo.Arguments = cmdLn;
            Process.Start(startInfo);
        }

        public static List<string> populateInfoFromWeb(string versionFile, string resourceDownloadFolder, int line)
        {
            List<string> templist = new List<string>();

            int ln;
            int i;

            ln = 0;

            if(File.Exists(resourceDownloadFolder + versionFile))
                foreach(string strline in File.ReadAllLines(resourceDownloadFolder + versionFile))
                {
                    if(ln == line)
                    {
                        string[] parts = strline.Split('|');
                        foreach(string part in parts)
                        {
                            templist.Add(part);
                        }

                        return templist;
                    }

                    ln++;
                }

            return null;
        }

        public static bool unZip(string file, string unZipTo)
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(file))
                {
                    zip.ExtractAll(unZipTo);
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static void updateMe(string updaterPrefix, string containingFolder)
        {
            DirectoryInfo dInfo = new DirectoryInfo(containingFolder);
            FileInfo[] updaterFiles = dInfo.GetFiles(updaterPrefix + "*");
            int fileCount = updaterFiles.Length;

            foreach(FileInfo file in updaterFiles)
            {
                string newFile = containingFolder + file.Name;
                string origFile = containingFolder + @"\" + file.Name.Substring(updaterPrefix.Length, file.Name.Length - updaterPrefix.Length);

                if (File.Exists(origFile))
                {
                    File.Delete(origFile);
                }

                File.Move(newFile, origFile);
            }
        }
    }
}
