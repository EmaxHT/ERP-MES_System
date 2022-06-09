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
    public partial class rptMatMove : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatMove()
        {
            InitializeComponent();
        }

        private void rptMatMove_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Move_Date", "이동일자", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "IN_HOUSE", "입고창고", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "OUT_HOUSE", "출고창고", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Move_Part", "이동사유", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Name", "품목명", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qty", "수량", "150", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "MatSer_No", "시리얼넘버", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Loc_Code", "출고 Location", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Loc_Code1", "입고 Location", "100", false, false, true);

            DbHelp.No_ReadOnly(gv_WorkM);

            gc_WorkM.PopMenuChk = false;
            gc_WorkM.MouseWheelChk = false;
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMatMove");

            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("To", dt_To.Text.Replace("-", ""));
            else
                sp.AddParam("To", DateTime.MaxValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            gc_WorkM.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            gc_WorkM.RefreshDataSource();
            gv_WorkM.BestFitColumns();

            DbHelp.Footer_Set(gv_WorkM, ForMat.SetDecimal(this.Name, "Qty1").NullString(), new string[] { "Qty" });
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
            FileIF.Excel_Down(gc_WorkM, "이동현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
    }
}
