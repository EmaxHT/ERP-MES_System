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
    public partial class rptMatOut : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatOut()
        {
            InitializeComponent();
        }

        private void rptMatOut_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Order_No", "수주번호", "125", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Order_Date", "수주일자", "80", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Short_Name", "거래처", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Name", "제품명", "150", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Ssize", "규격", "80", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "SItem_Name", "자재명", "80", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "SItem_size", "자재구격", "120", false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Qty", "총소요량", "80", true, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Out_Date", "출고일자", "80", true, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Out_Qty", "출고수량", "80", true, false, true);

            DbHelp.GridColumn_NumSet(gv_MatOutM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "Out_Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            DbHelp.No_ReadOnly(gv_MatOutM);
            gc_MatOutM.PopMenuChk = false;
            gc_MatOutM.MouseWheelChk = false;
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatOutM, "수주자채 출고현황");
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_rptMatOut");
                sp.AddParam("Kind", "S");
                sp.AddParam("FDATE", dt_From.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("TDATE", dt_To.Text == "" ? DateTime.MaxValue.ToString("yyyyMMdd") : dt_To.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_MatOutM.DataSource = ret.ReturnDataSet.Tables[0];
                gv_MatOutM.BestFitColumns();
                DbHelp.Footer_Set(gv_MatOutM, ForMat.SetDecimal("regMatOut", "Qty1").ToString(), new string[] { "Qty" });
                DbHelp.Footer_Set(gv_MatOutM, ForMat.SetDecimal("regMatOut", "Qty1").ToString(), new string[] { "Out_Qty" });
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
    }
}
