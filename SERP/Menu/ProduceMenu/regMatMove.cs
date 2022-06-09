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
    public partial class regMatMove : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        private string sDeptCode = "";

        public regMatMove()
        {
            InitializeComponent();
        }

        private void regMatMove_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            ForMat.sBasic_Set(this.Name, txt_MoveNo);

            dt_Move.Focus();

            txt_UserCode.Text = GlobalValue.sUserID;

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void Grid_Set()
        {
            //MotIn_M 그리드
            gc_MatMoveM.AddRowYN = true;

            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "In_No", "입고번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "In_Sort", "입고Sort", "", false, false, false, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Item_Name", "품목명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Q_Unit", "단위", "50", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Q_Unit_CD", "단위", "50", false, false, false, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "MatSer_No", "시리얼넘버", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Loc_Code", "출고 Location", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Loc_Code1", "입고 Location", "100", false, true, true, true);
            DbHelp.GridSet(gc_MatMoveM, gv_MatMoveM, "Loc_Code1NM", "Location", "120", false, false, false, true);

            DbHelp.GridColumn_Help(gv_MatMoveM, "In_No", "Y");
            RepositoryItemButtonEdit btn_Grid = (RepositoryItemButtonEdit)gv_MatMoveM.Columns["In_No"].ColumnEdit;
            btn_Grid.Buttons[0].Click += new EventHandler(grid_Help);
            gv_MatMoveM.Columns["In_No"].ColumnEdit = btn_Grid;

            DbHelp.GridColumn_Help(gv_MatMoveM, "Loc_Code1", "Y");
            RepositoryItemButtonEdit btn_Loc = (RepositoryItemButtonEdit)gv_MatMoveM.Columns["Loc_Code1"].ColumnEdit;
            btn_Loc.Buttons[0].Click += new EventHandler(Help_Loc);

            gv_MatMoveM.Columns["Loc_Code"].ColumnEdit = btn_Loc;

            DbHelp.GridColumn_NumSet(gv_MatMoveM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            gc_MatMoveM.DeleteRowEventHandler += new EventHandler(Delete_S);

            gv_MatMoveM.OptionsView.ShowAutoFilterRow = false;
        }

        #region 그리드 Help 함수

        private void grid_Help(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_MovePartNM.Text))
            {
                XtraMessageBox.Show("출고 창고를 먼저 입력해주세요");
                return;
            }

            int iRow = gv_MatMoveM.GetFocusedDataSourceRowIndex();

            if(gv_MatMoveM.FocusedColumn.FieldName == "In_No")
            {
                PopHelpForm help_Form = new PopHelpForm("MatStock", "sp_Help_MatStock", gv_MatMoveM.GetRowCellValue(iRow, "In_No").ToString(), "Y");
                help_Form.Set_Value(txt_Whouse.Text, "", "", "", "");
                help_Form.sNotReturn = "Y";
                help_Form.sLevelYN = "Y";
                if (help_Form.ShowDialog() == DialogResult.OK)
                {
                    foreach (DataRow row in help_Form.drReturn)
                    {
                        gv_MatMoveM.SetRowCellValue(iRow, "In_No", row["In_No"].ToString());
                        if (!string.IsNullOrWhiteSpace(row["In_No"].ToString()))
                        {
                            In_Search(row["In_No"].ToString(), row["Item_Code"].ToString(), row["Location"].ToString(), iRow);
                        }

                        if (iRow + 1 == gv_MatMoveM.RowCount)
                            gv_MatMoveM.AddNewRow();

                        iRow++;

                        gv_MatMoveM.UpdateCurrentRow();
                    }

                    gv_MatMoveM.DeleteRow(iRow);
                }
            }
        }

        private void Help_Loc(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Whouse1NM.Text))
            {
                XtraMessageBox.Show("입고창고를 먼저 입력하세요");
                return;
            }

            int iRow = gv_MatMoveM.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_MatMoveM.GetRowCellValue(iRow, "Loc_Code1NM").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("Loc", "sp_Help_Loc", gv_MatMoveM.GetRowCellValue(iRow, "Loc_Code1").ToString(), "N");
                HelpForm.Set_Value(txt_Whouse1.Text, "", "", "", "");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_MatMoveM.SetRowCellValue(iRow, "Loc_Code1", HelpForm.sRtCode);
                    gv_MatMoveM.SetRowCellValue(iRow, "Loc_Code1NM", HelpForm.sRtCodeNm);
                }
            }
        }

        private void gc_MatOutM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(gv_MatMoveM.FocusedColumn.FieldName == "In_No")
                {
                    grid_Help(null, null);
                }
                else if(gv_MatMoveM.FocusedColumn.FieldName == "Loc_Code1")
                {
                    Help_Loc(null, null);
                }
            }
        }

        private void gv_MatOutM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            if (e.Column.FieldName == "In_No")
            {
                In_Search(e.Value.ToString(), gv_MatMoveM.GetRowCellValue(e.RowHandle, "Item_Code").ToString(), gv_MatMoveM.GetRowCellValue(e.RowHandle, "Loc_Code").ToString(), e.RowHandle);
            }
            else if (e.Column.FieldName == "Loc_Code1")
            {
                string sLoc_CodeNM = PopHelpForm.Return_Help("sp_Help_Loc", e.Value.ToString(), "", txt_Whouse1.Text);
                gv_MatMoveM.SetRowCellValue(e.RowHandle, "Loc_Code1NM", sLoc_CodeNM);
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_MatMoveM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            if(gv_MatMoveM.GetDataRow(e.FocusedRowHandle).RowState == DataRowState.Added)
            {
                gv_MatMoveM.Columns["In_No"].OptionsColumn.AllowEdit = true;
                gv_MatMoveM.Columns["In_No"].OptionsColumn.ReadOnly = false;
            }
            else
            {
                gv_MatMoveM.Columns["In_No"].OptionsColumn.AllowEdit = false;
                gv_MatMoveM.Columns["In_No"].OptionsColumn.ReadOnly = true;
            }

        }

        #endregion

        #region 함수

        private void In_Search(string sInNo, string sItem_Code, string sLoc_Code, int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SN");
                sp.AddParam("In_No", sInNo);
                sp.AddParam("Item_Code", sItem_Code);
                sp.AddParam("Whouse_Code", txt_Whouse.Text);
                sp.AddParam("Loc_Code", sLoc_Code);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt_In = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                if (dt_In.Rows.Count > 0)
                {
                    DataRow dr_In = dt_In.Rows[0];

                    gv_MatMoveM.SetRowCellValue(iRow, "Item_Code", sItem_Code);
                    gv_MatMoveM.SetRowCellValue(iRow, "Item_Name", dr_In["Item_Name"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "Ssize", dr_In["Ssize"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "Q_Unit", dr_In["Q_Unit"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "Q_Unit_CD", dr_In["Q_Unit_CD"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "In_Sort", dr_In["In_Sort"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "Qty", dr_In["Qty"].NumString());
                    gv_MatMoveM.SetRowCellValue(iRow, "MatSer_No", dr_In["MatSer_No"].ToString());
                    gv_MatMoveM.SetRowCellValue(iRow, "Loc_Code", sLoc_Code);
                }
                else
                {
                    gv_MatMoveM.SetRowCellValue(iRow, "Item_Code", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Item_Name", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Ssize", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Q_Unit", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Q_Unit_CD", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "In_Sort", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Qty", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "MatSer_No", "");
                    gv_MatMoveM.SetRowCellValue(iRow, "Loc_Code", "");
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
            try
            {
                int iRow = gv_MatMoveM.GetFocusedDataSourceRowIndex();

                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DS");
                sp.AddParam("Move_No", txt_MoveNo.Text);
                sp.AddParam("In_No", gv_MatMoveM.GetRowCellValue(iRow, "In_No").ToString());
                sp.AddParam("In_Sort", gv_MatMoveM.GetRowCellValue(iRow, "In_Sort").ToString());
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

        #region Help 이벤트
        private void txt_UserCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserCodeNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_UserCode.Text);
            User_Search();
        }

        private void txt_UserCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_UserCode_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_UserCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_UserCodeNM.Text))
            {
                PopHelpForm HelpForm = new PopHelpForm("User", "sp_Help_User", txt_UserCode.Text, "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    txt_UserCode.Text = HelpForm.sRtCode;
                    txt_UserCodeNM.Text = HelpForm.sRtCodeNm;
                    User_Search();
                }
            }
        }

        //이동 사유
        private void txt_MovePart_EditValueChanged(object sender, EventArgs e)
        {
            txt_MovePartNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_MovePart.Text, "40050", "");
        }

        private void txt_MovePart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txt_MovePart_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_MovePart_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm help_form = new PopHelpForm("General", "sp_Help_General", "40050", txt_MovePart.Text, "N");
            if(help_form.ShowDialog() == DialogResult.OK)
            {
                txt_MovePart.Text = help_form.sRtCode;
                txt_MovePartNM.Text = help_form.sRtCodeNm;
            }
        }

        //창고
        private void txt_Whouse_EditValueChanged(object sender, EventArgs e)
        {
            txt_WhouseNM.Text = PopHelpForm.Return_Help("sp_Help_Whouse", txt_Whouse.Text);
        }

        private void txt_Whouse_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Whouse_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_Whouse_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm help_Form = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_Whouse.Text, "N");
            if (help_Form.ShowDialog() == DialogResult.OK)
            {
                txt_Whouse.Text = help_Form.sRtCode;
                txt_WhouseNM.Text = help_Form.sRtCodeNm;
            }
        }

        //입고창고
        private void txt_Whouse1_EditValueChanged(object sender, EventArgs e)
        {
            txt_Whouse1NM.Text = PopHelpForm.Return_Help("sp_Help_Whouse", txt_Whouse1.Text);
        }

        private void txt_Whouse1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txt_Whouse1_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_Whouse1_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm help_Form = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_Whouse1.Text, "N");
            if (help_Form.ShowDialog() == DialogResult.OK)
            {
                txt_Whouse1.Text = help_Form.sRtCode;
                txt_Whouse1NM.Text = help_Form.sRtCodeNm;
            }
        }

        #endregion

        #region 내부 함수

        private void User_Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SU");
                sp.AddParam("User_Code", txt_UserCode.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk == 0)
                {
                    DataTable dt = ret.ReturnDataSet.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        dt = DbHelp.Fill_Table(dt);
                        DataRow dr = dt.Rows[0];

                        txt_DeptNM.Text = dr["Dept_Name"].ToString();
                        sDeptCode = dr["Dept_Code"].ToString();
                    }
                    else
                    {
                        txt_DeptNM.Text = "";
                        sDeptCode = "";
                    }
                }
                else
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SH");
                sp.AddParam("Move_No", txt_MoveNo.Text);

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
                    txt_MovePart.Text = dr_H["Move_Part"].ToString();
                    dt_Move.Text = dr_H["Move_Date"].ToString();
                    txt_UserCode.Text = dr_H["User_Code"].ToString();
                    txt_Whouse.Text = dr_H["Whouse_Code"].ToString();
                    txt_Whouse1.Text = dr_H["Whouse_Code1"].ToString();
                    memo_BIgo.Text = dr_H["Move_Bigo"].ToString();

                    txt_RegUser.Text = dr_H["Reg_User"].ToString();
                    txt_RegDate.Text = dr_H["Reg_Date"].ToString();
                    txt_UpUser.Text = dr_H["Up_User"].ToString();
                    txt_UpDate.Text = dr_H["Up_Date"].ToString();
                }
                else
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panel_M);
                }

                gc_MatMoveM.DataSource = dt_S;

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }
        #endregion

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm help_Form = new PopHelpForm("MatoutMove", "sp_Help_MatMove", "", "N");
            help_Form.sNotReturn = "Y";
            help_Form.sLevelYN = "Y";
            btn_Select.clsWait.CloseWait();
            if (help_Form.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                txt_MoveNo.Text = help_Form.sRtCode;
                Search();

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (btn_Insert.Result_Update == DialogResult.Yes)
            {
                btn_Save_Click(null, null);
            }

            txt_MoveNo.Text = "";
            
            Search();

            ForMat.sBasic_Set(this.Name, txt_MoveNo);

            dt_Move.Select();

            txt_UserCode.Text = GlobalValue.sUserID;

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sInNo = "", sInSort = "", sLoc_Code = "", sQty = "", sQ_Unit = "", sLoc_Code1 = "";

                for(int i = 0; i < gv_MatMoveM.RowCount; i++)
                {
                    if (gv_MatMoveM.GetRowCellValue(i, "In_Sort").ToString() != "")
                    {
                        sInNo += gv_MatMoveM.GetRowCellValue(i, "In_No").ToString() + "_/";
                        sInSort += gv_MatMoveM.GetRowCellValue(i, "In_Sort").ToString() + "_/";
                        sQty += gv_MatMoveM.GetRowCellValue(i, "Qty").ToString() + "_/";
                        sQ_Unit += gv_MatMoveM.GetRowCellValue(i, "Q_Unit_CD").ToString() + "_/";
                        sLoc_Code += gv_MatMoveM.GetRowCellValue(i, "Loc_Code").ToString() + "_/";
                        sLoc_Code1 += gv_MatMoveM.GetRowCellValue(i, "Loc_Code1").ToString() + "_/";
                    }
                }

                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "I");
                sp.AddParam("Move_No", txt_MoveNo.Text);
                sp.AddParam("Move_Date", dt_Move.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("User_Code", txt_UserCode.Text);
                sp.AddParam("Whouse_Code", txt_Whouse.Text);
                sp.AddParam("Whouse_Code1", txt_Whouse1.Text);
                sp.AddParam("Move_Part", txt_MovePart.Text);
                sp.AddParam("Move_Bigo", memo_BIgo.Text);
                sp.AddParam("Dept_Code", sDeptCode);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                sp.AddParam("InNo", sInNo);
                sp.AddParam("InSort", sInSort);
                sp.AddParam("Qty", sQty);
                sp.AddParam("Q_Unit", sQ_Unit);
                sp.AddParam("LocCode", sLoc_Code);
                sp.AddParam("LocCode1", sLoc_Code1);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_MoveNo.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Move_No"].ToString();
                btn_Save.sCHK = "Y";
                Search();

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
                SqlParam sp = new SqlParam("sp_regMatMove");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DH");
                sp.AddParam("Move_No", txt_MoveNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                btn_Delete.sCHK = "Y";
                txt_MoveNo.Text = "";
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
            FileIF.Excel_Down(gc_MatMoveM, "자재출고");
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

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }


        #endregion
    }
}
