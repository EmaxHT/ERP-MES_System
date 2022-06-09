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
    public partial class rptProcess : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptProcess()
        {
            InitializeComponent();
        }

        private void rptProcess_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
            Timer t = new Timer();
            t.Tick += btn_Select_Click;
            t.Interval = 1000 * 60 * 5;
            t.Start();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드

            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Custom_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Custom_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Project_Title", "수주제목", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Dept_Name", "영업부서", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Order_Qty", "수주수량", "80", true, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Not_Qty", "미출고수량", "80", true, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Cf_Date", "확정일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Exp_MatRece_Date", "자재출고예정일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "MatOut_Date", "자재출고일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Exp_WorkRst_Date", "생산입고예정일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "WorkResult_Date", "생산입고일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Exp_QC_Date", "검사완료예정일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Qc_Date", "검사완료일", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_ResultM, "Order_Qty", ForMat.SetDecimal("regSales", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Not_Qty", ForMat.SetDecimal("regSales", "Qty1"));

            gc_ResultM.PopMenuChk = false;
            gc_ResultM.MouseWheelChk = false;
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

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_ResultM, "제품별 공정진척현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptNotSales");
            sp.AddParam("Kind", "S");
            sp.AddParam("dt_FOrder", dt_From.DateTime.ToString("yyyyMMdd"));
            sp.AddParam("dt_TOrder", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));
            sp.AddParam("dt_FDelivery", DateTime.MinValue.ToString("yyyyMMdd"));
            sp.AddParam("dt_TDelivery", DateTime.MaxValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_ResultM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_ResultM.RefreshDataSource();
            gv_ResultM.BestFitColumns();

            DbHelp.Footer_Set(gv_ResultM, ForMat.SetDecimal("regSales", "Qty1").NullString(), new string[] { "Order_Qty", "Not_Qty" });
        }

        private void gc_ResultM_DoubleClick(object sender, EventArgs e)
        {
            if (gv_ResultM.FocusedRowHandle >= 0)
            {
                string Result_No = gv_ResultM.GetFocusedRowCellValue("Result_No").NullString();
                PopWorkResultForm Pop_Form = new PopWorkResultForm(Result_No);
                Pop_Form.StartPosition = FormStartPosition.CenterScreen;
                Pop_Form.ShowDialog();
            }
        }

        private void gv_ResultM_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            string temp = gv_ResultM.GetRowCellValue(e.RowHandle, "Delivery").NullString();
            if (string.IsNullOrWhiteSpace(temp))
                return;
            DateTime d_date = Convert.ToDateTime(temp);

            DateTime ago = DateTime.Now.AddDays(-7);
            DateTime later = DateTime.Now.AddDays(7);

            // 금일이 2021-06-10 이라고 가정
            // ago   = 2021-06-03
            // later = 2021-06-17
            // 좌측이 더 크면 +, 우측이 더 크면 -
            int left = DateTime.Compare(d_date, ago);               // 예정일이 2021-06-03 이후일 경우   => +
            int left_bet = DateTime.Compare(d_date, DateTime.Now);  // 예정일이 금일 이전일 경우         => -

            bool svn_ago = (left_bet < left);                       // 예정일이 2021-06-03 ~ 2021-06-09 일 경우 true

            int right = DateTime.Compare(later, d_date);            // 예정일이 2021-06-17 이전일 경우   => +
            int right_bet = DateTime.Compare(DateTime.Now, d_date); // 예정일이 금일 이후일 경우         => -

            bool svn_later = (right_bet < right);                   // 예정일이 2021-06-03 ~ 2021-06-09 일 경우 true

            if (svn_ago || left_bet == 0)                           // 7일 이전 ~ 금일 => Red
            {
                e.Appearance.ForeColor = Color.Red;
            }
            if (svn_later)                                          // 금일 ~ 7일 이후 => Green
            {
                e.Appearance.ForeColor = Color.Green;
            }
        }
    }
}
