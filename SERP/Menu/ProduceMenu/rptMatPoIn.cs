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
    public partial class rptMatPoIn : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatPoIn()
        {
            InitializeComponent();
        }

        private void rptMatPoIn_Load(object sender, EventArgs e)
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
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Po_Date", "발주일자", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "In_Date", "입고예정일", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Po_No", "발주번호", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Short_Name", "거래처", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "User_Name", "영업담당자", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Dept_Name", "영업부서", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Ssize", "구격", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Qty", "발주수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "InGo_Date", "입고일자", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "In_Qty", "입고수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Not_Qty", "미입고수량", "80", true, true, true, true);

            DbHelp.GridColumn_NumSet(gv_MatPoM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "In_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Not_Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            DbHelp.No_ReadOnly(gv_MatPoM);
        }

        #region 버튼 이벤트

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatPoM, "발주대비 입고현황");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        #endregion

        #region 내부 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptMatPoIn");
                sp.AddParam("Kind", "S");
                sp.AddParam("FDate_Po", dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDate_Po", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("FDate_In", dt_FIn.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDate_In", dt_InT.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_InT.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_MatPoM.DataSource = ret.ReturnDataSet.Tables[0];
                gv_MatPoM.BestFitColumns();

                DbHelp.Footer_Set(gv_MatPoM, ForMat.SetDecimal("regMatPo", "Qty1").NullString(), new string[] { "Qty", "In_Qty", "Not_Qty" });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
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
    }
}