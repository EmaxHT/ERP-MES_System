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
    public partial class rptMatPo : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatPo()
        {
            InitializeComponent();
        }

        private void rptMatPo_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Po_Date", "발주일자", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "In_Date", "입고예정일", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Po_No", "발주번호", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Custom_Code", "거래처코드", "150", false, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Short_Name", "거래처", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "User_Code", "발주담당자 코드", "150", false, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "User_Name", "발주담당자", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Dept_Code", "발주부서 코드", "100", false, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Dept_Name", "발주부서", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Qty", "수량", "80", true, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "P_Price", "단가", "150", true, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Amt", "발주금액", "150", true, false, true, true);
            //DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Vat_Amt", "부가세액", "150", true, true, true);
            //DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Tot_Amt", "합계금액", "150", true, true, true);

            DbHelp.GridColumn_NumSet(gv_MatPoM, "Qty", ForMat.SetDecimal("regMatPo", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "P_Price", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Amt", ForMat.SetDecimal("regMatPo", "Price1"));

            gc_MatPoM.PopMenuChk = false;
            gc_MatPoM.MouseWheelChk = false;

            DbHelp.No_ReadOnly(gv_MatPoM);
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatPoM, "발주현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatPo");
            sp.AddParam("Kind", "N");

            if (dt_From.Text.Replace("-", "") != "")
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            if (dt_To.Text.Replace("-", "") != "")
                sp.AddParam("To", dt_To.Text.Replace("-", ""));

            if (dt_F.Text.Replace("-", "") != "")
                sp.AddParam("Start", dt_F.Text.Replace("-", ""));
            if (dt_End.Text.Replace("-", "") != "")
                sp.AddParam("End", dt_End.Text.Replace("-", ""));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_MatPoM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_MatPoM.RefreshDataSource();
            gv_MatPoM.BestFitColumns();

            DbHelp.Footer_Set(gv_MatPoM, ForMat.SetDecimal("regMatPo", "Qty1").NullString(), new string[] { "Qty" });
            DbHelp.Footer_Set(gv_MatPoM, ForMat.SetDecimal("regMatPo", "Amt1").NullString(), new string[] { "Amt" });
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
