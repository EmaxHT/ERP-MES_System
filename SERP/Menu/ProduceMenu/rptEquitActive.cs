using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SERP
{
    public partial class rptEquitActive : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptEquitActive()
        {
            InitializeComponent();
        }
        
        private void rptEquitActive_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            dt_From.DateTime = DateTime.Now;
            Grid_Set();
            btn_Select_Click(null, null);

            Timer Timer = new Timer(); //타이머 객체 선언
            Timer.Tick += timer_Tick;
            Timer.Interval = 300000;
            Timer.Start();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(GridMaster, ViewMaster, "Equip_Date", "가동일자", "100", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Equip_Code", "설비코드", "120", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Equip_Name", "설비명", "250", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Start_Date", "가동시작", "200", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "End_Date", "가동완료", "200", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Tot_Date", "총가동시간(분)", "200",true, false, true);

            GridMaster.MouseWheelChk = false;
            GridMaster.PopMenuChk = false;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            //if (dt_From.Text.Trim().Length == 0)
            //{
            //    XtraMessageBox.Show("날짜를 선택해 주십시요");
            //    return;
            //}
            
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(GridMaster, "설비별 가동시간 현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptEquitActive");
            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }
            GridMaster.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            GridMaster.RefreshDataSource();
            ViewMaster.BestFitColumns();

        }

        #region 버튼 상속
        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            btn_Select_Click(null, null);
        }
    }
}
