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
    public partial class regMatIn : BaseReg
    {
        private string sMatIn_No = "";
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        private bool bCopy = true;
        private string sCopyItemCode = "";

        public regMatIn()
        {
            InitializeComponent();
        }

        private void regMatIn_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
            txt_User.Text = GlobalValue.sUserID;
            ForMat.sBasic_Set(this.Name, txt_InNo);

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";

            dt_EnterDate.Focus();
        }

        private void Grid_Set()
        {
            //MotIn_M 그리드

            DbHelp.GridSet(gc_MatInM, gv_MatInM, "In_Sort", "DB 순번", "", false, false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Sort_No", "정렬 순번", "", false, false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Po_No", "발주번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Memo", "Spec", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Q_Unit", "단위코드", "50", false, false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Q_Unit_Name", "단위", "50", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Old_Qty", "기입고수량", "80", true, false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Qty", "수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Added_Qty", "추가 수량", "80", true, true, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "P_Price", "단가", "100", true, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "입고금액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Vat_Amt", "부가세액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Tot_Amt", "합계금액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "MatSer_No", "시리얼넘버", "120", true, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Loc_CodeNM", "Location", "100", false, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Loc_Code", "로케이션코드", "120", false, false, false, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "In_Bigo", "비고", "150", false, true, true, true);

            DbHelp.GridColumn_Help(gv_MatInM, "Po_No", "Y");
            RepositoryItemButtonEdit btn_edit = (RepositoryItemButtonEdit)gv_MatInM.Columns["Po_No"].ColumnEdit;
            btn_edit.Buttons[0].Click += new EventHandler(Item_Code_Click);

            gv_MatInM.Columns["Po_No"].ColumnEdit = btn_edit;
            gc_MatInM.DeleteRowEventHandler += new EventHandler(Item_Delete); // 그리드 우클릭 삭제

            DbHelp.GridColumn_Help(gv_MatInM, "Loc_CodeNM", "Y");
            RepositoryItemButtonEdit btn_Loc = (RepositoryItemButtonEdit)gv_MatInM.Columns["Loc_CodeNM"].ColumnEdit;
            btn_Loc.Buttons[0].Click += new EventHandler(Help_Loc);

            gv_MatInM.Columns["Loc_CodeNM"].ColumnEdit = btn_Loc;

            DbHelp.GridColumn_NumSet(gv_MatInM, "Qty", ForMat.SetDecimal("regMatIn", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "P_Price", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Amt", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Vat_Amt", ForMat.SetDecimal("regMatIn", "Price1"));
            DbHelp.GridColumn_NumSet(gv_MatInM, "Tot_Amt", ForMat.SetDecimal("regMatIn", "Price1"));

            txt_Supply_Amt.Properties.Mask.EditMask = "n" + ForMat.SetDecimal("regMatIn", "Price1");
            txt_Vat_Amt.Properties.Mask.EditMask = "n" + ForMat.SetDecimal("regMatIn", "Price1");
            txt_Tot_Amt.Properties.Mask.EditMask = "n" + ForMat.SetDecimal("regMatIn", "Price1");

            gc_MatInM.AddRowYN = true;
            gv_MatInM.OptionsView.ShowAutoFilterRow = false;

            //엑세 붙여넣기 자동 행 추가로 처리되도록 처리
            gc_MatInM.Execl_GB = GridControlEx.Excel_GB.Append;
        }

        #region 공통코드 관련 이벤트 및 메소드

        //거래처
        private void txt_CustomCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_CustomCodeNM.Text = PopHelpForm.Return_Help("sp_Help_Custom_IN", txt_CustomCode.Text);
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
            PopHelpForm HelpForm = new PopHelpForm("WHouse", "sp_Help_Whouse", txt_WHouse.Text, "N");

            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_WHouse.Text = HelpForm.sRtCode;
                txt_WHouseNM.Text = HelpForm.sRtCodeNm;
            }
        }

        // 담당자
        private void txt_User_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserCodeNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_User.Text);
            if (!string.IsNullOrWhiteSpace(txt_UserCodeNM.Text))
                Get_User_Info();
            else
            {
                txt_UserCodeNM.Text = "";
                txt_Dept.ToolTip = "";
                txt_Dept.Text = "";
            }
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
                txt_UserCodeNM.Text = Helpform.sRtCodeNm;

                string[] dept = Search_Dept(txt_User.Text).Split('/');

                txt_Dept.Text = dept[0];
                txt_Dept.ToolTip = dept[1];

                txt_User.EditValueChanged += new System.EventHandler(this.txt_User_EditValueChanged);
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
            PopHelpForm HelpForm = new PopHelpForm("MatIn", "sp_Help_MatIn", "", "N");
            HelpForm.sLevelYN = "Y";
            HelpForm.sNotReturn = "Y";
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                sMatIn_No = HelpForm.sRtCode;
                Search_Data(sMatIn_No);

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            if(btn_Insert.Result_Update == DialogResult.Yes)
            {
                btn_Save_Click(null, null);
            }
            
            //if (btn_Save.sCHK == "Y")
            //{
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panelControl3);
            Search_Data();
            txt_User.Text = GlobalValue.sUserID;
            sMatIn_No = null;

                
                //Button_State("Initialize");
            btn_Save.sCHK = "N";
            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";

            dt_EnterDate.Select();
            ForMat.sBasic_Set(this.Name, txt_InNo);
            //}
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            DataTable initialized = (gc_MatInM.DataSource as DataTable).Clone();

            if (string.IsNullOrWhiteSpace(txt_InNo.Text))
            {
                gc_MatInM.DataSource = initialized;
                gc_MatInM.RefreshDataSource();

                //Button_State("Delete");
            }
            else
            {
                SqlParam sp = new SqlParam("sp_regMatIn");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_Kind", "M");
                sp.AddParam("Po_No", Summary_Parameter("Po_No"));
                sp.AddParam("In_Sort", Summary_Parameter("In_Sort"));
                sp.AddParam("Item_Code", Summary_Parameter("Item_Code"));
                sp.AddParam("In_No", txt_InNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk == 0)
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panelControl3);
                    txt_Supply_Amt.EditValue = 0;
                    txt_Vat_Amt.EditValue = 0;
                    txt_Tot_Amt.EditValue = 0;

                    gc_MatInM.DataSource = initialized;
                    gc_MatInM.RefreshDataSource();

                    txt_User.Text = GlobalValue.sUserID;
                    sMatIn_No = null;

                    //Button_State("Delete");
                    ForMat.sBasic_Set(this.Name, txt_InNo);

                    btn_Delete.sCHK = "Y";
                }
                else
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }
            btn_Delete.sCHK = "Y";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (Check_Necessity() && gv_MatInM.RowCount > 0) // 필수 입력값 모두 존재
            {
                if (Save_Data() == false)
                    return;
                //Button_State("Initialize");
                btn_Save.sCHK = "Y";
            }
            else
            {
                btn_Save.sCHK = "N";
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_MatInM, "입고등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            if (btn_Close.Result_Update == DialogResult.Yes)
            {
                btn_Save_Click(null, null);
            }

            btn_Close.sUpdate = "N";

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

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);
            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        #endregion

        #region 기타 메소드 및 이벤트
        private void Search_Data(string code = "")
        {
            SqlParam sp = new SqlParam("sp_regMatIn");
            sp.AddParam("Kind", "S");
            sp.AddParam("In_No", code);

            ret = DbHelp.Proc_Search(sp);
            ds = ret.ReturnDataSet;

            DataTable info_table = ds.Tables[0];
            DataTable items_table = ds.Tables[1];

            if (info_table != null && info_table.Rows.Count > 0)
            {
                info_table = DbHelp.Fill_Table(info_table);
                DataRow row = info_table.Rows[0];

                txt_InNo.Text = Convert.ToString(row["In_No"]);
                txt_RegDate.Text = Convert.ToString(row["Reg_Date"]);
                txt_RegUser.Text = Convert.ToString(row["Reg_User_Name"]);
                txt_UpDate.Text = Convert.ToString(row["Up_Date"]);
                txt_UpUser.Text = Convert.ToString(row["Up_User_Name"]);

                dt_EnterDate.Text = Convert.ToString(row["In_Date"]).Substring(0, 10);
                txt_CustomCode.Text = Convert.ToString(row["Custom_Code"]);
                txt_CustomCodeNM.Text = Convert.ToString(row["Short_Name"]);

                txt_UserCodeNM.Text = Convert.ToString(row["User_Name"]);
                txt_User.Text = Convert.ToString(row["User_Code"]);
                txt_Dept.Text = Convert.ToString(row["Dept_Name"]);
                txt_Dept.ToolTip = Convert.ToString(row["Dept_Code"]);

                dt_Expected.Text = Convert.ToString(row["Pay_Date"]);
                txt_MatInMemo.Text = Convert.ToString(row["In_Memo"]);

                txt_PayNM.Text = Convert.ToString(row["Pay_Code_Name"]);
                txt_Pay.Text = Convert.ToString(row["Pay_Code"]);

                txt_WHouseNM.Text = Convert.ToString(row["Whouse_Name"]);
                txt_WHouse.Text = Convert.ToString(row["Whouse_Code"]);

                txt_PUnit.Text = Convert.ToString(row["P_Unit_Name"]);
                txt_PUnit.ToolTip = Convert.ToString(row["P_Unit"]);
                check_Vat.EditValue = Convert.ToString(row["Vat_Ck"]);
            }
            else
            {
                DbHelp.Clear_Panel(panelControl3);
            }

            if (items_table != null && items_table.Rows.Count > 0)
            {
                items_table = DbHelp.Fill_Table(items_table);
            }

            gc_MatInM.DataSource = items_table;
            gc_MatInM.RefreshDataSource();

            Get_Total();
            //Button_State("Bind");
        }
        private bool Save_Data()
        {
            string[] pr = new string[20];    // 0.In_Sort   1.Sort_No    2.Po_No      3.Item_Code       4.Item_Name      5.Q_Unit       6.Old_Qty       7.Qty       8.Added_Qty     9.P_Price     10.Amt     11.Vat_Amt    12.Tot_Amt     13.MatSer_No     14.In_Bigo
            int n = 0;

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gv_MatInM.Columns)
            {
                pr[n] = Summary_Parameter(col.FieldName);
                n++;
            }

            SqlParam sp = new SqlParam("sp_regMatIn");

            // MatIn_M
            sp.AddParam("Kind", "I");
            sp.AddParam("In_No", txt_InNo.Text);
            sp.AddParam("In_Date", dt_EnterDate.Text.Replace("-",""));
            sp.AddParam("Custom_Code", txt_CustomCode.Text);
            sp.AddParam("Whouse_Code", txt_WHouse.Text);
            sp.AddParam("Pay_Code", txt_Pay.Text);
            sp.AddParam("User_Code", txt_User.Text);
            sp.AddParam("Dept_Code", txt_Dept.ToolTip);
            sp.AddParam("Vat_Ck", Convert.ToString(check_Vat.EditValue));
            sp.AddParam("P_Unit", txt_PUnit.ToolTip);
            sp.AddParam("In_Memo", txt_MatInMemo.Text);
            sp.AddParam("Pay_Date", dt_Expected.Text.Replace("-", ""));
            sp.AddParam("PoIn_Part", "000");
            sp.AddParam("Reg_User", GlobalValue.sUserID);
            sp.AddParam("Up_User", GlobalValue.sUserID);

            // MatIn_S
            sp.AddParam("In_Sort", pr[0]);
            sp.AddParam("Sort_No", pr[1]);
            sp.AddParam("Po_No", pr[2]);
            sp.AddParam("Item_Code", pr[3]);
            sp.AddParam("Q_Unit", pr[7]);
            sp.AddParam("Qty", pr[10]);
            sp.AddParam("Added_Qty", pr[11]);
            sp.AddParam("P_Price", pr[12]);
            sp.AddParam("Amt", pr[13]);
            sp.AddParam("Vat_Amt", pr[14]);
            sp.AddParam("Tot_Amt", pr[15]);
            sp.AddParam("MatSer_No", pr[16]);
            sp.AddParam("Loc_Code", pr[18]);
            sp.AddParam("In_Bigo", pr[19]);

            ret = DbHelp.Proc_Save(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return false;
            }

            txt_InNo.Text = Convert.ToString(ret.ReturnDataSet.Tables[0].Rows[0]["In_No"]);
            sMatIn_No = txt_InNo.Text;
            Search_Data(sMatIn_No);

            return true;
        }

        private void Item_Code_Click(object sender, EventArgs e) // 수정 필요
        {
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                XtraMessageBox.Show("거래처를 우선 선택해주시길 바랍니다.");
                return;
            }
            // 기존 로우는 품목 수정 불가하도록 수정
            int iRow = gv_MatInM.GetFocusedDataSourceRowIndex();
            PopHelpForm Help_Form = new PopHelpForm("MatNotIn", "sp_Help_Item_In", Convert.ToString(gv_MatInM.GetRowCellValue(iRow, "Po_No")), "Y");
            Help_Form.Set_Value(txt_CustomCode.Text, "", "", "", "");

            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                for (int i = 0; i < end; i++)
                {
                    gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                    gv_MatInM.SetRowCellValue(iRow, "Po_No", Convert.ToString(Help_Form.drReturn[i]["Po_No"]));
                    gv_MatInM.SetRowCellValue(iRow, "Item_Code", Convert.ToString(Help_Form.drReturn[i]["Item_Code"]));
                    gv_MatInM.SetRowCellValue(iRow, "Item_Name", Convert.ToString(Help_Form.drReturn[i]["Item_Name"]));
                    gv_MatInM.SetRowCellValue(iRow, "Q_Unit", Convert.ToString(Help_Form.drReturn[i]["Q_Unit"]));
                    gv_MatInM.SetRowCellValue(iRow, "Q_Unit_Name", Convert.ToString(Help_Form.drReturn[i]["Q_Unit_Name"]));
                    gv_MatInM.SetRowCellValue(iRow, "Item_Memo", Convert.ToString(Help_Form.drReturn[i]["Item_Memo"]));
                    gv_MatInM.SetRowCellValue(iRow, "Ssize", Convert.ToString(Help_Form.drReturn[i]["Ssize"]));

                    gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                    gv_MatInM.SetRowCellValue(iRow, "Qty", Convert.ToString(Help_Form.drReturn[i]["Qty"]));
                    gv_MatInM.SetRowCellValue(iRow, "P_Price", Convert.ToString(Help_Form.drReturn[i]["P_Price"]));

                    gv_MatInM.AddNewRow();
                    gv_MatInM.UpdateCurrentRow();
                    iRow++;
                }
                if (!string.IsNullOrWhiteSpace(Help_Form.sRtCode))
                {
                    gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                    gv_MatInM.SetRowCellValue(iRow, "Po_No", Help_Form.sRtCode);
                    gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                    gv_MatInM.SetRowCellValue(iRow, "Item_Code", Help_Form.sRtCodeNm);
                }
            }
        }

        private void Help_Loc(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_WHouseNM.Text))
            {
                XtraMessageBox.Show("입고창고를 먼저 입력하세요");
                return;
            }

            int iRow = gv_MatInM.GetFocusedDataSourceRowIndex();

            if(string.IsNullOrWhiteSpace(gv_MatInM.GetRowCellValue(iRow, "Loc_Code").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("Loc", "sp_Help_Loc", gv_MatInM.GetRowCellValue(iRow, "Loc_Code").ToString(), "N");
                HelpForm.Set_Value(txt_WHouse.Text, "", "", "", "");
                if(HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_MatInM.SetRowCellValue(iRow, "Loc_Code", HelpForm.sRtCode);
                    gv_MatInM.SetRowCellValue(iRow, "Loc_CodeNM", HelpForm.sRtCodeNm);
                }
            }
        }

        private void Item_Delete(object sender, EventArgs e) // 그리드 우클릭 삭제 이벤트
        {
            if (gv_MatInM.RowCount > 0)
            {
                if (!string.IsNullOrWhiteSpace(sMatIn_No))
                {
                    string sort = Convert.ToString(gv_MatInM.GetFocusedRowCellValue("In_Sort"));
                    string item_code = Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Item_Code"));

                    if (!string.IsNullOrWhiteSpace(sort))
                    {
                        SqlParam sp = new SqlParam("sp_regMatIn");
                        sp.AddParam("Kind", "D");
                        sp.AddParam("Delete_Kind", "S");
                        sp.AddParam("Po_No", Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Po_No")));
                        sp.AddParam("In_No", sMatIn_No);
                        sp.AddParam("In_Sort", sort);
                        sp.AddParam("Item_Code", item_code);
                        sp.AddParam("Up_User", GlobalValue.sUserID);

                        ret = DbHelp.Proc_Save(sp);

                        if (ret.ReturnChk != 0)
                        {
                            XtraMessageBox.Show(ret.ReturnMessage);
                            return;
                        }

                        gv_MatInM.DeleteRow(gv_MatInM.FocusedRowHandle);


                        //sp = new SqlParam("sp_regMatIn");
                        //sp.AddParam("Kind", "I");
                        //sp.AddParam("In_No", txt_InNo.Text);
                        //sp.AddParam("In_Date", dt_EnterDate.EditValue);
                        //sp.AddParam("Custom_Code", txt_CustomCode.Text);
                        //sp.AddParam("Whouse_Code", txt_WHouse.Text);
                        //sp.AddParam("Pay_Code", txt_Pay.Text);
                        //sp.AddParam("User_Code", txt_User.Text);
                        //sp.AddParam("Dept_Code", txt_Dept.ToolTip);
                        //sp.AddParam("Vat_Ck", "Y");  // 현재 해당 데이터를 저장하는 텍스트에디트가 존재하지 않음
                        //sp.AddParam("P_Unit", "");   // 현재 해당 데이터를 저장하는 텍스트에디트가 존재하지 않음
                        //sp.AddParam("In_Memo", txt_MatInMemo.Text);
                        //sp.AddParam("Pay_Date", dt_Expected.EditValue);
                        //sp.AddParam("PoIn_Part", "000");
                        //sp.AddParam("Up_User", GlobalValue.sUserID);

                        //ret = DbHelp.Proc_Save(sp);

                        //if (ret.ReturnChk != 0)
                        //{
                        //    XtraMessageBox.Show(ret.ReturnMessage);
                        //    return;
                        //}

                        // btn_Delete.sCHK = "Y";
                        //XtraMessageBox.Show("삭제되었습니다.");
                    }
                }
            }
        }

        private bool Check_Necessity()
        {
            if (string.IsNullOrWhiteSpace(txt_InNo.Text) && txt_InNo.Enabled)
            {
                XtraMessageBox.Show("입고번호는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(dt_EnterDate.Text))
            {
                XtraMessageBox.Show("입고일자는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                XtraMessageBox.Show("거래처는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_UserCodeNM.Text))
            {
                XtraMessageBox.Show("입고담당자는 필수 입력값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_PayNM.Text))
            {
                XtraMessageBox.Show("결제조건은 필수 입력값입니다.");
                return false;
            }
            return true;
        }

        private string Summary_Parameter(string col)
        {
            string sum = "";

            for (int i = 0; i < gv_MatInM.RowCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatInM.GetRowCellValue(i, "Item_Name"))))
                {
                    if (col == "Sort_No")
                        sum += (i + 1).ToString() + "_/";
                    else if (col == "Qty")
                    {
                        decimal old_qty = Convert.ToDecimal(gv_MatInM.GetRowCellValue(i, "Old_Qty").NumString());
                        decimal new_qty = Convert.ToDecimal(gv_MatInM.GetRowCellValue(i, "Qty").NumString());

                        sum += Convert.ToString(new_qty) + "_/";
                    }
                    else if (col == "Added_Qty")
                    {
                        decimal old_qty = Convert.ToDecimal(gv_MatInM.GetRowCellValue(i, "Old_Qty").NumString());
                        decimal new_qty = Convert.ToDecimal(gv_MatInM.GetRowCellValue(i, "Qty").NumString());

                        sum += Convert.ToString(new_qty - old_qty) + "_/";
                    }
                    else if (col.Contains("Amt") || col.Contains("_Qty") || col.Contains("Price"))
                        sum += gv_MatInM.GetRowCellValue(i, col).NumString() + "_/";
                    else
                        sum += Convert.ToString(gv_MatInM.GetRowCellValue(i, col)) + "_/";
                }
            }

            return sum;
        }

        private void Get_Total()
        {
            decimal total = 0;

            for (int i = 0; i < gv_MatInM.RowCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatInM.GetRowCellValue(i, "Amt"))))
                    total += Convert.ToDecimal(gv_MatInM.GetRowCellValue(i, "Amt"));
            }

            txt_Supply_Amt.EditValue = total;
            txt_Vat_Amt.EditValue = total / 10;
            txt_Tot_Amt.EditValue = total + (total / 10);
        }
        #endregion

        #region 그리드 이벤트

        private void gv_MatInM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Qty" || e.Column.FieldName == "P_Price")
            {
                decimal qty = 0;
                decimal price = 0;

                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Qty"))))
                    qty = Convert.ToDecimal(gv_MatInM.GetFocusedRowCellValue("Qty"));

                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatInM.GetFocusedRowCellValue("P_Price"))))
                    price = Convert.ToDecimal(gv_MatInM.GetFocusedRowCellValue("P_Price"));

                decimal supply_amt = qty * price;
                decimal vat_amt = supply_amt / 10;
                decimal tot_amt = supply_amt + vat_amt;

                gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);

                gv_MatInM.SetRowCellValue(e.RowHandle, "Amt", supply_amt);
                gv_MatInM.SetRowCellValue(e.RowHandle, "Vat_Amt", vat_amt);
                gv_MatInM.SetRowCellValue(e.RowHandle, "Tot_Amt", tot_amt);

                gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
            }
            else if (e.Column.FieldName == "Po_No")
            {
                if (!bCopy) //엑셀에서 복사 붙여넣기 햇을경우
                {
                    gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Code", sCopyItemCode);
                }
                else
                {
                    string now_str = e.Value.ToString();                            // GridColumn의 NewValue
                    string old_str = (gv_MatInM.ActiveEditor != null) ? Convert.ToString(gv_MatInM.ActiveEditor.OldEditValue) : "";   // GridColumn의 OldValue

                    if (now_str.Length < old_str.Length)
                    {
                        gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);

                        gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Code", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Name", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Q_Unit", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Q_Unit_Name", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Memo", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Ssize", "");

                        gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);

                        gv_MatInM.SetRowCellValue(e.RowHandle, "Qty", "");
                        gv_MatInM.SetRowCellValue(e.RowHandle, "P_Price", "");
                    }
                    else
                    {
                        int iRow = e.RowHandle;
                        PopHelpForm Help_Form = new PopHelpForm("MatNotIn", "sp_Help_Item_In", Convert.ToString(e.Value), "Y");
                        Help_Form.Set_Value(txt_CustomCode.Text, "", "", "", "");
                        if (Help_Form.ShowDialog() == DialogResult.OK)
                        {
                            int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                            for (int i = 0; i < end; i++)
                            {
                                gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                                gv_MatInM.SetRowCellValue(iRow, "Po_No", Convert.ToString(Help_Form.drReturn[i]["Po_No"]));
                                gv_MatInM.SetRowCellValue(iRow, "Item_Code", Convert.ToString(Help_Form.drReturn[i]["Item_Code"]));
                                gv_MatInM.SetRowCellValue(iRow, "Item_Name", Convert.ToString(Help_Form.drReturn[i]["Item_Name"]));
                                gv_MatInM.SetRowCellValue(iRow, "Q_Unit", Convert.ToString(Help_Form.drReturn[i]["Q_Unit"]));
                                gv_MatInM.SetRowCellValue(iRow, "Q_Unit_Name", Convert.ToString(Help_Form.drReturn[i]["Q_Unit_Name"]));
                                gv_MatInM.SetRowCellValue(iRow, "Item_Memo", Convert.ToString(Help_Form.drReturn[i]["Item_Memo"]));
                                gv_MatInM.SetRowCellValue(iRow, "Ssize", Convert.ToString(Help_Form.drReturn[i]["Ssize"]));
                                gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);

                                gv_MatInM.SetRowCellValue(iRow, "Qty", Convert.ToString(Help_Form.drReturn[i]["Qty"]));
                                gv_MatInM.SetRowCellValue(iRow, "P_Price", Convert.ToString(Help_Form.drReturn[i]["P_Price"]));
                                gv_MatInM.UpdateCurrentRow();
                                iRow++;
                            }
                            if (!string.IsNullOrWhiteSpace(Help_Form.sRtCode))
                            {
                                gv_MatInM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                                gv_MatInM.SetRowCellValue(iRow, "Po_No", Help_Form.sRtCode);
                                gv_MatInM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gv_MatInM_CellValueChanged);
                                gv_MatInM.SetRowCellValue(iRow, "Item_Code", Help_Form.sRtCodeNm);
                            }
                        }
                    }
                }
            }
            else if (e.Column.FieldName == "Item_Code")
            {
                if (!string.IsNullOrWhiteSpace(Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Po_No"))))
                {
                    try
                    {
                        SqlParam sp = new SqlParam("sp_regMatIn");
                        sp.AddParam("Kind", "C");
                        sp.AddParam("Po_No", Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Po_No")));
                        sp.AddParam("Item_Code", Convert.ToString(gv_MatInM.GetFocusedRowCellValue("Item_Code")));

                        ReturnStruct temp_rst = DbHelp.Proc_Search(sp);

                        if (temp_rst.ReturnChk != 0)
                        {
                            XtraMessageBox.Show(temp_rst.ReturnMessage);
                            return;
                        }

                        if (temp_rst.ReturnDataSet.Tables[0].Rows.Count > 0)
                        {
                            DataRow row = (DbHelp.Fill_Table(temp_rst.ReturnDataSet.Tables[0])).Rows[0];
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Name", row["Item_Name"].ToString());
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Q_Unit", row["Q_Unit"].ToString());
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Q_Unit_Name", Convert.ToString(row["Q_Unit_Name"]));
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Memo", Convert.ToString(row["Item_Memo"]));
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Ssize", Convert.ToString(row["Ssize"]));
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Qty", Convert.ToString(row["Qty"]));
                            gv_MatInM.SetRowCellValue(e.RowHandle, "P_Price", Convert.ToString(row["P_Price"]));
                        }
                        else
                        {
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Po_No", "");
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Item_Code", "");
                            XtraMessageBox.Show("발주번호, 품목 중 잘못 입력된 자료가 존재합니다");
                            return;
                        }
                    }
                    catch(Exception ex)
                    {
                        XtraMessageBox.Show(ex.Message);
                        return;
                    }
                }
            }
            else if (e.Column.FieldName == "Loc_CodeNM")
            {
                if (!bCopy) //엑셀에서 복사 붙여넣기 햇을경우
                {
                    if (string.IsNullOrWhiteSpace(gv_MatInM.GetRowCellValue(e.RowHandle, "Item_Code").ToString()))
                    {
                        gv_MatInM.DeleteRow(e.RowHandle);
                        return;
                    }

                }

                string sLoc_Code = gv_MatInM.GetRowCellValue(e.RowHandle, "Loc_Code").ToString();
                string sLoc_CodeNM = "";

                if (string.IsNullOrWhiteSpace(sLoc_Code))
                {
                    sLoc_CodeNM = PopHelpForm.Return_Help("sp_Help_Loc", e.Value.ToString(), "", txt_WHouse.Text);
                    if (!string.IsNullOrWhiteSpace(sLoc_CodeNM))
                    {
                        if (sLoc_CodeNM.Contains("*"))
                        {
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_Code", sLoc_CodeNM.Replace("*", ""));
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_CodeNM", e.Value.ToString());
                        }
                        else
                        {
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_Code", e.Value.ToString());
                            gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_CodeNM", sLoc_CodeNM);
                        }
                    }
                }
                else
                {
                    sLoc_CodeNM = PopHelpForm.Return_Help("sp_Help_Loc", sLoc_Code, "", txt_WHouse.Text).Replace("*", "");
                    if (sLoc_CodeNM != e.Value.ToString())
                    {
                        gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_Code", "");
                    }
                }
                //string sLoc_CodeNM = PopHelpForm.Return_Help("sp_Help_Loc", e.Value.ToString(), "", txt_WHouse.Text);
                //gv_MatInM.SetRowCellValue(e.RowHandle, "Loc_CodeNM", sLoc_CodeNM);
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_MatInM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            string code = Convert.ToString(gv_MatInM.GetFocusedRowCellValue("In_Sort"));
            if (!string.IsNullOrWhiteSpace(code))
                gv_MatInM.Columns["Po_No"].OptionsColumn.AllowEdit = false;
            else
                gv_MatInM.Columns["Po_No"].OptionsColumn.AllowEdit = true;
        }

        private void gc_MatInM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (gv_MatInM.FocusedColumn == gv_MatInM.Columns["Po_No"])
                {
                    Item_Code_Click(sender, null);
                }
                else if(gv_MatInM.FocusedColumn == gv_MatInM.Columns["Loc_CodeNM"])
                {
                    Help_Loc(null, null);
                }
            }
                
        }

        #endregion

        private void txt_CustomCodeNM_EditValueChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                SqlParam sp = new SqlParam("sp_regMatIn");
                sp.AddParam("Kind", "P");
                sp.AddParam("Short_Name", txt_CustomCodeNM.Text);

                ReturnStruct temp_ret = DbHelp.Proc_Search(sp);
                if (temp_ret.ReturnChk == 0)
                {
                    if (temp_ret.ReturnDataSet != null && temp_ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = (DbHelp.Fill_Table(temp_ret.ReturnDataSet.Tables[0])).Rows[0];
                        //txt_PayB.Text = row["Pay_Part_Name"].ToString();
                        txt_Pay.Text = row["Pay_Part"].ToString();
                        txt_PUnit.Text = row["P_Unit_Name"].ToString();
                        txt_PUnit.ToolTip = row["P_Unit"].ToString();
                    }
                }
            }
            else
            {
                txt_Pay.Text = "";
                txt_Pay.ToolTip = "";
                txt_PUnit.Text = "";
                txt_PUnit.ToolTip = "";
            }
        }

        private void gv_MatInM_ClipboardRowPasting(object sender, DevExpress.XtraGrid.Views.Grid.ClipboardRowPastingEventArgs e)
        {
            gv_MatInM.DeleteRow(e.RowHandle);

            bCopy = false;
            try
            {
                sCopyItemCode = e.Values["Item_Code"].ToString();
            }
            catch(Exception ex)
            {
                e.Cancel = true;
                XtraMessageBox.Show("발주번호를 클릭하고 붙여넣기를 해주세요");
                return;
            }
        }

        private void gv_MatInM_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control && e.KeyCode == Keys.V)
            {
                gv_MatInM.FocusedColumn = gv_MatInM.Columns["Po_No"];
            }
        }
    }
}
