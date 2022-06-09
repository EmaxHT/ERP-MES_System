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
    public partial class rptOrdSales : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        public rptOrdSales()
        {
            InitializeComponent();
        }

        private void rptOrdSales_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            Timer t = new Timer();
            t.Tick += btn_Select_Click;
            t.Interval = 1000 * 60 * 5;
            t.Start();
        }

        private void Grid_Set()
        {
            // Quot_M 그리드
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Delivery", "고객협의일", "125", false, true, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Custom_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Custom_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Project_Title", "수주제목", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Dept_Name", "영업부서", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Order_Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Out_Date", "출고일자", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Out_Qty", "출고수량", "150", false, false, true);
            DbHelp.GridSet(gc_OrderM, gv_OrderM, "Not_Qty", "미출고수량", "80", true, false, true);

            DbHelp.GridColumn_NumSet(gv_OrderM, "Order_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_OrderM, "Not_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_OrderM, "Out_Qty", ForMat.SetDecimal(this.Name, "Qty1"));
        }

        #region 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptOrderSales");
                sp.AddParam("Kind", "S");
                sp.AddParam("dt_FOrder", dt_FOrder.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_TOrder", dt_TOrder.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_TOrder.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_FDelivery", DateTime.MinValue.ToString("yyyyMMdd"));
                sp.AddParam("dt_TDelivery", DateTime.MaxValue.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_OrderM.DataSource = ret.ReturnDataSet.Tables[0];
                gv_OrderM.BestFitColumns();

                DbHelp.Footer_Set(gv_OrderM, ForMat.SetDecimal(this.Name, "Qty1").NullString(), new string[] { "Order_Qty", "Not_Qty", "Out_Qty" });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        #region 버튼 이벤트

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_OrderM, "주문대비 납품현황");
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


    }
}
