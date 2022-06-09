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
    public partial class rptMatStock1 : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptMatStock1()
        {
            InitializeComponent();
        }

        private void rptMatStock1_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_Stock, gv_Stock, "Item_Code", "품목코드", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Item_Name", "품목명", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Ssize", "규격", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Loc_Code", "Location 코드", "100", false, false, false);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Loc_Name", "Location", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "In_No", "입고번호", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "In_Date", "입고일자", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Custom_Code", "거래처 코드", "100", false, false, false);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Short_Name", "거래처", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Qty", "재고수량", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "P_Price", "단가", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "Amt", "금액", "100", false, false, true);
            DbHelp.GridSet(gc_Stock, gv_Stock, "MatSer_No", "Serial_No", "", false, false, true);

            DbHelp.GridColumn_NumSet(gv_Stock, "Qty", ForMat.SetDecimal("4275", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_Stock, "P_Price", ForMat.SetDecimal("4275", "Price1"));
            DbHelp.GridColumn_NumSet(gv_Stock, "Amt", ForMat.SetDecimal("4275", "Price1"));

            gc_Stock.AddRowYN = false;
            gc_Stock.PopMenuChk = false;
            gc_Stock.MouseWheelChk = false;

            DbHelp.Footer_Set(gv_Stock, "0", new string[] { "Amt" });

            DbHelp.No_ReadOnly(gv_Stock);
        }

        #region 버튼 에디트 이벤트
        private void txt_WhouseCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_WhouseName.Text = PopHelpForm.Return_Help("sp_Help_Whouse", txt_WhouseCode.Text);
        }

        private void txt_WhouseCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_WhouseCode_Properties_ButtonClick(null, null);
        }

        private void txt_WhouseCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm Help_Form = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_WhouseCode.Text, "N");

            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                txt_WhouseCode.Text = Help_Form.sRtCode;
                txt_WhouseName.Text = Help_Form.sRtCodeNm;
            }
        }
        #endregion

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_Stock, "창고별 재고현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            if (!string.IsNullOrWhiteSpace(txt_WhouseName.Text))
            {
                SqlParam sp = new SqlParam("sp_rptMatStock");
                sp.AddParam("Kind", "W");
                sp.AddParam("Whouse_Code", txt_WhouseCode.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_Stock.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                gc_Stock.RefreshDataSource();
                gv_Stock.BestFitColumns();

                DbHelp.Footer_Set(gv_Stock, ForMat.SetDecimal(this.Name, "Qty1").NullString(), new string[] { "Qty" });
                DbHelp.Footer_Set(gv_Stock, ForMat.SetDecimal(this.Name, "Amt1").NullString(), new string[] { "Amt" });
            }
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

        private void txt_WhouseName_EditValueChanged(object sender, EventArgs e)
        {
            Search_Data();
        }
    }
}
