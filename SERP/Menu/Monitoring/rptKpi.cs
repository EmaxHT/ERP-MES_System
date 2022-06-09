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
    public partial class rptKpi : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptKpi()
        {
            InitializeComponent();
        }

        private void rptKpi_Load(object sender, EventArgs e)
        {
            Grid_Set();

            dtYear.DateTime = DateTime.Now;

            Search_Data();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드
            gc_Kpi.PopMenuChk = false;
            gc_Kpi.MouseWheelChk = false;

            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Item_Part", "품목그룹", "125", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Make_Qty", "생산수량", "125", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Bad_Qty", "불량수량", "120", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Tot_Qty", "총작업량", "150", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Bad_Per", "불량률", "150", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "L_Time", "총 작업시간", "120", false, false, true);
            DbHelp.GridSet(gc_Kpi, gv_Kpi, "Time", "평균시간", "150", false, false, true);

            DbHelp.GridColumn_NumSet(gv_Kpi, "Make_Qty", 0);
            DbHelp.GridColumn_NumSet(gv_Kpi, "Bad_Qty", 0);
            DbHelp.GridColumn_NumSet(gv_Kpi, "Tot_Qty", 0);
            DbHelp.GridColumn_NumSet(gv_Kpi, "Bad_Per", 2);
            DbHelp.GridColumn_NumSet(gv_Kpi, "L_Time", 0);
            DbHelp.GridColumn_NumSet(gv_Kpi, "Time", 1);


            gc_KpiTot.PopMenuChk = false;
            gc_KpiTot.MouseWheelChk = false;

            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "Part", "구분", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M01", "1월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M02", "2월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M03", "3월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M04", "4월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M05", "5월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M06", "6월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M07", "7월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M08", "8월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M09", "9월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M10", "10월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M11", "11월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "M12", "12월", "120", false, false, true);
            DbHelp.GridSet(gc_KpiTot, gv_KpiTot, "Total", "합계", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_KpiTot, "M01", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M02", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M03", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M04", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M05", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M06", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M07", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M08", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M09", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M10", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M11", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "M12", 1);
            DbHelp.GridColumn_NumSet(gv_KpiTot, "Total", 1);

            gv_KpiTot.RowHeight = 43;
            gv_KpiTot.ColumnPanelRowHeight = 43;

            gv_KpiTot.Appearance.HeaderPanel.Font = new Font("나눔바른고딕", 20F, FontStyle.Regular);
            gv_KpiTot.Appearance.Row.Font = new Font("나눔바른고딕", 20F, FontStyle.Regular);
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

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_Kpi, this.Name);
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptKpi");
                sp.AddParam("Kind", "S");
                sp.AddParam("dt_F", dt_From.Text == "" ? "" : dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_T", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Year", dtYear.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_Kpi.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                gv_Kpi.BestFitColumns();

                gc_KpiTot.DataSource = ret.ReturnDataSet.Tables[1];
                gv_KpiTot.BestFitColumns();

                DbHelp.Footer_Set(gv_Kpi, "0", new string[] { "Make_Qty", "Bad_Qty", "Tot_Qty" });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
