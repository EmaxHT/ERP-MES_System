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
using DevExpress.XtraEditors.Controls;
using System.IO;

namespace SERP
{
    public partial class regNotice : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();

        public regNotice()
        {
            InitializeComponent();
        }

        private void regNotice_Load(object sender, EventArgs e)
        {
            Set_Default();
            ForMat.sBasic_Set(this.Name, txt_NoticeNo);

            dt_From.DateTime = DateTime.Now;
            dt_To.DateTime = DateTime.Now;

            Set_Grid();

            Search_Data(txt_NoticeNo.Text);
        }

        private void Set_Default()
        {
            string[] kind_codes = DbHelp.Set_Default("9110");

            if (kind_codes != null)
            {
                txt_Kind.Text = kind_codes[0];
                txt_KindNM.Text = kind_codes[1];
            }
        }

        private void Set_Grid()
        {
            gc_Dept.AddRowYN = false;

            DbHelp.GridSet(gc_Dept, gv_Dept, "Dept_Ck", "체크", "80", false, true, true, true);
            DbHelp.GridSet(gc_Dept, gv_Dept, "Dept_Code", "부서코드", "80", false, false, true, true);
            DbHelp.GridSet(gc_Dept, gv_Dept, "Dept_Name", "부서명", "100", false, false, true, true);

            DbHelp.GridColumn_CheckBox(gv_Dept, "Dept_Ck");

            gv_Dept.OptionsView.ShowAutoFilterRow = false;
            gc_Dept.PopMenuChk = false;
            gc_Dept.MouseWheelChk = false;
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            PopHelpForm HelpForm = new PopHelpForm("Notice", "sp_Help_Notice", "", "N");
            HelpForm.sLevelYN = "Y";
            HelpForm.sNotReturn = "Y";
            if (HelpForm.ShowDialog() == DialogResult.OK)
            {
                txt_NoticeNo.Text = HelpForm.sRtCode;

                Search_Data(txt_NoticeNo.Text);

                btn_Insert.sUpdate = "N";
                btn_Close.sUpdate = "N";
            }
        }

        private void btn_Insert_Click(object sender, EventArgs e)
        {
            btn_Save_Click(null, null);
            btn_Save.sCHK = "Y";

            //DbHelp.Clear_Panel(panelControl1);
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panelControl3);


            dt_From.DateTime = DateTime.Now;
            dt_To.DateTime = DateTime.Now;

            Set_Default();

            txt_File.ToolTip = string.Empty;
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            Delete_Data();
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            Save_Data();
            btn_Save.sCHK = "Y";
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Search_Data(string notice_no)
        {
            SqlParam sp = new SqlParam("sp_regNotice");
            sp.AddParam("Kind", "S");
            sp.AddParam("Notice_No", notice_no);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            DataTable dt_Detp = ret.ReturnDataSet.Tables[1];

            if (ret.ReturnDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow row = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]).Rows[0];

