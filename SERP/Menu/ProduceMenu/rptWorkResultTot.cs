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
    public partial class rptWorkResultTot : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptWorkResultTot()
        {
            InitializeComponent();
        }

        private void rptWorkResultTot_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Kind", "구분", "80", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Jan", "1월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Fab", "2월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Mar", "3월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Apr", "4월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "May", "5월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Jun", "6월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Jul", "7월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Aug", "8월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Sep", "9월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Oct", "10월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Nov", "11월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Dec", "12월", "100", false, false, true);
            DbHelp.GridSet(gc_ResultM, gv_ResultM, "Total", "총합", "100", false, false, true);

            DbHelp.GridColumn_NumSet(gv_ResultM, "Jan", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Fab", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Mar", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Apr", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "May", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Jun", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Jul", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Aug", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Sep", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Oct", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Nov", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Dec", ForMat.SetDecimal("regOrder", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultM, "Total", ForMat.SetDecimal("regOrder", "Qty1"));
            gc_ResultM.PopMenuChk = false;
            gc_ResultM.MouseWheelChk = false;

            gv_ResultM.OptionsView.ShowFooter = true;
            Footer_Set();
        }


        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_ResultM, "작업지시 집계표");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptWorkResult");
            sp.AddParam("Kind", "T");
            sp.AddParam("Year", dtYear.DateTime.Year.ToString());
            //sp.AddParam("Year", DateTime.Now.Year.ToString());

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_ResultM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            Set_Total();
            gc_ResultM.RefreshDataSource();
        }

        private void Set_Total()
        {
            for (int i = 0; i < gv_ResultM.RowCount; i++)
            {
                decimal sum = 0;

                foreach (GridColumn col in gv_ResultM.Columns)
                {
                    string temp = Convert.ToString(gv_ResultM.GetRowCellValue(i, col));
                    if (decimal.TryParse(temp, out decimal d))
                        sum += (string.IsNullOrWhiteSpace(temp)) ? 0 : Convert.ToDecimal(temp);
                }

                gv_ResultM.SetRowCellValue(i, "Total", sum);
            }
        }

        private void Footer_Set()
        {
            foreach (GridColumn col in gv_ResultM.Columns)
            {
                if (col.Caption.Contains("월") || col.FieldName == "Total")
                {
                    GridColumnSummaryItem summaryItem = new GridColumnSummaryItem();
                    summaryItem.FieldName = col.FieldName;
                    summaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                    summaryItem.DisplayFormat = "{0:n" + ForMat.SetDecimal("regOrder", "Qty1") + "}";

                    gv_ResultM.Columns[col.FieldName].Summary.Add(summaryItem);
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
