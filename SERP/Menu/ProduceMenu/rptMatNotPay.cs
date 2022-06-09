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
    public partial class rptMatNotPay : DevExpress.XtraEditors.XtraUserControl
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public rptMatNotPay()
        {
            InitializeComponent();
        }

        private void rptMatNotPay_Load(object sender, EventArgs e)
        {

            Grid_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드

            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Item_Memo", "입고번호", "120", false, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Sort_No", "입고일자", "125", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Q_Unit", "거래처", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "입고금액", "150", true, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "부가세액", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "합계금액", "150", true, true, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "결제금액", "150", false, false, true, true);
            DbHelp.GridSet(gc_MatInM, gv_MatInM, "Amt", "미결제금액", "150", true, true, true, true);
        }
    }
}
