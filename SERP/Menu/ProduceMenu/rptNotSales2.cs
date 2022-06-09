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
    public partial class rptNotSales2 : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptNotSales2()
        {
            InitializeComponent();
        }

        private void rptNotSales2_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Custom_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Custom_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Project_Title", "수주제목", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Dept_Name", "영업부서", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_Qty", "수주수량", "80", true, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Not_Qty", "미출고수량", "80", true, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "S_Price", "단가", "150", true, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Not_Amt", "미출고금액", "150", true, false, false);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Cf_Date", "확정일", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "MatOut_Date", "자재출고일", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "WorkResult_Date", "생산입고일", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Qc_Date", "검사완료일", "125", false, false, true);

            DbHelp.GridColumn_NumSet(gv_OrderM, "Order_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_OrderM, "Not_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_OrderM, "S_Price", ForMat.SetDecimal(this.Name, "Price1"));
            DbHelp.GridColumn_NumSet(gv_OrderM, "Not_Amt", ForMat.SetDecimal(this.Name, "Amt1"));

            DbHelp.No_ReadOnly(gv_OrderM);
        }

        #region 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptNotSales");
                sp.AddParam("Kind", "S");
                sp.AddParam("dt_FOrder", dt_FOrder.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_TOrder", dt_TOrder.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_TOrder.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_FDelivery", dt_FDelivery.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_TDelivery", dt_TDelivery.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_TDelivery.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_OrderM.DataSource = ret.ReturnDataSet.Tables[0];
                gv_OrderM.BestFitColumns();

                DbHelp.Footer_Set(gv_OrderM, ForMat.SetDecimal("regSales", "Qty1").ToString(), new string[] { "Order_Qty", "Not_Qty" });
                //DbHelp.Footer_Set(gv_OrderM, ForMat.SetDecimal("regSales", "Price1").ToString(), new string[] { "S_Price" });
                DbHelp.Footer_Set(gv_OrderM, ForMat.SetDecimal("regSales", "Amt1").ToString(), new string[] { "Not_Amt" });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        #region 버튼 이벤트

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_OrderM, "미출고현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        #endregion

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
