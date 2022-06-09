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
    public partial class rptWorkQc : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptWorkQc()
        {
            InitializeComponent();
        }

        private void rptWorkQc_Load(object sender, EventArgs e)
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
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Qc_Date", "검사일자", "125", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Qc_No", "검사번호", "120", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Order_No", "수주번호", "120", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Short_Name", "고객사", "150", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Whouse_Name", "입고창고", "120", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Qty", "입고수량", "80", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "GoodQty", "정품수량", "80", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "BadQty", "불량수량", "80", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Pass_Code", "합격여부", "150", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "End_Date", "완료일자", "50", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcM, gv_WorkQcM, "Result_No", "검사번호", "50", false, false, false, true);

            DbHelp.GridColumn_NumSet(gv_WorkQcM, "Qty"      , ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_WorkQcM, "GoodQty"  , ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_WorkQcM, "BadQty"   , ForMat.SetDecimal(this.Name, "Qty1"));

            gc_WorkQcM.MouseWheelChk = false;
            gc_WorkQcM.PopMenuChk = false;

            // Quot_S2 그리드
            DbHelp.GridSet(gc_WorkQcS1, gv_WorkQcS1, "Qc_Sort", "순번", "50", true, true, true, true);
            DbHelp.GridSet(gc_WorkQcS1, gv_WorkQcS1, "Qty", "불량수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_WorkQcS1, gv_WorkQcS1, "Fail_Code", "불량사유", "150", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS1, gv_WorkQcS1, "Qc_Bigo", "비고", "300", true, false, true, true);

            DbHelp.GridColumn_NumSet(gv_WorkQcS1, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            gc_WorkQcS1.MouseWheelChk = false;
            gc_WorkQcS1.PopMenuChk = false;

            // Quot_S1 그리드
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Qc_Sort", "순번", "80", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Qc_Code", "검사항목", "120", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Pass_Code", "검사결과", "80", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Qc_Date", "검사일자", "125", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "End_Date", "완료일자", "125", true, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Fail_Code", "사유", "150", false, false, true, true);
            DbHelp.GridSet(gc_WorkQcS2, gv_WorkQcS2, "Qc_Bigo", "비고", "300", false, false, true, true);

            gc_WorkQcS2.MouseWheelChk = false;
            gc_WorkQcS2.PopMenuChk = false;

            DbHelp.No_ReadOnly(gv_WorkQcM);
        }

        #region 버튼 이벤트

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_WorkQcM, "공정검사 현황");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            gc_WorkQcS1.DataSource = null;
            gc_WorkQcS2.DataSource = null;

            Search_H();
        }

        #endregion

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }
        #endregion

        #region 내부 함수
        private void Search_H()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "H");
                sp.AddParam("FDate", dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDate", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_WorkQcM.DataSource = ret.ReturnDataSet.Tables[0];
                gv_WorkQcM.BestFitColumns();

                DbHelp.Footer_Set(gv_WorkQcM, ForMat.SetDecimal(this.Name, "Qty1").NullString(), new string[] { "Qty", "GoodQty", "BadQty" });

                if(gv_WorkQcM.RowCount > 0)
                    Search_D(0);
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search_D(int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "D");
                sp.AddParam("Qc_No", gv_WorkQcM.GetRowCellValue(iRow, "Qc_No").ToString());
                sp.AddParam("Result_No", gv_WorkQcM.GetRowCellValue(iRow, "Result_No").ToString());
                sp.AddParam("Order_No", gv_WorkQcM.GetRowCellValue(iRow, "Order_No").ToString());
                sp.AddParam("Item_Code", gv_WorkQcM.GetRowCellValue(iRow, "Item_Code").ToString());

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_WorkQcS1.DataSource = ret.ReturnDataSet.Tables[0];
                gc_WorkQcS2.DataSource = ret.ReturnDataSet.Tables[1];

                gv_WorkQcS1.BestFitColumns();
                gv_WorkQcS2.BestFitColumns();

                DbHelp.Footer_Set(gv_WorkQcS1, ForMat.SetDecimal(this.Name, "Qty1").NullString(), new string[] { "Qty" });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        private void gv_WorkQcM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            Search_D(e.FocusedRowHandle);
        }
    }
}
