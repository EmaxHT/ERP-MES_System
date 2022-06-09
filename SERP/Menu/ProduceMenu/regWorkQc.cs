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
using System.IO;

namespace SERP
{
    public partial class regWorkQc : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        DataTable dt_S1 = new DataTable();  //S1
        DataTable dt_S2 = new DataTable(); //S2

        string sDept_Code = "";

        public regWorkQc()
        {
            InitializeComponent();
        }

        private void regWorkQc_Load(object sender, EventArgs e)
        {
            dt_QcDate.Text = DateTime.Today.ToString("yyyy-MM-dd");

            ForMat.sBasic_Set(this.Name, txt_QcNo);

            Grid_Set();

            dt_S2 = DbHelp.Return_DT(gv_QcS2);
            dt_S2.PrimaryKey = new DataColumn[] { dt_S2.Columns["Result_No"], dt_S2.Columns["Order_No"], dt_S2.Columns["Item_Code"], dt_S2.Columns["Qc_Code"] };

            FileIF.Set_URL();

            dt_QcDate.Focus();

            txt_UserCode.Text = GlobalValue.sUserID;

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }


        private void Grid_Set()
        {
            //gc_WorkQcM 그리드
            DbHelp.GridSet(gc_QcS, gv_QcS, "Sort_No", "순번", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Result_Date", "입고일자", "100", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Result_No", "입고번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Custom_Code1", "고객사", "100", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Order_No", "수주번호", "130", true, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Process_Code", "공정", "100", true, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Whouse_Code", "입고창고", "100", true, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Code", "품목코드", "120", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Name", "품명", "150", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Ssize", "규격", "150", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qty", "입고수량", "80", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "GoodQty", "정품수량", "80", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Add_File", "파일첨부", "150", false, false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Down_File", "파일다운", "70", false, true, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "File_URL", "파일주소", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Pass_CodeNM", "합격여부", "100", false, true, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Pass_Code", "합격여부코드", "80", false, false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "End_Date", "완료일자", "100", false, true, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qc_Bigo", "비고", "150", false, true, true, true);

            DbHelp.GridColumn_Help(gv_QcS, "Result_No", "Y");
            RepositoryItemButtonEdit button_Help_S = (RepositoryItemButtonEdit)gv_QcS.Columns["Result_No"].ColumnEdit;
            button_Help_S.Buttons[0].Click += new EventHandler(grid_S_Help);
            gv_QcS.Columns["Result_No"].ColumnEdit = button_Help_S;

            DbHelp.GridColumn_Help(gv_QcS, "Pass_CodeNM", "Y");
            RepositoryItemButtonEdit button_HelpPass_S = (RepositoryItemButtonEdit)gv_QcS.Columns["Pass_CodeNM"].ColumnEdit;
            button_HelpPass_S.Buttons[0].Click += new EventHandler(grid_S_HelpPass);
            gv_QcS.Columns["Pass_CodeNM"].ColumnEdit = button_HelpPass_S;

            DbHelp.GridColumn_Data(gv_QcS, "End_Date");

            DbHelp.GridColumn_NumSet(gv_QcS, "Qty", ForMat.SetDecimal(this.Name, "Qty1"));
            DbHelp.GridColumn_NumSet(gv_QcS, "GoodQty", ForMat.SetDecimal(this.Name, "Qty1"));

            //파일 다운 버튼
            RepositoryItemButtonEdit btn_File = new RepositoryItemButtonEdit();
            btn_File.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            btn_File.Buttons[0].Caption = "파일다운";
            btn_File.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btn_File.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btn_File.Buttons[0].Click += new EventHandler(btn_FileDown);
            gv_QcS.Columns["Down_File"].ColumnEdit = btn_File;

            gc_QcS.DeleteRowEventHandler += new EventHandler(Delete_QcS);

            gv_QcS.OptionsView.ShowAutoFilterRow = false;
            gc_QcS.AddRowYN = true;

            //gc_WorkQcS2
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Sort_No", "순번", "50", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Result_No", "입고번호", "130", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Order_No", "수주번호", "130", true, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Item_Code", "품목코드", "120", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Qc_CodeNM", "검사항목", "100", false, true, true);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Qc_Code", "검사항목코드", "80", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Pass_CodeNM", "검사결과", "100", false, true, true);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Pass_Code", "검사결과코드", "80", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Qc_Date", "검사일자", "100", false, true, true);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "End_Date", "완료일자", "100", false, false, true);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Fail_CodeNM", "사유", "100", false, true, true);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Fail_Code", "사유코드", "80", false, false, false);
            DbHelp.GridSet(gc_QcS2, gv_QcS2, "Qc_Bigo", "비고", "150", false, true, true);

