using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Program_Update
{
    public partial class Update : DevExpress.XtraEditors.XtraForm
    {
        bool called = true;

        private string tempDownloadFolder = "";
        private string processToEnd = "";
        private string downloadFile = "";
        private string URL = "";
        private string destinationFolder = "";
        private string updateFolder = Application.StartupPath + @"\Program_Update\";
        private string postProcessFile = "";
        private string postProcessCommand = "";

        public Update()
        {
            InitializeComponent();
        }

        private void Update_Load(object sender, EventArgs e)
        {
            Hide();

            if (called)
            {
                WindowState = FormWindowState.Normal;
                if(this.ClientRectangle.Height < progress_Bar.Location.Y + progress_Bar.Size.Height)
                {
                    this.Height = 21 + progress_Bar.Location.Y + progress_Bar.Size.Height * 2;
                }

                Show();

                BackgroundWorker bw = new BackgroundWorker();

                bw.DoWork -= new DoWorkEventHandler(backgroundWorker);
                bw.DoWork += new DoWorkEventHandler(backgroundWorker);
                bw.WorkerSupportsCancellation = true;
                bw.RunWorkerAsync();
            }
        }
        private void backgroundWorker(object sender, DoWorkEventArgs e)
        {
            preDownload();

            if (called)
            {
                WindowState = FormWindowState.Normal;
                Show();

                Thread.Sleep(1000);

                try
                {
                    Process[] processes = Process.GetProcesses();

                    foreach(Process process in processes)
                    {
                        if(process.ProcessName == processToEnd)
                        {
                            process.Kill();
                        }
                    }
                }
                catch(Exception ex)
                { }

                webdata.bytesDownloaded += Bytesdownloaded;
                webdata.downloadFromWeb(URL, downloadFile, tempDownloadFolder);

                unZip(tempDownloadFolder + downloadFile, tempDownloadFolder);
                Thread.Sleep(1000);
                moveFiles();
                Thread.Sleep(1000);
                wrapUp();
                if (postProcessFile != "")
                    postDownload();
            }

            Close();
        }

        private void unpackCommandline()
        {
            string cmdLn = "";

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                cmdLn += arg;
            }

            if (cmdLn.IndexOf('|') == -1)
            {
                called = false;
                Close();
            }

            string[] tmpCmd = cmdLn.Split('|');

            for(int i = 1; i < tmpCmd.GetLength(0); i++)
            {
                if (tmpCmd[i] == "downloadFile") downloadFile = tmpCmd[i + 1];
                if (tmpCmd[i] == "URL") URL = tmpCmd[i + 1];
                if (tmpCmd[i] == "destinationFolder") destinationFolder = tmpCmd[i + 1];
                if (tmpCmd[i] == "processToEnd") processToEnd = tmpCmd[i + 1];
                if (tmpCmd[i] == "postProcessFile") postProcessFile = tmpCmd[i + 1];
                if (tmpCmd[i] == "command") postProcessCommand += @"/" + tmpCmd[i + 1];

                i++;
            }
        }

        private void unZip(string file, string unZipTo)
        {
            try
            {
                using(ZipFile zip = ZipFile.Read(file))
                {
                    zip.ExtractAll(unZipTo);
                }
            }
            catch (System.Exception)
            {

            }
        }

        private void preDownload()
        {
            if (!Directory.Exists(updateFolder))
                Directory.CreateDirectory(updateFolder);

            tempDownloadFolder = updateFolder + DateTime.Now.ToString("yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture) + @"\";

            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }

            Directory.CreateDirectory(tempDownloadFolder);

            unpackCommandline();
        }

        private void postDownload()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = postProcessFile;
            startInfo.Arguments = postProcessCommand;
            Process.Start(startInfo);
        }

        private void wrapUp()
        {
            if (Directory.Exists(tempDownloadFolder))
            {
                Directory.Delete(tempDownloadFolder, true);
            }
        }

        private void moveFiles()
        {
            DirectoryInfo di = new DirectoryInfo(tempDownloadFolder);
            FileInfo[] files = di.GetFiles();

            foreach(FileInfo fi in files)
            {
                if (fi.Name != downloadFile)
                    File.Copy(tempDownloadFolder + fi.Name, destinationFolder + fi.Name, true);
            }
        }

        private void Bytesdownloaded(ByteArgs e)
        {
            progress_Bar.Maximum = e.total;

            if (progress_Bar.Value + e.downloaded <= progress_Bar.Maximum)
            {
                progress_Bar.Value += e.downloaded;
            }
            else
            {

            }

            progress_Bar.Refresh();
            Invalidate();
        }
    }
}
