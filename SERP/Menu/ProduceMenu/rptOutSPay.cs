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
    public partial class rptOutSPay : BaseReg
    {
        public rptOutSPay()
        {
            InitializeComponent();
        }

        private void rptOutSPay_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Pay_Date", "결제일자", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Pay_No", "결제번호", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Custom_Code", "외주처 코드", "120", false, true, false);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Short_Name", "외주처", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Pay_Date", "입고일자", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Order_No", "수주번호", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Item_Name", "품명", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "P_Price", "단가", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Amt", "입고금액", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Vat_Amt", "부가세액", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Tot_Amt", "합계금액", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Pay_Amt", "결제금액", "100", true, false, true);
        }

        

        private void btn_Select_Click(object sender, EventArgs e)
        {

        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_OutPayM, "외주비현황");
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
    }
}
