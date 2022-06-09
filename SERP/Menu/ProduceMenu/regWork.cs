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
using DevExpress.XtraEditors.Controls;
using System.IO;

namespace SERP
{
    public partial class regWork : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        DataTable dt_WorkS = null;

        public regWork()
        {
            InitializeComponent();
        }
        
        private void regWork_Load(object sender, EventArgs e)
        {
            Grid_Set();

            btn_Select_Click(null, null);

            dt_S.Focus();

            FileIF.Set_URL();
        }

        private void Grid_Set()
        {
            //gc_EnterM 그리드
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Check", "Check", "130", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Date", "수주일자", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_No", "수주번호", "130", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Order_Part", "구분", "80", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code", "거래처", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Custom_Code1", "고객사", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Project_Title", "수주제목", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Area_Name", "납품현장", "150", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "User_Code", "영업담당자", "100", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Dept_Code", "영업부서", "100", true, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Code", "품목코드", "120", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Item_Name", "품명", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Qty", "수량", "80", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Del_Memo", "납품예정일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Del_Memo1", "납품가능일", "100", false, true, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Delivery", "고객협의일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "MatRece_Date", "자재출고예정일", "100", false, false, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_UserNM", "작업담당자명", "100", false, true, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_User", "작업담당자코드", "80", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_UserNM", "납품담당자명", "100", false, true, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Out_User", "납품담당자코드", "80", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Work_Date", "작지일자", "100", false, true, true);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "WorkResult_Date", "생산 입고 예정일", "100", false, true, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "QC_Date", "검사 예정일", "100", false, true, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Delivert_Chk", "고객협의일 체크", "100", false, false, false);
            DbHelp.GridSet(gc_WorkM, gv_WorkM, "Cf_Chk", "확정일 체크", "100", false, false, false);

            DbHelp.GridColumn_Help(gv_WorkM, "Work_UserNM", "Y");
            RepositoryItemButtonEdit button_Help_M2 = (RepositoryItemButtonEdit)gv_WorkM.Columns["Work_UserNM"].ColumnEdit;
            button_Help_M2.Buttons[0].Click += new EventHandler(grid_Work_Help);
            gv_WorkM.Columns["Work_UserNM"].ColumnEdit = button_Help_M2;

            DbHelp.GridColumn_Help(gv_WorkM, "Out_UserNM", "Y");
            gv_WorkM.Columns["Out_UserNM"].ColumnEdit = button_Help_M2;

            DbHelp.GridColumn_Data(gv_WorkM, "Work_Date");
            DbHelp.GridColumn_Data(gv_WorkM, "WorkResult_Date");
            DbHelp.GridColumn_Data(gv_WorkM, "QC_Date");

