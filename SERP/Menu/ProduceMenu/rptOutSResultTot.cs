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
    public partial class rptOutSResultTot : BaseReg
    {
        public rptOutSResultTot()
        {
            InitializeComponent();
        }

        private void rptOutSResultTot_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Kind", "구분", "80", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Jan", "1월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Fab", "2월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Mar", "3월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Apr", "4월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "May", "5월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Jun", "6월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Jul", "7월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Aug", "8월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Sep", "9월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Oct", "10월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Nov", "11월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Dec", "12월", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Total", "총합", "100", false, false, true);
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
            FileIF.Excel_Down(gc_OutResultM, "외주실적 집계현황");
        }
    }
}
