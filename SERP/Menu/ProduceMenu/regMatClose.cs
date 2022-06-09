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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;

namespace SERP
{
    public partial class regMatClose : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regMatClose()
        {
            InitializeComponent();
        }

        private void regMatClose_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            dt_F.Focus();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "In_No", "입고번호", "130", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "In_Date", "입고일자", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Custom_Name", "거래처", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "User_Name", "입고담당자", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Whouse_Name", "입고창고", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Amt", "입고금액", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Vat_Amt", "부가세액", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Tol_Amt", "합계금액", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Close_Ck", "마감여부", "80", false, true, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Close_Date", "마감일자", "100", false, false, true);
            DbHelp.GridSet(gc_MatClose, gv_MatClose, "Close_User", "마감자", "100", false, false, true);

            DbHelp.GridColumn_NumSet(gv_MatClose, "Amt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatClose, "Vat_Amt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatClose, "Tol_Amt", ForMat.SetDecimal(this.Name, "Amt1"));

            DbHelp.GridColumn_CheckBox(gv_MatClose, "Close_Ck");
        }

        #region 내부 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatClose");
                sp.AddParam("Kind", "S");
                sp.AddParam("dt_F", dt_F.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("dt_T", dt_T.Text == "" ? DateTime.Now.ToString("yyyyMMdd") : dt_T.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_MatClose.DataSource = ret.ReturnDataSet.Tables[0];
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion


        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sInNo = "", sClose = "";

                for(int i = 0; i < gv_MatClose.RowCount; i++)
                {
                    if(gv_MatClose.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        sInNo += gv_MatClose.GetRowCellValue(i, "In_No").ToString() + "_/";
                        sClose += gv_MatClose.GetRowCellValue(i, "Close_Ck").ToString() + "_/";
                    }
                }

                SqlParam sp = new SqlParam("sp_regMatClose");
                sp.AddParam("Kind", "I");
                sp.AddParam("InNo", sInNo);
                sp.AddParam("CloseCk", sClose);
                sp.AddParam("User_Code", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                Search();
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatClose, "매입마감등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }


        #endregion
    }
}
