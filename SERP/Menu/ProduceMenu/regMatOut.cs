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
    public partial class regMatOut : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        private string sDeptCode = "";

        public regMatOut()
        {
            InitializeComponent();
        }

        private void regMatOut_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            ForMat.sBasic_Set(this.Name, txt_OutNo);

            txt_UserCode.Text = GlobalValue.sUserID;

            dt_OutDate.Focus();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void Grid_Set()
        {
            //MotIn_M 그리드
            gc_MatOutM.AddRowYN = true;

            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Sort_No", "순번", "80", false, false, false, false);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "In_No", "입고번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "In_Sort", "입고Sort", "", false, false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Code", "자재코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Item_Name", "자재명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Ssize", "자재 규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Q_Unit", "단위", "50", false, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Q_Unit_CD", "단위", "50", false, false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "P_Price", "단가", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Amt", "출고가액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Out_Bigo", "비고", "150", false, true, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "MatSer_No", "시리얼넘버", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Loc_Code", "Location", "120", false, false, false, true);
            DbHelp.GridSet(gc_MatOutM, gv_MatOutM, "Loc_Name", "Location", "120", false, false, true, true);

            DbHelp.GridColumn_Help(gv_MatOutM, "In_No", "Y");
            RepositoryItemButtonEdit btn_Grid = (RepositoryItemButtonEdit)gv_MatOutM.Columns["In_No"].ColumnEdit;
            btn_Grid.Buttons[0].Click += new EventHandler(grid_Help);

            gv_MatOutM.Columns["In_No"].ColumnEdit = btn_Grid;

            DbHelp.GridColumn_NumSet(gv_MatOutM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "P_Price", ForMat.SetDecimal(this.Name, "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatOutM, "Amt", ForMat.SetDecimal(this.Name, "Price1"));

            txt_TotalAMT.Properties.Mask.EditMask = "n" + ForMat.SetDecimal("regMatOut", "Price1");

            gc_MatOutM.DeleteRowEventHandler += new EventHandler(Delete_S);

            gv_MatOutM.OptionsView.ShowAutoFilterRow = false;
        }

        #region 그리드 Help 함수

        private void grid_Help(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_OutPartNM.Text))
            {
                XtraMessageBox.Show("출고 구분을 먼저 입력해주세요");
                return;
            }

            int iRow = gv_MatOutM.GetFocusedDataSourceRowIndex();

            PopHelpForm help_Form = new PopHelpForm("MatOutIn", "sp_Help_MatOut_In", gv_MatOutM.GetRowCellValue(iRow, "In_No").ToString(), "Y");
            help_Form.Set_Value(txt_Whouse.Text, "", "", "", "");
            help_Form.sNotReturn = "Y";

            if(help_Form.ShowDialog() == DialogResult.OK)
            {
                foreach(DataRow row in help_Form.drReturn)
                {
                    Set_Data(row, iRow);

                    if (iRow + 1 == gv_MatOutM.RowCount)
                        gv_MatOutM.AddNewRow();

                    iRow++;

                    gv_MatOutM.UpdateCurrentRow();
                }

                gv_MatOutM.DeleteRow(iRow);
            }


            Sum_Amt();
        }

        private void gc_MatOutM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(gv_MatOutM.FocusedColumn.FieldName == "In_No")
                {
                    grid_Help(null, null);
                }
            }
        }

        private void gv_MatOutM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            if(e.Column.FieldName == "In_No")
            {
                In_Search(e.Value.ToString(), gv_MatOutM.GetRowCellValue(e.RowHandle, "Item_Code").ToString(), gv_MatOutM.GetRowCellValue(e.RowHandle, "In_Sort").ToString(), gv_MatOutM.GetRowCellValue(e.RowHandle, "Loc_Code").ToString(), e.RowHandle);
            }
            else if(e.Column.FieldName == "Qty")
            {
                decimal dQty = decimal.Parse(e.Value.NumString());
                decimal dP_Price = decimal.Parse(gv_MatOutM.GetRowCellValue(e.RowHandle, "P_Price").NumString());

                gv_MatOutM.SetRowCellValue(e.RowHandle, "Amt", (dQty * dP_Price).ToString());

                Sum_Amt();
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_MatOutM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;
        }

        #endregion

        #region 함수

        private void Set_Data(DataRow row, int row_idx)
        {
            gv_MatOutM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_MatOutM_CellValueChanged);
            gv_MatOutM.SetRowCellValue(row_idx, "Item_Code", row["Item_Code"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "Item_Name", row["Item_Name"].NullString());

            gv_MatOutM.SetRowCellValue(row_idx, "Ssize", row["Ssize"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "Q_Unit", row["Q_Unit"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "Q_Unit_CD", row["Q_Unit_CD"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "In_No", row["In_No"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "In_Sort", row["In_Sort"].NullString());

            gv_MatOutM.SetRowCellValue(row_idx, "Qty", row["Qty"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "P_Price", row["P_Price"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "Amt", row["Amt"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "MatSer_No", row["MatSer_No"].NullString());

            gv_MatOutM.SetRowCellValue(row_idx, "Loc_Code", row["Loc_Code"].NullString());
            gv_MatOutM.SetRowCellValue(row_idx, "Loc_Name", row["Loc_Name"].NullString());
            gv_MatOutM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_MatOutM_CellValueChanged);
        }

        private void In_Search(string sInNo, string sItem_Code, string sIn_Sort, string sLocation, int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatOut");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SN");
                sp.AddParam("In_No", sInNo);
                sp.AddParam("Item_Code", sItem_Code);
                sp.AddParam("In_Sort", sIn_Sort);
                sp.AddParam("Whouse_Code", txt_Whouse.Text);
                sp.AddParam("Loc_Code", sLocation);

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

                    gv_MatOutM.SetRowCellValue(iRow, "Item_Code", sItem_Code);
                    gv_MatOutM.SetRowCellValue(iRow, "Item_Name", dr_In["Item_Name"].ToString());
                    gv_MatOutM.SetRowCellValue(iRow, "Ssize", dr_In["Ssize"].ToString());
                    gv_MatOutM.SetRowCellValue(iRow, "Q_Unit", dr_In["Q_Unit"].ToString());
                    gv_MatOutM.SetRowCellValue(iRow, "Q_Unit_CD", dr_In["Q_Unit_CD"].ToString());   
                    gv_MatOutM.SetRowCellValue(iRow, "In_Sort", dr_In["In_Sort"].ToString());
                    gv_MatOutM.SetRowCellValue(iRow, "Qty", dr_In["Qty"].NumString());
                    gv_MatOutM.SetRowCellValue(iRow, "P_Price", dr_In["P_Price"].NumString());
                    gv_MatOutM.SetRowCellValue(iRow, "Amt", dr_In["Amt"].NumString());
                    gv_MatOutM.SetRowCellValue(iRow, "MatSer_No", dr_In["MatSer_No"].ToString());
                    gv_MatOutM.SetRowCellValue(iRow, "Loc_Code", dr_In["Loc_Code"].ToString());
                }
                else
                {
                    gv_MatOutM.SetRowCellValue(iRow, "Item_Code", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Item_Name", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Ssize", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Q_Unit", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Q_Unit_CD", "");
                    gv_MatOutM.SetRowCellValue(iRow, "In_Sort", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Qty", "");
                    gv_MatOutM.SetRowCellValue(iRow, "P_Price", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Amt", "");
                    gv_MatOutM.SetRowCellValue(iRow, "MatSer_No", "");
                    gv_MatOutM.SetRowCellValue(iRow, "Loc_Code", "");
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
                int iRow = gv_MatOutM.GetFocusedDataSourceRowIndex();

                SqlParam sp = new SqlParam("sp_regMatOut");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DS");
                sp.AddParam("Out_No", txt_OutNo.Text);
                sp.AddParam("In_No", gv_MatOutM.GetRowCellValue(iRow, "In_No").ToString());
                sp.AddParam("In_Sort", gv_MatOutM.GetRowCellValue(iRow, "In_Sort").ToString());
                sp.AddParam("Loc_Code", gv_MatOutM.GetRowCellValue(iRow, "Loc_Code").ToString());
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

        //출고 구분
        private void txt_OutPart_EditValueChanged(object sender, EventArgs e)
        {
            txt_OutPartNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_OutPart.Text, "40030", "");
        }

        private void txt_OutPart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txt_OutPart_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_OutPart_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm help_form = new PopHelpForm("General", "sp_Help_General", "40030", txt_OutPart.Text, "N");
            if(help_form.ShowDialog() == DialogResult.OK)
            {
                txt_OutPart.Text = help_form.sRtCode;
                txt_OutPartNM.Text = help_form.sRtCodeNm;
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

        #endregion

        #region 내부 함수

        private void User_Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatOut");
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
                SqlParam sp = new SqlParam("sp_regMatOut");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SH");
                sp.AddParam("Out_No", txt_OutNo.Text);

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
                    txt_OutPart.Text = dr_H["Out_Part"].ToString();
                    dt_OutDate.Text = dr_H["Out_Date"].ToString();
                    txt_UserCode.Text = dr_H["User_Code"].ToString();
                    txt_Whouse.Text = dr_H["Whouse_Code"].ToString();
                    memo_BIgo.Text = dr_H["Out_Memo"].ToString();

                    txt_RegUser.Text = dr_H["Reg_User"].ToString();
                    txt_RegDate.Text = dr_H["Reg_Date"].ToString();
                    txt_UpUser.Text = dr_H["Up_User"].ToString();
                    txt_UpDate.Text = dr_H["Up_Date"].ToString();
                }
                else
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panel_M);
                    Set_Default();
                }

                gc_MatOutM.DataSource = dt_S;
                Sum_Amt();

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        // 출고 구분 기본 셋팅
        private void Set_Default()
        {
            string[] kind_codes = DbHelp.Set_Default("40030");

            if (kind_codes != null)
            {
                txt_OutPart.Text = kind_codes[0];
                txt_OutPartNM.Text = kind_codes[1];
            }
        }
     
        private void Sum_Amt()
        {
            decimal dAmt = 0;

            for(int i = 0; i < gv_MatOutM.RowCount; i++)
            {
                dAmt += decimal.Parse(gv_MatOutM.GetRowCellValue(i, "Amt").NumString());
            }

            txt_TotalAMT.Text = dAmt.ToString();
        }

        #endregion

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm help_Form = new PopHelpForm("Matout", "sp_Help_Matout", "", "N");
            help_Form.sNotReturn = "Y";
            help_Form.sLevelYN = "Y";
            btn_Select.clsWait.CloseWait();
            if (help_Form.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                txt_OutNo.Text = help_Form.sRtCode;
                Search();

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (btn_Insert.Result_Update == DialogResult.Yes)
            {
                btn_Select_Click(null, null);
            }

            txt_OutNo.Text = "";
            
            Search();

            ForMat.sBasic_Set(this.Name, txt_OutNo);

            txt_UserCode.Text = GlobalValue.sUserID;

            dt_OutDate.Select();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sInNo = "", sInSort = "", sItemCode = "", sQty = "", sP_Price = "", sAmt = "", sOutBigo = "", sQ_Unit = "", sLoc_Code = "";

                for(int i = 0; i < gv_MatOutM.RowCount; i++)
                {
                    if (gv_MatOutM.GetRowCellValue(i, "In_Sort").ToString() != "")
                    {
                        sInNo += gv_MatOutM.GetRowCellValue(i, "In_No").ToString() + "_/";
                        sInSort += gv_MatOutM.GetRowCellValue(i, "In_Sort").ToString() + "_/";
                        sItemCode += gv_MatOutM.GetRowCellValue(i, "Item_Code").ToString() + "_/";
                        sQty += gv_MatOutM.GetRowCellValue(i, "Qty").ToString() + "_/";
                        sP_Price += gv_MatOutM.GetRowCellValue(i, "P_Price").ToString() + "_/";
                        sAmt += gv_MatOutM.GetRowCellValue(i, "Amt").ToString() + "_/";
                        sOutBigo += gv_MatOutM.GetRowCellValue(i, "Out_Bigo").ToString() + "_/";
                        sQ_Unit += gv_MatOutM.GetRowCellValue(i, "Q_Unit_CD").ToString() + "_/";
                        sLoc_Code += gv_MatOutM.GetRowCellValue(i, "Loc_Code").ToString() + "_/";
                    }
                }

                SqlParam sp = new SqlParam("sp_regMatOut");
                sp.AddParam("Kind", "I");
                sp.AddParam("Out_No", txt_OutNo.Text);
                sp.AddParam("Out_Date", dt_OutDate.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("User_Code", txt_UserCode.Text);
                sp.AddParam("Whouse_Code", txt_Whouse.Text);
                sp.AddParam("Out_Part", txt_OutPart.Text);
                sp.AddParam("Out_Memo", memo_BIgo.Text);
                sp.AddParam("Dept_Code", sDeptCode);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                sp.AddParam("InNo", sInNo);
                sp.AddParam("InSort", sInSort);
                sp.AddParam("ItemCode", sItemCode);
                sp.AddParam("Qty", sQty);
                sp.AddParam("P_Price", sP_Price);
                sp.AddParam("Amt", sAmt);
                sp.AddParam("OutBigo", sOutBigo);
                sp.AddParam("Q_Unit", sQ_Unit);
                sp.AddParam("Loc_Code", sLoc_Code);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_OutNo.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Out_No"].ToString();
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
                SqlParam sp = new SqlParam("sp_regMatOUt");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DH");
                sp.AddParam("Out_No", txt_OutNo.Text);
                sp.AddParam("Whouse_Code", txt_Whouse.Text);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_OutNo.Text = "";
                //txt_Supply_Amt.EditValue = 0;
                Search();

                txt_UserCode.Text = GlobalValue.sUserID;

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
            FileIF.Excel_Down(gc_MatOutM, "자재출고");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                btn_Save_Click(null, null);
            }

            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
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
