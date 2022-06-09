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
    public partial class rptOutSMatOut : BaseReg
    {
        public rptOutSMatOut()
        {
            InitializeComponent();
        }

        private void rptOutSMatOut_Load(object sender, EventArgs e)
        {            
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Out_Date", "출고일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Whouse_Name", "출고창고", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Out_Part", "출고구분", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Out_Custom_Code", "외주처 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Out_Custom_Name", "외주처", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Item_Name", "품목명", "150", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Ssize", "규격", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Q_Unit", "단위", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "In_No", "입고번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "In_Sort", "입고Sort", "120", false, false, false);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "P_Price", "단가", "150", true, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Amt", "출고가액", "150", true, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Out_Bigo", "비고", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "MatSer_No", "시리얼넘버", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Loc_Name", "Location", "120", false, false, true);

            DbHelp.No_ReadOnly(gv_OutMatOutM);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
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

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_OutMatOutM, "외주자재사용현황");
        }
    }
}
