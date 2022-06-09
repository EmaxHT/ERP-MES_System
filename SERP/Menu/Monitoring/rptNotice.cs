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
using DevExpress.XtraEditors.Repository;
using System.IO;

namespace SERP
{
    public partial class rptNotice : BaseReg
    {

        ReturnStruct ret = new ReturnStruct();

        public rptNotice()
        {
            InitializeComponent();
        }

        private void rptNotice_Load(object sender, EventArgs e)
        {
            Grid_Set();

            btn_Select_Click(sender, e);
            Timer_Control.Tick += btn_Select_Click;
            Timer_Control.Interval = 1000 * 60 * 5;
            Timer_Control.Start();
        }

        private void Grid_Set()
        {
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_Date", "등록일자", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_User", "등록자", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_STime", "공지기간(시작)", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_ETime", "공지기간(종료)", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_Part_Name", "구분", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_Title", "공지제목", "80", false, false, true);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Notice_Memo", "공지내용", "80", false, false, false);
            DbHelp.GridSet(Grid_Notice, View_Notice, "Add_File", "첨부파일URI", "80", false, false, false);
            DbHelp.GridSet(Grid_Notice, View_Notice, "File_Down", "첨부파일", "80", false, true, true);

            RepositoryItemButtonEdit btn_File = new RepositoryItemButtonEdit();
            btn_File.Buttons[0].Kind = DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph;
            btn_File.Buttons[0].Caption = "첨부파일";
            btn_File.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            btn_File.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            btn_File.Buttons[0].Click += new EventHandler(btn_FileDown);
            View_Notice.Columns["File_Down"].ColumnEdit = btn_File;

            Grid_Notice.AddRowYN = false;
            Grid_Notice.PopMenuChk = false;
            Grid_Notice.MouseWheelChk = false;
        }

        private void btn_FileDown(object sender, EventArgs e)
        {
            try
            {
                string uri = View_Notice.GetFocusedRowCellValue("Add_File").NullString();

                if (!string.IsNullOrWhiteSpace(uri))
                {
                    FileIF.Set_URL();

                    XtraSaveFileDialog fileSave = new XtraSaveFileDialog();
                    fileSave.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    fileSave.FileName = Path.GetFileName(uri);
                    fileSave.Filter = "All Files (*.*)|*.*"; //Excel File 97~2003 (*.xls)|*.xls|Excel File (*.xlsx)|*.xlsx|

                    if (fileSave.ShowDialog() == DialogResult.OK)
                    {
                        FileIF.FTP_Download_File(uri, fileSave.FileName);
                        XtraMessageBox.Show("다운로드가 완료되었습니다.");
                    }
                }
                else
                {
                    XtraMessageBox.Show("첨부파일이 존재하지 않습니다.");
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("파일이 존재하지 않거나 본래 파일정보가 변경되었습니다.\n다운로드에 실패하였습니다.");
                return;
            }
        }

        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_regNotice");
            sp.AddParam("Kind", "S");

            ret = DbHelp.Proc_Search(sp);

            if(ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            DataTable Table = DbHelp.Fill_Table(ret.ReturnDataSet.Tables[0]);

            string today = DateTime.Now.ToString("yyyyMMdd");
            if (Table.Select("ST <= '" + today + "' AND '" + today + "' <= ED").Count() > 0)
                Table = Table.Select("ST <= '" + today + "' AND '" + today + "' <= ED").CopyToDataTable();
            else
                Table = Table.Clone();

            Grid_Notice.DataSource = Table;
            Grid_Notice.RefreshDataSource();
            View_Notice.BestFitColumns();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(Grid_Notice, "공지현황");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }
        #endregion


        #region 버튼 상속
        protected override void btnSelect()
        {
            btn_Select.PerformClick();
        }

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }
        #endregion

        private void View_Notice_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            txt_Memo.Text = View_Notice.GetRowCellValue(e.FocusedRowHandle, "Notice_Memo").NullString();
        }
    }
}
