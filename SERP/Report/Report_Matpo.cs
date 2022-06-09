using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using System.IO;
using DevExpress.XtraPrinting.Drawing;

namespace SERP
{
    public partial class Report_Matpo : DevExpress.XtraReports.UI.XtraReport
    {
        public Report_Matpo()
        {
            InitializeComponent();
        }

        public Report_Matpo(DataSet ds, string preview = "N") : this()
        {
            SetInit(ds, preview);
        }

        private void SetInit(DataSet ds, string preview)
        {
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds.Tables[1];

            label_Addr.Text = dt.Rows[0]["Addr"].ToString();
            label_Custom.Text = dt.Rows[0]["Custom"].ToString();
            label_Gluesys.Text = dt.Rows[0]["Gluesys"].ToString();
            label_Custom_Name.Text = dt.Rows[0]["Custom_Name"].ToString();
            label_PoDate.Text = dt.Rows[0]["Po_Date"].ToString();

            MemoryStream ms = new MemoryStream((byte[])dt.Rows[0]["Logo_Img"]);
            Image logo = Image.FromStream(ms);
            Picture_Logo.ImageSource = new ImageSource(logo);

            label_Bigo.Text = dt.Rows[0]["Matpo_Memo"].ToString();
            
            string sVat_Add = dt.Rows[0]["Vat_Add"].ToString();

            Amt.Text = dt.Rows[0]["Amt"].ToString();
            label_Po_Amt.Text = ForMat.Return_NumKr(long.Parse(dt.Rows[0]["Amt_K"].NumString())) + "원정 " + sVat_Add;

            label_Tot.Text = "합                 계       " + sVat_Add;


            for (int i = 0; i < Table_S1.Cells.Count; i++)
            {
                if (dt2.Columns[Table_S1.Cells[i].Name].DataType == typeof(decimal))
                {
                    Table_S1.Cells[i].DataBindings.Add("Text", null, dt2.Columns[Table_S1.Cells[i].Name].ColumnName, "{0:#,#}");
                }
                else
                {
                    Table_S1.Cells[i].DataBindings.Add("Text", null, dt2.Columns[Table_S1.Cells[i].Name].ColumnName);
                }
            }
            this.DataSource = dt2;

            ReportPrintTool print = new ReportPrintTool(this);
            this.ShowPrintMarginsWarning = false;

            if (preview == "Y")
                print.ShowPreview();
        }

        private void ReportFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //if (PrintingSystem.PageCount == 0)
            //{
            //    e.Cancel = true;
            //}
        }
    }
}
