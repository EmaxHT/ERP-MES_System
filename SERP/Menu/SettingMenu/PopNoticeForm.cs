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
using System.IO;

namespace SERP
{
    public partial class PopNoticeForm : BaseForm
    {
        DataTable all_notice = new DataTable();

        ReturnStruct ret = new ReturnStruct();
        string notice_no = string.Empty;

        public PopNoticeForm(string no)
        {
            InitializeComponent();

            notice_no = no;
        }

        private void PopNoticeForm_Load(object sender, EventArgs e)
        {
            Search_Data(notice_no);

            SqlParam sp = new SqlParam("sp_regNotice");
            sp.AddParam("Kind", "S");

            all_notice = (DbHelp.Proc_Search(sp)).ReturnDataSet.Tables[0];
        }

        private void Search_Data(string notice_no)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regNotice");
                sp.AddParam("Kind", "S");
                sp.AddParam("Notice_No", notice_no);

                ret = DbHelp.Proc_Search(sp);

                DataTable table = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);
                DataRow row = table.Select("Notice_No = '" + notice_no + "'")[0];

                if (ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                dt_From.Text = row["Notice_STime"].NullString();
                dt_To.Text = row["Notice_ETime"].NullString();
                txt_Kind.Text = row["Notice_Part"].NullString();
                txt_Title.Text = row["Notice_Title"].NullString();
                Memo_Notice.Text = row["Notice_Memo"].NullString();
                txt_File.ToolTip = row["Add_File"].NullString();
                txt_File.Text = Path.GetFileName(txt_File.ToolTip);

            }
            catch (Exception)
            {
                this.Close();
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
            catch (Exception)
            {
                XtraMessageBox.Show("파일이 존재하지 않거나 본래 파일정보가 변경되었습니다.\n다운로드에 실패하였습니다.");
                return;
            }
        }

        private void txt_Kind_EditValueChanged(object sender, EventArgs e)
        {
            txt_KindNM.Text = PopHelpForm.Return_Help("sp_Help_General", txt_Kind.Text, "9110");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_Post_Click(object sender, EventArgs e)
        {
            try
            {
                Notice_Move("Post");
            }
            catch (Exception)
            {
                return;
            }
        }

        private void btn_Pre_Click(object sender, EventArgs e)
        {
            try
            {
                Notice_Move("Pre");
            }
            catch (Exception)
            {
                return; // 기간내 공지가 하나도 존재하지 않는 경우
            }
        }

        private void PopNoticeForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Shift && e.KeyCode == Keys.Left)
            {
                Notice_Move("Post");
            }
            else if(!e.Shift && e.KeyCode == Keys.Right)
            {
                Notice_Move("Pre");
            }
        }

        private void Notice_Move(string way)
        {
            //string today = DateTime.Now.ToString("yyyy-MM-dd");
            //int notice_count = all_notice.Select("'" + today + "' >= Notice_STime AND '" + today + "' <= Notice_ETime").Count();

            //if (notice_count > 1)
            //{
            //    DataTable notices = all_notice.Select("'" + today + "' >= Notice_STime AND '" + today + "' <= Notice_ETime").CopyToDataTable();
            //    DataRow present_notice = notices.Select("Notice_No = '" + notice_no + "'")[0];
            DataRow present_notice = all_notice.Select("Notice_No = '" + notice_no + "'")[0];

            int present_idx = all_notice.Rows.IndexOf(present_notice);      // 현재 Index
            int block = (way == "Pre") ? 1 : -1;                            // 인덱스 이동 방향
            int limit = (way == "Pre") ? (all_notice.Rows.Count - 1) : 0;   // 인덱스 경계치 (Post = 0, Pre = 전체 공지 수 - 1)
            int tip_index = (limit == 0) ? (all_notice.Rows.Count - 1) : 0; // 현재 인덱스가 경계치일 경우 반대편 끝 인덱스 [예) 현재 인덱스 0 -> 이전 버튼 클릭 => 마지막 인덱스로 이동]

            if (present_idx == limit)
            {
                string tip_notice = all_notice.Rows[tip_index]["Notice_No"].NullString();
                notice_no = tip_notice;
                Search_Data(tip_notice);
            }
            else
            {
                string next_notice = all_notice.Rows[present_idx + block]["Notice_No"].NullString();
                notice_no = next_notice;
                Search_Data(next_notice);
            }
            //}
        }
    }
}