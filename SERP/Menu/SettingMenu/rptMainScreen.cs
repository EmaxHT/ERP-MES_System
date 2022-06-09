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
    public partial class rptMainScreen : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        int iMaxNum_Msg = 1;
        int iMaxNum_Notice = 1;
        public rptMainScreen()
        {
            InitializeComponent();
        }

        private void rptMainScreen_Load(object sender, EventArgs e)
        {
            //버튼 클릭 이벤트 선언
            this.btn_Select.Click += btn_Select_Click;
            this.btn_Excel.Click += btn_Excel_Click;
            this.btn_Close.Click += btn_Close_Click;
            Grid_Set();
            Search_Data();
        }

        private void btn_Select_Click(object sender, EventArgs e)
        {
            Search_Data();
        }

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            if (gc_Notice.IsFocused)
                FileIF.Excel_Down(gc_Notice, "공지사항");
            else
                FileIF.Excel_Down(gc_Message, "메세지");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

        private void Grid_Set()
        {
            gc_Notice.AddRowYN = false;
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_No", "Notice_No", "100", false, false, false);
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_STime", "공지 시작일", "150", false, false, true);
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_ETime", "공지 종료일", "150", false, false, true);
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_Part", "공지 구분", "80", false, false, true);
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_User", "공지 등록자", "100", false, false, true);
            DbHelp.GridSet(gc_Notice, gv_Notice, "Notice_Chk", "Notice_Chk", "100", false, false, false);

            gc_Notice.MouseWheelChk = false;
            gc_Notice.PopMenuChk = false;

            gv_Notice.RowHeight = 15;
            gv_Notice.Appearance.HeaderPanel.Font = new Font("나눔바른고딕", 13F, FontStyle.Regular);
            gv_Notice.Appearance.Row.Font = new Font("나눔바른고딕", 13F, FontStyle.Regular);

            gv_Notice.OptionsView.ShowAutoFilterRow = false;

            gc_Message.AddRowYN = false;
            DbHelp.GridSet(gc_Message, gv_Message, "New_Ck", "신규", "100", false, false, true);
            DbHelp.GridSet(gc_Message, gv_Message, "Mg_No", "Mg_N0", "100", false, false, false);
            DbHelp.GridSet(gc_Message, gv_Message, "Send_Date", "메세지 전송일", "120", false, false, true);
            DbHelp.GridSet(gc_Message, gv_Message, "Send_User", "전송자", "100", false, false, true);
            DbHelp.GridSet(gc_Message, gv_Message, "Mg_Message", "메세지 내용", "150", false, false, true);
            DbHelp.GridSet(gc_Message, gv_Message, "Mg_Rcv_YN", "신규", "100", false, false, false);

            gc_Message.MouseWheelChk = false;
            gc_Message.PopMenuChk = false;

            gv_Message.RowHeight = 15;
            gv_Message.Appearance.HeaderPanel.Font = new Font("나눔바른고딕", 13F, FontStyle.Regular);
            gv_Message.Appearance.Row.Font = new Font("나눔바른고딕", 13F, FontStyle.Regular);

            gv_Message.OptionsView.ShowAutoFilterRow = false;
        }

        private void Search_Data()
        {
            SqlParam sp = new SqlParam("sp_rptMainScreen");
            sp.AddParam("Kind", "S");
            sp.AddParam("User_Code", GlobalValue.sUserID);

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return;
            }

            DataTable dt_Notice = ret.ReturnDataSet.Tables[0];;
            DataTable dt_Message = ret.ReturnDataSet.Tables[1];

            gc_Notice.DataSource = dt_Notice;
            gc_Notice.RefreshDataSource();
            gv_Notice.BestFitColumns();

            //공지사항
            iMaxNum_Notice = int.Parse(ret.ReturnDataSet.Tables[3].Rows[0]["MaxNum_Notice"].NumString());
            label_PageNum_Notice.Text = "1/" + iMaxNum_Notice.ToString();


            //메시지
            iMaxNum_Msg = int.Parse(ret.ReturnDataSet.Tables[2].Rows[0]["MaxNum_Msg"].NumString());
            label_PageNum_Msg.Text = "1/" + iMaxNum_Msg.ToString();

            gc_Message.DataSource = dt_Message;
            gv_Message.BestFitColumns();
        }
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

        private void gv_Notice_DoubleClick(object sender, EventArgs e)
        {
            int iRow = gv_Notice.FocusedRowHandle;

            if (iRow < 0)
                return;

            string sNotice_No = gv_Notice.GetRowCellValue(iRow, "Notice_No").ToString();

            PopNoticeForm pop = new PopNoticeForm(sNotice_No);
            pop.StartPosition = FormStartPosition.CenterScreen;
            pop.ShowDialog();
        }

        private void gv_Message_DoubleClick(object sender, EventArgs e)
        {
            int iRow = gv_Message.FocusedRowHandle;

            if (iRow < 0)
                return;

            string sMg_No = gv_Message.GetRowCellValue(iRow, "Mg_No").ToString();

            MainForm.btn_Message.ToolTip = sMg_No;
            MainForm.btn_Message.PerformClick();

            btn_Select_Click(null, null);
        }

        private void rptMainScreen_Paint(object sender, PaintEventArgs e)
        {
            btn_Select_Click(null, null);
        }

        private void gv_Message_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            string sNewMessage = gv_Message.GetRowCellValue(e.RowHandle, "New_Ck").ToString();

            if (!string.IsNullOrWhiteSpace(sNewMessage))
            {
                e.Appearance.Font = new Font("나눔바른고딕", 13F, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Red;
            }
        }

        private void gv_Notice_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            string sNotice_Chk = gv_Notice.GetRowCellValue(e.RowHandle, "Notice_Chk").ToString();

            if(sNotice_Chk == "Y")
            {
                e.Appearance.Font = new Font("나눔바른고딕", 13F, FontStyle.Bold);
                e.Appearance.ForeColor = Color.Red;
            }
        }

        //페이지 작업
        private void btn_Back_Msg_Click(object sender, EventArgs e)
        {
            string sPage = label_PageNum_Msg.Text;

            int iPage_Num = int.Parse(sPage.Substring(0, sPage.IndexOf("/")));

            if (iPage_Num == 1)
                return;

            label_PageNum_Msg.Text = (iPage_Num - 1).ToString() + "/" + iMaxNum_Msg.ToString();
            Search_Page("M", iPage_Num - 1);
        }

        private void btn_Next_Msg_Click(object sender, EventArgs e)
        {
            string sPage = label_PageNum_Msg.Text;

            int iPage_Num = int.Parse(sPage.Substring(0, sPage.IndexOf("/")));

            if (iPage_Num == iMaxNum_Msg)
                return;

            label_PageNum_Msg.Text = (iPage_Num + 1).ToString() + "/" + iMaxNum_Msg.ToString();
            Search_Page("M", iPage_Num + 1);
        }

        private void Search_Page(string sKind, int iPage_Num)
        {
            //sKind = "M" = Message, "N" = Notice
            try
            {
                SqlParam sp = new SqlParam("sp_rptMainScreen");
                sp.AddParam("Kind", "S");
                sp.AddParam("User_Code", GlobalValue.sUserID);
                sp.AddParam("PageNum", iPage_Num);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                if (sKind == "M")
                {
                    gc_Message.DataSource = ret.ReturnDataSet.Tables[1];
                    gv_Message.BestFitColumns();
                }
                else
                {
                    gc_Notice.DataSource = ret.ReturnDataSet.Tables[0];
                    gv_Notice.BestFitColumns();
                }
            }
            catch(Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Back_Notice_Click(object sender, EventArgs e)
        {
            string sPage = label_PageNum_Notice.Text;

            int iPage_Num = int.Parse(sPage.Substring(0, sPage.IndexOf("/")));

            if (iPage_Num == 1)
                return;

            label_PageNum_Notice.Text = (iPage_Num - 1).ToString() + "/" + iMaxNum_Notice.ToString();
            Search_Page("N", iPage_Num - 1);
        }

        private void btn_Next_Notice_Click(object sender, EventArgs e)
        {
            string sPage = label_PageNum_Notice.Text;

            int iPage_Num = int.Parse(sPage.Substring(0, sPage.IndexOf("/")));

            if (iPage_Num == iMaxNum_Notice)
                return;

            label_PageNum_Notice.Text = (iPage_Num + 1).ToString() + "/" + iMaxNum_Notice.ToString();
            Search_Page("M", iPage_Num + 1);
        }
    }
}
