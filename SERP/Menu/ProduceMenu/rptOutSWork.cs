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
    public partial class rptOutSWork : BaseReg
    {
        public rptOutSWork()
        {
            InitializeComponent();
        }

        private void rptOutSWork_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_Part", "구분코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_Part_Name", "구분", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_Custom_Code", "외주처 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_Custom_Name", "외주처", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Custom_Code", "거래처 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Short_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Custom_Code1", "고객사 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Short_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Project_Title", "수주제목", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Area_Name", "납품현장", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "User_Code", "영업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Dept_Code", "영업부서 코드", "100", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Dept_Name", "영업부서", "100", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Del_Memo1", "납품가능일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Work_User", "작업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Work_Name", "작업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_User", "납품담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_Name", "납품담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Work_Date", "작지일자", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "WorkResult_Date", "생산입고예정일", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "QC_Date", "검사예정일", "150", false, false, true);
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
            FileIF.Excel_Down(gc_OutWorkM, "외주지시현황");
        }
    }
}