            DbHelp.GridColumn_Help(gv_QcS2, "Qc_CodeNM", "Y");
            RepositoryItemButtonEdit button_Help_S1 = (RepositoryItemButtonEdit)gv_QcS2.Columns["Qc_CodeNM"].ColumnEdit;
            button_Help_S1.Buttons[0].Click += new EventHandler(grid_S2_HelpQc);
            gv_QcS2.Columns["Qc_CodeNM"].ColumnEdit = button_Help_S1;

            DbHelp.GridColumn_Help(gv_QcS2, "Pass_CodeNM", "Y");
            RepositoryItemButtonEdit button_HelpPass_S1 = (RepositoryItemButtonEdit)gv_QcS2.Columns["Pass_CodeNM"].ColumnEdit;
            button_HelpPass_S1.Buttons[0].Click += new EventHandler(grid_S2_HelpPass);
            gv_QcS2.Columns["Pass_CodeNM"].ColumnEdit = button_HelpPass_S1;

            DbHelp.GridColumn_Help(gv_QcS2, "Fail_CodeNM", "Y");
            RepositoryItemButtonEdit button_HelpFail_S1 = (RepositoryItemButtonEdit)gv_QcS2.Columns["Fail_CodeNM"].ColumnEdit;
            button_HelpFail_S1.Buttons[0].Click += new EventHandler(grid_S2_HelpFail);
            gv_QcS2.Columns["Fail_CodeNM"].ColumnEdit = button_HelpFail_S1;

            DbHelp.GridColumn_Data(gv_QcS2, "Qc_Date");

            gv_QcS2.OptionsView.ShowAutoFilterRow = false;

            gc_QcS2.DeleteRowEventHandler += new EventHandler(Delete_QcS2);

