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
    public partial class rptMatNotOut : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptMatNotOut()
        {
            InitializeComponent();
        }

        private void rptMatNotOut_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Order_Date", "수주일자", "130", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Order_No", "수주번호", "130", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Custom_Code", "거래처코드", "", false, false, false, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Short_Name", "거래처", "150", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "MItem_Code", "품목코드", "120", false, false, false, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "MItem_Name", "품목명", "130", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M_Ssize", "규격", "150", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Code", "자재코드", "120", false, false, false, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Name", "자재명", "130", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Ssize", "규격", "150", false, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Tot_Qty", "총소요량", "80", true, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Out_Qty", "출고수량", "80", true, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "UnReleased", "미출고수량", "80", true, false, true, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Stock_Qty", "재고수량", "80", true, false, true, false);

            DbHelp.No_ReadOnly(gv_MatOutM);
            gc_MatOutM.PopMenuChk = false;
            gc_MatOutM.MouseWheelChk = false;
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
            FileIF.Excel_Down(gc_MatOutM, "자재미출고현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatNotOut");

            string from = (string.IsNullOrWhiteSpace(dt_From.Text)) ? DateTime.MinValue.ToString("yyyyMMdd") : dt_From.Text;
            string to = (string.IsNullOrWhiteSpace(dt_To.Text)) ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.Text;
            sp.AddParam("From", from);
            sp.AddParam("To", to);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_MatOutM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_MatOutM.RefreshDataSource();
            gv_MatOutM.BestFitColumns();
        }
    }
}
