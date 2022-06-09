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
    public partial class regMatPo : BaseReg
    {
        private string sMatPo_No = "";
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        string sMatpo_Memo = "";

        public regMatPo()
        {
            InitializeComponent();
        }

        private void regMatPo_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data("");
            txt_User.Text = GlobalValue.sUserID;
            Button_State("Search");
            ForMat.sBasic_Set(this.Name, txt_OrderNo);

            dt_OrderDate.Focus();
        }

        private void Grid_Set()
        {
            //Order_M 그리드

            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Sort_No", "순번", "50", false, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Code", "품목코드", "120", false, true, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Name", "품명", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Item_Memo", "Spec", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Q_Unit", "단위코드", "80", false, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Q_Unit_Name", "단위", "50", false, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "P_Price", "단가", "100", true, true, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Amt", "공급가액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Vat_Amt", "부가세액", "100", true, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Tot_Amt", "합계금액", "100", true, false, false, true);
            DbHelp.GridSet(gc_MatPoM, gv_MatPoM, "Po_Bigo", "비고", "150", false, true, true, true);

            DbHelp.GridColumn_Help(gv_MatPoM, "Item_Code", "Y");
            RepositoryItemButtonEdit btn_edit = (RepositoryItemButtonEdit)gv_MatPoM.Columns["Item_Code"].ColumnEdit;
            btn_edit.Buttons[0].Click += new EventHandler(Item_Code_Click);

            gv_MatPoM.Columns["Item_Code"].ColumnEdit = btn_edit;
            gc_MatPoM.DeleteRowEventHandler += new EventHandler(Item_Delete); // 그리드 우클릭 삭제
            gc_MatPoM.PopMenuChk = true;
            gc_MatPoM.MouseWheelChk = true;
            gc_MatPoM.AddRowYN = true;

            DbHelp.GridColumn_NumSet(gv_MatPoM, "Qty", ForMat.SetDecimal("regMatPo", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "P_Price", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Amt", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Vat_Amt", ForMat.SetDecimal("regMatPo", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatPoM, "Tot_Amt", ForMat.SetDecimal("regMatPo", "Price1"));
            txt_Supply_Amt.Properties.Mask.EditMask = "n" + ForMat.SetDecimal("regMatPo", "Price1");

            gv_MatPoM.OptionsView.ShowAutoFilterRow = false;
        }

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

        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("MatPo", "sp_Help_MatPo", "", "N");
            HelpForm.sLevelYN = "Y";
            HelpForm.sNotReturn = "Y";
            btn_Select.clsWait.CloseWait();
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                sMatPo_No = HelpForm.sRtCode;
                //sSearch = "Y";
                Search_Data(sMatPo_No);

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

            //if (btn_Save.sCHK == "Y")
            //{
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panelControl3);
                
            sMatPo_No = null;
            Search_Data("");
            txt_User.Text = GlobalValue.sUserID;

            Button_State("Initialize");
            btn_Save.sCHK = "N";
            ForMat.sBasic_Set(this.Name, txt_OrderNo);

            dt_OrderDate.Select();
            btn_Close.sUpdate = "N";
            btn_Insert.sUpdate = "N";
            //}
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DataTable initialized = (gc_MatPoM.DataSource as DataTable).Clone();

            if (string.IsNullOrWhiteSpace(txt_OrderNo.Text))
            {
                gc_MatPoM.DataSource = initialized;
                gc_MatPoM.RefreshDataSource();

                Button_State("Delete");
            }
            else
            {
                SqlParam sp = new SqlParam("sp_regMatPo");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_Kind", "M");
                sp.AddParam("Po_No", txt_OrderNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk == 0)
                {
                    gc_MatPoM.DataSource = initialized;
                    gc_MatPoM.RefreshDataSource();

                    Button_State("Delete");
                    ForMat.sBasic_Set(this.Name, txt_OrderNo);
                }
                else
                {
                    XtraMessageBox.Show("입고된 발주 내역은 삭제가 불가능합니다.");
                    //XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Necessity() && gv_MatPoM.RowCount > 0) // 필수 입력값 모두 존재
            {
                if (Save_Data() == false)
                    return;
                Button_State("Initialize");
                btn_Save.sCHK = "Y";
            }
            else
            {
                btn_Save.sCHK = "N";
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatPoM, "발주등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion

        #region 그리드 이벤트
        void Item_Code_Click(object sender, EventArgs e)
        {
            string sort_no = Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("Sort_No"));
            string item_Code = Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("Item_Code"));

            if (!string.IsNullOrWhiteSpace(sort_no)) return;

            int iRow = gv_MatPoM.GetFocusedDataSourceRowIndex();
            PopHelpForm Help_Form = new PopHelpForm("Item", "sp_Help_Item", item_Code, "Y");
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                for (int i = 0; i < end; i++)
                {
                    string str_1 = Help_Form.drReturn[i]["Item_Code"].ToString();
                    string str_2 = Help_Form.drReturn[i]["Item_Name"].ToString();
                    gv_MatPoM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                    gv_MatPoM.SetRowCellValue(iRow, "Item_Code", str_1);
                    gv_MatPoM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                    gv_MatPoM.SetRowCellValue(iRow, "Item_Name", str_2);

                    gv_MatPoM.AddNewRow();
                    gv_MatPoM.UpdateCurrentRow();
                    iRow++;
                }
                if (!string.IsNullOrWhiteSpace(Help_Form.sRtCode))
                {
                    gv_MatPoM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                    gv_MatPoM.SetRowCellValue(iRow, "Item_Code", Help_Form.sRtCode);
                    gv_MatPoM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                    gv_MatPoM.SetRowCellValue(iRow, "Item_Name", Help_Form.sRtCodeNm);
                }
            }
        }

        private void gc_MatPoM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && gv_MatPoM.FocusedColumn == gv_MatPoM.Columns["Item_Code"])
            {
                Item_Code_Click(sender, null);
            }
        }

        private void gv_MatPoM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Qty" || e.Column.FieldName == "P_Price")
            {
                decimal qty = 0;
                decimal price = 0;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("Qty"))))
                    qty = Convert.ToDecimal(gv_MatPoM.GetFocusedRowCellValue("Qty"));

                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("P_Price"))))
                    price = Convert.ToDecimal(gv_MatPoM.GetFocusedRowCellValue("P_Price"));

                decimal supply_amt = qty * price;
                decimal vat_amt = (Convert.ToString(check_Vat.EditValue) == "Y") ? (supply_amt / 10) : 0;
                decimal tot_amt = supply_amt + vat_amt;

                gv_MatPoM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);

                gv_MatPoM.SetFocusedRowCellValue("Amt", supply_amt);
                gv_MatPoM.SetFocusedRowCellValue("Vat_Amt", vat_amt);
                gv_MatPoM.SetFocusedRowCellValue("Tot_Amt", tot_amt);

                gv_MatPoM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
            }
            else if (e.Column.FieldName == "Item_Name")
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("Item_Name"))))
                {
                    SqlParam sp = new SqlParam("sp_regMatPo");
                    sp.AddParam("Kind", "F");
                    sp.AddParam("Item_Code", Convert.ToString(gv_MatPoM.GetFocusedRowCellValue("Item_Code")));

                    ReturnStruct temp_rst = DbHelp.Proc_Search(sp);
                    if(temp_rst.ReturnChk == 0)
                    {
                        if(temp_rst.ReturnDataSet != null && temp_rst.ReturnDataSet.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = (DbHelp.Fill_Table(temp_rst.ReturnDataSet.Tables[0])).Rows[0];

                            gv_MatPoM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                            gv_MatPoM.SetFocusedRowCellValue("Item_Name", row["Item_Name"]);
                            gv_MatPoM.SetFocusedRowCellValue("Item_Memo", row["Item_Memo"]);
                            gv_MatPoM.SetFocusedRowCellValue("Ssize", row["Ssize"]);
                            gv_MatPoM.SetFocusedRowCellValue("Q_Unit", row["Q_Unit"]);
                            gv_MatPoM.SetFocusedRowCellValue("Q_Unit_Name", row["Q_Unit_Name"]);
                            gv_MatPoM.SetFocusedRowCellValue("P_Price", row["P_Price"]); 
                            if (!string.IsNullOrWhiteSpace(gv_MatPoM.GetFocusedRowCellValue("Qty").NullString()))
                            {
                                decimal qty = Convert.ToDecimal(gv_MatPoM.GetFocusedRowCellValue("Qty").NullString());
                                decimal price = Convert.ToDecimal(row["P_Price"].NumString());
                                gv_MatPoM.SetFocusedRowCellValue("Amt", qty * price);
                            }
                            gv_MatPoM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                        }
                    }
                    else
                    {
                        XtraMessageBox.Show(temp_rst.ReturnMessage);
                        return;
                    }
                }
                else
                {
                    gv_MatPoM.SetFocusedRowCellValue("Q_Unit", "");
                    gv_MatPoM.SetFocusedRowCellValue("Q_Unit_Name", "");
                    gv_MatPoM.SetFocusedRowCellValue("Item_Memo", "");
                    gv_MatPoM.SetFocusedRowCellValue("Ssize", "");
                    gv_MatPoM.SetFocusedRowCellValue("P_Price", "");
                    gv_MatPoM.SetFocusedRowCellValue("Amt", "");
                }
            }
            else if (e.Column.FieldName == "Item_Code")
            {
                string now_str = e.Value.ToString();                                      // GridColumn의 NewValue
                string old_str = (gv_MatPoM.ActiveEditor != null) ? Convert.ToString(gv_MatPoM.ActiveEditor.OldEditValue) : "";   // GridColumn의 OldValue

                if (now_str.Length < old_str.Length)
                {
                    gv_MatPoM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);
                    
                    gv_MatPoM.SetFocusedRowCellValue("Item_Code", "");
                    gv_MatPoM.SetFocusedRowCellValue("Q_Unit", "");
                    gv_MatPoM.SetFocusedRowCellValue("Q_Unit_Name", "");
                    gv_MatPoM.SetFocusedRowCellValue("Item_Memo", "");
                    gv_MatPoM.SetFocusedRowCellValue("Ssize", "");
                    
                    gv_MatPoM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatPoM_CellValueChanged);

                    gv_MatPoM.SetFocusedRowCellValue("Item_Name", "");
                }
                else
                {
                    //Item_Code_Click(sender, null);
                }
            }
            Get_Total();

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }
        #endregion

        #region 공통코드 관련 이벤트

        // 거래처 코드 부분
        private void txt_CustomCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_CustomCodeNM.Text = PopHelpForm.Return_Help("sp_Help_Custom_IN", txt_CustomCode.Text);
            if(string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                txt_CustomSort.Text = "";
                txt_CustomSortNM.Text = "";
                txt_MobileNo.Text = "";
                txt_EMail.Text = "";
                txt_PUnit.Text = "";
                check_Vat.EditValue = "N";
                txt_Vat.Text = "";
            }
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
                }
            }
        }
        private void txt_CustomCodeNM_EditValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
                return;
            ReturnStruct temp_rst = new ReturnStruct();

            SqlParam sp = new SqlParam("sp_regMatPo");
            sp.AddParam("Kind", "C");
            sp.AddParam("Custom_Name", txt_CustomCodeNM.Text);

            temp_rst = DbHelp.Proc_Search(sp);

            if (temp_rst.ReturnDataSet != null && temp_rst.ReturnDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = (DbHelp.Fill_Table(temp_rst.ReturnDataSet.Tables[0])).Rows[0];

                txt_PUnit.ToolTip = Convert.ToString(row["P_Unit"]);
                txt_PUnit.Text = Convert.ToString(row["P_Unit_Name"]);
                txt_Vat.Text = Convert.ToString(row["Vat_Chk"]);
                check_Vat.EditValue = Convert.ToString(row["Vat_Ck"]);
            }
        }

        // 거래처 담당자 코드 부분
        private void txt_CustomSort_EditValueChanged(object sender, EventArgs e)
        {
            txt_CustomSortNM.Text = PopHelpForm.Return_Help("sp_Help_CustomD", txt_CustomSort.Text);
            
        }
        private void txt_CustomSortNM_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_CustomSortNM.Text))
            {
                SqlParam sp = new SqlParam("sp_Help_CustomD");
                sp.AddParam("GB", "1");
                sp.AddParam("value1", txt_CustomCode.Text);
                sp.AddParam("Basic", txt_CustomSortNM.Text);

                ReturnStruct rst = DbHelp.Proc_Search(sp);
                if (rst.ReturnDataSet.Tables[0] != null && rst.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow row = DbHelp.Fill_Table(rst.ReturnDataSet.Tables[0]).Rows[0];

                    txt_MobileNo.Text = Convert.ToString(row["Mobile_No"]);
                    txt_EMail.Text = Convert.ToString(row["E_Mail"]);
                }
            }
            else
            {
                txt_MobileNo.Text = "";
                txt_EMail.Text = "";
            }
        }
        private void txt_CustomSort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_CustomSort_Properties_ButtonClick(sender, null);
        }
        private void txt_CustomSort_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CustomSortNM.Text))
            {
                PopHelpForm HelpForm = new PopHelpForm("Custom_Detail", "sp_Help_CustomD", txt_CustomSort.Text);
                HelpForm.Set_Value(txt_CustomCode.Text, "", "", "", "");//해당 거래처 정보
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    txt_CustomSort.Text = HelpForm.sRtCode;
                    txt_CustomSortNM.Text = HelpForm.sRtCodeNm;
                }
            }
        }

        // 발주 담당자 코드 부분
        private void txt_User_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_User.Text);
            if (!string.IsNullOrWhiteSpace(txt_UserNM.Text))
                Get_User_Info();
            else
            {
                txt_UserNM.Text = "";
                txt_Dept.ToolTip = "";
            }
        }

        private void txt_User_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_User_Properties_ButtonClick(sender, null);
        }
        private void txt_User_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_UserNM.Text))
            {
                Get_User_Info();
            }
        }

        private void Get_User_Info()
        {
            PopHelpForm Helpform = new PopHelpForm("User", "sp_Help_User", txt_User.Text, "N");
            if (Helpform.ShowDialog() == DialogResult.OK)
            {
                txt_User.EditValueChanged -= new System.EventHandler(this.txt_User_EditValueChanged);

                txt_User.Text = Helpform.sRtCode;
                txt_UserNM.Text = Helpform.sRtCodeNm;

                string[] dept = Search_Dept(txt_User.Text).Split('/');
                
                txt_Dept.Text = dept[0];
                txt_Dept.ToolTip = dept[1];

                txt_User.EditValueChanged += new System.EventHandler(this.txt_User_EditValueChanged);
            }
        }

        #endregion

        #region 기타 메소드 및 이벤트
        private void Search_Data(string code)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatPo");
                sp.AddParam("Kind", "S");
                sp.AddParam("Po_No", code);

                ret = DbHelp.Proc_Search(sp);
                ds = ret.ReturnDataSet;

                sMatpo_Memo = ds.Tables[2].Rows[0]["Matpo_Memo"].ToString();

                DataTable info_table = ds.Tables[0];
                DataTable items_table = ds.Tables[1];

                if (info_table != null && info_table.Rows.Count > 0)
                {
                    info_table = DbHelp.Fill_Table(info_table);
                    DataRow row = info_table.Rows[0];

                    txt_OrderNo.Text    = Convert.ToString(row["Po_No"]);
                    txt_RegDate.Text    = Convert.ToString(row["Reg_Date"]);
                    txt_RegUser.Text    = Convert.ToString(row["Reg_User_Name"]);
                    txt_UpDate.Text     = Convert.ToString(row["Up_Date"]);
                    txt_UpUser.Text     = Convert.ToString(row["Up_User_Name"]);

                    dt_OrderDate.Text       = Convert.ToString(row["Po_Date"]);
                    txt_CustomCode.Text     = Convert.ToString(row["Custom_Code"]);
                    txt_CustomCodeNM.Text   = Convert.ToString(row["Short_Name"]);
                    txt_CustomSort.Text     = Convert.ToString(row["Custom_Sort"]);
                    txt_CustomSortNM.Text   = Convert.ToString(row["Custom_Sort_Name"]);

                    txt_MobileNo.Text   = Convert.ToString(row["Mobile_No"]);
                    txt_EMail.Text      = Convert.ToString(row["E_Mail"]);

                    txt_User.Text       = Convert.ToString(row["User_Code"]);
                    txt_UserNM.Text     = Convert.ToString(row["User_Name"]);
                    txt_Dept.Text       = Convert.ToString(row["Dept_Name"]);
                    txt_Dept.ToolTip    = Convert.ToString(row["Dept_Code"]);

                    dt_Expected.Text    = Convert.ToString(row["In_Date"]);
                    txt_PoTitle.Text    = Convert.ToString(row["Po_Title"]);
                    txt_QuotMemo.Text   = Convert.ToString(row["Po_Memo"]);

                    txt_PUnit.Text = Convert.ToString(row["P_Unit_Name"]);
                    txt_PUnit.ToolTip = Convert.ToString(row["P_Unit"]);
                    check_Vat.EditValue = Convert.ToString(row["Vat_Ck"]);

                    txt_Vat.Text = Convert.ToString(row["Vat_Chk"]);
                }
                else
                {
                    DbHelp.Clear_Panel(panelControl3);

                    txt_QuotMemo.Text = sMatpo_Memo;
                }

                if (items_table != null && items_table.Rows.Count > 0)
                {
                    items_table = DbHelp.Fill_Table(items_table);
                }

                gc_MatPoM.DataSource = items_table;
                gc_MatPoM.RefreshDataSource();

                Get_Total();
                Button_State("Bind");
            }
            catch(Exception)
            {

            }
        }

        private void Item_Delete(object sender, EventArgs e) // 그리드 우클릭 삭제 이벤트
        {
            if (gv_MatPoM.RowCount > 0)
            {
                if (!string.IsNullOrWhiteSpace(sMatPo_No))
                {
                    string sort = gv_MatPoM.GetFocusedRowCellValue("Sort_No").ToString();

                    SqlParam sp = new SqlParam("sp_regMatPo");
                    sp.AddParam("Kind", "D");
                    sp.AddParam("Delete_Kind", "S");
                    sp.AddParam("Po_No", sMatPo_No);
                    sp.AddParam("Sort_No", sort);

                    ret = DbHelp.Proc_Save(sp);

                    if (ret.ReturnChk != 0)
                    {
                        XtraMessageBox.Show(ret.ReturnMessage);
                        return;
                    }

                    gv_MatPoM.DeleteRow(gv_MatPoM.FocusedRowHandle);
                    Save_Data();
                    btn_Delete.sCHK = "Y";
                }
            }
        }

        private void Button_State(string str)
        {
            if (str.Contains("I"))
            {
                btn_Delete.sCHK = "N";
                btn_Close.sCHK = "N";
            }

            else if (str.Contains("D"))
            {
                btn_Delete.sCHK = "Y";
                DbHelp.Clear_Panel(panel_H);
                DbHelp.Clear_Panel(panelControl3);
                txt_Supply_Amt.EditValue = 0;
            }

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private bool Check_Necessity()
        {
            if (string.IsNullOrWhiteSpace(txt_OrderNo.Text) && txt_OrderNo.Enabled)
            {
                XtraMessageBox.Show("발주번호는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(dt_OrderDate.Text))
            {
                XtraMessageBox.Show("발주일자는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                XtraMessageBox.Show("거래처는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_Dept.Text))
            {
                XtraMessageBox.Show("발주담당자는 필수 입력값입니다.");
                return false;
            }
            return true;
        }

        private string Summary_Parameter(string col)
        {
            string sum = "";

            for (int i = 0; i < gv_MatPoM.RowCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatPoM.GetRowCellValue(i,"Item_Name"))))
                {
                    if (col.Contains("Sort"))
                        sum += (i + 1).ToString() + "_/";
                    else if (col.Contains("Amt") || col.Contains("Qty") || col.Contains("Price"))
                        sum += gv_MatPoM.GetRowCellValue(i, col).NumString() + "_/";
                    else
                        sum += Convert.ToString(gv_MatPoM.GetRowCellValue(i, col)) + "_/";
                }
            }

            return sum;
        }

        private bool Save_Data()
        {
            string[] pr = new string[13];    // 0.Sort_No   1.Item_Code      2.Item_Name       3.Item_Memo      4.Ssize       5.Q_Unit      6.Q_Unit_Name       7.Qty     8.P_Price     9.Amt     10.Vat_Amt     11.Tot_Amt     12.Po_Bigo
            int n = 0;

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv_MatPoM.Columns)
            {
                pr[n] = Summary_Parameter(col.FieldName);
                n++;
            }

            SqlParam sp = new SqlParam("sp_regMatPo");
            sp.AddParam("Kind", "I");
            // MatPo_M
            sp.AddParam("Po_No", txt_OrderNo.Text);
            sp.AddParam("Po_Date", dt_OrderDate.Text.Replace("-", ""));
            sp.AddParam("Custom_Code", txt_CustomCode.Text);
            sp.AddParam("Custom_Sort", txt_CustomSort.Text);
            sp.AddParam("User_Code", txt_User.Text);
            sp.AddParam("Dept_Code", txt_Dept.ToolTip);
            sp.AddParam("Vat_Ck", Convert.ToString(check_Vat.EditValue));
            sp.AddParam("P_Unit", txt_PUnit.ToolTip);
            sp.AddParam("Po_Title", txt_PoTitle.Text);
            sp.AddParam("Po_Memo", txt_QuotMemo.Text);
            sp.AddParam("In_Date", dt_Expected.Text.Replace("-", ""));
            sp.AddParam("Po_Part", "000");
            sp.AddParam("Reg_User", GlobalValue.sUserID);
            sp.AddParam("Up_User", GlobalValue.sUserID);

            // MatPo_S

            sp.AddParam("Sort_No", pr[0]);
            sp.AddParam("Item_Code", pr[1]);
            sp.AddParam("Q_Unit", pr[5]);
            sp.AddParam("Qty", pr[7]);
            sp.AddParam("P_Price", pr[8]);
            sp.AddParam("Amt", pr[9]);
            sp.AddParam("Vat_Amt", pr[10]);
            sp.AddParam("Tot_Amt", pr[11]);
            sp.AddParam("Po_Bigo", pr[12]);

            ret = DbHelp.Proc_Save(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return false;
            }

            txt_OrderNo.Text = Convert.ToString(ret.ReturnDataSet.Tables[0].Rows[0]["Po_No"]);
            sMatPo_No = txt_OrderNo.Text;
            Search_Data(sMatPo_No);
            
            return true;
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

        private void Get_Total()
        {
            decimal total = 0;

            for (int i = 0; i < gv_MatPoM.RowCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatPoM.GetRowCellValue(i, "Amt"))))
                    total += Convert.ToDecimal(gv_MatPoM.GetRowCellValue(i, "Amt"));
            }

            txt_Supply_Amt.EditValue = total;
        }



        #endregion

        private void btn_Print_Click(object sender, EventArgs e)
        {
            if (txt_OrderNo.Text == "")
            {
                XtraMessageBox.Show("발주를 선택해주세요");
                return;
            }

            try
            {
                SqlParam sp = new SqlParam("sp_regMatPo");
                sp.AddParam("Kind", "P");
                sp.AddParam("PO_No", txt_OrderNo.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk == 0)
                {
                    Report_Matpo report = new Report_Matpo(ret.ReturnDataSet, "Y");
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
    }
}
