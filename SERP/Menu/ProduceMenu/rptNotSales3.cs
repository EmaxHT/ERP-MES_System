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
    public partial class rptNotSales3 : BaseReg
    {
        public rptNotSales3()
        {
            InitializeComponent();
        }

        private void rptNotSales3_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언

            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Custom_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Custom_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_Custom_Code", "외주처 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Out_Custom_Name", "외주처", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Project_Title", "수주제목", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Dept_Name", "영업부서", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Order_Qty", "수주수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Not_Qty", "미출고수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "S_Price", "단가", "150", true, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Not_Amt", "미출고금액", "150", true, false, false);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Cf_Date", "확정일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "MatOut_Date", "자재출고일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "WorkResult_Date", "생산입고일", "125", false, false, true);
            DbHelp.GridSet(gc_OutWorkM, gv_OutWorkM, "Qc_Date", "검사완료일", "125", false, false, true);

            DbHelp.No_ReadOnly(gv_OutWorkM);
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
            FileIF.Excel_Down(gc_OutWorkM, "외주지시진행현황");
        }
    }
}
