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
    public partial class regWorkResult : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet DS = new DataSet();
        DataTable S_Items = new DataTable();

        public regWorkResult()
        {
            InitializeComponent();
        }

        private void WorkResult_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
            ForMat.sBasic_Set(this.Name, txt_EnterNo);

            dt_EnterDate.Focus();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Result_No", "순번", "80", false, false, false, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Order_No", "수주번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Sort_No", "정렬번호", "80", false, false, false, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Item_Name", "품명", "100", false, false, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "ItemSer_No", "Serial-No", "100", true, true, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "ItemLic_No", "라이센스", "100", true, true, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ver_Code", "버전코드", "100", false, false, false, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ver_Name", "버전", "100", false, true, true, true);
            DbHelp.GridSet(gc_ResultS, gv_ResultS, "Old_Qty", "기존수량", "80", true, true, false, true);

            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Result_No", "실적번호", "130", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Order_No", "수주번호", "130", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Item_Code", "모품목코드", "120", false, true, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "SItem_Code", "품목코드", "120", false, true, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Item_Name", "모품목명", "100", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "SItem_Name", "품목명", "100", false, false, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Custom_Code", "거래처코드", "100", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Short_Name", "거래처", "100", false, false, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Sort_No", "순번", "80", true, true, false, true);
            //DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Out_No", "출고번호", "130", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "In_No", "입고번호", "130", false, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "In_Sort", "순번", "80", true, false, false, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "MatSer_No", "시리얼넘버", "100", true, false, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "P_Price", "단가", "100", false, false, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_ResultS1, gv_ResultS1, "ChkQty", "수량", "80", true, true, false, true);
            gv_ResultS.Columns["Ver_Name"].AppearanceCell.BackColor = Color.FromArgb(252, 247, 182);

            DbHelp.GridColumn_NumSet(gv_ResultS, "Qty", ForMat.SetDecimal("regWorkResult", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultS1, "P_Price", ForMat.SetDecimal("regWorkResult", "Price1"));
            DbHelp.GridColumn_NumSet(gv_ResultS1, "Qty", ForMat.SetDecimal("regWorkResult", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_ResultS1, "ChkQty", ForMat.SetDecimal("regWorkResult", "Qty1"));

            DbHelp.GridColumn_Help(gv_ResultS, "Order_No", "Y");
            DbHelp.GridColumn_Help(gv_ResultS1, "SItem_Code", "Y");

            gv_ResultS.Columns["Sort_No"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            //gv_ResultS1.Columns["Sort_No"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

            // 좌측 그리드 Help
            RepositoryItemButtonEdit btn_edit = (RepositoryItemButtonEdit)gv_ResultS.Columns["Order_No"].ColumnEdit;
            btn_edit.Buttons[0].Click += new EventHandler(Left_Grid_Help);
            gv_ResultS.Columns["Order_No"].ColumnEdit = btn_edit;
            gv_ResultS.Columns["Ver_Name"].ColumnEdit = btn_edit;

            gc_ResultS.DeleteRowEventHandler += new EventHandler(Left_Row_Delete);

            RepositoryItemMemoEdit Memo_Spec = new RepositoryItemMemoEdit();
            gv_ResultS.Columns["ItemSer_No"].ColumnEdit = Memo_Spec;
            gv_ResultS.Columns["ItemLic_No"].ColumnEdit = Memo_Spec;

            gv_ResultS.OptionsView.RowAutoHeight = true;

            // 우측 그리드 Help
            RepositoryItemButtonEdit btn_edit2 = (RepositoryItemButtonEdit)gv_ResultS1.Columns["SItem_Code"].ColumnEdit;
            btn_edit2.Buttons[0].Click += new EventHandler(Right_Grid_Help);
            gv_ResultS1.Columns["SItem_Code"].ColumnEdit = btn_edit2;

            gc_ResultS1.DeleteRowEventHandler += new EventHandler(Right_Row_Delete);

            gv_ResultS.OptionsCustomization.AllowSort = false;
            gv_ResultS.OptionsView.ShowAutoFilterRow = false;
            gc_ResultS.MultiSelectChk = false;
            gc_ResultS.AddRowYN = true;

            gv_ResultS1.OptionsCustomization.AllowSort = false;
            gv_ResultS1.OptionsView.ShowAutoFilterRow = false;
            gc_ResultS1.MultiSelectChk = false;
            gc_ResultS1.AddRowYN = true;
        }

        #region 버튼 상속
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

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        #endregion

        #region 좌측 그리드 이벤트 및 메소드
        private void gv_ResultS_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(gv_ResultS.GetFocusedRowCellValue("Result_No").NullString()))
                gv_ResultS.Columns["Order_No"].OptionsColumn.AllowEdit = false;
            else
                gv_ResultS.Columns["Order_No"].OptionsColumn.AllowEdit = true;

            string Order_No = gv_ResultS.GetRowCellValue(e.RowHandle, "Order_No").NullString();
            string Item_Code = gv_ResultS.GetRowCellValue(e.RowHandle, "Item_Code").NullString();
            string Where = "Order_No = '" + Order_No + "' AND Item_Code = '" + Item_Code + "'";

            DataTable Items = S_Items.Clone();

            if (S_Items.Select(Where).Count() > 0)
                Items = S_Items.Select(Where).CopyToDataTable();

            gc_ResultS1.DataSource = Items;
            gc_ResultS1.RefreshDataSource();
        }

        private void gv_ResultS_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gv_ResultS.FocusedColumn.FieldName == "Order_No")
                Left_Grid_Help(null, null);
            else if (e.KeyCode == Keys.Enter && gv_ResultS.FocusedColumn.FieldName == "Ver_Name")
                Left_Grid_Help(null, null);
        }

        private void Left_Grid_Help(object sender, EventArgs e)
        {
            int iRow = gv_ResultS.FocusedRowHandle;
            int Sort_No = New_Sort_No(gc_ResultS);

            if (gv_ResultS.FocusedColumn.FieldName == "Order_No")
            {
                string param = (gv_ResultS.ActiveEditor == null) ? "" : gv_ResultS.ActiveEditor.EditValue.NullString();
                PopHelpForm Help_Form = new PopHelpForm("MakeResult_S", "sp_Help_Item_Result", param, "Y");
                Help_Form.sNotReturn = "Y";

                if (Help_Form.ShowDialog() == DialogResult.OK)
                {
                    gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);

                    int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                    int chk = 0;

                    for (int i = 0; i < end; i++)
                    {
                        // 수량 체크
                        string Where = "Order_No = '" + Help_Form.drReturn[i]["Order_No"].NullString() + "' AND Item_Code = '" + Help_Form.drReturn[i]["Item_Code"].NullString() + "'";

                        if ((gc_ResultS.DataSource as DataTable).Select(Where).Count() > 0)
                        {
                            chk++;
                            continue;
                        }

                        Set_Left_Data(iRow, Help_Form.drReturn[i], Sort_No);

                        iRow++;
                        Sort_No++;
                    }

                    if (chk > 0)
                        XtraMessageBox.Show("이미 본 실적내에 동일한 수주 품목이 존재합니다. " + end + "건 중 " + chk + "건");
                    
                    gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                }
            }
            else if (gv_ResultS.FocusedColumn.FieldName == "Ver_Name")
            {
                string Ver = string.Empty;

                if (gv_ResultS.ActiveEditor == null)
                    Ver = "";
                else
                    Ver = gv_ResultS.ActiveEditor.EditValue.NullString();

                PopHelpForm Help_Form = new PopHelpForm("General", "sp_Help_General", "30040", Ver, "N");

                if (Help_Form.ShowDialog() == DialogResult.OK)
                {
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Code", Help_Form.sRtCode);
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Name", Help_Form.sRtCodeNm);

                    gv_ResultS.UpdateCurrentRow();
                }
            }
        }

        private DataRow Get_Item_Info(string Order_No, string Item_Code)
        {
            SqlParam sp = new SqlParam("sp_regWorkResult");
            sp.AddParam("Kind", "Q");
            sp.AddParam("Order_No", Order_No);
            sp.AddParam("Item_Code", Item_Code);

            ReturnStruct temp_ret = DbHelp.Proc_Search(sp);

            if (temp_ret.ReturnChk == 0 && temp_ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                return temp_ret.ReturnDataSet.Tables[0].Rows[0];
            else
                return null;
        }

        private void Set_Left_Data(int Row_Index, DataRow Row, int Sort_No)
        {
            gv_ResultS.SetRowCellValue(Row_Index, "Sort_No", Sort_No);
            gv_ResultS.SetRowCellValue(Row_Index, "Order_No", Row["Order_No"].NullString());
            gv_ResultS.SetRowCellValue(Row_Index, "Item_Code", Row["Item_Code"].NullString());
            gv_ResultS.SetRowCellValue(Row_Index, "Item_Name", Row["Item_Name"].NullString());
            gv_ResultS.SetRowCellValue(Row_Index, "Ssize", Row["Ssize"].NullString());
            gv_ResultS.SetRowCellValue(Row_Index, "Qty", Row["In_Qty"].NullString()); // Row["Not_In_Qty"].NullString()
            gv_ResultS.SetRowCellValue(Row_Index, "Old_Qty", Row["In_Qty"].NullString());

            gv_ResultS.AddNewRow();
            gv_ResultS.UpdateCurrentRow();
        }

        private void Left_Row_Delete(object sender, EventArgs e)
        {
            if (gv_ResultS.RowCount > 0)
            {
                if (!string.IsNullOrWhiteSpace(gv_ResultS.GetFocusedRowCellValue("Result_No").NullString()))
                {
                    string Order_No = gv_ResultS.GetFocusedRowCellValue("Order_No").NullString();
                    string Item_Code = gv_ResultS.GetFocusedRowCellValue("Item_Code").NullString();

                    DataRow[] S_Items_Rows = S_Items.Select("Order_No = '" + Order_No + "' AND Item_Code = '" + Item_Code + "'");

                    // MatOut_S의 Result_Ck = 'N'으로 Update 될 자재들
                    DataTable S_Items_Table = S_Items.Clone();

                    foreach (DataRow del_row in S_Items_Rows)
                    {
                        //if (!string.IsNullOrWhiteSpace(del_row["Result_No"].NullString()))
                        //    S_Items_Table.ImportRow(del_row);

                        S_Items.Rows.Remove(del_row);   // S_Items 테이블에서 해당 자재들 제거
                    }

                    //DataRow row = DbHelp.Summary_Data_2(S_Items_Table, "Item_Code", new string[] { "Order_No", "SItem_Code", "Item_Code", "Sort_No" });

                    //Update_MatOut_S(row);   // 모품목 제거시 해당 자재(MatOut_S)의 Result_Ck = 'N'으로 Update

                    SqlParam sp = new SqlParam("sp_regWorkResult");
                    sp.AddParam("Kind", "D");
                    sp.AddParam("Delete_Kind", "S");
                    sp.AddParam("Result_No", txt_EnterNo.Text);
                    sp.AddParam("OrderNo", Order_No);
                    sp.AddParam("ItemCode", Item_Code);
                    sp.AddParam("Up_User", GlobalValue.sUserID);

                    ret = DbHelp.Proc_Save(sp);

                    if (ret.ReturnChk != 0)
                    {
                        XtraMessageBox.Show(ret.ReturnMessage);
                        return;
                    }

                    btn_Delete.sCHK = "Y";

                    gv_ResultS.DeleteRow(gv_ResultS.FocusedRowHandle);
                    gv_ResultS.UpdateCurrentRow();
                }
            }
        }
        #endregion

        #region 우측 그리드 이벤트 및 메소드
        private void gv_ResultS1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gv_ResultS1.FocusedColumn.FieldName == "SItem_Code")
                Right_Grid_Help(null, null);
        }
        private void Right_Grid_Help(object sender, EventArgs e)
        {
            //int Sort_No = New_Sort_No(gc_ResultS1);
            string Order_No = gv_ResultS.GetFocusedRowCellValue("Order_No").NullString();
            string Item_Code = gv_ResultS.GetFocusedRowCellValue("Item_Code").NullString();

            int iRow = gv_ResultS1.FocusedRowHandle;

            PopHelpForm Help_Form = new PopHelpForm("MatOut_S", "sp_Help_Result_S1", Order_No, "Y");
            Help_Form.Set_Value(Item_Code, "", "", "", "");
            Help_Form.sNotReturn = "Y";
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                int chk = 0;

                if (end > 0)
                {
                    foreach (DataRow row in Help_Form.drReturn)
                    {
                        string Where = "Order_No = '" + Order_No + "' AND Item_Code = '" + Item_Code + "' AND SItem_Code = '" + row["SItem_Code"].NullString() + "' AND Sort_No = '" + row["Sort_No"].NumString() + "'";
                        if ((gc_ResultS1.DataSource as DataTable).Select(Where).Count() > 0)
                        {
                            chk++;
                        }
                        else
                        {
                            DataRow S1_Row = Get_S1_Info(iRow, Item_Code, row["SItem_Code"].NullString(), row["Sort_No"].NumString(), Order_No);

                            if (S1_Row != null)
                            {
                                S1_Row["Order_No"] = Order_No;
                                S1_Row["Sort_No"] = row["Sort_No"].NumString();

                                S_Items.ImportRow(S1_Row);

                                gv_ResultS1.AddNewRow();
                                gv_ResultS1.UpdateCurrentRow();

                                iRow++;
                            }
                        }
                    }
                }

                if (chk > 0)
                    XtraMessageBox.Show("동일한 출고정보가 존재합니다. " + end + "건 중 " + chk + "건");
            }
        }

        // 그리드에 자재 정보 기입
        private DataRow Get_S1_Info(int Row_Idx, string sItem_Code, string sSItem_Code, string sSort_No, string sOrder_No)
        {
            SqlParam sp = new SqlParam("sp_regWorkResult");
            sp.AddParam("Kind", "H");
            sp.AddParam("OrderNo", sOrder_No);
            sp.AddParam("ItemCode", sItem_Code);
            sp.AddParam("SItemCode", sSItem_Code);
            sp.AddParam("SortNo", sSort_No);

            ReturnStruct temp = DbHelp.Proc_Search(sp);
            if (temp.ReturnDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = DbHelp.Fill_Table(temp.ReturnDataSet.Tables[0]).Rows[0];

                gv_ResultS1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS1_CellValueChanged);

                gv_ResultS1.SetRowCellValue(Row_Idx, "Order_No", sOrder_No);
                gv_ResultS1.SetRowCellValue(Row_Idx, "Sort_No", sSort_No);
                gv_ResultS1.SetRowCellValue(Row_Idx, "Item_Code", row["Item_Code"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "SItem_Code", row["SItem_Code"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "Item_Name", row["Item_Name"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "SItem_Name", row["SItem_Name"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "Ssize", row["Ssize"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "In_No", row["In_No"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "In_Sort", row["In_Sort"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "P_Price", row["P_Price"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "Custom_Code", row["Custom_Code"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "Short_Name", row["Short_Name"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "MatSer_No", row["MatSer_No"].NullString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "Qty", row["Qty"].NumString());
                gv_ResultS1.SetRowCellValue(Row_Idx, "ChkQty", row["Qty"].NumString());

                gv_ResultS1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS1_CellValueChanged);

                return row;
            }
            else
                return null;
        }

        // 저장되지 않은 자재 로우 삭제했을 경우
        private void gv_ResultS1_RowDeleted(object sender, DevExpress.Data.RowDeletedEventArgs e)
        {
            S1_Row_Delete();
        }

        // 저장된 자재 로우 삭제했을 경우
        private void Right_Row_Delete(object sender, EventArgs e)
        {
            gv_ResultS1.RowDeleted -= new DevExpress.Data.RowDeletedEventHandler(gv_ResultS1_RowDeleted);
            S1_Row_Delete();
            gv_ResultS1.RowDeleted += new DevExpress.Data.RowDeletedEventHandler(gv_ResultS1_RowDeleted);
        }

        private void S1_Row_Delete()
        {
            if (gv_ResultS1.RowCount > 0)
            {
                DataRow S_Item_Row = gv_ResultS1.GetFocusedDataRow();

                DataRow Focused_Row = (S_Items.Select("Order_No = '" + S_Item_Row["Order_No"].NullString() + "' AND Item_Code = '" + S_Item_Row["Item_Code"].NullString() + "' AND SItem_Code = '" + S_Item_Row["SItem_Code"].NullString() + "' AND Sort_No = " + S_Item_Row["Sort_No"].NumString()))[0];

                if (!string.IsNullOrWhiteSpace(Focused_Row["Result_No"].NullString()))
                {
                    string Order_No = Focused_Row["Order_No"].NullString();
                    string Item_Code = Focused_Row["Item_Code"].NullString();
                    string SItem_Code = Focused_Row["SItem_Code"].NullString();
                    string Sort_No = Focused_Row["Sort_No"].NullString();

                    //DataTable Table = new DataTable();
                    //Table.Columns.Add("Order_No");
                    //Table.Columns.Add("Item_Code");
                    //Table.Columns.Add("Sort_No");
                    //Table.Rows.Add(Order_No + "_/", Item_Code + "_/", Sort_No + "_/");

                    //Update_MatOut_S(Table.Rows[0]);

                    SqlParam sp = new SqlParam("sp_regWorkResult");
                    sp.AddParam("Kind", "D");
                    sp.AddParam("Delete_Kind", "H");
                    sp.AddParam("Result_No", txt_EnterNo.Text);
                    sp.AddParam("SortNo", Sort_No);
                    sp.AddParam("ItemCode", Item_Code);
                    sp.AddParam("OrderNo", Order_No);
                    sp.AddParam("SItemCode", SItem_Code);
                    sp.AddParam("Up_User", GlobalValue.sUserID);

                    ret = DbHelp.Proc_Save(sp);

                    if (ret.ReturnChk != 0)
                    {
                        XtraMessageBox.Show(ret.ReturnMessage);
                        return;
                    }

                    S_Items.Rows.Remove(Focused_Row);
                    gv_ResultS1.DeleteRow(gv_ResultS1.FocusedRowHandle);

                    btn_Delete.sCHK = "Y";
                }
                else
                {
                    S_Items.Rows.Remove(Focused_Row);
                    gv_ResultS1.DeleteRow(gv_ResultS1.FocusedRowHandle);
                    gv_ResultS1.UpdateCurrentRow();
                }

            }
        }

        private void gv_ResultS1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            string Result_No = gv_ResultS1.GetFocusedRowCellValue("Result_No").NullString();

            if (!string.IsNullOrWhiteSpace(Result_No))
                gv_ResultS1.Columns["SItem_Code"].OptionsColumn.AllowEdit = false;
            else
                gv_ResultS1.Columns["SItem_Code"].OptionsColumn.AllowEdit = true;
        }
        #endregion

        #region 공통코드 및 공통 메소드, 이벤트
        private void txt_WareHS_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_WareHS_Properties_ButtonClick(sender, null);
        }

        private void txt_WareHS_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_WareHS.Text, "N");

            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_WareHS.Text = HelpForm.sRtCode;
                txt_WareHS_Name.Text = HelpForm.sRtCodeNm;
            }
        }

        private void txt_WareHS_EditValueChanged(object sender, EventArgs e)
        {
            ReturnStruct ret_R = new ReturnStruct();

            try
            {
                SqlParam sp = new SqlParam("sp_regWorkResult");
                sp.AddParam("Kind", "W");
                sp.AddParam("Pr", txt_WareHS.Text);

                ret_R = DbHelp.Proc_Search(sp);
                if (ret_R.ReturnChk == 0)
                {
                    if (ret_R.ReturnDataSet.Tables[0].Rows.Count == 1)
                    {
                        txt_WareHS_Name.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["Whouse_Name"].ToString();
                        txt_WareHS.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["Whouse_Code"].ToString();
                    }
                    else
                    {
                        txt_WareHS_Name.Text = "";
                        return;
                    }
                }
                else
                {
                    txt_WareHS_Name.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txt_WareHS.Text = "";
            }
        }



        private void txt_Rout_EditValueChanged(object sender, EventArgs e)
        {
            ReturnStruct ret_R = new ReturnStruct();

            try
            {
                SqlParam sp = new SqlParam("sp_regWorkResult");
                sp.AddParam("Kind", "R");
                sp.AddParam("Pr", txt_Rout.Text);
                sp.AddParam("GMCODE", "30030");

                ret_R = DbHelp.Proc_Search(sp);
                if (ret_R.ReturnChk == 0)
                {
                    if (ret_R.ReturnDataSet.Tables[0].Rows.Count == 1)
                    {
                        txt_Rout_Name.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["GS_Name"].ToString();
                        txt_Rout.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["GS_Code"].ToString();
                        return;
                    }
                    else
                    {
                        txt_Rout_Name.Text = "";
                        return;
                    }
                }
                else
                {
                    txt_Rout_Name.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txt_Rout_Name.Text = "";
            }
        }

        private void txt_Rout_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Rout_Properties_ButtonClick(sender, null);
        }

        private void txt_Rout_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30030", txt_Rout.Text, "N");

            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_Rout.Text = HelpForm.sRtCode;
                txt_Rout_Name.Text = HelpForm.sRtCodeNm;
            }
        }



        private void txt_WorkCharger_EditValueChanged(object sender, EventArgs e)
        {
            txt_WorkChargerNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_WorkCharger.Text);
            if (!string.IsNullOrWhiteSpace(txt_WorkChargerNM.Text))
                Get_User_Info();
            else
            {
                txt_Dept.Text = "";
                txt_Dept.ToolTip = "";
            }
        }
        private void txt_WorkCharger_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_WorkCharger_Properties_ButtonClick(sender, null);
        }

        private void txt_WorkCharger_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_WorkChargerNM.Text))
            {
                Get_User_Info();
            }
        }
        private void Get_User_Info()
        {
            PopHelpForm Helpform = new PopHelpForm("User", "sp_Help_User", txt_WorkCharger.Text, "N");
            if (Helpform.ShowDialog() == DialogResult.OK)
            {
                txt_WorkCharger.EditValueChanged -= new System.EventHandler(this.txt_WorkCharger_EditValueChanged);

                txt_WorkCharger.Text = Helpform.sRtCode;
                txt_WorkChargerNM.Text = Helpform.sRtCodeNm;

                string[] dept = Search_Dept(txt_WorkCharger.Text).Split('/');

                txt_Dept.Text = dept[0];
                txt_Dept.ToolTip = dept[1];

                txt_WorkCharger.EditValueChanged += new System.EventHandler(this.txt_WorkCharger_EditValueChanged);
            }
        }

        private string Search_Dept(string user_code)
        {
            SqlParam sp = new SqlParam("sp_Help_User");
            sp.AddParam("GB", "1");
            sp.AddParam("Basic", user_code);

            ReturnStruct temp_rst = DbHelp.Proc_Search(sp);

            string dept_code = "";
            string dept_name = "";

            if (temp_rst.ReturnDataSet.Tables[0].Rows.Count > 0)
            {
                dept_name = temp_rst.ReturnDataSet.Tables[0].Rows[0]["Dept_Name"].ToString();

                SqlParam temp_sp = new SqlParam("sp_Help_Dept");
                temp_sp.AddParam("GB", "1");
                temp_sp.AddParam("Basic", dept_name);

                temp_rst = DbHelp.Proc_Search(temp_sp);

                dept_code = temp_rst.ReturnDataSet.Tables[0].Rows[0]["Dept_Code"].ToString();
            }
            return dept_name + "/" + dept_code;
        }

        // MatOut_S => Result_Ck = 'N'
        private void Update_MatOut_S(DataRow row)
        {
            if (!string.IsNullOrWhiteSpace(txt_EnterNo.Text))
            {
                SqlParam sp = new SqlParam("sp_regWorkResult");

                sp.AddParam("Kind", "U");
                sp.AddParam("Result_No", txt_EnterNo.Text);
                sp.AddParam("Order_No", row["Order_No"].NullString());
                sp.AddParam("Item_Code", row["Item_Code"].NullString());
                sp.AddParam("Sort_No", row["Sort_No"].NullString());

                ReturnStruct temp_ret = DbHelp.Proc_Save(sp);

                if (temp_ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(temp_ret.ReturnMessage);
                    return;
                }
            }
        }

        // 검색 컬럼의 값 변동이 발생했을 경우
        //private void GridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        //{
        //    gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(GridView_CellValueChanged);
        //    gv_ResultS1.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(GridView_CellValueChanged);

        //    if (e.Column.FieldName == "Order_No")
        //    {
        //        SqlParam sp = new SqlParam("sp_regWorkResult");
        //        sp.AddParam("Kind", "L");
        //        sp.AddParam("Form_Name", "regOrder");

        //        ReturnStruct temp = DbHelp.Proc_Search(sp);

        //        int len;

        //        if (temp.ReturnChk != 0)
        //            len = 0;
        //        else
        //            len = Convert.ToInt32(temp.ReturnDataSet.Tables[0].Rows[0][0].NumString());

        //        int new_len = (gv_ResultS.ActiveEditor == null) ? 0 : gv_ResultS.ActiveEditor.EditValue.NullString().Length;

        //        if (new_len != len && len != 0)
        //        {
        //            foreach (GridColumn col in gv_ResultS.Columns)
        //            {
        //                gv_ResultS.SetFocusedRowCellValue(col, "");
        //            }
        //        }
        //    }
        //    else if (e.Column.FieldName == "Ver_Name")
        //    {
        //        int old_len = gv_ResultS.ActiveEditor.OldEditValue.NullString().Length;
        //        int new_len = gv_ResultS.ActiveEditor.EditValue.NullString().Length;

        //        if (old_len > new_len)
        //            gv_ResultS.SetFocusedRowCellValue("Ver_Code", "");
        //    }
        //    else if (e.Column.FieldName == "SItem_Code")
        //    {
        //        string sItem_Code = gv_ResultS1.GetRowCellValue(e.RowHandle, "Item_Code").ToString();
        //        string sSortNo = gv_ResultS1.GetRowCellValue(e.RowHandle, "Sort_No").ToString();
        //        string sOrder_No = gv_ResultS1.GetRowCellValue(e.RowHandle, "Order_No").ToString();

        //        DataRow S1_Row = Get_S1_Info(e.RowHandle, sItem_Code, e.Value.ToString(), sSortNo, sOrder_No);

        //        if(S1_Row == null)
        //        {
        //            foreach (GridColumn col in gv_ResultS1.Columns)
        //            {
        //                gv_ResultS1.SetFocusedRowCellValue(col, "");
        //            }
        //        }
        //        //int new_len = gv_ResultS1.ActiveEditor.EditValue.NullString().Length;

        //        //if (new_len != 5)
        //        //{
        //        //    foreach (GridColumn col in gv_ResultS1.Columns)
        //        //    {
        //        //        gv_ResultS1.SetFocusedRowCellValue(col, "");
        //        //    }
        //        //}
        //    }

        //    gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(GridView_CellValueChanged);
        //    gv_ResultS1.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(GridView_CellValueChanged);

        //    btn_Insert.sUpdate = "Y";
        //    btn_Close.sUpdate = "Y";
        //}

        // 중간에 비어있는 Sort_No 또는 최댓값 반환
        private int New_Sort_No(GridControlEx Grid)
        {
            DataTable Table = Grid.DataSource as DataTable;

            if (Table != null)
            {
                Table.DefaultView.Sort = "Sort_No ASC";
                Table = Table.DefaultView.ToTable();

                DataRow[] Rows = Table.Select("Sort_No IS NOT NULL");

                if (Rows.Count() > 0)
                    Table = Rows.CopyToDataTable();
                else
                    return 0;

                for (int i = 0; i < Table.Rows.Count; i++)
                {
                    if (Table.Rows[i]["Sort_No"].NumString() != i.ToString())
                        return i;
                }

                return Table.Rows.Count;
            }
            else
                return 0;
        }
        #endregion

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("MakeResult", "sp_Help_WorkResult", "", "N");
            HelpForm.sLevelYN = "Y";
            HelpForm.sNotReturn = "Y";
            btn_Select.clsWait.CloseWait();
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                Search_Data(HelpForm.sRtCode);

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void Search_Data(string code = "")
        {
            SqlParam sp = new SqlParam("sp_regWorkResult");
            sp.AddParam("Kind", "S");
            sp.AddParam("Search_Kind", "All");
            sp.AddParam("Result_No", code);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnDataSet != null && ret.ReturnDataSet.Tables.Count > 1)
            {
                DataTable info = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                if (info != null && info.Rows.Count > 0)
                {
                    DataRow row = info.Rows[0];

                    txt_EnterNo.Text = row["Result_No"].ToString();
                    dt_EnterDate.Text = row["Result_Date"].ToString();

                    txt_Rout.Text = row["Process_Code"].ToString();
                    txt_Rout_Name.Text = row["Process_Name"].ToString();
                    txt_WareHS.Text = row["Whouse_Code"].ToString();
                    txt_WareHS_Name.Text = row["Whouse_Name"].ToString();
                    txt_WorkCharger.Text = row["Work_User"].ToString();
                    txt_WorkChargerNM.Text = row["Work_User_Name"].ToString();
                    txt_Dept.Text = row["Dept_Name"].ToString();
                    txt_Dept.ToolTip = row["Dept_Code"].ToString();

                    txt_RegDate.Text = row["Reg_Date"].ToString();
                    txt_RegUser.Text = row["Reg_User_Name"].ToString();
                    txt_UpDate.Text = row["Up_Date"].ToString();
                    txt_UpUser.Text = row["Up_User_Name"].ToString();
                }
                else
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panelControl3);

                    txt_WorkCharger.Text = GlobalValue.sUserID;
                    txt_WorkChargerNM.Text = GlobalValue.sUserNm;
                }

                S_Items = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[2]);

                DataTable M_Items = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[1]);
                gc_ResultS.DataSource = M_Items;
                gc_ResultS.RefreshDataSource();
                gv_ResultS.Columns["Order_No"].OptionsColumn.AllowEdit = true;

                if (M_Items.Rows.Count == 0)
                    gc_ResultS1.DataSource = S_Items.Clone();
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if (btn_Insert.Result_Update == DialogResult.Yes)
            {
                if (!Check())
                    return;
                btn_Save_Click(null, null);
            }
            //Save_Data();
            Search_Data();

            dt_EnterDate.Select();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                txt_EnterNo.Text = Save_Data();
                Search_Data(txt_EnterNo.Text);
            }
        }

        private bool Check()
        {
            // 필수값 체크
            if (string.IsNullOrWhiteSpace(dt_EnterDate.Text))
            {
                XtraMessageBox.Show("입고일자는 필수 입력값입니다.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_WareHS_Name.Text))
            {
                XtraMessageBox.Show("창고는 필수 입력값입니다.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_Rout_Name.Text))
            {
                XtraMessageBox.Show("공정은 필수 입력값입니다.");
                return false;
            }
            return true;
        }

        private string Save_Data()
        {
            try
            {
                DataRow Left_Data = DbHelp.Summary_Data(gv_ResultS, "Order_No", new string[] { "Order_No", "Item_Code", "Sort_No", "Qty", "ItemSer_No", "ItemLic_No", "Ver_Code" });

                S_Items.DefaultView.Sort = "Item_Code, Sort_No ASC";
                S_Items = S_Items.DefaultView.ToTable();

                DataRow Right_Data = DbHelp.Summary_Data_2(S_Items, "SItem_Code", new string[] { "Order_No", "Item_Code", "SItem_Code", "Sort_No", "Qty" });

                SqlParam sp = new SqlParam("sp_regWorkResult");
                sp.AddParam("Kind", "I");

                // WorkResult_M
                sp.AddParam("Result_No", txt_EnterNo.Text);
                sp.AddParam("Result_Date", dt_EnterDate.Text.Replace("-", ""));
                sp.AddParam("Process_Code", txt_Rout.Text);
                sp.AddParam("Whouse_Code", txt_WareHS.Text);
                sp.AddParam("Work_User", txt_WorkCharger.Text);
                sp.AddParam("Reg_User", GlobalValue.sUserID);
                sp.AddParam("Up_User", GlobalValue.sUserID);

                // WorkResult_S
                sp.AddParam("Order_No", Left_Data["Order_No"].NullString());
                sp.AddParam("Item_Code", Left_Data["Item_Code"].NullString());
                sp.AddParam("Sort_No", Left_Data["Sort_No"].NullString());
                sp.AddParam("Qty", Left_Data["Qty"].NullString());
                sp.AddParam("ItemSer_No", Left_Data["ItemSer_No"].NullString());
                sp.AddParam("ItemLic_No", Left_Data["ItemLic_No"].NullString());
                sp.AddParam("Ver_Code", Left_Data["Ver_Code"].NullString());

                // WorkResult_S1
                sp.AddParam("Order_No_2", Right_Data["Order_No"].NullString());
                sp.AddParam("Item_Code_2", Right_Data["Item_Code"].NullString());
                sp.AddParam("SItem_Code_2", Right_Data["SItem_Code"].NullString());
                sp.AddParam("Sort_No_2", Right_Data["Sort_No"].NullString());
                sp.AddParam("Qty_2", Right_Data["Qty"].NullString());

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return null;
                }

                btn_Save.sCHK = "Y";
                return Convert.ToString(ret.ReturnDataSet.Tables[0].Rows[0]["Result_No"]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_EnterNo.Text))
            {
                //DataRow Row = DbHelp.Summary_Data_2(S_Items, "Item_Code", new string[] { "Order_No", "Item_Code", "Sort_No" });
                //Update_MatOut_S(Row);

                DataRow Del_Row = DbHelp.Summary_Data(gv_ResultS, "Result_No", new string[] { "Order_No", "Item_Code", "Sort_No" });

                SqlParam sp = new SqlParam("sp_regWorkResult");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_Kind", "M");
                sp.AddParam("Result_No", txt_EnterNo.Text);
                sp.AddParam("Order_No", Del_Row["Order_No"].NullString());
                sp.AddParam("Item_Code", Del_Row["Item_Code"].NullString());
                sp.AddParam("Sort_No", Del_Row["Sort_No"].NullString());

                ret = DbHelp.Proc_Save(sp);
                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
                Search_Data();
                btn_Delete.sCHK = "Y";
            }
            else
            {
                Search_Data();
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_ResultS, "작업실적등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                if (!Check())
                    return;
                btn_Save_Click(null, null);
            }

            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion

        private void gv_ResultS1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "SItem_Code")
            {
                string sItem_Code = gv_ResultS1.GetRowCellValue(e.RowHandle, "Item_Code").ToString();
                string sSortNo = gv_ResultS1.GetRowCellValue(e.RowHandle, "Sort_No").ToString();
                string sOrder_No = gv_ResultS1.GetRowCellValue(e.RowHandle, "Order_No").ToString();

                DataRow S1_Row = Get_S1_Info(e.RowHandle, sItem_Code, e.Value.ToString(), sSortNo, sOrder_No);

                if (S1_Row == null)
                {
                    foreach (GridColumn col in gv_ResultS1.Columns)
                    {
                        if(col.FieldName != "SItem_Code")
                            gv_ResultS1.SetFocusedRowCellValue(col, "");
                    }

                    gv_ResultS1.UpdateCurrentRow();
                }
            }
            else if(e.Column.FieldName == "Qty")
            {
                decimal iChkQty = decimal.Parse(gv_ResultS1.GetRowCellValue(e.RowHandle, "ChkQty").NumString());
                decimal iQty = decimal.Parse(e.Value.ToString());

                if (iChkQty < iQty)
                {
                    iQty = iChkQty;
                    gv_ResultS1.CellValueChanged -= gv_ResultS1_CellValueChanged;
                    gv_ResultS1.SetRowCellValue(e.RowHandle, "Qty", iQty.ToString());
                    gv_ResultS1.CellValueChanged += gv_ResultS1_CellValueChanged;
                }

                string sOrder_No = gv_ResultS1.GetRowCellValue(e.RowHandle, "Order_No").ToString();
                string sItem_Code = gv_ResultS1.GetRowCellValue(e.RowHandle, "Item_Code").ToString();
                string sSItem_Code = gv_ResultS1.GetRowCellValue(e.RowHandle, "SItem_Code").ToString();
                string sSort_No = gv_ResultS1.GetRowCellValue(e.RowHandle, "Sort_No").ToString();

                DataRow dr = S_Items.Select("Order_No = '" + sOrder_No + "' AND Item_Code = '" + sItem_Code + "' AND SItem_Code = '" + sSItem_Code + "' AND Sort_No = '" + sSort_No + "'")[0];

                dr.BeginEdit();
                dr["Qty"] = iQty.ToString();
                dr.EndEdit();

                S_Items.AcceptChanges();
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_ResultS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Order_No")
            {
                SqlParam sp = new SqlParam("sp_regWorkResult");
                sp.AddParam("Kind", "L");
                sp.AddParam("OrderNo", e.Value.ToString());
                sp.AddParam("ItemCode", gv_ResultS.GetRowCellValue(e.RowHandle, "Item_Code").ToString());

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
                
                if(ret.ReturnDataSet.Tables[0].Rows.Count == 0)
                {
                    foreach (GridColumn col in gv_ResultS.Columns)
                    {
                        if (col.FieldName != "Order_No")
                            gv_ResultS.SetFocusedRowCellValue(col, "");
                    }

                    gv_ResultS.UpdateCurrentRow();
                }

            }
            else if (e.Column.FieldName == "Ver_Name")
            {
                int old_len = gv_ResultS.ActiveEditor.OldEditValue.NullString().Length;
                int new_len = gv_ResultS.ActiveEditor.EditValue.NullString().Length;

                if (old_len > new_len)
                    gv_ResultS.SetFocusedRowCellValue("Ver_Code", "");
            }
        }
    }
}
