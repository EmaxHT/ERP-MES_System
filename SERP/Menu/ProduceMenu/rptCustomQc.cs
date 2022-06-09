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

namespace SERP
{
    public partial class rptCustomQc : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public rptCustomQc()
        {
            InitializeComponent();
        }

        private void rptCustomQc_Load(object sender, EventArgs e)
        { 
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;

            Grid_Set();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_QcS, gv_QcS, "Custom_Code", "거래처코드", "100", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Short_Name", "거래처", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Code", "품목코드", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Name", "품목명", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qty", "입고수량", "80", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Qty", "불량수량", "80", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Name", "원인", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Code", "불량유형 코드", "80", false, false, false);

            DbHelp.GridColumn_NumSet(gv_QcS, "Qty", ForMat.SetDecimal("regMatIn!c", "Qty1"));
            DbHelp.GridColumn_NumSet(gv_QcS, "Fail_Qty", ForMat.SetDecimal("regMatIn!c", "Qty1"));
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            SqlParam sp = new SqlParam("sp_rptCustomQc");

            if (dt_From.Text.Replace("-", "").Length == 8)
                sp.AddParam("From", dt_From.Text.Replace("-", ""));
            else
                sp.AddParam("From", DateTime.MinValue.ToString("yyyyMMdd"));

            if (dt_To.Text.Replace("-", "").Length == 8)
                sp.AddParam("To", dt_To.Text.Replace("-", ""));
            else
                sp.AddParam("To", DateTime.MaxValue.ToString("yyyyMMdd"));

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }
            DataTable Table = Table_Set(DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]), "Qty");
            Table = Table_Set_2(DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]), "Fail_Qty"); 
            gc_QcS.DataSource = Table;
            gc_QcS.RefreshDataSource();
            gv_QcS.BestFitColumns();
            DbHelp.Footer_Set(gv_QcS, ForMat.SetDecimal("regMatIn", "Qty").ToString(), new string[] { "Qty", "Fail_Qty" });
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_QcS, "업체품질검사현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        // 거래처별로 입고수량 묶기
        private DataTable Table_Set(DataTable Table, string column_name)
        {
            if (Table.Rows.Count > 0)
            {
                List<string> customs = new List<string>();
                customs.Add(Table.Rows[0]["Short_Name"].NullString());

                for (int i = 0; i < Table.Rows.Count - 1; i++)
                {
                    if (Table.Rows[i]["Short_Name"].NullString() != Table.Rows[i + 1]["Short_Name"].NullString())
                        customs.Add(Table.Rows[i + 1]["Short_Name"].NullString());
                }

                for (int i = 0; i < customs.Count; i++)
                {
                    string filter = string.Format("Short_Name = '{0}'", customs[i]);
                    DataRow[] rows = Table.Select(filter);

                    decimal total = 0;

                    foreach (DataRow row in rows)
                    {
                        total += Convert.ToDecimal(row[column_name].NumString());
                    }

                    foreach (DataRow row in rows)
                    {
                        row[column_name] = total;
                    }
                }
            }

            return Table;
        }

        // 거래처내의 품목코드별로 묶기
        private DataTable Table_Set_2(DataTable Table, string column_name)
        {
            if (Table.Rows.Count > 0)
            {
                List<string> customs = new List<string>();
                List<string> items = new List<string>();
                customs.Add(Table.Rows[0]["Short_Name"].NullString());
                items.Add(Table.Rows[0]["Item_Code"].NullString());

                for (int i = 1; i < Table.Rows.Count - 1; i++)
                {
                    if (Table.Rows[i]["Short_Name"].NullString() != Table.Rows[i + 1]["Short_Name"].NullString())
                        if (Table.Rows[i]["Item_Code"].NullString() != Table.Rows[i + 1]["Item_Code"].NullString())
                        {
                            customs.Add(Table.Rows[i + 1]["Short_Name"].NullString());
                            items.Add(Table.Rows[i + 1]["Item_Code"].NullString());
                        }
                            
                }

                for (int i = 0; i < customs.Count; i++)
                {
                    string filter = string.Format("Short_Name = '{0}' AND Item_Code = '{1}'", customs[i], items[i]);
                    DataRow[] rows = Table.Select(filter);

                    decimal total = 0;

                    foreach (DataRow row in rows)
                    {
                        total += Convert.ToDecimal(row[column_name].NumString());
                    }

                    foreach (DataRow row in rows)
                    {
                        row[column_name] = total;
                    }
                }
            }

            return Table;
        }

        private void gv_QcS_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            string custom_name_1 = gv_QcS.GetRowCellValue(e.RowHandle1, "Custom_Code").NullString();
            string custom_name_2 = gv_QcS.GetRowCellValue(e.RowHandle2, "Custom_Code").NullString();

            if (e.Column.FieldName == "Fail_Name" || e.Column.FieldName == "Fail_Code")
            {
                string item_name_1 = gv_QcS.GetRowCellValue(e.RowHandle1, "Item_Name").NullString();
                string item_name_2 = gv_QcS.GetRowCellValue(e.RowHandle2, "Item_Name").NullString();
                if ((custom_name_1 == custom_name_2) && (item_name_1 == item_name_2))
                    e.Merge = e.CellValue1.Equals(e.CellValue2);
                else
                    e.Merge = false;

                e.Handled = true;
            }
            else if (e.Column.FieldName == "Fail_Qty")
            {
                string item_code_1 = gv_QcS.GetRowCellValue(e.RowHandle1, "Item_Code").NullString();
                string item_code_2 = gv_QcS.GetRowCellValue(e.RowHandle2, "Item_Code").NullString();
                if ((custom_name_1 == custom_name_2) && (item_code_1 == item_code_2))
                    e.Merge = e.CellValue1.Equals(e.CellValue2);
                else
                    e.Merge = false;

                e.Handled = true;
            }
            else
            {
                if (custom_name_1 == custom_name_2)
                    e.Merge = e.CellValue1.Equals(e.CellValue2);
                else
                    e.Merge = false;

                e.Handled = true;
            }
        }
    }
}
