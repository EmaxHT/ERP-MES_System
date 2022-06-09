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
using DevExpress.XtraGrid.Views.Grid;

namespace SERP
{
    public partial class regWorkResult1 : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        DataTable right = new DataTable();

        public regWorkResult1()
        {
            InitializeComponent();
        }
        
        private void regWorkResult1_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
            ForMat.sBasic_Set(this.Name, txt_EnterNo);

            dt_EnterDate.Focus();
        }

        private void Grid_Set()
        {
            //gc_EnterM 그리드
            /* 0 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Result_No", "순번", "", false, false, false);
            /* 1 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Order_No", "수주번호", "130", false, true, true);
            /* 2 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Item_Code", "품목코드", "120", false, false, true);
            /* 3 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Sort_No", "정렬번호", "", false, false, false);
            /* 4 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Item_Name", "품명", "150", false, false, true);
            /* 5 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ssize", "규격", "150", false, false, true);
            /* 6 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Qty", "수량", "80", true, true, true);
            /* 7 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "ItemSer_No", "Serial-No", "120", true, true, true);
            /* 8 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "ItemLic_No", "라이센스", "120", true, true, true);
            /* 9 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ver_Code", "버전코드", "100", false, false, false);
            /* 10 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Ver_Name", "버전", "100", false, true, true);
            /* 11 */DbHelp.GridSet(gc_ResultS, gv_ResultS, "Old_Qty", "기존수량", "80", true, true, false);


            // 왼쪽 그리드 버튼 에디트
            DbHelp.GridColumn_Help(gv_ResultS, "Order_No", "Y");
            RepositoryItemButtonEdit btn_edit = (RepositoryItemButtonEdit)gv_ResultS.Columns["Order_No"].ColumnEdit;
            btn_edit.Buttons[0].Click += new EventHandler(Item_Code_Click);
            gv_ResultS.Columns["Order_No"].ColumnEdit = btn_edit;
            gc_ResultS.DeleteRowEventHandler += new EventHandler(Item_Delete); // 그리드 우클릭 삭제

            RepositoryItemButtonEdit button_Help = new RepositoryItemButtonEdit();
            button_Help.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Search;
            button_Help.Buttons[0].Click += new EventHandler(Version_Click);

            gv_ResultS.Columns["Ver_Name"].ColumnEdit = button_Help;
            gv_ResultS.Columns["Ver_Name"].AppearanceCell.BackColor = Color.LightGray;
        
            // 기타 그리드 설정
            DbHelp.GridColumn_NumSet(gv_ResultS, "Qty", ForMat.SetDecimal("regWorkResult1", "Qty1"));

            gc_ResultS.AddRowYN = true;

            gv_ResultS.OptionsView.ShowAutoFilterRow = false;

            gv_ResultS.OptionsCustomization.AllowSort = false;
        }

        #region 좌측 그리드 이벤트 및 메소드
        // 좌측 그리드 Row 추가
        private void gc_ResultS_NewRowAdd(object sender, InitNewRowEventArgs e)
        {
            for (int i = 0; i < gv_ResultS.RowCount; i++)
            {
                gv_ResultS.SetRowCellValue(i, "Sort_No", (i+ 1));
            }
            gv_ResultS.UpdateSummary();
        }

        // 좌측 그리드 키다운
        private void gv_ResultM_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && gv_ResultS.FocusedColumn.FieldName == "Order_No")
                Item_Code_Click(null, null);
            else if (e.KeyCode == Keys.Enter && gv_ResultS.FocusedColumn.FieldName == "Ver_Name")
                Version_Click(null, null);
        }

        // 좌측 그리드 값 변경 이벤트
        private void gv_ResultS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Order_No")
            {
                string order_no = "";
                if (Convert.ToString(gv_ResultS.ActiveEditor.EditValue).Length < Convert.ToString(gv_ResultS.ActiveEditor.OldEditValue).Length)
                    order_no = Convert.ToString(gv_ResultS.ActiveEditor.EditValue);
                else
                    order_no = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Order_No"));

                string item_code = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Item_Code"));

                SqlParam sp = new SqlParam("sp_regWorkResult1");
                sp.AddParam("Kind", "B");
                sp.AddParam("Order_No", order_no);
                sp.AddParam("Item_Code", item_code);

                ReturnStruct temp_rst = DbHelp.Proc_Search(sp);
                
                // 해당 수주번호와 품목코드에 해당한 데이터 뿌리기
                if (temp_rst.ReturnChk != 0)
                {
                    XtraMessageBox.Show(temp_rst.ReturnMessage);
                    return;
                }
                if (temp_rst.ReturnDataSet.Tables[0].Rows.Count == 1)
                {
                    DataRow row = (DbHelp.Fill_Table(temp_rst.ReturnDataSet.Tables[0])).Rows[0];

                    gv_ResultS.SetFocusedRowCellValue("Item_Name", row["Item_Name"].ToString());
                    gv_ResultS.SetFocusedRowCellValue("Ssize", row["Ssize"].ToString());
                    gv_ResultS.SetFocusedRowCellValue("Qty", row["Not_In_Qty"].ToString());
                }
                else
                {
                    gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                    gv_ResultS.SetFocusedRowCellValue("Order_No", "");
                    gv_ResultS.SetFocusedRowCellValue("Item_Code", "");
                    gv_ResultS.SetFocusedRowCellValue("Sort_No", "");
                    gv_ResultS.SetFocusedRowCellValue("Item_Name", "");
                    gv_ResultS.SetFocusedRowCellValue("Ssize", ""); 
                    gv_ResultS.SetFocusedRowCellValue("Qty", "");
                    gv_ResultS.SetFocusedRowCellValue("ItemSer_No", "");
                    gv_ResultS.SetFocusedRowCellValue("ItemLic_No", "");
                    gv_ResultS.SetFocusedRowCellValue("Ver_Code", "");
                    gv_ResultS.SetFocusedRowCellValue("Ver_Name", "");
                    gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                }
            }
            else if(e.Column.FieldName == "Ver_Name")
            {
                int old_len = Convert.ToString(gv_ResultS.ActiveEditor.OldEditValue).Length;
                int new_len = Convert.ToString(gv_ResultS.ActiveEditor.EditValue).Length;

                if (old_len > new_len)
                {
                    gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                    gv_ResultS.SetFocusedRowCellValue("Ver_Code", "");
                    gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                }
            }
            else if(e.Column.FieldName == "Qty")
            {
                try
                {
                    // 수량 체크
                    int old_qty = Convert.ToInt32(gv_ResultS.GetFocusedRowCellValue("Old_Qty"));
                    int new_qty = Convert.ToInt32(gv_ResultS.ActiveEditor.EditValue);

                    if (new_qty > old_qty)
                    {
                        gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                        gv_ResultS.SetFocusedRowCellValue("Qty", gv_ResultS.ActiveEditor.OldEditValue);
                        gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                    }
                }
                catch(Exception)
                {

                }
            }

        }


        // 좌측 그리드 포커스 변경
        private void gv_ResultS_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            int pre_index = e.PrevFocusedRowHandle;

            string code = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Result_No"));
            if (!string.IsNullOrWhiteSpace(code))
            {
                gv_ResultS.Columns["Order_No"].OptionsColumn.AllowEdit = false;
            }
            else
            {
                gv_ResultS.Columns["Order_No"].OptionsColumn.AllowEdit = true;
            }

            string order_no = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Order_No"));
            string item_code = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Item_Code"));
            string sort_no = (Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Sort_No"))).NumString();
        }

        // 좌측 그리드 수주번호 클릭
        private void Item_Code_Click(object sender, EventArgs e) // 수정 필요
        {
            // 기존 로우는 품목 수정 불가하도록 수정
            int iRow = gv_ResultS.GetFocusedDataSourceRowIndex();
            PopHelpForm Help_Form = new PopHelpForm("MakeResult_S", "sp_Help_Item_Result", Convert.ToString(gv_ResultS.ActiveEditor.EditValue), "Y");

            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                int end = (Help_Form.drReturn == null) ? 0 : Help_Form.drReturn.Count();
                for (int i = 0; i < end; i++)
                {
                    // 수량 체크
                    if ((gc_ResultS.DataSource as DataTable).Select("Order_No = '" + Convert.ToString(Help_Form.drReturn[i]["Order_No"]) + "' AND Item_Code = '" + Convert.ToString(Help_Form.drReturn[i]["Item_Code"]) + "'").Count() > 0)
                    {
                        continue;
                    }

                    gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                    gv_ResultS.SetRowCellValue(iRow, "Order_No", Convert.ToString(Help_Form.drReturn[i]["Order_No"]));
                    gv_ResultS.SetRowCellValue(iRow, "Item_Code", Convert.ToString(Help_Form.drReturn[i]["Item_Code"]));
                    gv_ResultS.SetRowCellValue(iRow, "Item_Name", Convert.ToString(Help_Form.drReturn[i]["Item_Name"]));
                    gv_ResultS.SetRowCellValue(iRow, "Ssize", Convert.ToString(Help_Form.drReturn[i]["Ssize"]));
                    gv_ResultS.SetRowCellValue(iRow, "Qty", Convert.ToString(Help_Form.drReturn[i]["In_Qty"]));
                    gv_ResultS.SetRowCellValue(iRow, "Old_Qty", Convert.ToString(Help_Form.drReturn[i]["In_Qty"]));
                    gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);

                    gv_ResultS.AddNewRow();
                    gv_ResultS.UpdateCurrentRow();
                    iRow++;
                }
                if (!string.IsNullOrWhiteSpace(Help_Form.sRtCode))
                {
                    if ((gc_ResultS.DataSource as DataTable).Select("Order_No = '" + Help_Form.sRtCode + "' AND Item_Code = '" + Help_Form.sRtCodeNm + "'").Count() > 0)
                    {
                        return;
                    }
                    gv_ResultS.SetFocusedRowCellValue("Item_Code", Help_Form.sRtCodeNm);
                    gv_ResultS.SetFocusedRowCellValue("Order_No", Help_Form.sRtCode);
                }
            }
            
        }

        // 좌측 그리드 로우 삭제
        private void Item_Delete(object sender, EventArgs e)
        {
            if (gv_ResultS.RowCount > 0)
            {
                // right 테이블에서 우측 데이터도 삭제해주기
                if (!string.IsNullOrWhiteSpace(txt_EnterNo.Text))
                {
                    string sort_no = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Sort_No"));
                    string order_no = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Order_No"));
                    string item_code = Convert.ToString(gv_ResultS.GetFocusedRowCellValue("Item_Code"));

                    if (!string.IsNullOrWhiteSpace(sort_no))
                    {
                        SqlParam sp = new SqlParam("sp_regWorkResult1");
                        sp.AddParam("Kind", "D");
                        sp.AddParam("Delete_Kind", "S");
                        sp.AddParam("Result_No", txt_EnterNo.Text);
                        sp.AddParam("Sort_No", sort_no);
                        sp.AddParam("Order_No", order_no);
                        sp.AddParam("Item_Code", item_code);
                        sp.AddParam("Up_User", GlobalValue.sUserID);

                        ret = DbHelp.Proc_Save(sp);
                        
                        if (ret.ReturnChk != 0)
                        {
                            XtraMessageBox.Show(ret.ReturnMessage);
                            return;
                        }

                        gv_ResultS.DeleteRow(gv_ResultS.FocusedRowHandle);

                        btn_Delete.sCHK = "Y";
                        XtraMessageBox.Show("삭제되었습니다.");
                    }
                    else
                    {
                        gv_ResultS.DeleteRow(gv_ResultS.FocusedRowHandle);
                        gv_ResultS.UpdateCurrentRow();
                    }
                }
                else
                {
                    gv_ResultS.DeleteRow(gv_ResultS.FocusedRowHandle);
                    gv_ResultS.UpdateCurrentRow();
                }
            }
        }

        // 좌측 그리드 버전 클릭
        private void Version_Click(object sender, EventArgs e) // 수정 필요
        {
            // 기존 로우는 품목 수정 불가하도록 수정
            int iRow = gv_ResultS.GetFocusedDataSourceRowIndex();
            PopHelpForm Help_Form = new PopHelpForm("General", "sp_Help_General", "30040", Convert.ToString(gv_ResultS.ActiveEditor.EditValue), "N");

            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                gv_ResultS.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
                if (!string.IsNullOrWhiteSpace(Help_Form.sRtCode))
                {
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Code", Help_Form.sRtCode);
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Name", Help_Form.sRtCodeNm);
                }
                else
                {
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Code", Help_Form.sRtCode);
                    gv_ResultS.SetRowCellValue(iRow, "Ver_Name", Help_Form.sRtCodeNm);

                    gv_ResultS.UpdateCurrentRow();
                }
                gv_ResultS.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_ResultS_CellValueChanged);
            }
        }
        #endregion


        #region 버튼관련 이벤트 및 메소드
        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("MakeResult", "sp_Help_WorkResult1", "", "N");
            HelpForm.sLevelYN = "Y";
            HelpForm.sNotReturn = "Y";
            btn_Select.clsWait.CloseWait();
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                Search_Data(HelpForm.sRtCode);
                btn_Select.clsWait.CloseWait();
            }
        }

        private void Search_Data(string code = "")
        {
            SqlParam sp = new SqlParam("sp_regWorkResult1");
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
                    
                    txt_EnterNo.Text        = row["Result_No"].ToString();
                    dt_EnterDate.Text       = row["Result_Date"].ToString();

                    txt_Rout.Text           = row["Process_Code"].ToString();
                    txt_Rout_Name.Text      = row["Process_Name"].ToString();
                    txt_WareHS.Text         = row["Whouse_Code"].ToString();
                    txt_WareHS_Name.Text    = row["Whouse_Name"].ToString();
                    txt_WorkCharger.Text    = row["Work_User"].ToString();
                    txt_WorkChargerNM.Text  = row["Work_User_Name"].ToString();
                    txt_Dept.Text           = row["Dept_Name"].ToString();
                    txt_Dept.ToolTip        = row["Dept_Code"].ToString();

                    txt_RegDate.Text        = row["Reg_Date"].ToString();
                    txt_RegUser.Text        = row["Reg_User_Name"].ToString();
                    txt_UpDate.Text         = row["Up_Date"].ToString();
                    txt_UpUser.Text         = row["Up_User_Name"].ToString();
                }
                else
                {
                    DbHelp.Clear_Panel(panel_H);
                    DbHelp.Clear_Panel(panelControl3);

                    txt_WorkCharger.Text = GlobalValue.sUserID;
                    txt_WorkChargerNM.Text = GlobalValue.sUserNm;
                }
                DataTable item_2 = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[2]);
                right = item_2;


                DataTable item = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[1]);
                gc_ResultS.DataSource = item;
                gc_ResultS.RefreshDataSource();
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            Save_Data();
            Search_Data();
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_EnterNo.Text))
            {
                string[] pr = Summary_Parameter(gv_ResultS);
                int n = 0;

                SqlParam sp = new SqlParam("sp_regWorkResult1");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_Kind", "M");
                sp.AddParam("Result_No", txt_EnterNo.Text);
                sp.AddParam("Order_No", pr[1]);
                sp.AddParam("Item_Code", pr[2]);
                sp.AddParam("Sort_No", pr[3]);

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

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if(Check())
            {
                txt_EnterNo.Text = Save_Data();
                Search_Data(txt_EnterNo.Text);
                btn_Save.sCHK = "Y";
            }
        }

        private string Save_Data()
        {
            try
            {
                string[] left = Summary_Parameter(gv_ResultS);
                string[] right_data = Summary_Parameter_1(right);
                DataColumnCollection col_name = right.Columns;

                SqlParam sp = new SqlParam("sp_regWorkResult1"); 
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
                sp.AddParam("Order_No"  , left[1]);
                sp.AddParam("Item_Code" , left[2]);
                sp.AddParam("Sort_No"   , left[3]);
                sp.AddParam("Qty"       , left[6]);
                sp.AddParam("ItemSer_No", left[7]);
                sp.AddParam("ItemLic_No", left[8]);
                sp.AddParam("Ver_Code"  , left[9]);

                // WorkResult_S1
                sp.AddParam("Order_No_2"    , right_data[col_name.IndexOf("Order_No")]);
                sp.AddParam("Item_Code_2"   , right_data[col_name.IndexOf("Item_Code")]);
                sp.AddParam("Sort_No_2"     , right_data[col_name.IndexOf("Sort_No")]);
                sp.AddParam("Out_No"        , right_data[col_name.IndexOf("Out_No")]);
                sp.AddParam("In_No"         , right_data[col_name.IndexOf("In_No")]);
                sp.AddParam("In_Sort"       , right_data[col_name.IndexOf("In_Sort")]);

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
                XtraMessageBox.Show("외주처는 필수 입력값입니다.");
                return false;
            }
            return true;
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

        #region 공통코드 메소드 및 이벤트
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
                SqlParam sp = new SqlParam("sp_regWorkResult1");
                sp.AddParam("Kind", "W");
                sp.AddParam("Pr", txt_WareHS.Text);

                ret_R = DbHelp.Proc_Search(sp);
                if (ret_R.ReturnChk == 0)
                {
                    if (ret_R.ReturnDataSet.Tables[0].Rows.Count == 1)
                    {
                        txt_WareHS_Name.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["Whouse_Name"].ToString();
                        txt_WareHS.Text= ret_R.ReturnDataSet.Tables[0].Rows[0]["Whouse_Code"].ToString();
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
                SqlParam sp = new SqlParam("sp_regWorkResult1");
                sp.AddParam("Kind", "R");
                sp.AddParam("Pr", txt_Rout.Text);

                ret_R = DbHelp.Proc_Search(sp);
                if (ret_R.ReturnChk == 0)
                {
                    if (ret_R.ReturnDataSet.Tables[0].Rows.Count == 1)
                    {
                        txt_Rout_Name.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["Short_Name"].ToString();
                        txt_Rout.Text = ret_R.ReturnDataSet.Tables[0].Rows[0]["Custom_Code"].ToString();
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
            PopHelpForm HelpForm = new PopHelpForm("Custom", "sp_Help_Custom", txt_Rout.Text, "N");

            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_Rout.Text = HelpForm.sRtCode;
                txt_Rout_Name.Text = HelpForm.sRtCodeNm;
            }
        }



        private void txt_WorkCharger_EditValueChanged(object sender, EventArgs e)
        {
            //txt_Dept.Text = PopHelpForm.Return_Help("sp_Help_User", txt_WorkCharger.Text);
            if (!string.IsNullOrWhiteSpace(txt_WorkCharger.Text))
                Get_User_Info();
            else
            {
                txt_WorkCharger.ToolTip = "";
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

                string[] dept = Search_Dept(txt_WorkCharger.ToolTip).Split('/');

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
        #endregion

        #region 기타 메소드 및 이벤트
        private string[] Summary_Parameter(GridView view)
        {
            int n = 0;

            string[] ret_string = new string[(view.GridControl.DataSource as DataTable).Columns.Count];

            foreach (GridColumn column in view.Columns)
            {
                string sum = "";    

                for (int i = 0; i < view.RowCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(Convert.ToString(view.GetRowCellValue(i, "Item_Name"))))
                    {
                        if (column.FieldName == "Qty")
                        {
                            decimal old_qty = Convert.ToDecimal(view.GetRowCellValue(i, "Old_Qty").NumString());
                            decimal new_qty = Convert.ToDecimal(view.GetRowCellValue(i, "Qty").NumString());

                            sum += Convert.ToString(new_qty) + "_/";
                        }
                        else
                        {
                            sum += Convert.ToString(view.GetRowCellValue(i, column.FieldName)) + "_/";
                        }

                    }
                }

                ret_string[n] = sum;
                n++;
            }

            return ret_string;
        }

        private string[] Summary_Parameter_1(DataTable table)
        {
            try
            {
                table = DbHelp.Fill_Table(table);
                int n = 0;
                int sort_no = 0;

                string[] ret_string = new string[table.Columns.Count];

                foreach (DataColumn column in table.Columns)
                {
                    string sum = "";
                    foreach (DataRow row in table.Rows)
                    {
                        if (column.ColumnName.Contains("Sort_No"))
                        {
                            sum += (sort_no + 1).ToString() + "_/";
                            sort_no++;
                        }
                            
                        else if (!string.IsNullOrWhiteSpace(Convert.ToString(row["Item_Name"])))
                        {
                            sum += Convert.ToString(row[column]) + "_/";
                        }
                    }

                    ret_string[n] = sum;
                    n++;
                }

                return ret_string;
            }
            catch(Exception)
            {
                return null;
            }
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


    }
}

