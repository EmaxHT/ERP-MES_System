using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Program_Start
{
    public partial class Start : DevExpress.XtraEditors.XtraForm
    {
        public const string updaterPrefix = "Old";

        private static string processToEnd = "";

        private static string postProcess = "";

        public static string updater = Application.StartupPath + @"\Program_Update.exe";

        public static List<string> info = new List<string>();

        private string thisVersion = string.Empty;
        private string versionfilename = "Version.txt";
        private string downloadsurl = "http://192.168.0.60/GLUESYS/";

        private string downloadZipFile = "SERP.zip";

        public Start()
        {
            InitializeComponent();
        }

        private void Start_Load(object sender, EventArgs e)
        {
            Hide();

            update.updateMe(updaterPrefix, Application.StartupPath + @"\");

            info = update.populateInfoFromWeb(versionfilename, Application.StartupPath + @"\", 1);

            if(info != null)
            {
                if (info.Count == 1)
                    info = update.populateInfoFromWeb(versionfilename, Application.StartupPath + @"\", 2);

                if(info.Count == 5)
                {
                    processToEnd = info[0];
                    postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
                    downloadsurl = info[3];
                    downloadZipFile = info[4];

                    thisVersion = info[1];
                    thisVersion = info[1];
                }
                else if(info.Count == 9)
                {
                    processToEnd = info[4];
                    postProcess = Application.StartupPath + @"\" + processToEnd + ".exe";
                    downloadsurl = info[7];
                    downloadZipFile = info[8];
                    thisVersion = info[5];
                }
                else
                {
                    StartApplication();
                    this.Close();
                    return;
                }

                checkForUpdate();
            }
            else
            {
                StartApplication();
            }

            this.Close();
        }

        private void StartApplication()
        {
            if (postProcess == "")
                postProcess = "SERP.exe"; //SERP.exe

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = postProcess;
            startInfo.Arguments = "";
            Process.Start(startInfo);
        }

        private void checkForUpdate()
        {
            info = update.getUpdateInfo(downloadsurl, versionfilename, Application.StartupPath + @"\", 1);

            if(info == null)
            {
                StartApplication();
            }
            else
            {
                if(decimal.Parse(info[1]) > decimal.Parse(thisVersion))
                {
                    update.installUpdateRestart(info[3], info[4], "\"" + Application.StartupPath + "\\", processToEnd, postProcess, "updated", updater);
                }
                else
                {
                    StartApplication();
                }
            }
        }
    }
}
