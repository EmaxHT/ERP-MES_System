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
    public partial class BasicForm : DevExpress.XtraEditors.XtraUserControl
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public BasicForm()
        {
            InitializeComponent();
        }

        //메뉴 닫기 버튼
        private void btnClose_Click(object sender, EventArgs e)
        {
            
        }
    }
}
