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
    public partial class Report_Quot_D : DevExpress.XtraReports.UI.XtraReport
    {
        //셀별 너비
        private float[] listWidth = new float[7] { 33F, 75F, 360F, 34F, 100F, 100F, 100F };
        public Report_Quot_D()
        {
            InitializeComponent();
        }

        public Report_Quot_D(DataSet ds, string preview = "N") : this()
        {
            SetInit(ds, preview);
        }

        private void SetInit(DataSet ds, string preview)
        {     
            DataTable dt = ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            DataTable dt2 = ds.Tables[2];

            label_QuotNo.Text = dt.Rows[0]["Quot_No"].ToString();
            label_Custom_Name.Text = dt.Rows[0]["Custom_Name"].ToString();
            label_CustomUser.Text = dt.Rows[0]["Custom_User"].ToString();
            label_CustomNumber.Text = dt.Rows[0]["Custom_Number"].ToString();
            label_CustomEmail.Text = dt.Rows[0]["Custom_Email"].ToString();
            label_QuotDate.Text = dt.Rows[0]["Quot_Date"].ToString();
            label_Addr1.Text = dt.Rows[0]["Addr1"].ToString();
            label_Addr2.Text = dt.Rows[0]["Addr2"].ToString();
            label_Owenr.Text = dt.Rows[0]["Owner"].ToString();
            label_ProjectTitle.Text = dt.Rows[0]["Project_Title"].ToString();
            label_UserName.Text = dt.Rows[0]["User_Name"].ToString();
            label_MoblieNo.Text = dt.Rows[0]["Mobile_No"].ToString();
            label_EMail.Text = dt.Rows[0]["E_Mail"].ToString();

            MemoryStream ms = new MemoryStream((byte[])dt.Rows[0]["Logo_Img"]);
            Image logo = Image.FromStream(ms);
            Picture_Logo.ImageSource = new ImageSource(logo);

            label_Bigo.Text = dt.Rows[0]["Quot_Bigo"].ToString();

            Amt.Text = dt.Rows[0]["Amt1"].ToString();
            Vat_Amt.Text = dt.Rows[0]["Vat_Amt"].ToString();
            Tot_Amt.Text = dt.Rows[0]["Tot_Amt"].ToString();

            //리스트 정보

            XRTable xrTable = new XRTable();
            xrTable.LocationF = new PointF(19.33F, 0F);
            xrTable.WidthF = 802F;
            xrTable.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);

            xrTable.BeginInit();

            for(int i = 0; i < dt1.Rows.Count; i++)
            {                
                xrTable.Rows.Add(Row_Header());
                xrTable.Rows.Add(Row_DataAdd(dt1.Rows[i]));
                xrTable.Rows.Add(Row_Part(true));

                string sItem_Code = dt1.Rows[i]["Item_Code"].NullString();
                DataRow[] dr_Part = dt2.Select("Item_Code = '" + sItem_Code + "'");
                DataTable dt_Part = new DataTable();
                if (dr_Part.Length > 0)
                    dt_Part = dr_Part.CopyToDataTable();

                for(int j = 0; j < dt_Part.Rows.Count; j++)
                    xrTable.Rows.Add(Row_DataAdd(dt_Part.Rows[j]));

                if (i + 1 < dt1.Rows.Count)
                    xrTable.Rows.Add(Row_Part(false));
            }

            xrTable.EndInit();

            Detail.Controls.Add(xrTable);

            ReportPrintTool print = new ReportPrintTool(this);
            this.ShowPrintMarginsWarning = false;

            if (preview == "Y")
                print.ShowPreview();
        }


        // S품목 칼럼명 행
        private XRTableRow Row_Header()
        {
            XRTableRow row = new XRTableRow();

            row.Cells.Add(Add_Cell("No", 33F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("품목", 75F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("상세", 360F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("수량", 34F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("소비자가", 100F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("공급단가", 100F, Color.SkyBlue, 10F, true));
            row.Cells.Add(Add_Cell("공급금액", 100F, Color.SkyBlue, 10F, true));

            return row;
        }

        //데이터(S, S1) 행
        private XRTableRow Row_DataAdd(DataRow dr)
        {
            XRTableRow row = new XRTableRow();

            for (int i = 0; i < dr.Table.Columns.Count - 1; i++) //Item_Code 뺌
            {
                if (dr[i].GetType() == typeof(decimal))
                    row.Cells.Add(Add_Cell(string.Format("{0:n0}", dr[i]), listWidth[i], Color.White, 9F, false, "1"));
                else if(i < 2)
                    row.Cells.Add(Add_Cell(string.Format("{0:n0}", dr[i]), listWidth[i], Color.White, 9F, false, "2")); //No, 품목은 TOP에 고정
                else if(i == 2)
                    row.Cells.Add(Add_Cell(string.Format("{0:n0}", dr[i]), listWidth[i], Color.White, 9F, false, "3")); //No, 품목은 TOP에 고정
                else
                    row.Cells.Add(Add_Cell(dr[i].NullString(), listWidth[i], Color.White, 9F, false));
            }

            return row;
        }

        //빈값 또는 PartList(true 이면 PartList로 표시)
        private XRTableRow Row_Part(bool PartChk)
        {
            XRTableRow row = new XRTableRow();

            if (PartChk)
                row.Cells.Add(Add_Cell("PartList", 802F, Color.AntiqueWhite, 10F, true));
            else
                row.Cells.Add(Add_Cell(" ", 802F, Color.White, 9F, false));

            return row;
        }

        //셀 추가
        private XRTableCell Add_Cell(string sText, float Width, Color BackColor, float fontSize, bool title, string amt = "0")
        {
            XRTableCell cell = new XRTableCell();

            cell.BackColor = BackColor;
            cell.BorderColor = Color.Black;
            cell.Borders = DevExpress.XtraPrinting.BorderSide.All;
            cell.Font = new Font("굴림", fontSize, title ? FontStyle.Bold : FontStyle.Regular);
            cell.Text = sText;
            cell.WidthF = Width;
            cell.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 2, 2);
            cell.Multiline = true;

            if (amt == "1")
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            else if (amt == "2")
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            else if (amt == "3")
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            else
                cell.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;


            return cell;
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
