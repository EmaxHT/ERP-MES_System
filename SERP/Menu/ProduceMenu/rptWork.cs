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
    public partial class rptWork : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        public rptWork()
        {
            InitializeComponent();
        }

        private void rptWork_Load(object sender, EventArgs e)
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

            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Date", "수주일자", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_No", "수주번호", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Part", "구분코드", "120", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Part_Name", "구분", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code", "거래처 코드", "150", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Short_Name", "거래처", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code1", "고객사 코드", "150", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Short_Name1", "고객사", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Project_Title", "수주제목", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Area_Name", "납품현장", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "User_Code", "영업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "User_Name", "영업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Dept_Code", "영업부서 코드", "100", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Dept_Name", "영업부서", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qty", "수량", "80", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Del_Memo1", "납품가능일", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Delivery", "고객협의일", "125", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_User", "작업담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_Name", "작업담당자", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_User", "납품담당자 코드", "150", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_Name", "납품담당자", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_Date", "작지일자", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "WorkResult_Date", "생산입고예정일", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "QC_Date", "검사예정일", "150", false, false, true);

            DbHelp.GridColumn_NumSet(gv_WorkM, "Qty", ForMat.SetDecimal("regWork", "Qty1"));

            DbHelp.No_ReadOnly(gv_WorkM);
            gc_WorkM.PopMenuChk = false;
            gc_WorkM.MouseWheelChk = false;
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptWork");

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

            DbHelp.Footer_Set(gv_WorkM, ForMat.SetDecimal("regWork", "Qty1").ToString(), new string[] { "Qty" });
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
            FileIF.Excel_Down(gc_WorkM, "작업지시현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }


    }
}
