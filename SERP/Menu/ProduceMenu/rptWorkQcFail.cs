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
    public partial class rptWorkQcFail : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptWorkQcFail()
        {
            InitializeComponent();
        }

        private void rptWorkQcFail_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qc_Date", "검사일자", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qc_No", "검사번호", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Short_Name", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Whouse_Name", "입고창고", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qty", "입고수량", "80", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "GoodQty", "정품수량", "80", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "BadQty", "불량수량", "80", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Fail_Code", "불량사유", "150", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qc_Bigo", "비고", "300", true, false, true);

            DbHelp.GridColumn_NumSet(gv_WorkM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "GoodQty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_WorkM, "BadQty", ForMat.SetDecimal(this.Name, "Qty1"));

            DbHelp.No_ReadOnly(gv_WorkM);
        }

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_WorkM, "공정검사 불량현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        #endregion

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        #endregion

        #region 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptWorkQcFail");
                sp.AddParam("Kind", "S");
                sp.AddParam("FDate", dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDate", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_WorkM.DataSource = ret.ReturnDataSet.Tables[0];
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion
    }
}
