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
    public partial class regEquip : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regEquip()
        {
            InitializeComponent();
        }

        private void regEquip_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();
        }

        private void Grid_Set()
        {
            gc_Equip.AddRowYN = true;
            DbHelp.GridSet(gc_Equip, gv_Equip, "Equip_Code", "설비코드", "130", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Equip_Name", "설비명", "100", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Model_No", "Model-No", "100", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Start_Date", "설치일자", "100", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "End_Date", "폐기일자", "100", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Custom_CodeNM", "구매처", "100", false, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Custom_Code", "구매처코드", "100", false, false, false);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Po_Date", "구매일자", "100", true, true, true);
            DbHelp.GridSet(gc_Equip, gv_Equip, "Equip_Memo", "비고", "150", false, true, true);

            DbHelp.GridColumn_Help(gv_Equip, "Custom_CodeNM", "Y");
            RepositoryItemButtonEdit button_Help_M1 = (RepositoryItemButtonEdit)gv_Equip.Columns["Custom_CodeNM"].ColumnEdit;
            button_Help_M1.Buttons[0].Click += new EventHandler(grid_Cust_Help);
            gv_Equip.Columns["Custom_CodeNM"].ColumnEdit = button_Help_M1;

            DbHelp.GridColumn_Data(gv_Equip, "Start_Date");
            DbHelp.GridColumn_Data(gv_Equip, "End_Date");
            DbHelp.GridColumn_Data(gv_Equip, "Po_Date");

            gv_Equip.OptionsView.ShowAutoFilterRow = false;

            gc_Equip.DeleteRowEventHandler += new EventHandler(Delete_D);
        }
      
        #region 함수

        //구매처 Help 함수
        private void grid_Cust_Help(object sender, EventArgs e)
        {
            int iRow = gv_Equip.GetFocusedDataSourceRowIndex();

            PopHelpForm HelpForm = new PopHelpForm("Custom", "sp_Help_Custom", gv_Equip.GetRowCellValue(iRow, "Custom_CodeNM").ToString(), "N");
            HelpForm.sNotReturn = "Y";
            if(HelpForm.ShowDialog() == DialogResult.OK)
            {
                gv_Equip.SetRowCellValue(iRow, "Custom_CodeNM", HelpForm.sRtCodeNm);
                gv_Equip.SetRowCellValue(iRow, "Custom_Code", HelpForm.sRtCode);
            }
        }

        //그리드 품목 조회
        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regEquip");
                sp.AddParam("Kind", "S");

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_Equip.DataSource = ret.ReturnDataSet.Tables[0];
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

      
        //삭제 D
        private void Delete_D(object sender, EventArgs e)
        {
            int iRow = gv_Equip.GetFocusedDataSourceRowIndex();

            try
            {
                SqlParam sp = new SqlParam("sp_regEquip");
                sp.AddParam("Kind", "D");
                sp.AddParam("Equip_Code", gv_Equip.GetRowCellValue(iRow, "Equip_Code").ToString());
                sp.AddParam("Reg_User", GlobalValue.sUserID);

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

        #endregion

        #region 상속 함수

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }
        #endregion

        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            gv_Equip.AddNewRow();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (gv_Equip.RowCount < 1)
                return;

            try
            {
                string sEquipCode = "", sEquipName = "", sModelNo = "", sStartDate = "", sEndDate = "", sCustomCode = "", sPoDate = "", sEquipMemo = "";
                
                for(int i = 0; i < gv_Equip.RowCount; i++)
                {
                    if (gv_Equip.GetDataRow(i).RowState != DataRowState.Unchanged)
                    {
                        if (!string.IsNullOrWhiteSpace(gv_Equip.GetRowCellValue(i, "Equip_Code").ToString()))
                        {
                            sEquipCode += gv_Equip.GetRowCellValue(i, "Equip_Code").ToString() + "_/";
                            sEquipName += gv_Equip.GetRowCellValue(i, "Equip_Name").ToString() + "_/";
                            sModelNo += gv_Equip.GetRowCellValue(i, "Model_No").ToString() + "_/";
                            sStartDate += gv_Equip.GetRowCellValue(i, "Start_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_Equip.GetRowCellValue(i, "Start_Date").ToString()).ToString("yyyyMMdd") + "_/";
                            sEndDate += gv_Equip.GetRowCellValue(i, "End_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_Equip.GetRowCellValue(i, "End_Date").ToString()).ToString("yyyyMMdd") + "_/";
                            sCustomCode += gv_Equip.GetRowCellValue(i, "Custom_Code").ToString() + "_/";
                            sPoDate += gv_Equip.GetRowCellValue(i, "Po_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_Equip.GetRowCellValue(i, "Po_Date").ToString()).ToString("yyyyMMdd") + "_/";
                            sEquipMemo += gv_Equip.GetRowCellValue(i, "Equip_Memo").ToString() + "_/";
                        }
                    }
                }

                SqlParam sp = new SqlParam("sp_regEquip");
                sp.AddParam("Kind", "I");
                sp.AddParam("EquipCode", sEquipCode);
                sp.AddParam("EquipName", sEquipName);
                sp.AddParam("ModelNo", sModelNo);
                sp.AddParam("StartDate", sStartDate);
                sp.AddParam("EndDate", sEndDate);
                sp.AddParam("CustomCode", sCustomCode);
                sp.AddParam("PoDate", sPoDate);
                sp.AddParam("EquipMemo", sEquipMemo);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);
                
                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                Search();

                btn_Save.sCHK = "Y";

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                Delete_D(null, null);

                btn_Delete.sCHK = "Y";
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_Equip, "설비 등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                btn_Save_Click(null, null);
            }

            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion

        #region 그리드 이벤트

        private void gv_Equip_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "Custom_CodeNM")
            {
                string sCustomCode = gv_Equip.GetRowCellValue(e.RowHandle, "Custom_Code").ToString();
                string sCustomCodeNM = "";

                if (string.IsNullOrWhiteSpace(sCustomCode))
                {
                    sCustomCodeNM = PopHelpForm.Return_Help("sp_Help_Custom", e.Value.ToString());
                    if (!string.IsNullOrWhiteSpace(sCustomCodeNM))
                    {
                        gv_Equip.SetRowCellValue(e.RowHandle, "Custom_Code", e.Value.ToString());
                        gv_Equip.SetRowCellValue(e.RowHandle, "Custom_CodeNM", sCustomCodeNM);
                    }
                }
                else
                {
                    sCustomCodeNM = PopHelpForm.Return_Help("sp_Help_Custom", sCustomCode);
                    if (sCustomCodeNM != e.Value.ToString())
                    {
                        gv_Equip.SetRowCellValue(e.RowHandle, "Custom_Code", "");
                        gv_Equip.SetRowCellValue(e.RowHandle, "Custom_CodeNM", "");
                    }
                }
            }
        }

        private void gc_Equip_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if (gv_Equip.FocusedColumn.FieldName == "Custom_CodeNM")
                {
                    grid_Cust_Help(sender, null);
                }
            }
        }

        #endregion

        private void gv_Equip_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            if(gv_Equip.GetDataRow(e.FocusedRowHandle).RowState == DataRowState.Added)
            {
                gv_Equip.Columns["Equip_Code"].OptionsColumn.ReadOnly = false;
                gv_Equip.Columns["Equip_Code"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                gv_Equip.Columns["Equip_Code"].OptionsColumn.ReadOnly = true;
                gv_Equip.Columns["Equip_Code"].OptionsColumn.AllowEdit = false;
            }
        }
    }
}
