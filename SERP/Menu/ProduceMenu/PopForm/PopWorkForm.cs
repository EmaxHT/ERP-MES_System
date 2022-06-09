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
using System.Security.Cryptography;
using System.IO;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;

namespace SERP
{
    public partial class PopWorkForm : BaseForm
    {
        ReturnStruct ret = new ReturnStruct();
        DataSet ds = new DataSet();

        public string sOrder_No = "";
        public string sItem_Code = "";

        private string MenuNm = "regWork";

        public PopWorkForm()
        {
            InitializeComponent();
        }

        private void PopWorkForm_Load(object sender, EventArgs e)
        {
            label_Name.Text = ForMat.Return_FormNM(MenuNm);

            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Sort_No", "순번", "", false, false, false);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Item_Code", "품목코드", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Item_Name", "품명", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Item_Memo", "Spec", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Ssize", "규격", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Q_Unit", "단위", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Qty", "수량", "", true, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "S_Price", "단가", "", true, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Amt", "공급가액", "", true, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Vat_Amt", "부가세액", "", true, false, false);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Total", "합계금액", "", true, false, false);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Order_Bigo", "비고", "", false, false, true);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Total_PPrice", "총매입원가", "", true, false, false);
            DbHelp.GridSet(gc_OrderS, gv_OrderS, "Total_CPrice", "총소비자가", "", true, false, false);

            RepositoryItemMemoEdit Memo_Spec = new RepositoryItemMemoEdit();
            gv_OrderS.Columns["Item_Memo"].ColumnEdit = Memo_Spec;

            gv_OrderS.Columns["Order_Bigo"].ColumnEdit = Memo_Spec;

            gv_OrderS.OptionsView.RowAutoHeight = true;
            gv_OrderS.OptionsView.ShowAutoFilterRow = false;

            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Sort_No", "순번", "", false, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Item_Code", "품목코드", "", false, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "SItem_Code", "품목코드", "", false, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Item_Name", "품명", "", false, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Item_Memo", "Spec", "", false, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Ssize", "규격", "", false, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Q_Unit", "단위", "", false, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Qty", "수량", "", true, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Real_Qty", "총소요량", "", true, false, true);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "P_Price", "매입원가", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Mg_Per", "마진율", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "S_Price", "공급단가", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Dc_Per", "할인률", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "C_Price", "소비자가", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Per_Amt", "공급합계", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Amt", "공급총계", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Vat_Amt", "부가세액", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "Total", "합계금액", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "T_PPrice", "매입총계", "", true, false, false);
            DbHelp.GridSet(gc_OrderS1, gv_OrderS1, "T_CPrice", "할인총계", "", true, false, false);

            gv_OrderS1.OptionsView.RowAutoHeight = true;
            gv_OrderS1.OptionsView.ShowAutoFilterRow = false;

            gv_OrderS1.Columns["Item_Memo"].ColumnEdit = Memo_Spec;

            Search_S1();

            Total_Sum();
        }

     
        private void Search_S1()
        {
            try
            {
                SqlParam sp = new SqlParam("sp_regWork");
                sp.AddParam("Kind", "S");
                sp.AddParam("Search_D", "S");
                sp.AddParam("Order_No", sOrder_No);
                sp.AddParam("Item_Code", sItem_Code);

                ret = DbHelp.Proc_Search(sp);

                if(ret.ReturnChk != 0)
                {
                    XtraMessageBox.Show(ret.ReturnMessage);
                    return;
                }

                gc_OrderS.DataSource = ret.ReturnDataSet.Tables[0];
                gc_OrderS1.DataSource = ret.ReturnDataSet.Tables[1];

                gv_OrderS1.BestFitColumns();

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void Total_Sum()
        {
            decimal iTotal_SPrice = 0, iTotal_Amt = 0, iTotal_C_Price = 0, iTotal_P_Price = 0;

            for (int i = 0; i < gv_OrderS1.RowCount; i++)
            {
                //iTotal_SPrice += decimal.Parse(gv_OrderS1.GetRowCellValue(i, "Per_Amt").NumString());
                iTotal_Amt += decimal.Parse(gv_OrderS1.GetRowCellValue(i, "Amt").NumString());
                iTotal_C_Price += decimal.Parse(gv_OrderS1.GetRowCellValue(i, "T_CPrice").NumString());
                iTotal_P_Price += decimal.Parse(gv_OrderS1.GetRowCellValue(i, "T_PPrice").NumString());
            }

            //gv_OrderS.SetRowCellValue(0, "S_Price", iTotal_SPrice);
            //gv_OrderS.SetRowCellValue(0, "Amt", iTotal_Amt);
            //gv_OrderS.SetRowCellValue(0, "Vat_Amt", iTotal_Amt * (decimal)0.1);
            //gv_OrderS.SetRowCellValue(0, "Total", iTotal_Amt + (iTotal_Amt * (decimal)0.1));
            //gv_OrderS.SetRowCellValue(0, "Total_PPrice", iTotal_P_Price);
            //gv_OrderS.SetRowCellValue(0, "Total_CPrice", iTotal_C_Price);

            gv_OrderS.BestFitColumns();

            txt_Amt.Text = iTotal_Amt.ToString();
            txt_Pprice.Text = iTotal_P_Price.ToString();
            txt_Cprice.Text = iTotal_C_Price.ToString();

        }


        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void btnClose()
        {
            btn_Close.PerformClick();
        }
    }
}
