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
    public partial class PopWorkResultForm : BaseForm
    {
        ReturnStruct ret = new ReturnStruct();
        string result_no = string.Empty;

        public PopWorkResultForm(string Result_No)
        {
            InitializeComponent();
            result_no = Result_No;
        }

        private void PopWorkResultForm_Load(object sender, EventArgs e)
        {
            Grid_Set();
            Search_Data();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Result_No", "실적번호", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Order_No", "수주번호", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Item_Code", "모품목코드", "100", false, true, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "SItem_Code", "품목코드", "100", false, true, true, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Item_Name", "모품목명", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "SItem_Name", "품목명", "100", false, false, true, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Ssize", "규격", "100", false, false, true, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Custom_Code", "거래처코드", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Short_Name", "거래처", "100", false, false, true, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Sort_No", "순번", "100", true, true, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "Out_No", "출고번호", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "In_No", "입고번호", "100", false, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "In_Sort", "순번", "100", true, false, false, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "MatSer_No", "시리얼넘버", "100", true, false, true, true);
            DbHelp.GridSet(Grid_ResultS, View_ResultS, "P_Price", "단가", "100", false, false, true, true);

            DbHelp.GridColumn_NumSet(View_ResultS, "P_Price", ForMat.SetDecimal("regWorkResult", "Price1"));

            Grid_ResultS.AddRowYN = false;
            Grid_ResultS.MouseWheelChk = false;
            Grid_ResultS.PopMenuChk = false;
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptWorkResult");
            sp.AddParam("Kind", "S");
            sp.AddParam("Result_No", result_no);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            Grid_ResultS.DataSource = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
            Grid_ResultS.RefreshDataSource();
            View_ResultS.BestFitColumns();
        }

        protected override void btnClose()
        {
            this.Close();
        }
    }
}
