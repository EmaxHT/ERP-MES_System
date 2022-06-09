using DevExpress.XtraEditors;
using Popbill;
using Popbill.Taxinvoice;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SERP
{
    class PopBill
    {
        private ReturnStruct ret = new ReturnStruct();

        private string sLinkID = "EMAX";
        private string SecretKey = "ChkZAua6z+oyF6paTMnmaKBbN0BpaibcNwMbo42Vq2A=";

        private TaxinvoiceService taxinvoiceService;

        public string sNtsConfirmNum = ""; //국세청 승인번호

        public PopBill()
        {
            taxinvoiceService = new TaxinvoiceService(sLinkID, SecretKey);

            //테스트 배드 일경우 true
            taxinvoiceService.IsTest = false;
        }

        //저장
        public bool taxSave(DataTable dt_Company, DataTable dt_Item, bool write)
        {
            Taxinvoice taxinvoice = new Taxinvoice();

            //작성일자
            taxinvoice.writeDate = dt_Company.Rows[0]["Date"].ToString();
            //과금방향(정과금. 역과금)
            taxinvoice.chargeDirection = dt_Company.Rows[0]["chargeDirection"].ToString();
            //발행 형태(정발행, 역발행, 위수탁)
            taxinvoice.issueType = dt_Company.Rows[0]["issueType"].ToString();
            //영수, 충구, 없음
            taxinvoice.purposeType = dt_Company.Rows[0]["purposeType"].ToString();
            //과세 형태(과세, 영세, 면세)
            taxinvoice.taxType = dt_Company.Rows[0]["taxType"].ToString();

            //공급자
            //공급자 사업자 번호
            taxinvoice.invoicerCorpNum = dt_Company.Rows[0]["Company_No"].NullString();
            //공급자 상호
            taxinvoice.invoicerCorpName = dt_Company.Rows[0]["Company_Name"].NullString();
            //공급자 문서번호
            taxinvoice.invoicerMgtKey = dt_Company.Rows[0]["Bill_No"].NullString();
            //공급자 대표명
            taxinvoice.invoicerCEOName = dt_Company.Rows[0]["Owner"].NullString();
            //공급자 주소
            taxinvoice.invoicerAddr = dt_Company.Rows[0]["Bill_Addr1"].NullString();
            //공급자 종목
            taxinvoice.invoicerBizClass = dt_Company.Rows[0]["UpJong"].NullString();
            //공급자 업태
            taxinvoice.invoicerBizType = dt_Company.Rows[0]["UpTai"].NullString();
            //공급자 담당자명
            taxinvoice.invoicerContactName = dt_Company.Rows[0]["User"].NullString();
            //공급자 담당자 메일 주소
            taxinvoice.invoicerEmail = dt_Company.Rows[0]["E_Mail"].NullString();
            //공급자 담당자 연락처
            taxinvoice.invoicerTEL = dt_Company.Rows[0]["Tel_No"].NullString();
            //공급자 담당자 휴대폰
            taxinvoice.invoicerHP = dt_Company.Rows[0]["Tel_No"].NullString();


            //공급받는 자
            taxinvoice.invoiceeType = "사업자";
            //공급받는자 사업자 번호
            taxinvoice.invoiceeCorpNum = dt_Company.Rows[0]["C_Company_No"].NullString();
            //공급받는자 상호
            taxinvoice.invoiceeCorpName = dt_Company.Rows[0]["C_Company_Name"].NullString();
            //공급받는자 대표명
            taxinvoice.invoiceeCEOName = dt_Company.Rows[0]["C_Owner"].NullString();
            //공급받는자 주소
            taxinvoice.invoiceeAddr = dt_Company.Rows[0]["C_Bill_Addr1"].NullString();
            //공급받는자 종목
            taxinvoice.invoiceeBizClass = dt_Company.Rows[0]["C_UpJong"].NullString();
            //공급받는자 업태
            taxinvoice.invoiceeBizType = dt_Company.Rows[0]["C_UpTai"].NullString();
            //공급받는자 담당자명
            taxinvoice.invoiceeContactName1 = dt_Company.Rows[0]["C_User"].NullString();
            //공급받는자 담당자 메일 주소
            taxinvoice.invoiceeEmail1 = dt_Company.Rows[0]["C_E_mail"].NullString();
            //공급받는자 담당자 연락처
            taxinvoice.invoiceeTEL1 = dt_Company.Rows[0]["C_Tel_No"].NullString();
            //공급받는자 담당자 휴대폰
            taxinvoice.invoiceeHP1 = dt_Company.Rows[0]["C_Tel_No"].NullString();

            //문자 발송 여부
            //taxinvoice.invoicerSMSSendYN = true;

            //세금 계산서 정보
            //공급가액 합계
            taxinvoice.supplyCostTotal = dt_Company.Rows[0]["Amt"].NumString();
            //세액 합계
            taxinvoice.taxTotal = dt_Company.Rows[0]["Vat_Amt"].NumString();
            //합계금액
            taxinvoice.totalAmount = dt_Company.Rows[0]["Tot_Amt"].NumString();

            //현금
            taxinvoice.cash = dt_Company.Rows[0]["Tot_Amt"].NumString();

            //세부 항목
            taxinvoice.detailList = new List<TaxinvoiceDetail>();

            TaxinvoiceDetail detail;

            for (int i = 0; i < dt_Item.Rows.Count; i++)
            {
                detail = new TaxinvoiceDetail();
                detail.serialNum = i + 1;
                detail.purchaseDT = dt_Company.Rows[0]["Date"].ToString();
                detail.itemName = dt_Item.Rows[i]["Item_Name"].NullString();
                detail.spec = dt_Item.Rows[i]["Spec"].NullString();
                detail.qty = dt_Item.Rows[i]["Qty"].NumString(); //수량
                detail.unitCost = dt_Item.Rows[i]["S_Price"].NumString(); //단가
                detail.supplyCost = dt_Item.Rows[i]["Amt"].NumString(); //공급가액
                detail.tax = dt_Item.Rows[i]["Vat_Amt"].NumString(); //세액
                detail.remark = dt_Item.Rows[i]["Bigo"].NullString();

                taxinvoice.detailList.Add(detail);
            }

            //taxinvoice.addContactList = new List<TaxinvoiceAddContact>();

            //TaxinvoiceAddContact addContact = new TaxinvoiceAddContact();

            //addContact.serialNum = 1;
            //addContact.email = "test2@invoicee.com";
            //addContact.contactName = "추가담당자명";

            //taxinvoice.addContactList.Add(addContact);

            try
            {
                //IssueResponse response = taxinvoiceService.RegistIssue(dt_Company.Rows[0]["Company_No"].NullString(), taxinvoice, false, "즉시발행", false, "", "");
                Response response = taxinvoiceService.Register(dt_Company.Rows[0]["Company_No"].NullString(), taxinvoice, "", write);

                //sNtsConfirmNum = response.ntsConfirmNum;//국세청 번호(필요시 처리하도록 수정)

                //XtraMessageBox.Show("전자 세금계산서 저장 완료");
            }
            catch (PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "임시저장");

                return false;
            }

            return true;
        }

        //발행
        public bool taxIssue(string Company_No, string MgtKey)
        {
            try
            {
                IssueResponse response = taxinvoiceService.Issue(Company_No, MgtKeyType.SELL, MgtKey, "", "");
            }
            catch(PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "발행");
                return false;
            }

            return true;
        }

        //국세청 즉시 전송
        public bool SendToNTS(string Company_No, string MgtKey)
        {
            try
            {
                Response response = taxinvoiceService.SendToNTS(Company_No, MgtKeyType.SELL, MgtKey, "");
            }
            catch(PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "국세청 즉시전송");
                return false;
            }

            return true;
        }

        //삭제
        public bool taxDelete(string Company_No, string MgtKey)
        {
            try
            {
                Response response = taxinvoiceService.Delete(Company_No, MgtKeyType.SELL, MgtKey, "");
            }
            catch(PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "세금계산서 삭제");
                return false;
            }

            return true;
        }

        public bool taxCancel(string Company_No, string MgtKey)
        {
            try
            {
                Response response = taxinvoiceService.CancelIssue(Company_No, MgtKeyType.SELL, MgtKey, "", "");
            }
            catch (PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "세금계산서 발행취소");
                return false;
            }

            return true;
        }

        public bool taxUpdate(string Company_No, string MgtKey, Taxinvoice taxinvoice)
        {
            try
            {
                Response response = taxinvoiceService.Update(Company_No, MgtKeyType.SELL, MgtKey, taxinvoice, "");
            }
            catch(PopbillException ex)
            {
                XtraMessageBox.Show("응답코드(code) : " + ex.code.ToString() + "\r\n" +
                        "응답메시지(message) : " + ex.Message, "세금계산서 수정");
                return false;
            }

            return true;
        }

        public List<TaxinvoiceInfo> taxSearch(string Company_No, string SDate, string EDate)
        {
            TISearchResult searchResult = new TISearchResult();
            List<TaxinvoiceInfo> taxList = new List<TaxinvoiceInfo>();

            searchResult = taxinvoiceService.Search(Company_No, MgtKeyType.SELL, "W", SDate, EDate, new string[] { }, new string[] { }, new string[] { }, null, "D", 1, 1000);

            //taxList = searchResult.list;
            
            //Type type = typeof(TaxinvoiceInfo);

            //FieldInfo[] field = type.GetFields(BindingFlags.Public);

            return searchResult.list;
        }

        public Taxinvoice getTaxDetail(string Company_No, string sMgtKey)
        {
            Taxinvoice returnTax = new Taxinvoice();

            returnTax = taxinvoiceService.GetDetailInfo(Company_No, MgtKeyType.SELL, sMgtKey);

            return returnTax;
        }

        public string getTaxState(string Company_No, string sMgtKey)
        {
            string sState = "";

            TaxinvoiceInfo taxinvoiceInfo = taxinvoiceService.GetInfo(Company_No, MgtKeyType.SELL, sMgtKey);

            int iState = 0;
            int iStateD = 0;

            iState = taxinvoiceInfo.stateCode / 100;
            iStateD = taxinvoiceInfo.stateCode % 100;

            if (iState == 1)
                sState = "임시저장";
            else if (iState == 3)
            {
                if (iStateD == 3)
                    sState = "국세청 전송";
                else if (iStateD == 4)
                    sState = "국세청 전송실패";
                else
                    sState = "발행완료";
            }
            else if (iState == 6)
                sState = "발행취소";

            return sState;
        }

        //국세청 전소 완료 리스트
        public string[] getTaxList(string sFDate, string sTDate)
        {
            string[] staxList = new string[3];

            List<TaxinvoiceInfo> taxList = new List<TaxinvoiceInfo>();

            string sBillNo = "";
            string sBillDate = "", sCustNM = "";

            SqlParam sp = new SqlParam("sp_regTax");
            sp.AddParam("Kind", "S");
            sp.AddParam("Search_D", "SH");

            ret = DbHelp.Proc_Search(sp);

            if (ret.ReturnChk != 0)
            {
                XtraMessageBox.Show(ret.ReturnMessage);
                return null;
            }

            taxList = taxSearch(ret.ReturnDataSet.Tables[0].Rows[0]["Company_No"].ToString(), sFDate, sTDate);

            for (int i = 0; i < taxList.Count; i++)
            {
                //if (taxList[i].stateCode == 304) // 국세청 전송 완료 확인
                //{
                    sBillNo += taxList[i].invoicerMgtKey + "_/";
                    sBillDate += taxList[i].issueDT == null ? "_/" : taxList[i].issueDT.Substring(0, 8) + "_/";
                    sCustNM += taxList[i].invoiceeCorpName + "_/";
                //}
            }

            staxList[0] = sBillNo;
            staxList[1] = sBillDate;
            staxList[2] = sCustNM;

            return staxList;
        }
    }
}
