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
    public partial class rptWorkResult : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptWorkResult()
        {
            InitializeComponent();
        }

        private void rptWorkResult_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드

            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Result_No", "입고번호", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Result_Date", "입고일자", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Process_Code", "공정코드", "100", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Process_Name", "공정", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Whouse_Code", "입고창고 코드", "120", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Whouse_Name", "입고창고", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Custom_Code1", "고객사 코드", "150", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Short_Name", "고객사 코드", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "User_Code", "영업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Dept_Code", "영업부서 코드", "100", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Dept_Name", "영업부서", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Qty", "지시수량", "80", true, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "In_Qty", "입고수량", "80", true, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Work_User", "작업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Work_Name", "작업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Out_User", "납품담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Out_Name", "납품담당자", "150", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "ItemSer_No", "Serial-No", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "ItemLic_No", "라이센스", "120", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Ver_Code", "버전 코드", "120", false, false, false);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Ver_Name", "버전", "120", false, false, true);

            DbHelp.GridColumn_NumSet(gv_ResultM, "Qty", ForMat.SetDecimal("regWorkResult", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "In_Qty", ForMat.SetDecimal("regWorkResult", "Qty1"));

            DbHelp.No_ReadOnly(gv_ResultM);
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
            FileIF.Excel_Down(gc_ResultM, "작업실적현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptWorkResult");
            sp.AddParam("Kind", "M");

            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("To", dt_To.Text.Replace("-", ""));
            else
                sp.AddParam("To", DateTime.MaxValue.ToString("yyyyMMdd"));

            if (dt_Start.Text.Replace("-", "").Length == 8)
                sp.AddParam("Start", dt_Start.Text.Replace("-", ""));
            else
                sp.AddParam("Start", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_End.Text.Replace("-", "").Length == 8)
                sp.AddParam("End", dt_End.Text.Replace("-", ""));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_ResultM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_ResultM.RefreshDataSource();
            gv_ResultM.BestFitColumns();

            DbHelp.Footer_Set(gv_ResultM, ForMat.SetDecimal("regWorkResult", "Qty1").ToString(), new string[] { "Qty", "In_Qty" });
        }

        private void gc_ResultM_DoubleClick(object sender, EventArgs e)
        {
            if(gv_ResultM.FocusedRowHandle >= 0)
            {
                string Result_No = gv_ResultM.GetFocusedRowCellValue("Result_No").NullString();
                PopWorkResultForm Pop_Form = new PopWorkResultForm(Result_No);
                Pop_Form.StartPosition = FormStartPosition.CenterScreen;
                Pop_Form.ShowDialog();
            }
        }
    }
}
