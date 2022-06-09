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
    public partial class rptEquipReq : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptEquipReq()
        {
            InitializeComponent();
        }
        
        private void rptEquipReq_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
            btn_Select_Click(null, null);
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(GridMaster, ViewMaster, "Req_No", "의뢰번호", "100", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Req_Date", "의뢰일자", "120", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Equip_Code", "설비코드", "100", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Equip_Name", "설비명", "130", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Req_Memo", "의뢰내용", "200", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Repair_Date", "수리일자", "120", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Short_Name", "수리업체", "150", false, false, true);
            DbHelp.GridSet(GridMaster, ViewMaster, "Repair_Amt", "수리비용", "150", true, false, true);

            DbHelp.No_ReadOnly(ViewMaster);

            GridMaster.MouseWheelChk = false;
            GridMaster.PopMenuChk = false;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(GridMaster, "설비별 수리 내역 ");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptEquipReq");
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
            GridMaster.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            GridMaster.RefreshDataSource();
            ViewMaster.BestFitColumns();

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


    }
}
