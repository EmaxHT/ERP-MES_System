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
    public partial class regBasicSet : BaseReg
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public regBasicSet()
        {
            InitializeComponent();
        }

        private void regBasicSet_Load(object sender, EventArgs e)
        {
            Grid_Set();
            LookUp_Set();
        }

        private void Grid_Set()
        {
            // WorkM 그리드
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Menu_SCode", "메뉴코드", "125", false, false, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Menu_SName", "메뉴명", "125", false, false, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Qty1", "수량1", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Qty2", "수량2", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Qty3", "수량3", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Price1", "단가1", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Price2", "단가2", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Price3", "단가3", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Amt1", "합계1", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Amt2", "합계2", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Amt3", "합계3", "100", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Ref_Code", "관리번호채번", "120", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Ref_Itl", "관리번호이니셜", "120", false, true, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Seq_No", "시퀀스", "120", false, false, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Date", "일자", "125", false, false, true);
            DbHelp.GridSet(gc_BasicSet, gv_BasicSet, "Search_YMD", "년월일", "125", false, true, true);

            DbHelp.GridColumn_NumSet(gv_BasicSet, "Qty1", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Qty2", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Qty3", 0);

            DbHelp.GridColumn_NumSet(gv_BasicSet, "Price1", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Price2", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Price3", 0);

            DbHelp.GridColumn_NumSet(gv_BasicSet, "Amt1", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Amt2", 0);
            DbHelp.GridColumn_NumSet(gv_BasicSet, "Amt3", 0);

            

            gc_BasicSet.PopMenuChk = false;
            gc_BasicSet.MouseWheelChk = false;
            gv_BasicSet.OptionsView.ShowAutoFilterRow = false;
        }

        private void LookUp_Set()
        {
            SqlParam sp = new SqlParam("sp_reg_Basic");
            sp.AddParam("Kind", "L");
            sp.AddParam("Menu_Scode", "0");

            ReturnStruct temp_ret = DbHelp.Proc_Search(sp);

            if (temp_ret.ReturnChk == 0)
            {
                DbHelp.GridColumn_LookUp(gv_BasicSet, "Search_YMD", temp_ret.ReturnDataSet.Tables[0], "YMD", "Cap", new string[] { "Cap" }, new string[] { "년월일" });
            }
        }

        #region 텍스트박스 이벤트
        private void txt_MenuM_EditValueChanged(object sender, EventArgs e)
        {
            txt_MenuM_NM.Text = PopHelpForm.Return_Help("sp_Help_MenuM", txt_MenuM.Text);
        }

        private void txt_MenuM_Properties_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_MenuM_NM.Text))
            {
                PopHelpForm help = new PopHelpForm("Menu", "sp_Help_MenuM", txt_MenuM.Text);
                if (help.ShowDialog() == DialogResult.OK)
                {
                    txt_MenuM.Text = help.sRtCode;
                    txt_MenuM_NM.Text = help.sRtCodeNm;
                }
            }
        }

        private void txt_MenuM_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                txt_MenuM_Properties_ButtonClick(null, null);
            }
        }
        #endregion

        #region 버튼 이벤트
        private void btn_Select_Click(object sender, EventArgs e)
        {
            try
            {
                SqlParam sp = new SqlParam("sp_reg_Basic");
                sp.AddParam("Kind", "S");
                sp.AddParam("Menu_SCode", txt_MenuM.Text);

                ret = DbHelp.Proc_Search(sp);

                if (ret.ReturnChk == 0)
                {
                    gc_BasicSet.DataSource = ret.ReturnDataSet.Tables[0];
                }
                else
                    XtraMessageBox.Show(ret.ReturnMessage);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_MenuM_NM.Text))
                return;

            try
            {
                for (int i = 0; i < gv_BasicSet.RowCount; i++)
                {
                    if (gv_BasicSet.GetDataRow(i).RowState == DataRowState.Added || gv_BasicSet.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        SqlParam sp = new SqlParam("sp_reg_Basic");
                        //if (gv_BasicSet.GetDataRow(i).RowState == DataRowState.Added)
                        // sp.AddParam("Kind", "I");
                        //else
                        //    sp.AddParam("Kind", "U");

                        sp.AddParam("Kind", "I");
                        sp.AddParam("Menu_SCode", gv_BasicSet.GetRowCellValue(i, "Menu_SCode"));
                        sp.AddParam("Qty1", gv_BasicSet.GetRowCellValue(i, "Qty1"));
                        sp.AddParam("Qty2", gv_BasicSet.GetRowCellValue(i, "Qty2"));
                        sp.AddParam("Qty3", gv_BasicSet.GetRowCellValue(i, "Qty3"));
                        sp.AddParam("Price1", gv_BasicSet.GetRowCellValue(i, "Price1"));
                        sp.AddParam("Price2", gv_BasicSet.GetRowCellValue(i, "Price2"));
                        sp.AddParam("Price3", gv_BasicSet.GetRowCellValue(i, "Price3"));
                        sp.AddParam("Amt1", gv_BasicSet.GetRowCellValue(i, "Amt1"));
                        sp.AddParam("Amt2", gv_BasicSet.GetRowCellValue(i, "Amt2"));
                        sp.AddParam("Amt3", gv_BasicSet.GetRowCellValue(i, "Amt3"));
                        sp.AddParam("Ref_Code", gv_BasicSet.GetRowCellValue(i, "Ref_Code"));
                        sp.AddParam("Ref_Itl", gv_BasicSet.GetRowCellValue(i, "Ref_Itl"));
                        sp.AddParam("Seq_No", gv_BasicSet.GetRowCellValue(i, "Seq_No"));
                        sp.AddParam("Date", gv_BasicSet.GetRowCellValue(i, "Date"));
                        sp.AddParam("Search_YMD", gv_BasicSet.GetRowCellValue(i, "Search_YMD"));

                        ret = DbHelp.Proc_Save(sp);

                        if (ret.ReturnChk != 0)
                        {
                            XtraMessageBox.Show(ret.ReturnMessage);
                            return;
                        }
                    }
                }

                btn_Save.sCHK = "Y";

                btn_Select_Click(null, null);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            gv_BasicSet.AddNewRow();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (gv_BasicSet.RowCount < 1)
                return;

            try
            {
                for (int i = 0; i < gv_BasicSet.RowCount; i++)
                {
                    if(gv_BasicSet.IsRowSelected(i))
                    {
                        SqlParam sp = new SqlParam("sp_reg_Basic");
                        sp.AddParam("Kind", "D");
                        sp.AddParam("Menu_SCode", gv_BasicSet.GetRowCellValue(i, "Menu_SCode"));

                        ret = DbHelp.Proc_Save(sp);

                        if (ret.ReturnChk != 0)
                        {
                            XtraMessageBox.Show(ret.ReturnMessage);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void btn_Excel_Click(object sender, EventArgs e)
        {
            FileIF.Excel_Down(gc_BasicSet, "관리번호등록");
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            MainForm.MainTab.TabPages.Remove(MainForm.MainTab.SelectedTabPage);
        }

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

        protected override void btnExcel()
        {
            btn_Excel.PerformClick();
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
    }
}
