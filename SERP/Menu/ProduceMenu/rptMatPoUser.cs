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
    public partial class rptMatPoUser : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatPoUser()
        {
            InitializeComponent();
        }

        private void rptMatPoUser_Load(object sender, EventArgs e)
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
            gv_MatPoM.OptionsView.ShowFooter = true;

            // Quot_M 그리드
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "User_Name", "담당자", "120", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Jan", "1월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Fab", "2월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Mar", "3월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Apr", "4월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "May", "5월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Jun", "6월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Jul", "7월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Aug", "8월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Sep", "9월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Oct", "10월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Nov", "11월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Dec", "12월", "125", false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Total", "총액", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_MatPoM, "Jan", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Fab", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Mar", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Apr", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "May", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Jun", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Jul", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Aug", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Sep", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Oct", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Nov", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Dec", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Total", ForMat.SetDecimal("regMatPo", "Price1"));

            gc_MatPoM.PopMenuChk = false;
            gc_MatPoM.MouseWheelChk = false;

            Footer_Set();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatPoM, "수주현황 (담당자)");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatPo");
            sp.AddParam("Kind", "U");
            sp.AddParam("Year", dtYear.Text);
            if (!string.IsNullOrWhiteSpace(txt_DeptName.Text))
                sp.AddParam("Dept", txt_DeptCode.Text);
            if (!string.IsNullOrWhiteSpace(txt_ItempartNM.Text))
                sp.AddParam("Item_Part", txt_Item_Part.Text);
            if (!string.IsNullOrWhiteSpace(txt_Cust_Name.Text))
                sp.AddParam("Cust", txt_Cust.Text);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_MatPoM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_MatPoM.RefreshDataSource();

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

        // 품목그룹
        private void txt_Item_Part_EditValueChanged(object sender, EventArgs e)
        {
            txt_ItempartNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_Item_Part.Text, "10041");
        }

        private void txt_Item_Part_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Item_Part_Properties_ButtonClick(sender, null);
        }

        private void txt_Item_Part_Properties_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            PopHelpForm Helpform = new PopHelpForm("General", "sp_Help_General", "10041", txt_Item_Part.Text, "N");
            if (Helpform.ShowDialog() == DialogResult.OK)
            {
                txt_Item_Part.Text = Helpform.sRtCode;
                txt_ItempartNM.Text = Helpform.sRtCodeNm;
            }
        }

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


        // 거래처
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

        private void Set_Total()
        {
            for (int i = 0; i < gv_MatPoM.RowCount; i++)
            {
                decimal sum = 0;

                foreach (GridColumn col in gv_MatPoM.Columns)
                {
                    string temp = Convert.ToString(gv_MatPoM.GetRowCellValue(i, col));
                    if (decimal.TryParse(temp, out decimal d))
                        sum += (string.IsNullOrWhiteSpace(temp)) ? 0 : Convert.ToDecimal(temp);
                }

                gv_MatPoM.SetRowCellValue(i, "Total", sum);
            }
        }

        private void Footer_Set()
        {
            foreach (GridColumn col in gv_MatPoM.Columns)
            {
                if (col.Caption.Contains("월") || col.FieldName == "Total")
                {
                    GridColumnSummaryItem summaryItem = new GridColumnSummaryItem();
                    summaryItem.FieldName = col.FieldName;
                    summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    summaryItem.DisplayFormat = "{0:n" + ForMat.SetDecimal("regMatPo", "Price1") + "}";

                    gv_MatPoM.Columns[col.FieldName].Summary.Add(summaryItem);
                }
            }
        }
    }
}
