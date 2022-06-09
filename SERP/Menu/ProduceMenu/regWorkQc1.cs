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
    public partial class regWorkQc1 : BaseReg
    {
        public regWorkQc1()
        {
            InitializeComponent();
        }

        private void regWorkQc1_Load(object sender, EventArgs e)
        {
            Grid_Set();

            dt_QcDate.Focus();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_Date", "입고일자", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qc_No", "검사번호", "100", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_No", "입고번호", "100", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "In_Sort", "입고순번", "80", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Code", "품목코드", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Item_Name", "품목명", "100", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Tot_Amt", "외주처", "100", true, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Ssize", "규격", "150", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qty", "입고수량", "80", false, false, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Qty", "불량수량", "80", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Name", "불량유형", "100", false, true, true);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Fail_Code", "불량유형 코드", "80", false, false, false);
            DbHelp.GridSet(gc_QcS, gv_QcS, "Qc_Bigo", "비고", "150", false, true, true);
            
        }

        private void txt_UserCode_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }
    }
}
