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
using DevExpress.XtraEditors.Repository;

namespace SERP
{
    public partial class regWork1 : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regWork1()
        {
            InitializeComponent();
        }
        
        private void regWork1_Load(object sender, EventArgs e)
        {
            dt_S.Text = DateTime.Today.ToString("yyyy-MM-dd");
            dt_E.Text = DateTime.Today.ToString("yyyy-MM-dd");

            Grid_Set();

            btn_Select_Click(null, null);
        }

        private void Grid_Set()
        {
            //gc_EnterM 그리드
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Date", "수주일자", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_No", "수주번호", "130", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Part", "구분", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code", "거래처", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code1", "고객사", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Project_Title", "수주제목", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Area_Name", "납품현장", "120", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "User_Code", "영업담당자", "100", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Dept_Code", "영업부서", "100", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qty", "수량", "80", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Del_Memo1", "납품가능일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Delivery", "고객협의일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Make_CodeNM", "외주처", "100", false, true, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Make_Code", "외주처코드", "", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_UserNM", "작업담당자명", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_User", "작업담당자코드", "120", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_UserNM", "납품담당자명", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_User", "납품담당자코드", "120", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_Date", "작지일자", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "WorkResult_Date", "생산 입고 예정일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "QC_Date", "검사 예정일", "100", false, false, true);

            DbHelp.GridColumn_Help(gv_WorkM, "Make_CodeNM", "Y");
            RepositoryItemButtonEdit button_Help_M2 = (RepositoryItemButtonEdit)gv_WorkM.Columns["Make_CodeNM"].ColumnEdit;
            button_Help_M2.Buttons[0].Click += new EventHandler(grid_Cust_Help);
            gv_WorkM.Columns["Make_CodeNM"].ColumnEdit = button_Help_M2;

            //gv_WorkM.OptionsView.ShowAutoFilterRow = false;
        }

        #region Help 함수

        //수정
        private void grid_Cust_Help(object sender, EventArgs e)
        {
            //품목 정보 수정 필요
            int iRow = gv_WorkM.GetFocusedDataSourceRowIndex();

            if (gv_WorkM.FocusedColumn.FieldName == "Make_CodeNM")
            {
                if (string.IsNullOrWhiteSpace(gv_WorkM.GetRowCellValue(iRow, "Make_Code").ToString()))
                {
                    PopHelpForm HelpForm = new PopHelpForm("Custom", "sp_Help_Custom", gv_WorkM.GetRowCellValue(iRow, "Make_CodeNM").ToString(), "N");
                    if (HelpForm.ShowDialog() == DialogResult.OK)
                    {
                        gv_WorkM.SetRowCellValue(iRow, "Make_Code", HelpForm.sRtCode);
                        gv_WorkM.SetRowCellValue(iRow, "Make_CodeNM", HelpForm.sRtCodeNm);
                    }
                }
            }
        }

        #endregion

        #region 그리드 이벤트 조회

        private void gv_WorkM_DoubleClick(object sender, EventArgs e)
        {
            int iRow = gv_WorkM.GetFocusedDataSourceRowIndex();

            if (iRow < 0)
                return;

            string sOrderNO = gv_WorkM.GetRowCellValue(iRow, "Order_No").ToString();
            string sItemCode = gv_WorkM.GetRowCellValue(iRow, "Item_Code").ToString();

            PopWorkForm WorkForm = new PopWorkForm();
            WorkForm.StartPosition = FormStartPosition.CenterParent;
            WorkForm.sOrder_No = sOrderNO;
            WorkForm.sItem_Code = sItemCode;

            WorkForm.Show();
        }

        private void gv_WorkM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //수정
            if (e.Column.FieldName == "Make_CodeNM")
            {
                string sMakeCode = gv_WorkM.GetRowCellValue(e.RowHandle, "Make_Code").ToString();
                string sMakeCodeNM = "";

                if (string.IsNullOrWhiteSpace(sMakeCode))
                {
                    sMakeCodeNM = PopHelpForm.Return_Help("sp_Help_Custom", e.Value.ToString());
                    if (!string.IsNullOrWhiteSpace(sMakeCodeNM))
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Make_Code", e.Value.ToString());
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Make_CodeNM", sMakeCodeNM);
                    }
                }
                else
                {
                    sMakeCodeNM = PopHelpForm.Return_Help("sp_Help_Custom", sMakeCode);
                    if(sMakeCodeNM != e.Value.ToString())
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Make_Code", "");
                    }
                }
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

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        #endregion


        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            gc_WorkM.DataSource = null;

            try
            {
                SqlParam sp = new SqlParam("sp_regWork1");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "H");
                sp.AddParam("Date_S", dt_S.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Date_E", dt_E.Text == "" ? DateTime.Today.ToString("yyyyMMdd") : dt_E.DateTime.ToString("yyyyMMdd"));

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_WorkM.DataSource = ret.ReturnDataSet.Tables[0];

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                int iRow = gv_WorkM.GetFocusedDataSourceRowIndex();

                SqlParam sp = new SqlParam("sp_regWork1");
                sp.AddParam("Kind", "I");
                sp.AddParam("Order_No", gv_WorkM.GetRowCellValue(iRow, "Order_No").ToString());
                sp.AddParam("Item_Code", gv_WorkM.GetRowCellValue(iRow, "Item_Code").ToString());
                sp.AddParam("Make_Code", gv_WorkM.GetRowCellValue(iRow, "Make_Code").ToString());
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                btn_Select_Click(null, null);

                btn_Save.sCHK = "Y";

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_WorkM, "외주지시등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        #endregion


        private void gc_WorkM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (gv_WorkM.FocusedColumn == gv_WorkM.Columns["Make_CodeNM"])
                {
                    grid_Cust_Help(null, null);
                }
            }
        }
    }
}