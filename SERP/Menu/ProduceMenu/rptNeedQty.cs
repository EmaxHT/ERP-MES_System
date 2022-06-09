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
    public partial class rptNeedQty : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptNeedQty()
        {
            InitializeComponent();
        }

        private void rptNeedQty_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
            btn_Select_Click(null, null);
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "SItem_Code", "품목코드", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Item_Name", "품목명", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Ssize", "규격", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "OUT_Qty", "소요량", "", true, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Qty", "재고수량", "", true, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "TOT_QTY", "재고부족분", "", true, false, true);

            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Whouse_Code", "창고코드", "", false, false, false);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Whouse_Name", "창고명", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Loc_Code", "Location 코드", "", false, true, false);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Loc_Name", "Location", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "In_No", "입고번호", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "In_Date", "입고일자", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Custom_Code", "거래처코드", "", false, false, false);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Short_Name", "거래처", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Qty", "재고수량", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "P_Price", "단가", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Amt", "금액", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "MatSer_No", "Serial_No", "", false, false, true);

            DbHelp.GridColumn_NumSet(gv_StockLeft, "Qty", ForMat.SetDecimal("4270", "Qty"));

            DbHelp.GridColumn_NumSet(gv_StockRight, "Qty", ForMat.SetDecimal("4270", "Qty"));
            DbHelp.GridColumn_NumSet(gv_StockRight, "P_Price", ForMat.SetDecimal("4270", "Price1"));
            DbHelp.GridColumn_NumSet(gv_StockRight, "Amt", ForMat.SetDecimal("4270", "Price1"));

            gc_StockLeft.MouseWheelChk = false;
            gc_StockRight.MouseWheelChk = false;

            gc_StockLeft.PopMenuChk = false;
            gc_StockRight.PopMenuChk = false;

            DbHelp.No_ReadOnly(gv_StockRight);
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data("L");
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_StockLeft, "생산사제청구서등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data(string side = "")
        {
            SqlParam sp = new SqlParam("sp_rptNeedQty");
            sp.AddParam("Kind", side);

            if (side == "R")
                sp.AddParam("Item_Code", Convert.ToString(gv_StockLeft.GetFocusedRowCellValue("SItem_Code")));

            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("To", dt_To.Text.Replace("-", ""));
            else
                sp.AddParam("To", DateTime.MaxValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            if (side == "L")
            {
                gc_StockLeft.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                gc_StockLeft.RefreshDataSource();
                gv_StockLeft.BestFitColumns();
            }

            else if (side == "R")
            {
                gc_StockRight.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                gc_StockRight.RefreshDataSource();
                gv_StockRight.BestFitColumns();
            }
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

        private void gv_StockLeft_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (gv_StockLeft.RowCount > 0)
                Search_Data("R");
        }
    }
}
