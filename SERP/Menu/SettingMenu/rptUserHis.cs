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
    public partial class rptUserHis : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        public rptUserHis()
        {
            InitializeComponent();
        }

        private void rptUserHis_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
            dt_From.DateTime = DateTime.Now.GetFirstDay();
            dt_To.DateTime = DateTime.Now;
            Search_Data();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            if (Grid_Log_M.IsFocused)
                FileIF.Excel_Down(Grid_Log_M, "로그 기록");
            else
                FileIF.Excel_Down(Grid_Log_S, "로그 상세 기록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "Log_No", "로그넘버", "100", false, false, false);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "Login_Date", "로그인 시각", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "Logout_Date", "로그아웃 시각", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "Login_IP", "사설 IP주소", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "External_IP", "공인 IP주소", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "Login_Host_Name", "Host 명", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "User_Code", "사원ID", "100", false, false, false);
            DbHelp.GridSet(Grid_Log_M, View_Log_M, "User_Name", "사용자", "100", false, false, true);

            DbHelp.GridSet(Grid_Log_S, View_Log_S, "Log_No", "로그넘버", "100", false, false, false);
            DbHelp.GridSet(Grid_Log_S, View_Log_S, "Log_Detail_No", "로그상세 넘버", "100", false, false, false);
            DbHelp.GridSet(Grid_Log_S, View_Log_S, "Excution_Date", "실행 시각", "100", false, false, true);
            DbHelp.GridSet(Grid_Log_S, View_Log_S, "Excuted_Menu", "실행 메뉴", "100", false, false, true);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_Log");
            sp.AddParam("Kind", "S");

            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("In_From", dt_From.DateTime.ToString("yyyyMMdd"));
            else
                sp.AddParam("In_From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("In_To", dt_To.DateTime.ToString("yyyyMMdd"));
            else
                sp.AddParam("In_To", DateTime.MaxValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            ds = ret.ReturnDataSet;

            Grid_Log_M.DataSource = DbHelp.Fill_Table(ds.Tables[0]);
            Grid_Log_M.RefreshDataSource();
            View_Log_M.BestFitColumns();
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

        private void View_Log_M_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string l_no = View_Log_M.GetFocusedRowCellValue("Log_No").NullString();
            DataTable temp = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[1]);

            if (string.IsNullOrWhiteSpace(l_no))
                return;
            if (temp.Select("Log_No = " + l_no) != null && temp.Select("Log_No = " + l_no).Count() > 0)
            {
                Grid_Log_S.DataSource = temp.Select("Log_No = " + l_no).CopyToDataTable();
            }
            else
            {
                Grid_Log_S.DataSource = temp.Clone();
            }
            View_Log_S.BestFitColumns();
        }
    }
}
