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
    public partial class rptMatOutItem : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatOutItem()
        {
            InitializeComponent();
        }

        private void rptMatOutItem_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
            btn_Select_Click(null, null);

            //Timer Timer = new Timer(); //타이머 객체 선언
            //Timer.Tick += btn_Select_Click;
            //Timer.Interval = 300000;
            //Timer.Start();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Order_No", "수주번호", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Order_Date", "수주일자", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Project_Title", "수주제목", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Short_Name", "거래처", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Item_Code", "품목코드", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Item_Name", "품명", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "Ssize", "규격", "", false, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "AMT", "수주금액", "", true, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "ONE_AMT", "원가", "", true, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "MARGIN", "마진", "", true, false, true);
            DbHelp.GridSet(gc_StockLeft, gv_StockLeft, "MARGIN_P", "마진율", "", false, false, true);
            DbHelp.GridColumn_NumSet(gv_StockLeft, "MARGIN_P", 2);

            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Out_Date", "출고일자", "", false, false, false);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "SItem_Code", "품목코드", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Item_Name", "품명", "", false, true, false);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Ssize", "규격", "", false, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Qty", "수량", "", true, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "P_Price", "단가", "", true, false, true);
            DbHelp.GridSet(gc_StockRight, gv_StockRight, "Amt", "금액", "", true, false, true);


            gc_StockLeft.MouseWheelChk = false;
            gc_StockRight.MouseWheelChk = false;

            gc_StockLeft.PopMenuChk = false;
            gc_StockRight.PopMenuChk = false;


            DbHelp.No_ReadOnly(gv_StockLeft);
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data("L");
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_StockLeft, "제품별자재투입 현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data(string side = "")
        {
            SqlParam sp = new SqlParam("sp_rptMatOutItem");
            sp.AddParam("Kind", side);

            if (side == "R")
            {
                sp.AddParam("Item_Code", Convert.ToString(gv_StockLeft.GetFocusedRowCellValue("Item_Code")));
                sp.AddParam("Order_No", Convert.ToString(gv_StockLeft.GetFocusedRowCellValue("Order_No")));
            }
            

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
                DbHelp.Footer_Set(gv_StockLeft, ForMat.SetDecimal("regMatOut", "Amt1").ToString(), new string[] { "AMT" });
            }

            else if (side == "R")
            {
                gc_StockRight.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                gc_StockRight.RefreshDataSource();
                gv_StockRight.BestFitColumns();
                DbHelp.Footer_Set(gv_StockRight, ForMat.SetDecimal("regMatOut", "Qty1").ToString(), new string[] { "Qty" });
                DbHelp.Footer_Set(gv_StockRight, ForMat.SetDecimal("regMatOut", "Amt1").ToString(), new string[] { "Amt" });
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
