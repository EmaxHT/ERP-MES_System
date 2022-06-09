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
    public partial class rptMatPay : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatPay()
        {
            InitializeComponent();
        }

        private void rptMatPay_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_MatPay, gv_MatPay, "Pay_Date", "결제일자", "125", false, false, true);
            DbHelp.GridSet(gc_MatPay, gv_MatPay, "Pay_No", "결제번호", "150", false, false, true);
            DbHelp.GridSet(gc_MatPay, gv_MatPay, "Custom_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_MatPay, gv_MatPay, "MatIn_No", "입고번호", "150", true, false, true);
            DbHelp.GridSet(gc_MatPay, gv_MatPay, "MatIn_Date", "입고일자", "150", true, false, true);
            DbHelp.GridSet(gc_MatPay, gv_MatPay, "Pay_Amt", "결제금액", "150", true, false, true);

            DbHelp.GridColumn_NumSet(gv_MatPay, "Pay_Amt", ForMat.SetDecimal(this.Name, "Amt1"));

            DbHelp.No_ReadOnly(gv_MatPay);

            gc_MatPay.PopMenuChk = false;
            gc_MatPay.MouseWheelChk = false;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatPay, "결제현황");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptMatPay");
                sp.AddParam("Kind", "S");
                sp.AddParam("FDATE", dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDATE", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));


                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_MatPay.DataSource = ret.ReturnDataSet.Tables[0];
                gv_MatPay.BestFitColumns();

                DbHelp.Footer_Set(gv_MatPay, ForMat.SetDecimal(this.Name, "Amt1").NullString(), new string[] { "Pay_Amt" });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
