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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;

namespace SERP
{
    public partial class rptMatInItem : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptMatInItem()
        {
            InitializeComponent();
        }

        private void rptMatInItem_Load(object sender, EventArgs e)
        {            
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;

            Grid_Set();
            dtYear.DateTime = DateTime.Now;
            btn_Select_Click(null, null);
        }

        private void Grid_Set()
        {
            gv_MatInM.OptionsView.ShowFooter = true;

            // Quot_M 그리드
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Name", "품목명", "120", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Jan", "1월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Fab", "2월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Mar", "3월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Apr", "4월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "May", "5월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Jun", "6월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Jul", "7월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Aug", "8월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Sep", "9월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Oct", "10월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Nov", "11월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Dec", "12월", "125", false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Total", "총액", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_MatInM, "Jan", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Fab", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Mar", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Apr", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "May", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Jun", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Jul", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Aug", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Sep", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Oct", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Nov", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Dec", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Total", ForMat.SetDecimal("regMatIn", "Price1"));

            gc_MatInM.PopMenuChk = false;
            gc_MatInM.MouseWheelChk = false;

            Footer_Set();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatInM, "구매현황 (품목)");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatIn");
            sp.AddParam("Kind", "I");
            sp.AddParam("Year", dtYear.Text);
            if (!string.IsNullOrWhiteSpace(txt_DeptName.Text))
                sp.AddParam("Dept", txt_DeptCode.Text);
            if (!string.IsNullOrWhiteSpace(txt_Cust_Name.Text))
                sp.AddParam("Cust", txt_Cust.Text);
            if (!string.IsNullOrWhiteSpace(txt_User_Name.Text))
                sp.AddParam("Charger", txt_Charger.Text);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_MatInM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_MatInM.RefreshDataSource();

            Set_Total();
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



        // 부서
        private void txt_DeptCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_DeptName.Text = PopHelpForm.Return_Help("sp_Help_Dept", txt_DeptCode.Text);
        }

        private void txt_DeptCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_DeptCode_Properties_ButtonClick(sender, null);
        }

        private void txt_DeptCode_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("Dept", "sp_Help_Dept", txt_DeptCode.Text, "N");
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_DeptCode.Text = HelpForm.sRtCode;
                txt_DeptName.Text = HelpForm.sRtCodeNm;
            }
        }

        //거래처
        private void txt_Cust_EditValueChanged(object sender, EventArgs e)
        {
            txt_Cust_Name.Text = PopHelpForm.Return_Help("sp_Help_Custom", txt_Cust.Text);
        }

        private void txt_Cust_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Cust_Properties_ButtonClick(sender, null);
        }

        private void txt_Cust_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("Custom", "sp_Help_Custom", txt_Cust.Text, "N");
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_Cust.Text = HelpForm.sRtCode;
                txt_Cust_Name.Text = HelpForm.sRtCodeNm;
            }
        }


        // 담당자
        private void txt_Charger_EditValueChanged(object sender, EventArgs e)
        {
            txt_User_Name.Text = PopHelpForm.Return_Help("sp_Help_User", txt_Charger.Text, "", "");
        }

        private void txt_Charger_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Charger_Properties_ButtonClick(sender, null);
        }

        private void txt_Charger_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PopHelpForm Helpform = new PopHelpForm("User", "sp_Help_User", txt_Charger.Text, "N");
            if (Helpform.ShowDialog() == DialogResult.OK)
            {
                txt_Charger.Text = Helpform.sRtCode;
                txt_User_Name.Text = Helpform.sRtCodeNm;
            }
        }

        private void Set_Total()
        {
            for (int i = 0; i < gv_MatInM.RowCount; i++)
            {
                decimal sum = 0;

                foreach (GridColumn col in gv_MatInM.Columns)
                {
                    string temp = Convert.ToString(gv_MatInM.GetRowCellValue(i, col));
                    if (decimal.TryParse(temp, out decimal d))
                        sum += (string.IsNullOrWhiteSpace(temp)) ? 0 : Convert.ToDecimal(temp);
                }

                gv_MatInM.SetRowCellValue(i, "Total", sum);
            }
        }

        private void Footer_Set()
        {
            foreach (GridColumn col in gv_MatInM.Columns)
            {
                if (col.Caption.Contains("월") || col.FieldName == "Total")
                {
                    GridColumnSummaryItem summaryItem = new GridColumnSummaryItem();
                    summaryItem.FieldName = col.FieldName;
                    summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    summaryItem.DisplayFormat = "{0:n" + ForMat.SetDecimal("regMatIn", "Price1") + "}";

                    gv_MatInM.Columns[col.FieldName].Summary.Add(summaryItem);
                }
            }
        }
    }
}
