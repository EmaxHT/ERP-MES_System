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
    public partial class rptOutSMatOutTot : BaseReg
    {
        public rptOutSMatOutTot()
        {
            InitializeComponent();
        }

        private void rptOutSMatOutTot_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Item_Code", "출고일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Item_Name", "출고창고", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Ssize", "출고구분", "80", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M1", "1월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M2", "2월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M3", "3월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M4", "4월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M5", "5월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M6", "6월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M7", "7월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M8", "8월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M9", "9월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M10", "10월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M11", "11월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "M12", "12월", "120", false, false, true);
            DbHelp.GridSet(gc_OutMatOutM, gv_OutMatOutM, "Total", "합계", "125", false, false, true);
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
            FileIF.Excel_Down(gc_OutMatOutM, "외주자재사용 집계현황");
        }
    }
}
