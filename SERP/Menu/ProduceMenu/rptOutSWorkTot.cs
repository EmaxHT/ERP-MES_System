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
    public partial class rptOutSWorkTot : BaseReg
    {
        public rptOutSWorkTot()
        {
            InitializeComponent();
        }

        private void rptOutSWorkTot_Load(object sender, EventArgs e)
        {         
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Kind", "구분", "80", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Jan", "1월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Fab", "2월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Mar", "3월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Apr", "4월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "May", "5월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Jun", "6월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Jul", "7월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Aug", "8월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Sep", "9월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Oct", "10월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Nov", "11월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Dec", "12월", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Total", "총액", "125", false, false, true);
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
            FileIF.Excel_Down(gc_OutWorkM, "외주지시집계현황");
        }
    }
}
