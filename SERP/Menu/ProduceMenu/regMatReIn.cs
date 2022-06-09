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
    public partial class regMatReIn : BaseReg
    {
        private string sMatIn_No = "";
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        private string sDept_Code = "";
        private string sP_Unit = "";

        public regMatReIn()
        {
            InitializeComponent();
        }

        private void regMatReIn_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            txt_User.Text = GlobalValue.sUserID;
            ForMat.sBasic_Set(this.Name, txt_ReInNo);

            dt_In.Focus();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void Grid_Set()
        {
            //MotIn_M 그리드
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Sort_No", "정렬 순번", "", false, false, false, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Out_No", "출고 번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "In_No", "입고 번호", "130", false, false, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "In_Sort", "", "", false, false, false, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Q_Unit", "단위코드", "50", false, false, false, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Q_Unit_Name", "단위", "50", false, false, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "MatSer_No", "시리얼넘버", "120", true, true, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Loc_Code", "Location코드", "100", false, true, true, true);
            DbHelp.GridSet(gc_MatReInM, gv_MatReInM, "Loc_CodeNM", "Location", "120", false, false, false, true);

            DbHelp.GridColumn_Help(gv_MatReInM, "Out_No", "Y");
            RepositoryItemButtonEdit btn_edit = (RepositoryItemButtonEdit)gv_MatReInM.Columns["Out_No"].ColumnEdit;
            btn_edit.Buttons[0].Click += new EventHandler(Help_OutNo);
            gv_MatReInM.Columns["Out_No"].ColumnEdit = btn_edit;

            gc_MatReInM.DeleteRowEventHandler += new EventHandler(Delete_S); // 그리드 우클릭 삭제

            DbHelp.GridColumn_Help(gv_MatReInM, "Loc_Code", "Y");
            RepositoryItemButtonEdit btn_Loc = (RepositoryItemButtonEdit)gv_MatReInM.Columns["Loc_Code"].ColumnEdit;
            btn_Loc.Buttons[0].Click += new EventHandler(Help_Loc);

            gv_MatReInM.Columns["Loc_Code"].ColumnEdit = btn_Loc;

            DbHelp.GridColumn_NumSet(gv_MatReInM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            gc_MatReInM.AddRowYN = true;
            gv_MatReInM.OptionsView.ShowAutoFilterRow = false;
        }

        #region 헬프

        //거래처
        private void txt_CustomCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_CustomCodeNM.Text = PopHelpForm.Return_Help("sp_Help_Custom_IN", txt_CustomCode.Text);
            Search_Custom();
        }

        private void txt_CustomCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_CustomCode_Properties_ButtonClick(sender, null);
        }

        private void txt_CustomCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                PopHelpForm Helpform = new PopHelpForm("Custom", "sp_Help_Custom_IN", txt_CustomCode.Text, "N");
                if (Helpform.ShowDialog() == DialogResult.OK)
                {
                    txt_CustomCode.Text = Helpform.sRtCode;
                    txt_CustomCodeNM.Text = Helpform.sRtCodeNm;
                    Search_Custom();
                }
            }
        }

        // 창고

        private void txt_WHouse_EditValueChanged(object sender, EventArgs e)
        {
            txt_WHouseNM.Text = PopHelpForm.Return_Help("sp_Help_Whouse", txt_WHouse.Text);
        }

        private void txt_WHouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_WHouse_Properties_ButtonClick(sender, null);
        }

        private void txt_WHouse_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_WHouseNM.Text))
            {
                PopHelpForm HelpForm = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_WHouse.Text, "N");

                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    txt_WHouse.Text = HelpForm.sRtCode;
                    txt_WHouseNM.Text = HelpForm.sRtCodeNm;
                }
            }
        }

        // 담당자
        private void txt_User_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserCodeNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_User.Text);
            Search_User();
        }

        private void txt_User_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_User_Properties_ButtonClick(sender, null);
        }

        private void txt_User_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_UserCodeNM.Text))
            {
                PopHelpForm help_form = new PopHelpForm("User", "sp_Help_User", txt_User.Text, "N");
                if(help_form.ShowDialog() == DialogResult.OK)
                {
                    txt_User.Text = help_form.sRtCode;
                    txt_UserCodeNM.Text = help_form.sRtCodeNm;
                    Search_User();
                }
            }
        }

        private void txt_Pay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Pay_Properties_ButtonClick(sender, null);
        }

        private void txt_Pay_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm Helpform = new PopHelpForm("General", "sp_Help_General", "10050", txt_Pay.Text, "N");
            if (Helpform.ShowDialog() == DialogResult.OK)
            {
                txt_Pay.Text = Helpform.sRtCode;
                txt_PayNM.Text = Helpform.sRtCodeNm;
            }
        }
        private void txt_Pay_EditValueChanged(object sender, EventArgs e)
        {
            txt_PayNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_Pay.Text, "10050", "");
        }

        #endregion

        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm Help_Form = new PopHelpForm("MatReIn", "sp_Help_MatReIn", "", "N");
            Help_Form.sNotReturn = "Y";
            Help_Form.sLevelYN = "Y";
            btn_Select.clsWait.CloseWait();
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                txt_ReInNo.Text = Help_Form.sRtCode;
                btn_Select.clsWait.ShowWait(this.FindForm());
                Search();

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            txt_ReInNo.Text = "";
            Search();

            txt_User.Text = GlobalValue.sUserID;

            dt_In.Select();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DH");
                sp.AddParam("ReIn_No", txt_ReInNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_ReInNo.Text = "";
                Search();

                btn_Delete.sCHK = "Y";
            }   
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sOutNo = "", sInNo = "", sInSort = "", sQty = "", sQUnit = "", sLoc_Code = "";

                for(int i = 0; i < gv_MatReInM.RowCount; i++)
                {
                    if(!string.IsNullOrWhiteSpace(gv_MatReInM.GetRowCellValue(i, "In_No").ToString()))
                    {
                        sOutNo += gv_MatReInM.GetRowCellValue(i, "Out_No").ToString() + "_/";
                        sInNo += gv_MatReInM.GetRowCellValue(i, "In_No").ToString() + "_/";
                        sInSort += gv_MatReInM.GetRowCellValue(i, "In_Sort").ToString() + "_/";
                        sQty += gv_MatReInM.GetRowCellValue(i, "Qty").ToString() + "_/";
                        sQUnit += gv_MatReInM.GetRowCellValue(i, "Q_Unit").ToString() + "_/";
                        sLoc_Code += gv_MatReInM.GetRowCellValue(i, "Loc_Code").ToString() + "_/";
                    }
                }

                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "I");
                sp.AddParam("ReIn_No", txt_ReInNo.Text);
                sp.AddParam("In_Date", dt_In.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Custom_Code", txt_CustomCode.Text);
                sp.AddParam("Whouse_Code", txt_WHouse.Text);
                sp.AddParam("Pay_Code", txt_Pay.Text);
                sp.AddParam("Pay_Date", dt_Pay.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("User_Code", txt_User.Text);
                sp.AddParam("Dept_Code", sDept_Code);
                sp.AddParam("Vat_Ck", check_Vat.EditValue.ToString());
                sp.AddParam("P_Unit", sP_Unit);
                sp.AddParam("In_Memo", txt_MatInMemo.Text);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                sp.AddParam("OutNo", sOutNo);
                sp.AddParam("InNo", sInNo);
                sp.AddParam("InSort", sInSort);
                sp.AddParam("Qty", sQty);
                sp.AddParam("Qunit", sQUnit);
                sp.AddParam("LocCode", sLoc_Code);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_ReInNo.Text = ret.ReturnDataSet.Tables[0].Rows[0]["ReIn_No"].ToString();

                Search();

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
            FileIF.Excel_Down(gc_MatReInM, "재입고등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion

        #region 버튼 상속
        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
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

        #region 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SH");
                sp.AddParam("ReIn_No", txt_ReInNo.Text);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt_H = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                DataTable dt_S = ret.ReturnDataSet.Tables[1];

                if(dt_H.Rows.Count > 0)
                {
                    DataRow dr_H = dt_H.Rows[0];

                    txt_CustomCode.Text = dr_H["Custom_Code"].ToString();
                    txt_Pay.Text = dr_H["Pay_Code"].ToString();
                    dt_In.Text = dr_H["In_Date"].ToString();
                    dt_Pay.Text = dr_H["Pay_Date"].ToString();
                    txt_User.Text = dr_H["User_Code"].ToString();
                    txt_WHouse.Text = dr_H["Whouse_Code"].ToString();

                    txt_RegDate.Text = dr_H["Reg_Date"].ToString();
                    txt_RegUser.Text = dr_H["Reg_User"].ToString();
                    txt_UpDate.Text = dr_H["Up_Date"].ToString();
                    txt_UpUser.Text = dr_H["Up_User"].ToString();
                }
                else
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panel_M);
                }

                gc_MatReInM.DataSource = dt_S;
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search_User()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SU");
                sp.AddParam("User_Code", txt_User.Text);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                if(ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    txt_Dept.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Name"].ToString();
                    sDept_Code = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Code"].ToString();
                }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Delete_S(object sender, EventArgs e)
        {
            int iRow = gv_MatReInM.GetFocusedDataSourceRowIndex();

            try
            {
                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DS");
                sp.AddParam("ReIn_No", txt_ReInNo.Text);
                sp.AddParam("Out_No", gv_MatReInM.GetRowCellValue(iRow, "Out_No").ToString());
                sp.AddParam("In_No", gv_MatReInM.GetRowCellValue(iRow, "In_No").ToString());
                sp.AddParam("In_Sort", gv_MatReInM.GetRowCellValue(iRow, "In_Sort").ToString());
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                Search();

                btn_Delete.sCHK = "Y";
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search_Custom()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatReIn");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SC");
                sp.AddParam("Custom_Code", txt_CustomCode.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    txt_PUnit.Text = ret.ReturnDataSet.Tables[0].Rows[0]["P_UnitNM"].ToString();
                    check_Vat.EditValue = ret.ReturnDataSet.Tables[0].Rows[0]["Vat_Ck"].ToString();
                    sP_Unit = ret.ReturnDataSet.Tables[0].Rows[0]["P_Unit"].ToString();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Help_OutNo(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            //{
            //    XtraMessageBox.Show("거래처를 먼저 입력해주세요");
            //    return;
            //}

            int iRow = gv_MatReInM.GetFocusedDataSourceRowIndex();

            if(string.IsNullOrWhiteSpace(gv_MatReInM.GetRowCellValue(iRow, "In_No").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("MatOutRe", "sp_Help_MatOutRe", gv_MatReInM.GetRowCellValue(iRow, "Out_No").ToString(), "Y");
                HelpForm.Set_Value(txt_CustomCode.Text, "", "", "", "");
                HelpForm.sNotReturn = "Y";
                HelpForm.sLevelYN = "Y";
                if(HelpForm.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataRow row in HelpForm.drReturn)
                    {
                        gv_MatReInM.SetRowCellValue(iRow, "Out_No", row["Out_No"].ToString());
                        if (!string.IsNullOrWhiteSpace(row["Out_No"].ToString()))
                        {
                            Search_Item(row["Out_No"].ToString(), row["In_No"].ToString(), row["Item_Code"].ToString(), iRow);
                        }


                        if (iRow + 1 == gv_MatReInM.RowCount)
                            gv_MatReInM.AddNewRow();

                        iRow++;

                        gv_MatReInM.UpdateCurrentRow();
                    }

                    gv_MatReInM.DeleteRow(iRow);
                }
            }
        }

        private void Search_Item(string sOut_No, string sIn_No, string sItem_Code, int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regmatReIn");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SO");
                sp.AddParam("Out_No", sOut_No);
                sp.AddParam("In_No", sIn_No);
                sp.AddParam("Item_Code", sItem_Code);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt_O = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                
                if(dt_O.Rows.Count > 0)
                {
                    DataRow dr_O = dt_O.Rows[0];

                    gv_MatReInM.SetRowCellValue(iRow, "In_No", sIn_No);
                    gv_MatReInM.SetRowCellValue(iRow, "In_Sort", dr_O["In_Sort"].ToString());
                    gv_MatReInM.SetRowCellValue(iRow, "Item_Code", sItem_Code);
                    gv_MatReInM.SetRowCellValue(iRow, "Item_Name", dr_O["Item_Name"].ToString());
                    gv_MatReInM.SetRowCellValue(iRow, "Ssize", dr_O["Ssize"].ToString());
                    gv_MatReInM.SetRowCellValue(iRow, "Q_Unit", dr_O["Q_Unit"].ToString());
                    gv_MatReInM.SetRowCellValue(iRow, "Q_Unit_Name", dr_O["Q_Unit_Name"].ToString());
                    gv_MatReInM.SetRowCellValue(iRow, "Qty", dr_O["Qty"].NumString());
                    gv_MatReInM.SetRowCellValue(iRow, "MatSer_No", dr_O["MatSer_No"].ToString());
                }
                else
                {
                    gv_MatReInM.SetRowCellValue(iRow, "In_No", "");
                    gv_MatReInM.SetRowCellValue(iRow, "In_Sort", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Item_Code", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Item_Name", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Ssize", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Q_Unit", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Q_Unit_CD", "");
                    gv_MatReInM.SetRowCellValue(iRow, "Qty", "");
                    gv_MatReInM.SetRowCellValue(iRow, "MatSer_No", "");
                }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        //로케이션
        private void Help_Loc(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_WHouseNM.Text))
            {
                XtraMessageBox.Show("입고창고를 먼저 입력하세요");
                return;
            }

            int iRow = gv_MatReInM.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_MatReInM.GetRowCellValue(iRow, "Loc_CodeNM").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("Loc", "sp_Help_Loc", gv_MatReInM.GetRowCellValue(iRow, "Loc_Code").ToString(), "N");
                HelpForm.Set_Value(txt_WHouse.Text, "", "", "", "");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_MatReInM.SetRowCellValue(iRow, "Loc_Code", HelpForm.sRtCode);
                    gv_MatReInM.SetRowCellValue(iRow, "Loc_CodeNM", HelpForm.sRtCodeNm);
                }
            }
        }

        #endregion

        #region 그리드 이벤트

        private void gv_MatInM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            if(e.Column.FieldName == "Out_No")
            {
                Search_Item(e.Value.ToString(), gv_MatReInM.GetRowCellValue(e.RowHandle, "In_No").ToString(), gv_MatReInM.GetRowCellValue(e.RowHandle, "Item_Code").ToString(), e.RowHandle);
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_MatReInM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            if(gv_MatReInM.GetDataRow(e.FocusedRowHandle).RowState == DataRowState.Added)
            {
                gv_MatReInM.Columns["Out_No"].OptionsColumn.AllowEdit = true;
                gv_MatReInM.Columns["Out_No"].OptionsColumn.ReadOnly = false;
            }
            else
            {
                gv_MatReInM.Columns["Out_No"].OptionsColumn.AllowEdit = false;
                gv_MatReInM.Columns["Out_No"].OptionsColumn.ReadOnly = true;
            }
        }

        private void gc_MatInM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
           if(e.KeyChar == 13)
            {
                if(gv_MatReInM.FocusedColumn.FieldName == "Out_No")
                {
                    Help_OutNo(null, null);
                }
                else if (gv_MatReInM.FocusedColumn.FieldName == "Loc_Code")
                {
                    Help_Loc(null, null);
                }
            }
        }

        #endregion


    }
}