                txt_NoticeNo.Text = row["Notice_No"].ToString();
                dt_From.Text = row["Notice_STime"].ToString();
                dt_To.Text = row["Notice_ETime"].ToString();
                txt_Kind.Text = row["Notice_Part"].ToString();
                txt_Title.Text = row["Notice_Title"].ToString();
                txt_Memo.Text = row["Notice_Memo"].ToString();
                txt_RegDate.Text = row["Notice_Date"].ToString();
                txt_RegUser.Text = row["Notice_User"].ToString();
                txt_File.ToolTip = row["Add_File"].ToString();
                txt_File.Text = Path.GetFileName(txt_File.ToolTip);
            }

            gc_Dept.DataSource = dt_Detp;
        }

        private void Save_Data()
        {
            if(Check_Data())
            {
                DataRow[] row_Dept = (gc_Dept.DataSource as DataTable).Select("Dept_Ck = 'Y'");

                if(row_Dept.Length == 0)
                {
                    XtraMessageBox.Show("선택된 부서가 없습니다");
                    return;
                }

                string sDept = "";

                for(int i = 0; i< row_Dept.Length; i++)
                {
                    sDept += row_Dept[i]["Dept_Code"].ToString() + "_/";
                }

                SqlParam sp = new SqlParam("sp_regNotice");

                sp.AddParam("Kind", "I");
                sp.AddParam("Notice_No", txt_NoticeNo.Text);
                sp.AddParam("Notice_STime", dt_From.Text);
                sp.AddParam("Notice_ETime", dt_To.Text);
                sp.AddParam("Notice_Part", txt_Kind.Text);
                sp.AddParam("Notice_Title", txt_Title.Text);
                sp.AddParam("Notice_Memo", txt_Memo.Text);
                sp.AddParam("Notice_User", GlobalValue.sUserID);
                sp.AddParam("Add_File", txt_File.ToolTip);

                sp.AddParam("DeptCode", sDept);

                if (!string.IsNullOrWhiteSpace(txt_File.Text))
                {
                    FileIF.Set_URL();
                    FileIF.FTP_Upload_File(txt_File.Text, FileIF.FTP_URL + txt_File.Text);
                }

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                string notice_no = ret.ReturnDataSet.Tables[0].Rows[0][0].NullString();

                Search_Data(notice_no);
                
            }
        }

        private bool Check_Data()
        {
            if (string.IsNullOrWhiteSpace(txt_Title.Text))
            {
                XtraMessageBox.Show("공지제목은 필수값입니다.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(dt_From.Text) || string.IsNullOrWhiteSpace(dt_To.Text))
            {
                XtraMessageBox.Show("공지 기간은 필수값입니다.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txt_KindNM.Text))
            {
                XtraMessageBox.Show("구분값은 필수값입니다.");
                return false;
            }

            return true;
        }

        private void Delete_Data()
        {
            if (!string.IsNullOrWhiteSpace(txt_NoticeNo.Text))
            {
                SqlParam sp = new SqlParam("sp_regNotice");
                sp.AddParam("Kind", "D");
                sp.AddParam("Notice_No", txt_NoticeNo.Text);

                ret = DbHelp.Proc_Save(sp);

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }
            }

            FileIF.FTP_Delete_File(txt_File.ToolTip);

            //DbHelp.Clear_Panel(panelControl1);
            DbHelp.Clear_Panel(panel_H);
            DbHelp.Clear_Panel(panelControl3);
            txt_File.ToolTip = string.Empty;

            dt_From.DateTime = DateTime.Now;
            dt_To.DateTime = DateTime.Now;

            btn_Delete.sCHK = "Y";
        }

        #region 공통코드
        private void txt_Kind_EditValueChanged(object sender, EventArgs e)
        {
            txt_KindNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_Kind.Text, "9110");
        }

        private void txt_Kind_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                txt_Kind_Properties_ButtonClick(sender, null);
        }

        private void txt_Kind_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            PopHelpForm Help_Form = new PopHelpForm("General", "sp_Help_General", "9110", txt_Kind.Text, "N");
            if (Help_Form.ShowDialog() == DialogResult.OK)
            {
                txt_Kind.Text = Help_Form.sRtCode;
                txt_KindNM.Text = Help_Form.sRtCodeNm;
            }
        }
        #endregion

        #region 상속 함수

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }

        protected override void btnDelete()
        {
            btn_Delete.PerformClick();
        }

        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnInsert()
        {
            btn_Insert.PerformClick();
        }

        protected override void btnSave()
        {
            btn_Save.PerformClick();
        }

        protected override void Control_TextChange(object sender, EventArgs e)
        {
            base.Control_TextChange(sender, e);

            btn_Insert.sUpdate = "Y";
            btn_Close.sUpdate = "Y";
        }


        #endregion

        #region 파일 다운, 업로드
        private void txt_File_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == ButtonPredefines.Search)
            {
                XtraOpenFileDialog xtraOpenFileDialog = new XtraOpenFileDialog();

                xtraOpenFileDialog.InitialDirectory = @"C:\";

                FileIF.Set_URL();

                if (xtraOpenFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txt_File.Text = Path.GetFileName(xtraOpenFileDialog.FileName);
                    txt_File.ToolTip = FileIF.FTP_URL + txt_File.Text;
                }
            }
            else if (e.Button.Kind == ButtonPredefines.Close)
            {
                if(!string.IsNullOrWhiteSpace(txt_File.Text) && !string.IsNullOrWhiteSpace(txt_File.ToolTip))
                {
                    if (XtraMessageBox.Show("파일을 삭제하시겠습니까?", "삭제", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
                    {
                        // 파일 삭제
                        FileIF.FTP_Delete_File(txt_File.ToolTip);

                        txt_File.Text = string.Empty;
                        txt_File.ToolTip = string.Empty;

                        Save_Data();
                        XtraMessageBox.Show("삭제되었습니다.");
                    }
                }
            }
        }

        private void btn_DownLoad_Click(object sender, EventArgs e)
        {
            try
            {
                string sFile_URL = txt_File.ToolTip;

                XtraSaveFileDialog fileSave = new XtraSaveFileDialog();
                fileSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                fileSave.FileName = txt_File.Text;
                fileSave.Filter = "All Files (*.*)|*.*"; //Excel File 97~2003 (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx|

                if (fileSave.ShowDialog() == DialogResult.OK)
                {
                    FileIF.Set_URL();
                    FileIF.FTP_Download_File(sFile_URL, fileSave.FileName);
                    XtraMessageBox.Show("다운로드가 완료되었습니다.");
                }
            }
            catch(Exception)
            {
                XtraMessageBox.Show("파일이 존재하지 않거나 본래 파일정보가 변경되었습니다.\n다운로드에 실패하였습니다.");
                return;
            }
        }
        #endregion
    }
}
