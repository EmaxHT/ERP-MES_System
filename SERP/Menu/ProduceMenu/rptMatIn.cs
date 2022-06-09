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
    public partial class rptMatIn : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatIn()
        {
            InitializeComponent();
        }

        private void rptMatIn_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_MatInM, gv_MatInM, "In_Date", "입고일자", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "In_No", "입고번호", "120", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Custom_Code", "거래처 코드", "150", false, false, false);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Short_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Po_No", "발주번호", "120", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Qty", "입고수량", "80", true, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "P_Price", "단가", "125", true, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "입고금액", "125", true, false, true);
            //DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Sort_No", "부가세액", "125", true, false, true);
            //DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Sort_No", "합계금액", "125", true, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "MatSer_No", "시리얼넘버", "80", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Pay_Code", "결제조건코드", "80", false, false, false);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Pay_Name", "결제조건", "80", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Pay_Date", "결제예정일", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_MatInM, "Qty", ForMat.SetDecimal("regMatIn", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "P_Price", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Amt", ForMat.SetDecimal("regMatIn", "Price1"));

            DbHelp.No_ReadOnly(gv_MatInM);

            gc_MatInM.MouseWheelChk = false;
            gc_MatInM.PopMenuChk = false;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatInM, "입고현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatIn");
            sp.AddParam("Kind", "N");


            //
            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("To", dt_To.Text.Replace("-", ""));
            else
                sp.AddParam("To", DateTime.MaxValue.ToString("yyyyMMdd"));

            //
            if (dt_F.Text.Replace("-", "").Length == 8)
                sp.AddParam("Start", dt_F.Text.Replace("-", ""));

            if (dt_End.Text.Replace("-", "").Length == 8)
                sp.AddParam("End", dt_End.Text.Replace("-", ""));
            else
                sp.AddParam("End", DateTime.MaxValue.ToString("yyyyMMdd"));



            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_MatInM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_MatInM.RefreshDataSource();
            gv_MatInM.BestFitColumns();

            DbHelp.Footer_Set(gv_MatInM, ForMat.SetDecimal("regMatIn", "Qty1").NullString(), new string[] { "Qty" });
            DbHelp.Footer_Set(gv_MatInM, ForMat.SetDecimal("regMatIn", "Amt1").NullString(), new string[] { "Amt" });
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
    }
}
