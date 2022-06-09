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
    public partial class regMatPay : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regMatPay()
        {
            InitializeComponent();
        }

        private void regMatPay_Load(object sender, EventArgs e)
        {
            Grid_Set();

            Search();

            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panel_M);

            ForMat.sBasic_Set(this.Name, txt_PayNo);

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";

            dt_Pay.Focus();
        }

        private void Grid_Set()
        {
            //MotIn_M 그리드
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "Sort_No", "순번", "80", false, false, false, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_No", "입고번호", "130", false, true, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_Date", "입고일자", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_Amt", "입고금액", "100", false, false, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_VatAmt", "부가세액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_TotAmt", "합계금액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "In_PayAmt", "미결제금액", "100", true, false, true, true);
            DbHelp.GridSet(gc_MatPayM, gv_MatPayM, "Pay_Amt", "결제금액", "100", true, true, true, true);

            DbHelp.GridColumn_Help(gv_MatPayM, "In_No", "Y");
            RepositoryItemButtonEdit btn_InNo = (RepositoryItemButtonEdit)gv_MatPayM.Columns["In_No"].ColumnEdit;
            btn_InNo.Buttons[0].Click += new EventHandler(help_InNo);

            gv_MatPayM.Columns["In_No"].ColumnEdit = btn_InNo;

            DbHelp.GridColumn_NumSet(gv_MatPayM, "In_Amt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatPayM, "In_VatAmt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatPayM, "In_TotAmt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatPayM, "In_PayAmt", ForMat.SetDecimal(this.Name, "Amt1"));
            DbHelp.GridColumn_NumSet(gv_MatPayM, "Pay_Amt", ForMat.SetDecimal(this.Name, "Amt1"));

            gc_MatPayM.DeleteRowEventHandler += new EventHandler(Delete_S);

            gc_MatPayM.AddRowYN = true;
            gv_MatPayM.OptionsView.ShowAutoFilterRow = false;

            txt_TotalAMT.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            txt_TotalAMT.Properties.Mask.EditMask = "n" + ForMat.SetDecimal(this.Name, "Amt1");
            txt_TotalAMT.Properties.Mask.UseMaskAsDisplayFormat = true;
        }

        #region Help 함수

        private void help_InNo(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                XtraMessageBox.Show("거래처를 먼저 입력하세요");
                return;
            }

            int iRow = gv_MatPayM.GetFocusedDataSourceRowIndex();

            if(string.IsNullOrWhiteSpace(gv_MatPayM.GetRowCellValue(iRow, "In_Date").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("NotPay", "sp_Help_NotPay", "", "Y");
                HelpForm.Set_Value(txt_CustomCode.Text, "", "", "", "");
                HelpForm.sNotReturn = "Y";
                HelpForm.sLevelYN = "Y";
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    foreach(DataRow row in HelpForm.drReturn)
                    {
                        gv_MatPayM.SetRowCellValue(iRow, "In_No", row["In_No"].ToString());
                        if (!string.IsNullOrWhiteSpace(row["In_No"].ToString()))
                        {
                            Search_InNo(row["In_No"].ToString(), iRow);
                        }

                        if (iRow + 1 == gv_MatPayM.RowCount)
                            gv_MatPayM.AddNewRow();

                        iRow++;

                        gv_MatPayM.UpdateCurrentRow();
                    }

                    gv_MatPayM.DeleteRow(iRow);
                }
            }
        }

        #endregion

        #region 내부 함수

        private void Search()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatPay");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SH");
                sp.AddParam("Pay_No", txt_PayNo.Text);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt_H = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                DataTable dt_D = ret.ReturnDataSet.Tables[1];

                if(dt_H.Rows.Count > 0)
                {
                    DataRow dr_H = dt_H.Rows[0];
                    txt_CustomCode.Text = dr_H["Custom_Code"].ToString();
                    dt_Pay.Text = dr_H["Pay_Date"].ToString();
                    memo_Bigo.Text = dr_H["Pay_Memo"].ToString();

                    txt_RegDate.Text = dr_H["Reg_Date"].ToString();
                    txt_RegUser.Text = dr_H["Reg_User"].ToString();
                    txt_UpDate.Text = dr_H["Up_Date"].ToString();
                    txt_UpUser.Text = dr_H["Up_User"].ToString();
                }
                else
                {
                    txt_CustomCode.Text = "";
                    memo_Bigo.Text = "";

                    txt_RegDate.Text = "";
                    txt_RegUser.Text = "";
                    txt_UpDate.Text = "";
                    txt_UpUser.Text = "";
                }

                gc_MatPayM.DataSource = dt_D;

                Tot_Sum();
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;                    
            }
        }

        private void Search_InNo(string sIn_No, int iRow)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regMatPay");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "SI");
                sp.AddParam("In_No", sIn_No);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                DataTable dt_In = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);

                if(dt_In.Rows.Count > 0)
                {
                    DataRow dr_In = dt_In.Rows[0];
                    gv_MatPayM.SetRowCellValue(iRow, "In_Date", dr_In["In_Date"].ToString());
                    gv_MatPayM.SetRowCellValue(iRow, "In_Amt", dr_In["In_Amt"].NumString());
                    gv_MatPayM.SetRowCellValue(iRow, "In_VatAmt", dr_In["In_VatAmt"].NumString());
                    gv_MatPayM.SetRowCellValue(iRow, "In_TotAmt", dr_In["In_TotAmt"].NumString());
                    gv_MatPayM.SetRowCellValue(iRow, "In_PayAmt", dr_In["In_PayAmt"].NumString());
                    gv_MatPayM.SetRowCellValue(iRow, "Pay_Amt", dr_In["In_PayAmt"].NumString());
                }
                else
                {
                    gv_MatPayM.SetRowCellValue(iRow, "In_Date", "");
                    gv_MatPayM.SetRowCellValue(iRow, "In_Amt", "");
                    gv_MatPayM.SetRowCellValue(iRow, "In_VatAmt", "");
                    gv_MatPayM.SetRowCellValue(iRow, "In_TotAmt", "");
                    gv_MatPayM.SetRowCellValue(iRow, "In_PayAmt", "");
                    gv_MatPayM.SetRowCellValue(iRow, "Pay_Amt", "");
                }

            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Tot_Sum()
        {
            decimal dPayAmt = 0;
            for(int i = 0; i < gv_MatPayM.RowCount; i++)
            {
                dPayAmt += decimal.Parse(gv_MatPayM.GetRowCellValue(i, "Pay_Amt").NumString());
            }

            txt_TotalAMT.Text = dPayAmt.NumString();
        }

        private void Delete_S(object sender, EventArgs e)
        {
            int iRow = gv_MatPayM.GetFocusedDataSourceRowIndex();

            try
            {
                SqlParam sp = new SqlParam("sp_regMatPay");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DS");
                sp.AddParam("Pay_No", txt_PayNo.Text);
                sp.AddParam("In_No", gv_MatPayM.GetRowCellValue(iRow, "In_No").ToString());
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

        #region 그리드 이벤트
        private void gv_MatPayM_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            if (e.Column.FieldName == "In_No")
            {
                Search_InNo(e.Value.ToString(), e.RowHandle);
            }
            else if (e.Column.FieldName == "Pay_Amt")
            {
                decimal dInPayAmt = decimal.Parse(gv_MatPayM.GetRowCellValue(e.RowHandle, "In_PayAmt").NumString());
                if(decimal.Parse(e.Value.NumString()) > dInPayAmt)
                {
                    gv_MatPayM.SetRowCellValue(e.RowHandle, "Pay_Amt", dInPayAmt.NumString());
                }

                Tot_Sum();
            }

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }

        private void gv_MatPayM_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0)
                return;

            if(gv_MatPayM.GetDataRow(e.FocusedRowHandle).RowState == DataRowState.Added)
            {
                gv_MatPayM.Columns["In_No"].OptionsColumn.AllowEdit = true;
                gv_MatPayM.Columns["In_No"].OptionsColumn.ReadOnly = false;
            }
            else
            {
                gv_MatPayM.Columns["In_No"].OptionsColumn.AllowEdit = false;
                gv_MatPayM.Columns["In_No"].OptionsColumn.ReadOnly = true;
            }
        }

        #endregion

        #region 거래처 정보

        private void txt_CustomCode_EditValueChanged(object sender, EventArgs e)
        {
            txt_CustomCodeNM.Text = PopHelpForm.Return_Help("sp_Help_Custom_IN", txt_CustomCode.Text);
        }

        private void txt_CustomCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
                txt_CustomCode_Properties_ButtonClick(sender, null);
            
        }

        private void txt_CustomCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_CustomCodeNM.Text))
            {
                PopHelpForm help_form = new PopHelpForm("Custom", "sp_Help_Custom_IN", txt_CustomCode.Text, "N");
                if(help_form.ShowDialog() == DialogResult.OK)
                {
                    txt_CustomCode.Text = help_form.sRtCode;
                    txt_CustomCodeNM.Text = help_form.sRtCodeNm;
                }
            }
        }

        #endregion

        #region 버튼 이벤트

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm help_form = new PopHelpForm("MatPay", "sp_Help_MatPay", "", "N");
            help_form.sNotReturn = "Y";
            help_form.sLevelYN = "Y";
            btn_Select.clsWait.CloseWait();
            if (help_form.ShowDialog() == DialogResult.OK)
            {
                btn_Select.clsWait.ShowWait(this.FindForm());
                txt_PayNo.Text = help_form.sRtCode;
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

            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panel_M);

            Search();

            dt_Pay.Select();

            btn_Insert.sUpdate = "N";
            btn_Close.sUpdate = "N";
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sInNo = "", sPayAmt = "";

                for(int i = 0; i < gv_MatPayM.RowCount; i++)
                {
                    if (gv_MatPayM.GetRowCellValue(i, "In_No").ToString() != "")
                    {
                        sInNo += gv_MatPayM.GetRowCellValue(i, "In_No").ToString() + "_/";
                        sPayAmt += gv_MatPayM.GetRowCellValue(i, "Pay_Amt").NumString() + "_/";
                    }
                }

                SqlParam sp = new SqlParam("sp_regMatPay");
                sp.AddParam("Kind", "I");
                sp.AddParam("Pay_No", txt_PayNo.Text);
                sp.AddParam("Pay_Date", dt_Pay.DateTime.ToString("yyyyMMdd"));
                sp.AddParam("Custom_Code", txt_CustomCode.Text);
                sp.AddParam("Pay_Memo", memo_Bigo.Text);
                sp.AddParam("InNo", sInNo);
                sp.AddParam("PayAmt", sPayAmt);
                sp.AddParam("Reg_User", GlobalValue.sUserID);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                btn_Save.sCHK = "Y";
                txt_PayNo.Text = ret.ReturnDataSet.Tables[0].Rows[0]["Pay_No"].ToString();
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
                SqlParam sp = new SqlParam("sp_regMatPay");
                sp.AddParam("Kind", "D");
                sp.AddParam("Delete_D", "DH");
                sp.AddParam("Pay_No", txt_PayNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                btn_Delete.sCHK = "Y";
                txt_PayNo.Text = "";
                txt_TotalAMT.EditValue = 0;
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
            FileIF.Excel_Down(gc_MatPayM, "자재결제등록");
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


    }
}
