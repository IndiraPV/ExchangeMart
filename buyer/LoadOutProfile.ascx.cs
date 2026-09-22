using Exchangebase.Com.Bll;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Exchangebase.Com.buyer
{
    public partial class LoadOutProfile : System.Web.UI.UserControl
    {
        public LoadOutViewModel ViewModel { get; set; }
        private EXBUser user;
        public EXBUser TheUser
        {
            get
            {
                user = EXBUser.GetUserSession();
                if (user == null)
                {
                    throw new EXBUserSessionException();
                }
                return user;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            int itmID = WebConvert.ToInt32(Request.QueryString["LoadOutID"], 0);
            ViewModel = LoadOutViewModel.GetViewModel(itmID, TheUser.CusID);
            ViewModel.itmID = itmID;

            //itmIDLabel.Text = ViewModel.itmID.ToString();
            if (!IsPostBack)
            {
                PopulateForm();
            }
        }

        private void PopulateForm()
        {
            int itmID = WebConvert.ToInt32(Request.QueryString["LoadOutID"], 0);
            //get the loadout things for the itm     
            if (ViewModel == null) { return; }
            LoadoutRepeater.DataSource = ViewModel.LoadOuts;
            LoadoutRepeater.DataBind();
            if(itmID == 0)
            {
                return;
            }
            var targetItem = new EXBItem(itmID);

            string basicInfo = EXBEmailUtility.GetItemDetails(targetItem);
            string itmName = targetItem.ItmName;
            string itmDescription = targetItem.ItmDescription;
            string itmIDString = targetItem.ItmID.ToString();
            
            itmBasicInfo.Text = basicInfo;
            itmNameLabel.Text = itmName;
            itmDescriptionLabel.Text = itmDescription;
            itmLoadShippedLabel.Text = ViewModel.LoadOuts.Count().ToString();
            if (targetItem.AssetType == EXBAssetType.Equipment)
            {
                itmTotalFeetShippedLabel.Text = String.Format("{0:N0}", ViewModel.LoadOuts.Sum(x => x.iblTotalQty));
            }
            else if (targetItem.AssetType == EXBAssetType.LinePipe || targetItem.AssetType == EXBAssetType.OCTG)
            {
                itmTotalFeetShippedLabel.Text =  String.Format("{0:N0} FT", ViewModel.LoadOuts.Sum(x => x.iblTotalFeet).ToString());
            }
            var firstOne = ViewModel.LoadOuts.FirstOrDefault();
            explanationTextDiv.Visible = false;
            
            //if (firstOne != null && firstOne.iblLoadsNote != null && firstOne.iblLoadsNote.Length > 0)
            //{
            //    explanation.Text = WebConvert.PreserveBreaks(firstOne.iblLoadsNote);

            //}
            //else
            //{
            //    explanationTextDiv.Visible = false;
            //}

            
            itmIDLabel.Text = itmID.ToString();

        }

        protected void LoadoutRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlContainerControl row = e.Item.FindControl("Tr1") as HtmlContainerControl;

            if (row != null) //hide the view documents button if there are no documents
            {
                if (e.Item.DataItem is LoadOut)
                {
                    if ((e.Item.DataItem as LoadOut).Documents.Count == 0)
                    {
                        if (row.Controls.Count > 7)
                        {
                            if (row.Controls[7].Controls.Count > 0)
                            {
                                row.Controls[7].Controls[0].Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }
    public class LoadOutViewModel
    {
        public List<LoadOut> LoadOuts { get; set; }
        public int itmID { get; set; }
        public LoadOutViewModel()
        {
        }
        public static LoadOutViewModel GetViewModel(int itmID, int cusID, bool isCustomer = false)
        {
            LoadOutViewModel vm = new LoadOutViewModel();
            EXBDbConnection db = new EXBDbConnection();
            string sql = "";
            if (isCustomer)
            {
                sql = "SELECT * FROM vwUserPortalItemsWantedBidLoadOutOne WHERE itmID = " + itmID + " AND iwaCusID  = " + cusID + " ";

            }
            else
            {
                sql = "SELECT * FROM vwUserPortalItemsWantedBidLoadOutOne WHERE itmID = " + itmID + " AND cusID  = " + cusID + " ";
            }
            DataTable dt = db.Query(sql);
            vm.LoadOuts = new List<LoadOut>();
            foreach (DataRow dr in dt.Rows)
	        {
                //check if already exists first, in that case just add the document
                LoadOut lo = new LoadOut();
                lo.itmID = SafeRead.ToInt32(dr, "itmID");
                lo.iblID = SafeRead.ToInt32(dr, "iblID");
                if (!vm.LoadOuts.Any(x => x.iblID == lo.iblID))
                {
                    lo.itmName = SafeRead.ToString(dr, "itmName");
                    lo.iblID = SafeRead.ToInt32(dr, "iblID");
                    lo.iwbID = SafeRead.ToInt32(dr, "iwbID");
                    lo.astID = SafeRead.ToInt32(dr, "astID");
                    lo.iblTotalFeet = SafeRead.ToInt32(dr, "iblTotalFeet");
                    lo.iblTotalQty = SafeRead.ToDecimal(dr, "iblTotalQty");
                    if (lo.astID == (int)EXBAssetType.Equipment)
                    {
                        lo.qtyShipped = String.Format("{0:N0}", lo.iblTotalQty);
                    }
                    else if(lo.astID == (int)EXBAssetType.LinePipe || lo.astID == (int)EXBAssetType.OCTG)
                    {
                        lo.qtyShipped = String.Format("{0:N0} FT", lo.iblTotalFeet);
                    }
                    lo.iwbTotalFeet = SafeRead.ToInt32(dr, "iwbTotalFeet");
                    lo.iwbQty = SafeRead.ToDecimal(dr, "iwbQty");
                    lo.motName = SafeRead.ToString(dr, "motName");
                    lo.iblLoadNo = SafeRead.ToString(dr, "iblLoadNo");
                    lo.iblLoadsNote = SafeRead.ToString(dr, "iwbLoadsNote");
                    lo.iblBillOfLadingNo = SafeRead.ToString(dr, "iblBillOfLadingNo");
                    lo.iblTransportationNo = SafeRead.ToString(dr, "iblTransportationNo");
                    lo.iblNote = SafeRead.ToString(dr, "iblNote");
                    lo.iblDocCount = SafeRead.ToInt32(dr, "iblDocCount");
                    lo.iblShippingDate = SafeRead.ToDateTime(dr, "iblShippingDate");
                    lo.iblTotalQty = SafeRead.ToDecimal(dr, "iblTotalQty");
                    vm.LoadOuts.Add(lo);
                }
                else
                {
                    lo = vm.LoadOuts.First(x => x.iblID == lo.iblID);
                }
                lo.docid = SafeRead.ToInt32(dr, "docID");
                if (lo.docid > 0)
                {
                    LoadOutDocument d = new LoadOutDocument();
                    d.docID = SafeRead.ToInt32(dr, "docID");
                    d.ildID = SafeRead.ToInt32(dr, "ildID");
                    d.docFilename = SafeRead.ToString(dr, "docFilename");
                    d.docExtension = SafeRead.ToString(dr, "docExtension");
                    d.docSize = SafeRead.ToInt32(dr, "docSize");
                    d.docTSCreated = SafeRead.ToDecimal(dr, "docTSCreated");
                    d.dccName = SafeRead.ToString(dr, "dccName");
                    //string starthtml = " <button type='button' class='btn btn-primary btn-sm btn-block' title='Documents' data-container='body' id='documentsButton' data-toggle='popover' data-placement='bottom' data-html='true' data-content='<%# Eval('DocumentHtml') %>'>
                    //                    View <span class='badge badge-light ml-2'>10</span>
                    //                </button>
                    lo.DocumentCount++;
                    string docurl = "../GetDoc.ashx?ildID=" + d.ildID;
                    string fName = d.docFilename;
                    string ext = fName.Substring(fName.IndexOf("."));
                    string fileName = fName.Substring(0, fName.IndexOf("."));
                    lo.DocumentHtml += String.Format("<div  class=\"row\"><div class=\"col-6\">{0}<b>{1}</b></div><div class=\"col-4\">{2}</div><div class=\"col-2\"><a style=\"color:#ea571a;float:right;\" href=\"{3}\"><i class=\"fa fa-download\"></i></a></div></div>", fileName, ext, d.dccName, docurl);
                    lo.Documents.Add(d);
                    Console.WriteLine(lo.DocumentHtml);
                }

	        }
            return vm;
        }


    }


    public class LoadOut
    {
        public LoadOut()
        {
            Documents = new List<LoadOutDocument>();
        }

        public int itmID { get; set; }

        public int iwbID { get; set; }

        public int docid { get; set; }

        public decimal iblTotalQty { get; set; }

        public DateTime iblShippingDate { get; set; }

        public int iblDocCount { get; set; }

        public int DocumentCount { get; set; }

        public string iblNote { get; set; }

        public string iblTransportationNo { get; set; }

        public string iblBillOfLadingNo { get; set; }

        public string iblLoadNo { get; set; }
        public string iblLoadsNote { get; set; }

        public string motName { get; set; }

        public int iblID { get; set; }

        public string itmName { get; set; }

        public string DocumentHtml { get; set; }

        public List<LoadOutDocument> Documents { get; set; }

        public int iwbTotalFeet { get; set; }

        public decimal iwbQty { get; set; }

        public int iblTotalFeet { get; set; }

        public int astID { get; set; }

        public string qtyShipped { get; set; }
    }
    public class LoadOutDocument
    {
        public LoadOutDocument()
        {

        }

        public string dccName { get; set; }
        public int ildID { get; set; }

        public decimal docTSCreated { get; set; }

        public int docSize { get; set; }

        public string docExtension { get; set; }

        public string docFilename { get; set; }

        public int docID { get; set; }
    }
}