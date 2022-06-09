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

namespace SERP
{
    public partial class rptMatOut1 : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatOut1()
        {
            InitializeComponent();
        }

        private void rptMatOut1_Load(object sender, EventArgs e)
        {            
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;

            Grid_Set();

            dt_Year.DateTime = DateTime.Now;
            Search();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Code", "출고일자", "125", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Name", "출고창고", "80", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Ssize", "출고구분", "80", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M1", "1월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M2", "2월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M3", "3월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M4", "4월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M5", "5월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M6", "6월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M7", "7월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M8", "8월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M9", "9월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M10", "10월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M11", "11월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "M12", "12월", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Total", "합계", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_MatOutM, "M1", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M2", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M3", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M4", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M5", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M6", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M7", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M8", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M9", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M10", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M11", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "M12", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "Total", ForMat.SetDecimal(this.Name, "Amt1"));

            gc_MatOutM.PopMenuChk = false;
            gc_MatOutM.MouseWheelChk = false;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatOutM, "생산자재집계사용현황");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptMatOut1");
                sp.AddParam("Kind", "S");
                sp.AddParam("DATE", dt_Year.DateTime.ToString("yyyy"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_MatOutM.DataSource = ret.ReturnDataSet.Tables[0];
                Set_Total();

                DbHelp.Group_Footer_Set(gv_MatOutM, ForMat.SetDecimal(this.Name, "Amt1").NullString());
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Set_Total()
        {
            for (int i = 0; i < gv_MatOutM.RowCount; i++)
            {
                decimal sum = 0;

                foreach (GridColumn col in gv_MatOutM.Columns)
                {
                    string temp = Convert.ToString(gv_MatOutM.GetRowCellValue(i, col));
                    if (decimal.TryParse(temp, out decimal d))
                        sum += (string.IsNullOrWhiteSpace(temp)) ? 0 : Convert.ToDecimal(temp);
                }

                gv_MatOutM.SetRowCellValue(i, "Total", sum);
            }
        }
    }
}
