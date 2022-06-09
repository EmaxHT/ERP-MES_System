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
using System.Security.Cryptography;
using System.IO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace SERP
{
    public partial class PopWorkQcForm : BaseForm
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();
        public DataTable dt_S1 = null;
        public DataRow[] dr_S1 = null;

        public string sQc_No = "";
        public string sResult_No = "";
        public string sItem_Code = "", sQty = "", sItem_Name = "", sCustomer1 = "", sOrder_No = "";

        public int iQty = 0;

        private string MenuNm = "regWorkQc";

        public PopWorkQcForm()
        {
            InitializeComponent();
        }

        private void PopWorkQcForm_Load(object sender, EventArgs e)
        {
            label_Name.Text = ForMat.Return_FormNM(MenuNm);

            gc_QcS1.AddRowYN = true;

            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Sort_No", "순번", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Result_No", "입고번호", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Order_No", "수주번호", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Item_Code", "품목코드", "", false, false, false, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Qty", "불량수량", "80", true, true, true, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Fail_Code", "불량사유코드", "80", false, true, true, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Fail_CodeNM", "불량사유", "100", false, false, true, true);
            DbHelp.GridSet(gc_QcS1, gv_QcS1, "Qc_Bigo", "비고", "100", false, true, true, true);

            DbHelp.GridColumn_NumSet(gv_QcS1, "Qty", ForMat.SetDecimal(MenuNm, "Qty1"));

            DbHelp.GridColumn_Help(gv_QcS1, "Fail_Code", "Y");
            RepositoryItemButtonEdit button_HelpFail = (RepositoryItemButtonEdit)gv_QcS1.Columns["Fail_Code"].ColumnEdit;
            button_HelpFail.Buttons[0].Click += new EventHandler(grid_HelpFail);
            gv_QcS1.Columns["Fail_Code"].ColumnEdit = button_HelpFail;

            //gc_QcS1.DeleteRowEventHandler += new EventHandler(Delete_S1);

            gv_QcS1.OptionsView.ShowAutoFilterRow = false;

            txt_ItemCode.Text = sItem_Code;
            txt_ItemName.Text = sItem_Name;
            txt_ResultNo.Text = sResult_No;
            txt_Qty.Text = sQty;
            txt_Customer1.Text = sCustomer1;

            Search_S1();
        }

        private void Search_S1()
        {
            try
            {
                if (dr_S1 == null || dr_S1.Length == 0)
                {
                    SqlParam sp = new SqlParam("sp_regWorkQc");
                    sp.AddParam("Kind", "S");
                    sp.AddParam("Search_D", "SD");
                    sp.AddParam("QC_No", sQc_No);
                    sp.AddParam("Item_Code", sItem_Code);
                    sp.AddParam("Result_No", sResult_No);
                    sp.AddParam("Order_No", sOrder_No);

                    ret = DbHelp.Proc_Search(sp);

                    if (ret.ReturnChk == 0)
                    {
                        gc_QcS1.DataSource = ret.ReturnDataSet.Tables[0];
                    }
                    else
                    {
                        XtraMessageBox.Show(ret.ReturnMessage);
                        return;
                    }
                }
                else
                {
                    DataTable dtS1 = DbHelp.Return_DT(gv_QcS1);

                    foreach (DataRow row in dr_S1)
                    {
                        DataRow dr = dtS1.NewRow();
                        for (int i = 0; i < dr.ItemArray.Length; i++)
                        {
                            dr[i] = row[i];
                        }

                        dtS1.Rows.Add(dr);
                    }

                    dtS1.DefaultView.Sort = "Sort_No ASC";
                    gc_QcS1.DataSource = dtS1;
                    gv_QcS1.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void grid_HelpFail(object sender, EventArgs e)
        {
            int iRow = gv_QcS1.GetFocusedDataSourceRowIndex();

            if (string.IsNullOrWhiteSpace(gv_QcS1.GetRowCellValue(iRow, "Fail_CodeNM").ToString()))
            {
                PopHelpForm HelpForm = new PopHelpForm("General", "sp_Help_General", "30070", gv_QcS1.GetRowCellValue(iRow, "Fail_Code").ToString(), "N");
                if (HelpForm.ShowDialog() == DialogResult.OK)
                {
                    gv_QcS1.SetRowCellValue(iRow, "Fail_Code", HelpForm.sRtCode);
                    gv_QcS1.SetRowCellValue(iRow, "Fail_CodeNM", HelpForm.sRtCodeNm);
                }
            }
        }

        private void gc_QcS1_EditorKeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                if(gv_QcS1.FocusedColumn.FieldName == "Fail_Code")
                {
                    grid_HelpFail(null, null);
                }
            }
        }

        private void gv_QcS1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Fail_Code")
            {
                gv_QcS1.SetRowCellValue(e.RowHandle, "Fail_CodeNM", PopHelpForm.Return_Help("sp_Help_General", e.Value.ToString(), "30070", ""));
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            for (int i = 0; i < gv_QcS1.RowCount; i++)
            {
                if (gv_QcS1.GetRowCellValue(i, "Fail_Code").ToString() == "")
                {
                    gv_QcS1.DeleteRow(i);
                    i--;
                }
                else
                {
                    iQty += (int)decimal.Parse(gv_QcS1.GetRowCellValue(i, "Qty").NumString());
                }
                gv_QcS1.SetRowCellValue(i, "Sort_No", (i + 1).ToString());
                gv_QcS1.SetRowCellValue(i, "Result_No", sResult_No);
                gv_QcS1.SetRowCellValue(i, "Order_No", sOrder_No);
                gv_QcS1.SetRowCellValue(i, "Item_Code", sItem_Code);

                gv_QcS1.UpdateCurrentRow();
            }
            dt_S1 = gc_QcS1.DataSource as DataTable;

            this.Close();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }
    }
}