            DbHelp.GridColumn_NumSet(gv_WorkM, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));

            DbHelp.GridColumn_CheckBox(gv_WorkM, "Check");
            
            //gv_WorkM.OptionsView.ShowAutoFilterRow = false;

            //gc_EnterS 그리드
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Sort_No", "순번", "", false, false, false);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Order_No", "순번", "", false, false, false);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Msg_Part", "전달사항", "80", false, false, false);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Msg_PartNM", "전달사항내용", "100", false, false, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Msg_Bigo", "내용", "150", false, false, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Msg_Date", "일자", "100", false, false, true);
            //DbHelp.GridSet(gc_WorkS, gv_WorkS, "Add_File", "파일첨부", "", false, false, true, true);
            //DbHelp.GridSet(gc_WorkS, gv_WorkS, "Down_File", "파일다운", "70", false, true, true, true);
            //DbHelp.GridSet(gc_WorkS, gv_WorkS, "File_URL", "파일주소", "", false, false, false, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Cf_Ck", "체크", "80", false, true, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Ck_Date", "체크일자", "100", false, false, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Ck_User", "담당자", "100", false, false, true);
            DbHelp.GridSet(gc_WorkS, gv_WorkS, "Ck_Bigo", "비고", "150", false, true, true);

            DbHelp.GridColumn_CheckBox(gv_WorkS, "Cf_Ck");

            gv_WorkS.OptionsView.ShowAutoFilterRow = false;

            //RepositoryItemButtonEdit btn_File = new RepositoryItemButtonEdit();
            //btn_File.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            //btn_File.Buttons[0].Caption = "파일다운";
            //btn_File.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            //btn_File.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            //btn_File.Buttons[0].Click += new EventHandler(btn_FileDown);
            //gv_WorkS.Columns["Down_File"].ColumnEdit = btn_File;

            //출고 구분
            DataTable dt_Out = new DataTable();
            dt_Out.Columns.Add("Code");
            dt_Out.Columns.Add("Name");
            dt_Out.Rows.Add("01", "출고");
            dt_Out.Rows.Add("02", "미출고");

            lookUp_Out.Properties.ValueMember = "Code";
            lookUp_Out.Properties.DisplayMember = "Name";
            lookUp_Out.Properties.Columns.Add(new LookUpColumnInfo("Name", "출고여부"));
            lookUp_Out.Properties.DataSource = dt_Out;
            lookUp_Out.EditValue = "02";
        }

        #region Help 함수

        private void grid_Work_Help(object sender, EventArgs e)
        {
            //품목 정보 수정 필요
            int iRow = gv_WorkM.FocusedRowHandle;

            if (gv_WorkM.FocusedColumn.FieldName == "Work_UserNM")
            {
                if (string.IsNullOrWhiteSpace(gv_WorkM.GetRowCellValue(iRow, "Work_User").NullString()))
                {
                    PopHelpForm HelpForm = new PopHelpForm("User", "sp_Help_User", gv_WorkM.GetRowCellValue(iRow, "Work_UserNM").NullString(), "N");
                    if (HelpForm.ShowDialog() == DialogResult.OK)
                    {
                        gv_WorkM.SetRowCellValue(iRow, "Work_User", HelpForm.sRtCode);
                        gv_WorkM.SetRowCellValue(iRow, "Work_UserNM", HelpForm.sRtCodeNm);

                        gv_WorkM.SetRowCellValue(iRow, "Work_Date", DateTime.Today.ToString("yyyy-MM-dd"));
                        gv_WorkM.SetRowCellValue(iRow, "WorkResult_Date", DateTime.Today.AddDays(1));
                        gv_WorkM.SetRowCellValue(iRow, "QC_Date", DateTime.Today.AddDays(5));
                    }
                }
            }
            else if(gv_WorkM.FocusedColumn.FieldName == "Out_UserNM")
            {
                if (string.IsNullOrWhiteSpace(gv_WorkM.GetRowCellValue(iRow, "Out_User").ToString()))
                {
                    PopHelpForm HelpForm = new PopHelpForm("User", "sp_Help_User", gv_WorkM.GetRowCellValue(iRow, "Out_UserNM").ToString(), "N");
                    if (HelpForm.ShowDialog() == DialogResult.OK)
                    {
                        gv_WorkM.SetRowCellValue(iRow, "Out_User", HelpForm.sRtCode);
                        gv_WorkM.SetRowCellValue(iRow, "Out_UserNM", HelpForm.sRtCodeNm);
                    }
                }
            }
        }

        private void btn_FileDown(object sender, EventArgs e)
        {
            //FTP 다운
            if (gv_WorkS.RowCount < 1)
                return;

            int iRow = gv_WorkS.FocusedRowHandle;

            if (gv_WorkS.GetRowCellValue(iRow, "File_URL").ToString() == "")
                return;

            string sFile_URL = gv_WorkS.GetRowCellValue(iRow, "File_URL").ToString();

            XtraSaveFileDialog fileSave = new XtraSaveFileDialog();
            fileSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileSave.FileName = gv_WorkS.GetRowCellValue(iRow, "Add_File").ToString();
            fileSave.Filter = "All Files (*.*)|*.*"; //Excel File 97~2003 (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx|

            if (fileSave.ShowDialog() == DialogResult.OK)
            {
                FileIF.FTP_Download_File(sFile_URL, fileSave.FileName);
            }
        }

        private void SEARCH_FTP_URL()
        {
            for (int i = 0; i < gv_WorkS.RowCount; i++)
            {
                string sFIle = "";
                sFIle = gv_WorkS.GetRowCellValue(i, "File_URL").ToString();
                if (sFIle != "")
                {
                    gv_WorkS.SetRowCellValue(i, "Add_File", Path.GetFileName(sFIle));
                }
            }
        }

        #endregion

        #region 그리드 이벤트 조회

        private void gv_WorkM_DoubleClick(object sender, EventArgs e)
        {
            int iRow = gv_WorkM.FocusedRowHandle;

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

        private void gv_WorkS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Cf_Ck")
            {
                if (e.Value.ToString() == "Y")
                {
                    gv_WorkS.SetRowCellValue(e.RowHandle, "Ck_User", GlobalValue.sUserNm);
                    gv_WorkS.SetRowCellValue(e.RowHandle, "Ck_Date", DateTime.Today.ToString("yyyy-MM-dd"));
                }
                else
                {
                    gv_WorkS.SetRowCellValue(e.RowHandle, "Ck_User", "");
                    gv_WorkS.SetRowCellValue(e.RowHandle, "Ck_Date", "");
                }

                string sOrder_No = gv_WorkM.GetRowCellValue(e.RowHandle, "Order_No").ToString();
                string sSort_No = gv_WorkM.GetRowCellValue(e.RowHandle, "Sort_No").NumString();

                if (dt_WorkS != null && dt_WorkS.Select("Order_No = '" + sOrder_No + "'").Length > 0)
                {
                    DataRow dr = dt_WorkS.Select("Order_No = '" + sOrder_No + "' AND Sort_No = '" + sSort_No + "'")[0];
                    dr.BeginEdit();
                    dr[e.Column.FieldName] = e.Value.ToString();
                    dr.EndEdit();
                }
                else
                {
                    if (dt_WorkS == null)
                    {
                        dt_WorkS = gc_WorkS.DataSource as DataTable;
                    }
                    else
                    {
                        for (int i = 0; i < gv_WorkS.RowCount; i++)
                        {
                            DataRow dr_Add = dt_WorkS.NewRow();
                            dr_Add.BeginEdit();
                            dr_Add.ItemArray = gv_WorkS.GetDataRow(i).ItemArray;
                            dr_Add.EndEdit();
                            dt_WorkS.Rows.Add(dr_Add);
                        }
                    }
                }
            }
        }

        private void gv_WorkM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Work_UserNM")
            {
                string sWorkUser = gv_WorkM.GetRowCellValue(e.RowHandle, "Work_User").ToString();
                string sWorkUserNM = "";

                if (string.IsNullOrWhiteSpace(sWorkUser))
                {
                    sWorkUserNM = PopHelpForm.Return_Help("sp_Help_User", e.Value.ToString());
                    if (!string.IsNullOrWhiteSpace(sWorkUserNM))
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Work_User", e.Value.ToString());
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Work_UserNM", sWorkUserNM);
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Work_Date", DateTime.Today.ToString("yyyy-MM-dd"));
                    }
                }
                else
                {
                    sWorkUserNM = PopHelpForm.Return_Help("sp_Help_User", sWorkUser);
                    if(sWorkUserNM != e.Value.ToString())
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Work_User", "");
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Work_Date", null);
                    }
                }
            }
            else if (e.Column.FieldName == "Out_UserNM")
            {
                string sOutUser = gv_WorkM.GetRowCellValue(e.RowHandle, "Out_User").ToString();
                string sOutUserNM = "";

                if (string.IsNullOrWhiteSpace(sOutUser))
                {
                    sOutUserNM = PopHelpForm.Return_Help("sp_Help_User", e.Value.ToString());
                    if (!string.IsNullOrWhiteSpace(sOutUserNM))
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Out_User", e.Value.ToString());
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Out_UserNM", sOutUserNM);
                    }
                }
                else
                {
                    sOutUserNM = PopHelpForm.Return_Help("sp_Help_User", sOutUser);
                    if (sOutUserNM != e.Value.ToString())
                    {
                        gv_WorkM.SetRowCellValue(e.RowHandle, "Out_User", "");
                    }
                }
            }
            else if (e.Column.FieldName == "WorkResult_Date")
            {
                gv_WorkM.SetRowCellValue(e.RowHandle, "QC_Date", DateTime.Parse(e.Value.ToString()).AddDays(4));
            }

            gv_WorkM.CellValueChanged -= new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_WorkM_CellValueChanged);
            gv_WorkM.SetRowCellValue(e.RowHandle, "Check", "Y");
            gv_WorkM.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(gv_WorkM_CellValueChanged);
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

        #region 함수

        private void Search_D(int iRow)
        {
            try
            {
                if (dt_WorkS != null && dt_WorkS.Select("Order_No = '" + gv_WorkM.GetRowCellValue(iRow, "Order_No").ToString() + "'").Length > 0)
                {
                    gc_WorkS.DataSource = dt_WorkS.Select("Order_No = '" + gv_WorkM.GetRowCellValue(iRow, "Order_No").ToString() + "'").CopyToDataTable();
                }
                else
                {
                    SqlParam sp = new SqlParam("sp_regWork");
                    sp.AddParam("Kind", "S");
                    sp.AddParam("Search_D", "D");
                    sp.AddParam("Order_No", gv_WorkM.GetRowCellValue(iRow, "Order_No").ToString());

                    ret = DbHelp.Proc_Search(sp);

                    if (ret.ReturnChk != 0)
                    {
                        XtraMessageBox.Show(ret.ReturnMessage);
                        return;
                    }

                    gc_WorkS.DataSource = ret.ReturnDataSet.Tables[0];
                }

                //SEARCH_FTP_URL();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        #endregion

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            gc_WorkM.DataSource = null;
            gc_WorkS.DataSource = null;

            try
            {
                SqlParam sp = new SqlParam("sp_regWork");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "H");
                sp.AddParam("Date_S", dt_S.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Date_E", dt_E.Text == "" ? DateTime.Today.ToString("yyyyMMdd") : dt_E.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Out_GB", lookUp_Out.EditValue.ToString());

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_WorkM.DataSource = ret.ReturnDataSet.Tables[0];

                if (gv_WorkM.RowCount > 0)
                    Search_D(0);

                gv_WorkM_FocusedRowChanged(gv_WorkM, new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs(-1, 0));
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
                string sOrderNo = "", sItemCode = "", sDelMemo1 = "", sWorkUser = "", sWorkDate = "", sOutUser = "", sWorkResultDate = "", sQcDate = "";

                string sSOrderNo = "", sMsg_Part = "", sCf_Ck = "", sCk_Bigo = "";

                for(int i = 0; i < gv_WorkM.RowCount; i++)
                {
                    if(gv_WorkM.GetRowCellValue(i, "Check").ToString() == "Y")
                    {
                        sOrderNo += gv_WorkM.GetRowCellValue(i, "Order_No").ToString() + "_/";
                        sItemCode += gv_WorkM.GetRowCellValue(i, "Item_Code").ToString() + "_/";
                        sDelMemo1 += gv_WorkM.GetRowCellValue(i, "Del_Memo1").ToString() + "_/";
                        sWorkUser += gv_WorkM.GetRowCellValue(i, "Work_User").ToString() + "_/";
                        sWorkDate += gv_WorkM.GetRowCellValue(i, "Work_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_WorkM.GetRowCellValue(i, "Work_Date").ToString()).ToString("yyyyMMdd") + "_/";
                        sOutUser += gv_WorkM.GetRowCellValue(i, "Out_User").ToString() + "_/";
                        sWorkResultDate += gv_WorkM.GetRowCellValue(i, "WorkResult_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_WorkM.GetRowCellValue(i, "WorkResult_Date").ToString()).ToString("yyyyMMdd") + "_/";
                        sQcDate += gv_WorkM.GetRowCellValue(i, "QC_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_WorkM.GetRowCellValue(i, "QC_Date").ToString()).ToString("yyyyMMdd") + "_/";
                    }
                }

                for (int i = 0; i < dt_WorkS.Rows.Count; i++)
                {
                    sSOrderNo += dt_WorkS.Rows[i]["Order_No"].ToString() + "_/";
                    sMsg_Part += dt_WorkS.Rows[i]["Msg_Part"].ToString() + "_/";
                    sCf_Ck += dt_WorkS.Rows[i]["Cf_Ck"].ToString() + "_/";
                    sCk_Bigo += dt_WorkS.Rows[i]["Ck_Bigo"].ToString() + "_/";
                }

                SqlParam sp = new SqlParam("sp_regWork");
                sp.AddParam("Kind", "I");
                sp.AddParam("OrderNo", sOrderNo);
                sp.AddParam("ItemCode", sItemCode);
                sp.AddParam("DelMemo1", sDelMemo1);
                sp.AddParam("WorkUser", sWorkUser);
                sp.AddParam("WorkDate", sWorkDate);
                sp.AddParam("OutUser", sOutUser);
                sp.AddParam("WorkResultDate", sWorkResultDate);
                sp.AddParam("QCDate", sQcDate);

                sp.AddParam("SOrderNo", sSOrderNo);
                sp.AddParam("Msg_Part", sMsg_Part);
                sp.AddParam("Cf_Ck", sCf_Ck);
                sp.AddParam("Ck_Bigo", sCk_Bigo);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                btn_Save.sCHK = "Y";
                btn_Select_Click(null, null);

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_WorkM, "작업지시");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        #endregion

        private void gv_WorkM_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            Search_D(e.RowHandle);
        }

        private void gc_WorkM_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (gv_WorkM.FocusedColumn == gv_WorkM.Columns["Out_UserNM"])
                {
                    grid_Work_Help(null, null);
                }
                else if (gv_WorkM.FocusedColumn == gv_WorkM.Columns["Work_UserNM"])
                {
                    grid_Work_Help(null, null);
                }
            }
        }

        private void gv_WorkM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            Search_D(e.FocusedRowHandle);
            txt_OrderNo.Text = gv_WorkM.GetRowCellValue(e.FocusedRowHandle, "Order_No").ToString();
        }

        private void gv_WorkM_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            if(gv_WorkM.GetRowCellValue(e.RowHandle, "Delivery_Chk").NullString() == "Y")
            {
                e.Appearance.BackColor = Color.Red;
                e.Appearance.ForeColor = Color.White;
            }
            else if(gv_WorkM.GetRowCellValue(e.RowHandle, "Cf_Ck").NullString() == "N")
            {
                e.Appearance.BackColor = Color.Blue;
            }
        }
    }
}