            Search_H();
        }

        #region 그리드 Help

        //입고 번호 조회
        private void grid_S_Help(object sender, EventArgs e)
        {
            int iRow = gv_QcS.GetFocusedDataSourceRowIndex();

            if(string.IsNullOrWhiteSpace(gv_QcS.GetRowCellValue(iRow, "Result_Date").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("MakeResult", "sp_Help_WorkResult", gv_QcS.GetRowCellValue(iRow, "Result_No").ToString(), "Y");
                HelpForm.Set_Value("Y", "", "", "", "");
                HelpForm.sLevelYN = "Y";
                HelpForm.sNotReturn = "Y";
                if(HelpForm.ShowDialog() == DialogResult.OK)
                {
                    foreach(DataRow row in HelpForm.drReturn)
                    {
                        gv_QcS.SetRowCellValue(iRow, "Result_No", row["Result_No"].ToString());
                        Search_Result(row["Result_No"].ToString(), row["Order_No"].ToString(), row["Item_Code"].ToString(), iRow);

                        if (iRow + 1 == gv_QcS.RowCount)
                            gv_QcS.AddNewRow();

                        iRow++;

                        gv_QcS.UpdateCurrentRow();

                        Search_Qc(row["Result_No"].ToString(), row["Item_Code"].ToString(), row["Order_No"].ToString());
                    }

                    gv_QcS.DeleteRow(iRow);
                    gv_QcS_RowCellClick(sender, new DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(MouseButtons.Left, 1, 0, 0, 0), iRow - 1, gv_QcS.Columns["Result_Date"]));
                }
            }
        }

        private void grid_S_HelpPass(object sender, EventArgs e)
        {
            int iRow = gv_QcS.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_QcS.GetRowCellValue(iRow, "Pass_Code").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30050", gv_QcS.GetRowCellValue(iRow, "Pass_CodeNM").ToString(), "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_QcS.SetRowCellValue(iRow, "Pass_Code", HelpForm.sRtCode);
                    gv_QcS.SetRowCellValue(iRow, "Pass_CodeNM", HelpForm.sRtCodeNm);
                    gv_QcS.SetRowCellValue(iRow, "End_Date", DateTime.Now);
                }
            }
        }

        private void grid_S2_HelpQc(object sender, EventArgs e)
        {
            int iRow = gv_QcS2.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_QcS2.GetRowCellValue(iRow, "Qc_Code").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30060", gv_QcS2.GetRowCellValue(iRow, "Qc_CodeNM").ToString(), "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_QcS2.SetRowCellValue(iRow, "Qc_Code", HelpForm.sRtCode);
                    gv_QcS2.SetRowCellValue(iRow, "Qc_CodeNM", HelpForm.sRtCodeNm);
                }
            }
        }

        private void grid_S2_HelpPass(object sender, EventArgs e)
        {
            int iRow = gv_QcS2.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_QcS2.GetRowCellValue(iRow, "Pass_Code").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30050", gv_QcS2.GetRowCellValue(iRow, "Pass_CodeNM").ToString(), "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_QcS2.SetRowCellValue(iRow, "Pass_Code", HelpForm.sRtCode);
                    gv_QcS2.SetRowCellValue(iRow, "Pass_CodeNM", HelpForm.sRtCodeNm);
                    Pass_Chk(gv_QcS.GetFocusedDataSourceRowIndex());
                }
            }
        }

        private void grid_S2_HelpFail(object sender, EventArgs e)
        {
            int iRow = gv_QcS2.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_QcS2.GetRowCellValue(iRow, "Fail_Code").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30080", gv_QcS2.GetRowCellValue(iRow, "Fail_CodeNM").ToString(), "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_QcS2.SetRowCellValue(iRow, "Fail_Code", HelpForm.sRtCode);
                    gv_QcS2.SetRowCellValue(iRow, "Fail_CodeNM", HelpForm.sRtCodeNm);
                }
            }
        }

        #endregion

        #region 그리드 이벤트

        private void gc_QcS_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(gv_QcS.FocusedColumn.FieldName == "Result_No")
                {
                    grid_S_Help(null, null);
                }
                else if(gv_QcS.FocusedColumn.FieldName == "Pass_CodeNM")
                {
                    grid_S_HelpPass(null, null);
                }
            }
        }

        private void gc_QcS2_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(gv_QcS2.FocusedColumn.FieldName == "Qc_CodeNM")
                {
                    grid_S2_HelpQc(null, null);
                }
                else if(gv_QcS2.FocusedColumn.FieldName == "Pass_CodeNM")
                {
                    grid_S2_HelpPass(null, null);
                }
                else if(gv_QcS2.FocusedColumn.FieldName == "Fail_CodeNM")
                {
                    grid_S2_HelpFail(null, null);
                }
            }
        }

        private void gv_QcS_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "Pass_CodeNM")
            {
                string sPass_Code = gv_QcS.GetRowCellValue(e.RowHandle, "Pass_Code").ToString();
                string sPass_CodeNM = "";

                if (string.IsNullOrWhiteSpace(sPass_Code))
                {
                    sPass_CodeNM = PopHelpForm.Return_Help("sp_Help_General", e.Value.ToString(), "30050", "");
                    if (!string.IsNullOrWhiteSpace(sPass_CodeNM))
                    {
                        gv_QcS.SetRowCellValue(e.RowHandle, "Pass_Code", e.Value.ToString());
                        gv_QcS.SetRowCellValue(e.RowHandle, "Pass_CodeNM", sPass_CodeNM);
                        gv_QcS.SetRowCellValue(e.RowHandle, "End_Date", DateTime.Now);
                    }
                }
                else
                {
                    sPass_CodeNM = PopHelpForm.Return_Help("sp_Help_General", sPass_Code, "30050", "");
                    if (sPass_CodeNM != e.Value.ToString())
                    {
                        gv_QcS.SetRowCellValue(e.RowHandle, "Pass_Code", "");
                        gv_QcS.SetRowCellValue(e.RowHandle, "End_Date", null);
                    }
                }
            }
            if(e.Column.FieldName == "Result_No")
            {
                Search_Result(e.Value.ToString(), gv_QcS.GetRowCellValue(e.RowHandle, "Order_No").ToString(), e.Value.ToString(), e.RowHandle);
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_QcS2_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if(e.Column.FieldName == "Qc_CodeNM")
            {
                string sQc_Code = gv_QcS2.GetRowCellValue(e.RowHandle, "Qc_Code").ToString();
                string sQc_CodeNM = "";

                if (string.IsNullOrWhiteSpace(sQc_Code))
                {
                    sQc_CodeNM = PopHelpForm.Return_Help("sp_Help_General", e.Value.ToString(), "30060", "");
                    if (!string.IsNullOrWhiteSpace(sQc_CodeNM))
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Qc_Code", e.Value.ToString());
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Qc_CodeNM", sQc_CodeNM);
                    }
                }
                else
                {
                    sQc_CodeNM = PopHelpForm.Return_Help("sp_Help_General", sQc_Code, "30060", "");
                    if(sQc_CodeNM != e.Value.ToString())
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Qc_Code", "");
                    }
                }
            }
            else if(e.Column.FieldName == "Pass_CodeNM")
            {
                string sPass_Code = gv_QcS2.GetRowCellValue(e.RowHandle, "Pass_Code").ToString();
                string sPass_CodeNM = "";

                if (string.IsNullOrWhiteSpace(sPass_Code))
                {
                    sPass_CodeNM = PopHelpForm.Return_Help("sp_Help_General", e.Value.ToString(), "30050", "");
                    if (!string.IsNullOrWhiteSpace(sPass_CodeNM))
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Pass_Code", e.Value.ToString());
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Pass_CodeNM", sPass_CodeNM);
                    }
                }
                else
                {
                    sPass_CodeNM = PopHelpForm.Return_Help("sp_Help_General", sPass_Code, "30050", "");
                    if (sPass_CodeNM != e.Value.ToString())
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Pass_Code", "");
                    }
                }

                Pass_Chk(gv_QcS.GetFocusedDataSourceRowIndex());
            }
            else if (e.Column.FieldName == "Fail_CodeNM")
            {
                string sFail_Code = gv_QcS2.GetRowCellValue(e.RowHandle, "Fail_Code").ToString();
                string sFail_CodeNM = "";

                if (string.IsNullOrWhiteSpace(sFail_Code))
                {
                    sFail_CodeNM = PopHelpForm.Return_Help("sp_Help_General", e.Value.ToString(), "30080", "");
                    if (!string.IsNullOrWhiteSpace(sFail_CodeNM))
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Fail_Code", e.Value.ToString());
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Fail_CodeNM", sFail_CodeNM);
                    }
                }
                else
                {
                    sFail_CodeNM = PopHelpForm.Return_Help("sp_Help_General", sFail_Code, "30080", "");
                    if (sFail_CodeNM != e.Value.ToString())
                    {
                        gv_QcS2.SetRowCellValue(e.RowHandle, "Fail_Code", "");
                    }
                }
            }

            DataRow_AddS2(gc_QcS2.DataSource as DataTable);
        }

        private void gv_QcS_DoubleClick(object sender, EventArgs e)
        {
            int iRow = gv_QcS.GetFocusedDataSourceRowIndex();

            if (gv_QcS.FocusedColumn == gv_QcS.Columns["Add_File"])
            {
                //FTP 업로드
                XtraOpenFileDialog xtraOpenFileDialog = new XtraOpenFileDialog();

                xtraOpenFileDialog.InitialDirectory = @"C:\";

                if (xtraOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string sFile = Path.GetFileName(xtraOpenFileDialog.FileName);
                    gv_QcS.SetRowCellValue(iRow, "Add_File", sFile);
                    FileIF.FTP_Upload_File(xtraOpenFileDialog.FileName, FileIF.FTP_URL + sFile);
                    gv_QcS.SetRowCellValue(iRow, "File_URL", FileIF.FTP_URL + sFile);
                }

                XtraMessageBox.Show("업로드 완료 되었습니다");
            }
            else
            {
                string sItem_Code = gv_QcS.GetRowCellValue(iRow, "Item_Code").ToString();
                string sResult_No = gv_QcS.GetRowCellValue(iRow, "Result_No").ToString();
                string sOrder_No = gv_QcS.GetRowCellValue(iRow, "Order_No").ToString();

                if (sItem_Code == "")
                {
                    XtraMessageBox.Show("입고번호를 먼저 입력해주세요");
                    return;
                }

                PopWorkQcForm PopWorkQc = new PopWorkQcForm();
                PopWorkQc.sResult_No = sResult_No;
                PopWorkQc.sItem_Code = sItem_Code;
                PopWorkQc.sOrder_No = sOrder_No;
                PopWorkQc.sItem_Name = gv_QcS.GetRowCellValue(iRow, "Item_Name").ToString();
                PopWorkQc.sCustomer1 = gv_QcS.GetRowCellValue(iRow, "Custom_Code1").ToString();
                PopWorkQc.sQty = gv_QcS.GetRowCellValue(iRow, "Qty").ToString();
                PopWorkQc.sQc_No = txt_QcNo.Text;
                if (dt_S1.Rows.Count > 0)
                    PopWorkQc.dr_S1 = dt_S1.Select("Result_No = '" + sResult_No + "' AND Item_Code = '" + sItem_Code + "' AND Order_No = '" + sOrder_No + "'");

                if (PopWorkQc.ShowDialog() == DialogResult.OK)
                {
                    if (dt_S1.Rows.Count < 1)
                    {
                        dt_S1 = PopWorkQc.dt_S1;
                        dt_S1.PrimaryKey = new DataColumn[] { dt_S1.Columns["Result_No"], dt_S1.Columns["Order_No"], dt_S1.Columns["Item_Code"], dt_S1.Columns["Fail_Code"] };
                    }
                    else
                    {
                        DataRow_Add(PopWorkQc.dt_S1);
                    }

                    int Qty = (int)decimal.Parse(gv_QcS.GetRowCellValue(iRow, "Qty").NumString());

                    gv_QcS.SetRowCellValue(iRow, "GoodQty", (Qty - PopWorkQc.iQty).ToString());
                }
            }
        }

        private void gv_QcS_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            string sResult_No = gv_QcS.GetRowCellValue(e.RowHandle, "Result_No").ToString();
            string sItem_Code = gv_QcS.GetRowCellValue(e.RowHandle, "Item_Code").ToString();
            string sOrder_No = gv_QcS.GetRowCellValue(e.RowHandle, "Order_No").ToString();

            DataTable dt_S2CHK = DbHelp.Return_DT(gv_QcS2);

            DataRow[] dr_S2 = dt_S2.Select("Result_No = '" + sResult_No + "' AND Item_Code = '" + sItem_Code + "' AND Order_No = '" + sOrder_No + "'");

            foreach (DataRow row in dr_S2)
            {
                DataRow dr = dt_S2CHK.NewRow();
                dr.BeginEdit();
                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    dr[i] = row[i];
                }
                dr.EndEdit();

                dt_S2CHK.Rows.Add(dr);
            }

            dt_S2CHK.DefaultView.Sort = "Sort_No ASC";
            gc_QcS2.DataSource = dt_S2CHK;
        }


        private void gv_QcS_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            if (gv_QcS.GetDataRow(e.FocusedRowHandle).RowState == DataRowState.Added)
            {
                gv_QcS.Columns["Result_No"].OptionsColumn.AllowEdit = true;
                gv_QcS.Columns["Result_No"].OptionsColumn.ReadOnly = false;
            }
            else
            {
                gv_QcS.Columns["Result_No"].OptionsColumn.AllowEdit = false;
                gv_QcS.Columns["Result_No"].OptionsColumn.ReadOnly = true;
            }
        }

        #endregion

        #region 함수

        //FTP 다운
        private void btn_FileDown(object sender, EventArgs e)
        {
            //FTP 다운
            if (gv_QcS.RowCount < 1)
                return;

            int iRow = gv_QcS.FocusedRowHandle;

            if (gv_QcS.GetRowCellValue(iRow, "File_URL").ToString() == "")
                return;

            string sFile_URL = gv_QcS.GetRowCellValue(iRow, "File_URL").ToString();

            XtraSaveFileDialog fileSave = new XtraSaveFileDialog();
            fileSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            fileSave.FileName = gv_QcS.GetRowCellValue(iRow, "Add_File").ToString();
            fileSave.Filter = "All Files (*.*)|*.*"; //Excel File 97~2003 (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx|

            if (fileSave.ShowDialog() == DialogResult.OK)
            {
                FileIF.FTP_Download_File(sFile_URL, fileSave.FileName);
            }
        }


        //그리드 삭제
        private void Delete_QcS2(object sender, EventArgs e)
        {
            int iRow = gv_QcS2.GetFocusedDataSourceRowIndex();
            string sResult_No = gv_QcS2.GetRowCellValue(iRow, "Result_No").ToString();
            string sItem_Code = gv_QcS2.GetRowCellValue(iRow, "Item_Code").ToString();
            string sOrder_No = gv_QcS2.GetRowCellValue(iRow, "Order_No").ToString();
            string sQc_Code = gv_QcS2.GetRowCellValue(iRow, "Qc_Code").ToString();

            DataRow deleteRow = dt_S2.Select("Result_No = '" + sResult_No + "' AND Item_Code = '" + sItem_Code + "' AND Order_No = '" + sOrder_No + "' AND Qc_Code = '" + sQc_Code + "'")[0];
            dt_S2.Rows.Remove(deleteRow);

            //if (txt_QcNo.Enabled)
            //{
                
            //}
            //else
            //{
            //    if (string.IsNullOrWhiteSpace(txt_QcNo.Text))
            //    {
            //        gv_QcS2.DeleteRow(iRow);
            //    }
            //    else
            //    {
            //        //DB 삭제
            //    }
            //}
        }

        //그리드 S 삭제
        private void Delete_QcS(object sender, EventArgs e)
        {

        }

        //입고번호 조회
        private void Search_Result(string sResult_No, string sOrder_No, string sItem_Code, int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SR");
                sp.AddParam("Result_No", sResult_No);
                sp.AddParam("Order_No", sOrder_No);
                sp.AddParam("Item_Code", sItem_Code);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
                else
                {
                    if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                        DataRow dr = dt.Rows[0];
                        gv_QcS.SetRowCellValue(iRow, "Result_Date", dr["Result_Date"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Custom_Code1", dr["Custom_Code1"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Order_No", dr["Order_No"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Process_Code", dr["Process_Code"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Whouse_Code", dr["Whouse_Code"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Item_Code", dr["Item_Code"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Item_Name", dr["Item_Name"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Ssize", dr["Ssize"].ToString());
                        gv_QcS.SetRowCellValue(iRow, "Qty", dr["Qty"].NumString());
                        gv_QcS.SetRowCellValue(iRow, "GoodQty", dr["Qty"].NumString());
                    }
                    else
                    {
                        gv_QcS.SetRowCellValue(iRow, "Result_Date", "");
                        gv_QcS.SetRowCellValue(iRow, "Custom_Code1", "");
                        gv_QcS.SetRowCellValue(iRow, "Order_No", "");
                        gv_QcS.SetRowCellValue(iRow, "Process_Code", "");
                        gv_QcS.SetRowCellValue(iRow, "Whouse_Code", "");
                        gv_QcS.SetRowCellValue(iRow, "Item_Code", "");
                        gv_QcS.SetRowCellValue(iRow, "Item_Name", "");
                        gv_QcS.SetRowCellValue(iRow, "Ssize", "");
                        gv_QcS.SetRowCellValue(iRow, "Qty", "0");
                        gv_QcS.SetRowCellValue(iRow, "GoodQty", "0");
                    }
                }

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search_Qc(string sResult_No, string sItem_Code, string sOrder_No)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SQ");
                sp.AddParam("Result_No", sResult_No);
                sp.AddParam("Item_Code", sItem_Code);
                sp.AddParam("Order_No", sOrder_No);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataRow_AddS2(ret.ReturnDataSet.Tables[0]);

                //gc_QcS2.DataSource = ret.ReturnDataSet.Tables[0];

                //for(int i = 0; i < gv_QcS2.RowCount; i++)
                //{
                //    gv_QcS.SetRowCellValue(i, "Result_No", sResult_No);
                //    gv_QcS.SetRowCellValue(i, "Item_Code", sItem_Code);
                //}
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
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

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
                if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
                {
                    txt_DeptCode.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Name"].ToString();
                    sDept_Code = ret.ReturnDataSet.Tables[0].Rows[0]["Dept_Code"].ToString();
                }
                else
                {
                    txt_DeptCode.Text = "";
                    sDept_Code = "";
                }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Search_H()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SH");
                sp.AddParam("QC_No", txt_QcNo.Text);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                DataTable dt1 = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[1]);
                DataTable dt2 = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[2]);

                if(dt.Rows.Count > 0)
                {
                    dt_QcDate.Text = dt.Rows[0]["Qc_Date"].ToString();
                    txt_UserCode.Text = dt.Rows[0]["User_Code"].ToString();
                    txt_UpUser.Text = dt.Rows[0]["Up_User"].ToString();
                    txt_UpDate.Text = dt.Rows[0]["Up_Date"].ToString();
                    txt_RegDate.Text = dt.Rows[0]["Reg_Date"].ToString();
                    txt_RegUser.Text = dt.Rows[0]["Reg_User"].ToString();
                }

                gc_QcS.DataSource = dt1;
                SEARCH_FTP_URL();
                gc_QcS2.DataSource = dt2;
                dt_S1 = new DataTable();
                dt_S2 = new DataTable();
                dt_S2 = DbHelp.Return_DT(gv_QcS2);
                dt_S2.PrimaryKey = new DataColumn[] { dt_S2.Columns["Result_No"], dt_S2.Columns["Order_No"], dt_S2.Columns["Item_Code"], dt_S2.Columns["Qc_Code"] };

                DataRow_AddS2(dt2);

                if(gv_QcS.RowCount > 0)
                    gv_QcS_RowCellClick(null, new DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs(new DevExpress.Utils.DXMouseEventArgs(MouseButtons.Left, 1, 0, 0, 0), 0, gv_QcS.Columns["Result_Date"]));
                //txt_QcNo.Enabled = false;
                //txt_QcNo.BackColor = Color.LightGray;
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Pass_Chk(int iRow)
        {
            string sCHK = "000";

            for(int i = 0; i < gv_QcS2.RowCount; i++)
            {
                if(gv_QcS2.GetRowCellValue(i, "Pass_Code").NullString() != "000")
                {
                    sCHK = "010";
                    gv_QcS.SetRowCellValue(iRow, "Pass_Code", sCHK);
                    gv_QcS.SetRowCellValue(iRow, "Pass_CodeNM", "Fail");
                    return;
                }
            }

            gv_QcS.SetRowCellValue(iRow, "Pass_Code", sCHK);
            gv_QcS.SetRowCellValue(iRow, "Pass_CodeNM", "Pass");
        }

        private void SEARCH_FTP_URL()
        {
            if (gv_QcS.RowCount < 1)
                return;

            string sFIle = "";
            for (int i = 0; i < gv_QcS.RowCount; i++)
            {
                sFIle = gv_QcS.GetRowCellValue(i, "File_URL").ToString();
                if (sFIle != "")
                {
                    gv_QcS.SetRowCellValue(i, "Add_File", Path.GetFileName(sFIle));
                }
            }
        }

        //테이블 정보 저장
        private void DataRow_Add(DataTable dt)
        {
            DataRow dr;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //업데이트 되는거
                if (dt_S1.Rows.Find(new string[] { dt.Rows[i]["Result_No"].ToString(), dt.Rows[i]["Order_No"].ToString(), dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Fail_Code"].ToString() }) != null)
                {
                    dr = dt_S1.Rows.Find(new string[] { dt.Rows[i]["Result_No"].ToString(), dt.Rows[i]["Order_No"].ToString(), dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Fail_Code"].ToString() });

                    dr.BeginEdit();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string sColumn = dt_S1.Columns[j].ColumnName;
                        dr[sColumn] = dt.Rows[i][sColumn];
                    }

                    dr.EndEdit();

                    dt_S1.AcceptChanges();
                }
                else
                {
                    dr = dt_S1.NewRow();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string sColumn = dt_S1.Columns[j].ColumnName;
                        dr[sColumn] = dt.Rows[i][sColumn];
                    }

                    dt_S1.Rows.InsertAt(dr, 0);
                }
            }
        }

        //테이블 정보 저장(S2)
        private void DataRow_AddS2(DataTable dt)
        {
            DataRow dr;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //업데이트 되는거
                if (dt_S2.Rows.Find(new string[] { dt.Rows[i]["Result_No"].ToString(),  dt.Rows[i]["Order_No"].ToString(), dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Qc_Code"].ToString() }) != null)
                {
                    dr = dt_S2.Rows.Find(new string[] { dt.Rows[i]["Result_No"].ToString(), dt.Rows[i]["Order_No"].ToString(), dt.Rows[i]["Item_Code"].ToString(), dt.Rows[i]["Qc_Code"].ToString() });

                    dr.BeginEdit();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string sColumn = dt_S2.Columns[j].ColumnName;
                        dr[sColumn] = dt.Rows[i][sColumn];
                    }

                    dr.EndEdit();

                    dt_S2.AcceptChanges();
                }
                else
                {
                    dr = dt_S2.NewRow();

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string sColumn = dt_S2.Columns[j].ColumnName;
                        dr[sColumn] = dt.Rows[i][sColumn];
                    }

                    dt_S2.Rows.InsertAt(dr, 0);
                }
            }
        }

        #endregion

        #region 검사담당자 조회

        private void txt_UserCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_UserCodeNM.Text = PopHelpForm.Return_Help("sp_Help_User", txt_UserCode.Text, "", "");
            Search_Dept(txt_UserCode.Text);
        }

        private void txt_UserCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txt_UserCode_Properties_ButtonClick(sender, null);
            }
        }

        private void txt_UserCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_UserCodeNM.Text))
            {
                PopHelpForm HelpForm = new PopHelpForm("User", "sp_Help_User", txt_UserCode.Text, "N");
                if(HelpForm.ShowDialog() == DialogResult.OK)
                {
                    txt_UserCode.Text = HelpForm.sRtCode;
                    txt_UserCodeNM.Text = HelpForm.sRtCodeNm;
                    Search_Dept(txt_UserCode.Text);
                }
            }
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


        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm Help_Form = new PopHelpForm("WorkQc", "sp_Help_WorkQc", "N");
            Help_Form.sLevelYN = "Y";
            Help_Form.sNotReturn = "Y";
            btn_Select.clsWait.CloseWait();
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                txt_QcNo.Text = Help_Form.sRtCode;
                Search_H();

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panel_M);
            txt_UserCode.Text = GlobalValue.sUserID;
            gc_QcS.DataSource = null;
            gc_QcS2.DataSource = null;

            Search_H();

            dt_QcDate.Select();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sResultNo_S = "", sPassCode_S = "", sEndDate_S = "", sItemCode_S = "", sOrderNo_S = "";
                string sResultNo_S1 = "", sItem_Code = "", sQty = "", sFailCode_S1 = "", sQcBigo_S1 = "", sFileURL = "", sOrderNo_S1 = "";
                string sResultNo_S2 = "", sItemCode_S2 = "", sQCCode = "", sPassCode_S2 = "", sQcDate_S2 = "", sFailCode_S2 = "", sQcBigo_S2 = "", sOrderNo_S2 = "";

                //S
                for(int i = 0; i < gv_QcS.RowCount; i++)
                {
                    if (!string.IsNullOrWhiteSpace(gv_QcS.GetRowCellValue(i, "Result_No").ToString()))
                    {
                        sResultNo_S += gv_QcS.GetRowCellValue(i, "Result_No").ToString() + "_/";
                        sItemCode_S += gv_QcS.GetRowCellValue(i, "Item_Code").ToString() + "_/";
                        sPassCode_S += gv_QcS.GetRowCellValue(i, "Pass_Code").ToString() + "_/";
                        sEndDate_S += gv_QcS.GetRowCellValue(i, "End_Date").ToString() == "" ? "_/" : DateTime.Parse(gv_QcS.GetRowCellValue(i, "End_Date").ToString()).ToString("yyyyMMdd") + "_/";
                        sFileURL += gv_QcS.GetRowCellValue(i, "File_URL").ToString() + "_/";
                        sOrderNo_S += gv_QcS.GetRowCellValue(i, "Order_No").ToString() + "_/";
                    }
                }

                //S1
                for(int i = 0; i < dt_S1.Rows.Count; i++)
                {
                    sResultNo_S1 += dt_S1.Rows[i]["Result_No"].ToString() + "_/";
                    sItem_Code += dt_S1.Rows[i]["Item_Code"].ToString() + "_/";
                    sQty += dt_S1.Rows[i]["Qty"].ToString() + "_/";
                    sFailCode_S1 += dt_S1.Rows[i]["Fail_Code"].ToString() + "_/";
                    sQcBigo_S1 += dt_S1.Rows[i]["Qc_Bigo"].ToString() + "_/";
                    sOrderNo_S1 += dt_S1.Rows[i]["Order_No"].ToString() + "_/";
                }

                //S2
                for(int i = 0; i < dt_S2.Rows.Count; i++)
                {
                    sResultNo_S2 += dt_S2.Rows[i]["Result_No"].ToString() + "_/";
                    sItemCode_S2 += dt_S2.Rows[i]["Item_Code"].ToString() + "_/";
                    sQCCode += dt_S2.Rows[i]["Qc_Code"].ToString() + "_/";
                    sPassCode_S2 += dt_S2.Rows[i]["Pass_Code"].ToString() + "_/";
                    sQcDate_S2 += DateTime.Parse(dt_S2.Rows[i]["Qc_Date"].ToString()).ToString("yyyyMMdd") + "_/";
                    sFailCode_S2 += dt_S2.Rows[i]["Fail_Code"].ToString() + "_/";
                    sQcBigo_S2 += dt_S2.Rows[i]["Qc_Bigo"].ToString() + "_/";
                    sOrderNo_S2 += dt_S2.Rows[i]["Order_No"].ToString() + "_/";
                }

                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "I");
                sp.AddParam("QC_No", txt_QcNo.Text);
                sp.AddParam("Qc_Date", dt_QcDate.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("User_Code", txt_UserCode.Text);
                sp.AddParam("Dept_Code", sDept_Code);
                sp.AddParam("Qc_Memo", txt_QcMemo.Text);
                //S
                sp.AddParam("ResultNo_S", sResultNo_S);
                sp.AddParam("OrderNo_S", sOrderNo_S);
                sp.AddParam("ItemCode_S", sItemCode_S);
                sp.AddParam("PassCode_S", sPassCode_S);
                sp.AddParam("EndDate_S", sEndDate_S);
                sp.AddParam("FileURL", sFileURL);
                //S1;
                sp.AddParam("ResultNo_S1", sResultNo_S1);
                sp.AddParam("ItemCode_S1", sItem_Code);
                sp.AddParam("Qty_S1", sQty);
                sp.AddParam("FailCode_S1", sFailCode_S1);
                sp.AddParam("QcBigo_S1", sQcBigo_S1);
                sp.AddParam("OrderNo_S1", sOrderNo_S1);
                //S2
                sp.AddParam("ResultNo_S2", sResultNo_S2);
                sp.AddParam("ItemCode_S2", sItemCode_S2);
                sp.AddParam("QC_Code", sQCCode);
                sp.AddParam("PassCode_S2", sPassCode_S2);
                sp.AddParam("QcDate_S2", sQcDate_S2);
                sp.AddParam("FailCode_S2", sFailCode_S2);
                sp.AddParam("QcBigo_S2", sQcBigo_S2);
                sp.AddParam("OrderNo_S2", sOrderNo_S2);

                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                txt_QcNo.Text = ret.ReturnDataSet.Tables[0].Rows[0]["QC_No"].ToString();
                btn_Save.sCHK = "Y";
                Search_H();

            }   
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_QcS, "공정검사");
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWorkQc");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "H");
                sp.AddParam("QC_No", txt_QcNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DbHelp.Clear_Panel(panel_H);
                DbHelp.Clear_Panel(panel_M);
                txt_UserCode.Text = GlobalValue.sUserID;
                gc_QcS.DataSource = null;
                gc_QcS2.DataSource = null;
                btn_Delete.sCHK = "Y";
                Search_H();

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        #endregion

    }
}
