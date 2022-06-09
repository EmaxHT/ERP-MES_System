using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SERP
{
    public partial class BaseForm : DevExpress.XtraEditors.XtraForm
    {
        public BaseForm()
        {
            InitializeComponent();
        }

        private void BaseForm_Load(object sender, EventArgs e)
        {
            foreach (Control control_M in this.Controls)
            {
                control_M.TextChanged += new EventHandler(Control_TextChange);
                foreach(Control control_S in control_M.Controls)
                {
                    control_S.TextChanged += new EventHandler(Control_TextChange);
                }
            }
        }

        protected virtual void Control_TextChange(object sender, EventArgs e) {}

        private void BaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            Type type = ((XtraForm)sender).ActiveControl.Parent.GetType();
            if (e.KeyCode == Keys.Enter && type != typeof(MemoEdit))
            {
                SendKeys.Send("{TAB}");
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F2) //추가
            {
                btnInsert();
            }
            else if (keyData == Keys.F5) //저장
            {
                btnSave();
            }
            else if (keyData == Keys.F9) //삭제
            {
                btnDelete();
            }
            else if (keyData == Keys.F7) //종료
            {
                btnClose();
            }
            else if(keyData == Keys.Escape)
            {
                btnClose();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected virtual void btnInsert() { }
        protected virtual void btnSave() { }
        protected virtual void btnDelete() { }
        protected virtual void btnClose() { }
    }
}