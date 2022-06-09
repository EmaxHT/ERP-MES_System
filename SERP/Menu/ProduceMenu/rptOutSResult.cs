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
    public partial class rptOutSResult : BaseReg
    {
        public rptOutSResult()
        {
            InitializeComponent();
        }

        private void rptOutSResult_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드

            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Result_No", "입고번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Result_Date", "입고일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Process_Code", "공정코드", "100", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Out_Custom_Code", "외주처 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Out_Custom_Name", "외주처", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Process_Name", "공정", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Whouse_Code", "입고창고 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Whouse_Name", "입고창고", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Custom_Code1", "고객사 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Short_Name", "고객사 코드", "150", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "User_Code", "영업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Dept_Code", "영업부서 코드", "100", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Dept_Name", "영업부서", "100", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Qty", "지시수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "In_Qty", "입고수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Work_User", "작업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Work_Name", "작업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Out_User", "납품담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Out_Name", "납품담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "ItemSer_No", "Serial-No", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "ItemLic_No", "라이센스", "120", false, false, true);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Ver_Code", "버전 코드", "120", false, false, false);
            DbHelp.GridSet(gc_OutResultM, gv_OutResultM, "Ver_Name", "버전", "120", false, false, true);

            DbHelp.No_ReadOnly(gv_OutResultM);
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
            FileIF.Excel_Down(gc_OutResultM, "외주실적현황");
        }
    }
}
