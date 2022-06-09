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
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;

namespace SERP
{
    public partial class rptWorkTot : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptWorkTot()
        {
            InitializeComponent();
        }

        private void rptWorkTot_Load(object sender, EventArgs e)
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
            // WorkM 그리드

            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Kind", "구분", "80", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Jan", "1월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Fab", "2월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Mar", "3월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Apr", "4월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "May", "5월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Jun", "6월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Jul", "7월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Aug", "8월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Sep", "9월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Oct", "10월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Nov", "11월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Dec", "12월", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Total", "총액", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_WorkM, "Jan", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Fab", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Mar", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Apr", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "May", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Jun", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Jul", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Aug", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Sep", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Oct", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Nov", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Dec", ForMat.SetDecimal("regOrder", "Price1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "Total", ForMat.SetDecimal("regOrder", "Price1"));
            
            gc_WorkM.PopMenuChk = false;
            gc_WorkM.MouseWheelChk = false;

            gv_WorkM.OptionsView.ShowFooter = true;
            Footer_Set();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_WorkM, "작업지시 집계표");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptWorkTot");
            sp.AddParam("Year", dtYear.DateTime.Year.ToString());

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_WorkM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            Set_Total();
            gc_WorkM.RefreshDataSource();
        }

        private void Set_Total()
        {
            for (int i = 0; i < gv_WorkM.RowCount; i++)
            {
                decimal sum = 0;

                foreach (GridColumn col in gv_WorkM.Columns)
                {
                    string temp = Convert.ToString(gv_WorkM.GetRowCellValue(i, col));
                    if (decimal.TryParse(temp, out decimal d))
                        sum += (string.IsNullOrWhiteSpace(temp)) ? 0 : Convert.ToDecimal(temp);
                }

                gv_WorkM.SetRowCellValue(i, "Total", sum);
            }
        }

        private void Footer_Set()
        {
            foreach (GridColumn col in gv_WorkM.Columns)
            {
                if (col.Caption.Contains("월") || col.FieldName == "Total")
                {
                    GridColumnSummaryItem summaryItem = new GridColumnSummaryItem();
                    summaryItem.FieldName = col.FieldName;
                    summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    summaryItem.DisplayFormat = "{0:n" + ForMat.SetDecimal("regOrder", "Price1") + "}";

                    gv_WorkM.Columns[col.FieldName].Summary.Add(summaryItem);
                }
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
    }
}
