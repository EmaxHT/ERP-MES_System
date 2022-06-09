using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SERP
{
    public class clsCommon
    {
        public void ShowWait(Form showForm, string sCaption = "", string sDescription = "데이터 처리중 입니다. \n잠시만 기다려주세요.", int iTime = 1000)
        {
            SplashScreenManager.CloseForm(false);

            SplashScreenManager.ShowForm(showForm, typeof(BaseWaitForm), true, true, false, iTime);
            SplashScreenManager.Default.SetWaitFormCaption(sCaption);
            SplashScreenManager.Default.SetWaitFormDescription(sDescription);
        }

        public void CloseWait()
        {
            SplashScreenManager.CloseForm(false);
        }
    }
}
