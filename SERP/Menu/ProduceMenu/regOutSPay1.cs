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
    public partial class regOutSPay1 : BaseReg
    {
        public regOutSPay1()
        {
            InitializeComponent();
        }

        private void regOutSPay_Load(object sender, EventArgs e)
        {
            Grid_Set();

            dt_EnterDate.Focus();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "In_Date", "입고일자", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Item_Name", "품명", "100", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "P_Price", "단가", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Amt", "입고금액", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Vat_Amt", "부가세액", "100", true, false, true);
            DbHelp.GridSet(gc_OutPayM, gv_OutPayM, "Tot_Amt", "합계금액", "100", true, false, true);
        }

        private void txt_WHouse_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void txt_User_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void txt_Pay_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void txt_CustomCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        

        private void btn_Select_Click(object sender, EventArgs e)
        {

        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {

        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {

        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_OutPayM, "외주비정산");
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

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);
            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        #endregion
    }
}
