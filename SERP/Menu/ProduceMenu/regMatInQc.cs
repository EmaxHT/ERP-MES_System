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
using DevExpress.XtraGrid.Columns;

namespace SERP
{
    public partial class regMatInQc : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public regMatInQc()
        {
            InitializeComponent();
        }

        private void regMatinQc_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Initialze_Data();

            dt_QcDate.Focus();

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_Date", "입고일자", "100", false, false, true); 
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qc_No", "검사번호", "100", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_No", "입고번호", "100", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_Sort", "입고순번", "80", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Code", "품목코드", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Name", "품목명", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qty", "입고수량", "80", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Qty", "불량수량", "80", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Name", "불량유형", "100", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Code", "불량유형 코드", "80", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qc_Bigo", "비고", "150", false, true, true);

            DbHelp.GridColumn_NumSet(gv_QcS, "Qty", ForMat.SetDecimal("regMatInQc", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_QcS, "Fail_Qty", ForMat.SetDecimal("regMatInQc", "Qty1"));

            // 자재 입고 품목 조회
            DbHelp.GridColumn_Help(gv_QcS, "In_No", "N");
            RepositoryItemButtonEdit button_Help = (RepositoryItemButtonEdit)gv_QcS.Columns["In_No"].ColumnEdit;
            button_Help.Buttons[0].Click += new EventHandler(In_No_Search);
            gv_QcS.Columns["In_No"].ColumnEdit = button_Help;

            // 불량 유형 조회
            DbHelp.GridColumn_Help(gv_QcS, "Fail_Name", "N");
            RepositoryItemButtonEdit button_Help_S = (RepositoryItemButtonEdit)gv_QcS.Columns["Fail_Name"].ColumnEdit;
            button_Help_S.Buttons[0].Click += new EventHandler(Fail_Code_Choice);
            gv_QcS.Columns["Fail_Name"].ColumnEdit = button_Help_S;

            // 로우 삭제
            gc_QcS.DeleteRowEventHandler += new EventHandler(Row_Delete);
            gc_QcS.AddRowYN = true;
            gc_QcS.PopMenuChk = true;
            gc_QcS.MouseWheelChk = true;
        }

        

        #region 담당자
        private void txt_UserCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserCodeNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_UserCode.Text, "", "");
            Search_Dept(txt_UserCode.Text);
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
                    Search_Dept(txt_UserCode.Text);
                }
            }
        }

        private void Search_Dept(string sUserCode)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SI");
                sp.AddParam("User_Code", sUserCode);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
                if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    txt_DeptCode.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Name"].ToString();
                    txt_DeptCode.ToolTip = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Code"].ToString();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
        #endregion

        

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm Help_Form = new PopHelpForm("MatInQc", "sp_Help_MatInQc", "N");
            Help_Form.sLevelYN = "Y";
            Help_Form.sNotReturn = "Y";
            btn_Select.clsWait.CloseWait();
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                Search_Data(Help_Form.sRtCode);
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (btn_Insert.Result_Update == DialogResult.Yes)
            {
                Save_Data();
            }

            Initialze_Data();

            btn_Insert.sUpdate = "N";
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_QcNo.Text))
            {
                SqlParam sp = new SqlParam("sp_regMatInQc");
                sp.AddParam("Kind", "D");
                sp.AddParam("Detail", "M");
                sp.AddParam("Qc_No", txt_QcNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }
            else
            {
                Initialze_Data();
            }
            btn_Delete.sCHK = "Y";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Save_Data();
            string qc_no = ret.ReturnDataSet.Tables[0].Rows[0]["Qc_No"].NullString();
            Search_Data(qc_no);
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_QcS, "수입검사");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                Save_Data();
            }

            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        #region 메소드
        private void Initialze_Data()
        {
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panel_M);
            txt_DeptCode.ToolTip = "";

            Search_Data();

            txt_UserCode.Text = GlobalValue.sUserID;
        }

        private void Search_Data(string qc_no = "")
        {
            SqlParam sp = new SqlParam("sp_regMatInQc");
            sp.AddParam("Kind", "S");
            sp.AddParam("Qc_No", qc_no);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            DataTable Info = ret.ReturnDataSet.Tables[0].Clone();
            DataRow Info_row = null;

            if (!string.IsNullOrWhiteSpace(qc_no))
            {
                Info = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                Info_row = Info.Rows[0];

                txt_QcNo.Text = Info_row["Qc_No"].NullString();
                dt_QcDate.Text = Info_row["Qc_Date"].NullString();
                txt_UserCode.Text = Info_row["User_Code"].NullString();
                txt_UserCodeNM.Text = Info_row["User_Name"].NullString();
                txt_DeptCode.Text = Info_row["Dept_Name"].NullString();
                txt_DeptCode.ToolTip = Info_row["Dept_Code"].NullString();
                txt_RegDate.Text = Info_row["Reg_Date"].NullString();
                txt_RegUser.Text = Info_row["Reg_User"].NullString();
                txt_UpDate.Text = Info_row["Up_Date"].NullString();
                txt_UpUser.Text = Info_row["Up_User"].NullString();
                txt_QcMemo.Text = Info_row["Qc_Memo"].NullString();
            }
            // Info 바인딩
            
            DataTable Items = ret.ReturnDataSet.Tables[1].Clone();

            if (!string.IsNullOrWhiteSpace(qc_no))
                Items = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[1]);

            gc_QcS.DataSource = Items;
            gc_QcS.RefreshDataSource();
        }



        private void Save_Data()
        {
            DataRow row = DbHelp.Summary_Data(gv_QcS, "In_No", new string[] { "In_No", "In_Sort", "Fail_Qty", "Fail_Code", "Qc_Bigo" });

            SqlParam sp = new SqlParam("sp_regMatInQc");
            sp.AddParam("Kind", "I");
            sp.AddParam("Qc_No", txt_QcNo.Text);
            sp.AddParam("Qc_Date", dt_QcDate.Text.Replace("-", ""));
            sp.AddParam("User_Code", txt_UserCode.Text);
            sp.AddParam("Dept_Code", txt_DeptCode.ToolTip);
            sp.AddParam("Qc_Memo", txt_QcMemo.Text);
            sp.AddParam("In_No", row["In_No"].NullString());
            sp.AddParam("In_Sort", row["In_Sort"].NullString());
            sp.AddParam("Qty", row["Fail_Qty"].NullString());
            sp.AddParam("Fail_Code", row["Fail_Code"].NullString());
            sp.AddParam("Qc_Bigo", row["Qc_Bigo"].NullString());
            sp.AddParam("Reg_User", GlobalValue.sUserID);
            sp.AddParam("Up_User", GlobalValue.sUserID);

            ret = DbHelp.Proc_Save(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            btn_Save.sCHK = "Y";
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }
        #endregion

        #region 그리드 이벤트

        private void gv_QcS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            int old_len = (gv_QcS.ActiveEditor == null) ? 0 : gv_QcS.ActiveEditor.OldEditValue.NullString().Length;
            int now_len = (gv_QcS.ActiveEditor == null) ? 0 : gv_QcS.ActiveEditor.EditValue.NullString().Length;

            if(e.Column.FieldName == "In_No")
            {
                if (old_len > now_len)
                {
                    gv_QcS.CellValueChanged -= gv_QcS_CellValueChanged;
                    foreach (GridColumn col in gv_QcS.Columns)
                    {
                        gv_QcS.SetFocusedRowCellValue(col, "");
                    }
                    gv_QcS.CellValueChanged += gv_QcS_CellValueChanged;
                }
            }
            else if (e.Column.FieldName == "Fail_Name")
            {
                string search = (gv_QcS.ActiveEditor == null) ? "" : gv_QcS.ActiveEditor.EditValue.NullString();
                string result_code = "";
                string result_name = "";
                SqlParam sp = new SqlParam("sp_regMatInQc");
                sp.AddParam("Kind", "G");
                sp.AddParam("General", search);

                ReturnStruct temp_ret = DbHelp.Proc_Search(sp);
                if (ret.ReturnChk == 0 && temp_ret.ReturnDataSet != null && temp_ret.ReturnDataSet.Tables[0].Rows.Count == 1)
                {
                    result_code = DbHelp.Fill_Table(temp_ret.ReturnDataSet.Tables[0]).Rows[0]["GS_Code"].NullString();
                    result_name = DbHelp.Fill_Table(temp_ret.ReturnDataSet.Tables[0]).Rows[0]["GS_Name"].NullString();
                }
                gv_QcS.CellValueChanged -= gv_QcS_CellValueChanged;
                gv_QcS.SetFocusedRowCellValue("Fail_Code", result_code);
                gv_QcS.SetFocusedRowCellValue("Fail_Name", result_name);
                gv_QcS.CellValueChanged += gv_QcS_CellValueChanged;
            }
        }
        private void gv_QcS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (gv_QcS.FocusedColumn.FieldName == "In_No")
                    In_No_Search(sender, null);

                if (gv_QcS.FocusedColumn.FieldName == "Fail_Name")
                    Fail_Code_Choice(sender, null);
            }
        }

        // 입고번호 클릭
        private void In_No_Search(object sender, EventArgs e)
        {
            PopHelpForm help_Form = new PopHelpForm("MatIn_Items", "sp_Help_MatIn_Items", gv_QcS.GetFocusedRowCellValue("In_No").NullString(), "Y");
            help_Form.sNotReturn = "Y";
            help_Form.sLevelYN = "Y";
            if (help_Form.ShowDialog() == DialogResult.OK)
            {
                if (help_Form.drReturn == null && help_Form.drReturn.Count() == 0)
                {
                    SqlParam sp = new SqlParam("sp_regMatInQc");
                    sp.AddParam("Kind", "H");
                    sp.AddParam("In_No", help_Form.sRtCode);
                    sp.AddParam("In_Sort", help_Form.sRtCodeNm);

                    ReturnStruct temp = DbHelp.Proc_Search(sp);
                    
                    if (temp.ReturnChk != 0)
                    {
                        XtraMessageBox.Show(temp.ReturnMessage);
                        return;
                    }
                    DataRow row = DbHelp.Fill_Table(temp.ReturnDataSet.Tables[0]).Rows[0];

                    gv_QcS.SetFocusedRowCellValue("In_Date", row["In_Date"].NullString());
                    gv_QcS.SetFocusedRowCellValue("In_No", row["In_No"].NullString());
                    gv_QcS.SetFocusedRowCellValue("In_Sort", row["In_Sort"].NullString());
                    gv_QcS.SetFocusedRowCellValue("Item_Code", row["Item_Code"].NullString());
                    gv_QcS.SetFocusedRowCellValue("Item_Name", row["Item_Name"].NullString());                   
                    gv_QcS.SetFocusedRowCellValue("Ssize", row["Ssize"].NullString());
                    gv_QcS.SetFocusedRowCellValue("Qty", row["Qty"].NumString());
                }
                else
                {
                    int n = gv_QcS.FocusedRowHandle;
                    foreach (DataRow row in help_Form.drReturn)
                    {
                        gv_QcS.SetRowCellValue(n, "In_Date", row["In_Date"].NullString());
                        gv_QcS.SetRowCellValue(n, "In_No", row["In_No"].NullString());
                        gv_QcS.SetRowCellValue(n, "In_Sort", row["In_Sort"].NullString());
                        gv_QcS.SetRowCellValue(n, "Item_Code", row["Item_Code"].NullString());
                        gv_QcS.SetRowCellValue(n, "Item_Name", row["Item_Name"].NullString());
                        gv_QcS.SetRowCellValue(n, "Ssize", row["Ssize"].NullString());
                        gv_QcS.SetRowCellValue(n, "Qty", row["Qty"].NumString());

                        gv_QcS.AddNewRow();
                        gv_QcS.UpdateCurrentRow();
                        n++;
                    }
                }
            }
        }

        private void Fail_Code_Choice(object sender, EventArgs e)
        {
            //string fail_code = gv_QcS.GetFocusedRowCellValue("Fail_Name").NullString();
            string word = (gv_QcS.ActiveEditor == null) ? "" : gv_QcS.ActiveEditor.EditValue.NullString();
            PopHelpForm Help_Form = new PopHelpForm("General", "sp_Help_General", "40060", word, "N");
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                gv_QcS.CellValueChanged -= gv_QcS_CellValueChanged;
                gv_QcS.SetFocusedRowCellValue("Fail_Code", Help_Form.sRtCode);
                gv_QcS.SetFocusedRowCellValue("Fail_Name", Help_Form.sRtCodeNm);
                gv_QcS.CellValueChanged += gv_QcS_CellValueChanged;
            }
        }

        private void Row_Delete(object sender, EventArgs e)
        {
            string qc_no = gv_QcS.GetFocusedRowCellValue("Qc_No").NullString();

            if (!string.IsNullOrWhiteSpace(qc_no))
            {
                string in_no = gv_QcS.GetFocusedRowCellValue("In_No").NullString();
                string in_sort = gv_QcS.GetFocusedRowCellValue("In_Sort").NullString();

                SqlParam sp = new SqlParam("sp_regMatInQc");
                sp.AddParam("Kind", "D");
                sp.AddParam("Detail", "S");
                sp.AddParam("Qc_No", qc_no);
                sp.AddParam("In_No", in_no);
                sp.AddParam("In_Sort", in_sort);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }

            gv_QcS.DeleteRow(gv_QcS.FocusedRowHandle);
            XtraMessageBox.Show("삭제되었습니다.");
        }
        #endregion

        #region 버튼 상속

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
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


    }
}

