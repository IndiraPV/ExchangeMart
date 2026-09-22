using Exchangebase.Com.Bll;
using Exchangebase.Com.Web;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace Exchangebase.Com.Buyer
{
    public partial class Inventory : EXBBuyerRestrictedPage
    {
        InventoryViewModel ViewModel
        {
            get
            {
                return ViewState["ViewModel"] as InventoryViewModel;
            }
            set
            {
                ViewState["ViewModel"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            var controlName = Request.Params.Get("__EVENTTARGET");
            var argument = Request.Params.Get("__EVENTARGUMENT");
            if (controlName == "card")
            {
                //card clicked, go to the item profile view
                ShowItemProfile(argument);
            }
            //clear the search now every time, so it's a global search
            //RadComboBoxItems.Text = "";
            //RadComboBoxItems2.Text = "";
            RadComboBoxItems.SelectedIndexChanged += RadComboBoxItems_SelectedIndexChanged;
            RadComboBoxItems2.SelectedIndexChanged += RadComboBoxItems_SelectedIndexChanged;

            if (!IsPostBack)
            {
                //viewProjectLiteral.tex
                //get all the grades and counts for equipment and line pipe
                List<vwOpenitemsUserPortalFilter> Items = vwOpenitemsUserPortalFilter.GetAll(TheUser.CusID);

                List<SidePanelGrade> equipmentGrades = new List<SidePanelGrade>();
                List<SidePanelGrade> MidequipmentGrades = new List<SidePanelGrade>();
                List<SidePanelGrade> UpequipmentGrades = new List<SidePanelGrade>();
                List<SidePanelGrade> linePipeIODs = new List<SidePanelGrade>();
                List<SidePanelGrade> ShipsGrades = new List<SidePanelGrade>();
                List<SidePanelGrade> RigsGrades = new List<SidePanelGrade>();
                List<SidePanelGrade> OCTGIods = new List<SidePanelGrade>();
                List<SidePanelGrade> CasingIods = new List<SidePanelGrade>();
                List<SidePanelGrade> TubingIods = new List<SidePanelGrade>();
                List<SidePanelGrade> DrillPipeIods = new List<SidePanelGrade>();
                List<int> ShipGradeIds = UtilityTools.GetShipGrades();
                List<int> RigGradeIds = UtilityTools.GetRigGrades();
                List<int> UpstreamGrades = UtilityTools.GetUpstreamGrades();
                List<int> MidstreamGrades = UtilityTools.GetMidstreamGrades();
                var CasingGrades = UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("CasingCategory"), -1));
                var TubingGrades = UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("TubingCategory"), -1));
                var DrillPipeGrades = UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("DrillPipeCategory"), -1));
                var MidStreamCount = 0;
                var UpStreamCount = 0;
                var LinePipeCount = 0;
                var CasingCount = 0;
                var TubingCount = 0;
                var ShipsCount = 0;
                var RigsCount = 0;
                var DrillPipeCount = 0;

                //start with list for equipment
                foreach (var item in Items)
                {
                    SidePanelGrade sidepanelGrade = new SidePanelGrade(item);

                    bool alreadyAdded = equipmentGrades.Any(x => x.grdID == item.grdID)
                        || UpequipmentGrades.Any(x => x.grdID == item.grdID)
                        || ShipsGrades.Any(x => x.grdID == item.grdID)
                        || RigsGrades.Any(x => x.grdID == item.grdID);

                    if (alreadyAdded)
                    {
                        continue;
                    }
                    if (item.astID == (int)EXBAssetType.OCTG && DrillPipeGrades.Contains(item.grdID))
                    {
                        if (DrillPipeIods.Any(x => x.iodID == item.iodID))
                        {
                            continue;
                        }
                        sidepanelGrade.Count = Items.Count(x => x.iodID == sidepanelGrade.iodID && DrillPipeGrades.Contains(x.grdID));
                        DrillPipeCount += sidepanelGrade.Count;
                        DrillPipeIods.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.OCTG && CasingGrades.Contains(item.grdID))
                    {
                        if (CasingIods.Any(x => x.iodID == item.iodID))
                        {
                            continue;
                        }
                        sidepanelGrade.Count = Items.Count(x => x.iodID == sidepanelGrade.iodID && CasingGrades.Contains(x.grdID));
                        CasingCount += sidepanelGrade.Count;
                        CasingIods.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.OCTG && TubingGrades.Contains(item.grdID))
                    {
                        if (TubingIods.Any(x => x.iodID == item.iodID))
                        {
                            continue;
                        }
                        sidepanelGrade.Count = Items.Count(x => x.iodID == sidepanelGrade.iodID && TubingGrades.Contains(x.grdID));
                        TubingCount += sidepanelGrade.Count;
                        TubingIods.Add(sidepanelGrade);
                    }

                    else if (item.astID == (int)EXBAssetType.LinePipe)
                    {
                        if (linePipeIODs.Any(x => x.iodID == item.iodID))
                        {
                            continue;
                        }
                        sidepanelGrade.Count = Items.Count(x => x.iodID == sidepanelGrade.iodID && x.astID == (int)EXBAssetType.LinePipe);
                        LinePipeCount += sidepanelGrade.Count;
                        linePipeIODs.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.Equipment && ShipGradeIds.Contains(item.grdID))
                    {
                        sidepanelGrade.Count = Items.Count(x => x.grdID == sidepanelGrade.grdID);
                        ShipsCount += sidepanelGrade.Count;
                        ShipsGrades.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.Equipment && RigGradeIds.Contains(item.grdID))
                    {
                        sidepanelGrade.Count = Items.Count(x => x.grdID == sidepanelGrade.grdID);
                        RigsCount += sidepanelGrade.Count;
                        RigsGrades.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.Equipment && MidstreamGrades.Contains(item.grdID))
                    {
                        sidepanelGrade.Count = Items.Count(x => x.grdID == sidepanelGrade.grdID);
                        MidStreamCount += sidepanelGrade.Count;
                        equipmentGrades.Add(sidepanelGrade);
                    }
                    else if (item.astID == (int)EXBAssetType.Equipment && UpstreamGrades.Contains(item.grdID))
                    {
                        sidepanelGrade.Count = Items.Count(x => x.grdID == sidepanelGrade.grdID);
                        UpStreamCount += sidepanelGrade.Count;
                        UpequipmentGrades.Add(sidepanelGrade);
                    }
                }
                EquipmentRepeater.DataSource = equipmentGrades.OrderBy(x => x.grade);
                EquipmentRepeater.DataBind();
                if (UpequipmentGrades.Any())
                {
                    UpstreamPanel.Visible = true;
                    UpstreamEquipmentRepeater.DataSource = UpequipmentGrades.OrderBy(x => x.grade);
                    UpstreamEquipmentRepeater.DataBind();
                }
                if (CasingIods.Any())
                {
                    CasingPanel.Visible = true;
                    CasingRepeater.DataSource = CasingIods.OrderBy(x => x.iodID);
                    CasingRepeater.DataBind();
                }
                if (TubingIods.Any())
                {
                    TubingPanel.Visible = true;
                    TubingRepeater.DataSource = TubingIods.OrderBy(x => x.iodID);
                    TubingRepeater.DataBind();
                }
                if (DrillPipeIods.Any())
                {
                    DrillPipePanel.Visible = true;
                    DrillPipeRepeater.DataSource = DrillPipeIods.OrderBy(x => x.iodID);
                    DrillPipeRepeater.DataBind();
                }
                LinePipeIODRepeater.DataSource = linePipeIODs.OrderBy(x => x.iodID);
                LinePipeIODRepeater.DataBind();
                if (ShipsGrades.Any())
                {
                    ShipsGradePanel.Visible = true;
                    ShipsGradeRepeater.DataSource = ShipsGrades.OrderBy(x => x.grade);
                    ShipsGradeRepeater.DataBind();
                }
                if (RigsGrades.Any())
                {
                    RigsPanel.Visible = true;
                    RigsRepeater.DataSource = RigsGrades.OrderBy(x => x.grade);
                    RigsRepeater.DataBind();
                }

                if (OCTGIods.Any())
                {
                    OCTGPanel.Visible = true;
                    OCTGRepeater.DataSource = OCTGIods.OrderBy(x => x.iodID);
                    OCTGRepeater.DataBind();
                }
                //linePipeSpan.DataBind();
                linePipeLiteral.Text = LinePipeCount.ToString();
                midstreamCountLiteral.Text = MidStreamCount.ToString();
                upstreamliteral.Text = UpStreamCount.ToString();
                casingliteral.Text = CasingCount.ToString();
                tubingliteral.Text = TubingCount.ToString();
                drillpipeliteral.Text = DrillPipeCount.ToString();
                shipsLiteral.Text = ShipsCount.ToString();
                rigsliteral.Text = RigsCount.ToString();

                ViewModel = new InventoryViewModel();
                SetWatchList();
                QueryItems(true, 0);
                if (AnalyticsInjection.IsWoopraTrackEnabled())
                {
                    WoopraInjection();
                }
                if (WebConvert.ToInt32(Request.QueryString["boats"], 0) > 0)
                {
                    //<%# Eval("grdID") %>' CssClass="d-flex" gradename='<%# Eval("grade") %>'
                    CommandEventArgs setBoatsArgs = new CommandEventArgs("Go_To_Attributes", 33);
                    Go_To_Attributes(this, setBoatsArgs);
                    //setBoatsArgs.CommandArgument = 33;
                    //Go_To_Attributes(this, new CommandEventArgs(
                }
            }
        }

        private void WoopraInjection()
        {
            //string email = "";

            //if (EXBBuyer.GetBuyerSession() != null)
            //{
            //    email = EXBAdministrator.GetAdministratorEmail(TheUser.PMAdmID);
            //}

            //StringBuilder script = new StringBuilder();
            //script.AppendLine("woopra.track('bidview', {bidid: '" + targetItem.ItmID + "', bidname: '" + targetBid.CusName + " - " + targetItem.ItmName.Replace("'", "") + "', buyerrepemail: '" + email + "'});");
            //ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script.ToString(), true);
        }

        #region sidepanel

        protected void Go_To_Attributes(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int grdID = WebConvert.ToInt32(e.CommandArgument, 0);
            if (sender is LinkButton)
            {
                SidePanelTitleLabel.Text = (sender as LinkButton).Attributes["gradename"];
            }
            else
            {
                SidePanelTitleLabel.Text = "Compressor Packages";
            }
            WoopraReportAction("Equipment - " + SidePanelTitleLabel.Text);
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            ViewModel.ListGradeIds = new List<int>();
            ViewModel.ListGradeIds.Add(grdID);
            SidePanel_Attributes.Visible = true;
            SidePanel_Category.Visible = false;
            AttributesRepeater.Visible = true;
            LinePipeGradePanel.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.EXBAssetType = EXBAssetType.Equipment;
            AttributesRepeater.DataBind();
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }
        protected void Go_To_LinePipeGrades(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int iodID = WebConvert.ToInt32(e.CommandArgument, 0);
            SidePanelTitleLabel.Text = (sender as LinkButton).Attributes["iodname"];
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            WoopraReportAction("Line Pipe - " + SidePanelTitleLabel.Text);
            ViewModel.iodID = iodID;
            ViewModel.EXBAssetType = EXBAssetType.LinePipe;
            SidePanel_Category.Visible = false;
            SidePanel_Attributes.Visible = true;
            LinePipeGradePanel.Visible = true;
            AttributesRepeater.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            LinePipeGradePanel.DataBind();
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }
        protected void Go_To_CasingGrades(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int iodID = WebConvert.ToInt32(e.CommandArgument, 0);
            SidePanelTitleLabel.Text = (sender as LinkButton).Attributes["iodname"];
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            WoopraReportAction("Line Pipe - " + SidePanelTitleLabel.Text);
            ViewModel.iodID = iodID;
            ViewModel.EXBAssetType = EXBAssetType.OCTG;
            SidePanel_Category.Visible = false;
            SidePanel_Attributes.Visible = true;
            LinePipeGradePanel.Visible = true;
            AttributesRepeater.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            LinePipeGradePanel.DataBind();
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }
        protected void Go_To_Categories(object sender, CommandEventArgs e)
        {
            //if it's line pipe then search by iodID, if it's equipment then search by grade. transition to the attributes panel
            //GradeTitleLabel.Text = "Featured Items";
            WoopraReportAction("View Categories");
            AttributesPanel.Visible = false;
            AttributesRepeater.Visible = false;
            LinePipeGradePanel.Visible = false;
            SidePanel_Attributes.Visible = false;
            SidePanel_Category.Visible = true;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;

            //ViewState["ViewModel"] = new InventoryViewModel();
            ViewModel = new InventoryViewModel();
            SetWatchList();
            QueryItems(true, 0);
            //QueryItems(false, 0);
        }

        protected IEnumerable<EXBTagAttributeField> GetAttributeFields()
        {
            int grdID = ViewModel.ListGradeIds.FirstOrDefault();
            int assetTypeID = (int)ViewModel.EXBAssetType;
            string query = "tagID IN (SELECT DISTINCT it.tagID FROM vwOpenItemsUserPortalNew as op INNER JOIN ItemTags as it ON op.itmID = it.itmID WHERE astID = "
                 + assetTypeID + " AND cusID <> " + TheUser.CusID + " AND  grdID = " + grdID + ") AND Tags.tagValue <> 'Not Specified' ";

            List<EXBTagAttributeField> attributeFields = EXBTagAttributeField.GetAttributesUserPortal(grdID, query);

            if (!attributeFields.Any())
            {
                AttributesPanel.Visible = false;
                AttributesRepeater.Visible = false;
                //clearButtonHR.Visible = false;
            }
            else
            {
                AttributesPanel.Visible = true;
                AttributesRepeater.Visible = true;
            }

            return attributeFields;
        }

        protected List<SidePanelGrade> GetLinePipeGrades()
        {
            var iodID = ViewModel.iodID;

            //update for casing and tubing
            var grdWhere = "";

            if (ViewModel.IsCasing)
            {
                grdWhere = " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("CasingCategory"), -1))) + " ) ";
            }
            else if (ViewModel.IsDrillPipe)
            {
                grdWhere = " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("DrillPipeCategory"), -1))) + " ) ";
            }
            else if (ViewModel.IsTubing)
            {
                grdWhere = " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("TubingCategory"), -1))) + " ) ";
            }

            var listItems = vwOpenitemsUserPortalFilter.GetAll(TheCustomer.CusID, " AND astID = " + (int)ViewModel.EXBAssetType + " AND iodID = " + iodID + grdWhere);


            List<SidePanelGrade> grades = new List<SidePanelGrade>();

            foreach (var item in listItems)
            {
                SidePanelGrade sidepanelGrade = new SidePanelGrade(item);
                if (grades.Any(x => x.grdID == item.grdID))
                {
                    continue;
                }
                sidepanelGrade.Count = listItems.Count(x => x.grdID == sidepanelGrade.grdID);
                grades.Add(sidepanelGrade);
            }
            LinePipeGradePanel.Visible = true;
            AttributesPanel.Visible = true;

            return grades;
        }

        #endregion

        #region attributes

        protected void Attribute_Check_Changed(object sender, EventArgs e)
        {
            //get a list of all items which are currently checked and build a list of tags
            //query the table for all the items with matching tags from the list of tags

            //or build the list one at a time and add-remove (only if the list can't be iterated each time)
            //the clear button will have to get all though right? 
            List<String> allTagValues = new List<string>();
            foreach (RepeaterItem tagField in AttributesRepeater.Items)
            {
                bool selectedAttribute = false;
                string attributeSelectedValues = "";
                Repeater valuesRepeater = tagField.FindControl("AttributeValuesRepeater") as Repeater;
                foreach (RepeaterItem tagValue in valuesRepeater.Items)
                {
                    CheckBox valueCheckBox = tagValue.FindControl("valueCheckbox") as CheckBox;
                    if (valueCheckBox.Checked)
                    {
                        attributeSelectedValues += valueCheckBox.Attributes["tagid"] + ", ";
                        selectedAttribute = true;
                    }
                }
                if (selectedAttribute)
                {
                    attributeSelectedValues = attributeSelectedValues.Remove(attributeSelectedValues.Length - 2, 2).ToString();
                    allTagValues.Add(attributeSelectedValues);
                }
            }

            CreateAttributeWoopra();

            //at this point we have [{14, 15, 16}, {5,10}, {19, 20, 21, 24}]
            //this array should turn into SELECT itmID FROM ItemTags WHERE itmID IN(SELECT itmID FROM ItemTags WHERE tagID in (70)) AND tagID in (74)
            string sqlSelectItmIDs = "";
            if (allTagValues.Count > 0)
            {
                sqlSelectItmIDs = "SELECT itmID from ItemTags where tagID IN ( " + allTagValues[0] + " ) ";
                for (int i = 1; i < allTagValues.Count; i++)
                {
                    sqlSelectItmIDs = "(SELECT itmID From ItemTags WHERE itmID IN ( " + sqlSelectItmIDs + ") AND tagID in ( " + allTagValues[i] + " )) ";
                }

            }
            if (sqlSelectItmIDs.Length > 0)
            {
                EXBDbConnection db = new EXBDbConnection();
                DataTable dt = db.Query(sqlSelectItmIDs);
                StringBuilder itmIDs = new StringBuilder();
                itmIDs.Append(" ");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        itmIDs.Append(dr["itmID"] + ", ");
                    }
                    if (itmIDs.Length > 0)
                    {
                        itmIDs.Remove(itmIDs.Length - 2, 2);
                    }
                    ViewModel.tagValue = itmIDs.ToString();
                }
                else
                {
                    //no results for this (there are no matches / no rows)
                    ViewModel.tagValue = " 0 ";
                }
            }
            else
            {
                //all the checkboxes are cleared, show all the results for the rest of the filter
                ViewModel.tagValue = "";
            }

            //ItemProfilePanel.Visible = false;
            //ItemCardsPanel.Visible = true;
            //ItemProfileControl.UpdateFlag = false;
            //ItemProfileControl.itmID = 0;
            //ItemProfileControl.iwaID = 0;
            //clearButtonHR.Visible = true;
            ////RadComboBoxItems.Text = "";
            QueryItems(true);
        }
        protected void CreateAttributeWoopra()
        {
            string gradeName = "Asset Type: " + GradeTitleLabel.Text + " ";
            if (ViewModel.EXBAssetType == EXBAssetType.LinePipe)
            {
                gradeName = "Line Pipe OD: " + GradeTitleLabel.Text + " ";
            }

            string attributeText = "";
            foreach (RepeaterItem AttributeField in AttributesRepeater.Items)
            {
                HtmlContainerControl span = AttributeField.FindControl("tagName") as HtmlContainerControl;
                if (span == null)
                    return;
                string attributeName = span.InnerText + ": ";
                Repeater valuesRepeater = AttributeField.FindControl("AttributeValuesRepeater") as Repeater;
                if (valuesRepeater == null)
                    return;
                var foundAttributeChecked = false;
                foreach (RepeaterItem value in valuesRepeater.Items)
                {
                    CheckBox valueCheckBox = value.FindControl("valueCheckbox") as CheckBox;
                    if (valueCheckBox == null)
                        return;
                    if (valueCheckBox.Checked)
                    {
                        attributeName += valueCheckBox.Text + ", ";
                        foundAttributeChecked = true;
                    }
                }
                if (foundAttributeChecked)
                {
                    attributeName = attributeName.Remove(attributeName.Length - 2, 2);
                    attributeName += ". ";
                    attributeText += attributeName;
                }
            }
            WoopraReportAction("Attribute Filter: " + attributeText.ToString());
        }

        protected string GetAttributeText()
        {
            string gradeName = "<span class='d-block'>Asset Type</span> <span class='d-block bold mb-3'>" + GradeTitleLabel.Text + "</span>";

            string attributeText = "";

            foreach (RepeaterItem AttributeField in AttributesRepeater.Items)
            {
                HtmlContainerControl span = AttributeField.FindControl("tagName") as HtmlContainerControl;
                if (span == null)
                    return "";
                string attributeName = "<span class='d-block'>" + span.InnerText + "</span>";
                Repeater valuesRepeater = AttributeField.FindControl("AttributeValuesRepeater") as Repeater;
                if (valuesRepeater == null)
                    return "";
                var foundAttributeChecked = false;

                attributeName += "<span class='d-block bold mb-3'>";
                foreach (RepeaterItem value in valuesRepeater.Items)
                {
                    CheckBox valueCheckBox = value.FindControl("valueCheckbox") as CheckBox;
                    if (valueCheckBox == null)
                        return "";
                    if (valueCheckBox.Checked)
                    {
                        attributeName += valueCheckBox.Text + ", ";
                        foundAttributeChecked = true;
                    }
                }
                if (foundAttributeChecked)
                {

                    attributeName = attributeName.Remove(attributeName.Length - 2, 2);
                    attributeText += attributeName + "</span>";

                }
            }

            if (ViewModel.GetDbSearchTerm().Length > 0)
            {
                attributeText += "<span class='d-block'>Search Term</span> <span class='d-block bold mb-3'> " + ViewModel.GetDbSearchTerm() + "</span>";
            }
            return gradeName + attributeText;
        }
        protected void SetAttributesChecked(Dictionary<int, bool> attributesToCheck)
        {
            foreach (RepeaterItem tagField in AttributesRepeater.Items)
            {
                Repeater valuesRepeater = tagField.FindControl("AttributeValuesRepeater") as Repeater;
                foreach (RepeaterItem tagValue in valuesRepeater.Items)
                {
                    CheckBox valueCheckBox = tagValue.FindControl("valueCheckbox") as CheckBox;
                    int tagID = WebConvert.ToInt32(valueCheckBox.Attributes["tagid"], 0);
                    if (attributesToCheck.ContainsKey(tagID) && attributesToCheck[tagID])
                    {
                        valueCheckBox.Checked = true;
                    }
                    else
                    {
                        valueCheckBox.Checked = false;
                    }
                }
            }
            //RadComboBoxItems.Text = "";
        }

        #endregion

        #region paging
        protected void PreviousPage_Command(object sender, CommandEventArgs e)
        {
            int currentPage = ViewModel.pageNum;
            currentPage--;
            currentPage = currentPage < 1 ? 1 : currentPage;
            ViewModel.pageNum = currentPage;
            QueryItems();
            PreviousPageLinkButton2.Enabled = PreviousPageLinkButton.Enabled = !(ViewModel.pageNum == 1 || ViewModel.pageNum == 0);
            WoopraReportAction("Previous Page: " + ViewModel.pageNum);
        }

        protected void NextPage_Command(object sender, CommandEventArgs e)
        {
            int currentPage = ViewModel.pageNum;
            currentPage++;
            ViewModel.pageNum = currentPage;
            QueryItems();
            PreviousPageLinkButton2.Enabled = PreviousPageLinkButton.Enabled = !(ViewModel.pageNum == 1 || ViewModel.pageNum == 0);
            WoopraReportAction("Next Page: " + ViewModel.pageNum);
        }
        private void SetUpPagination(int itemCount)
        {
            if (itemCount > ViewModel.NumberOfItemsPerPage)
            {
                //there are more items than can be displayed on a single page. start paging
                PageDisplayPanel.Visible = true;
                PagingPanel1.Visible = true;
                PagingPanel2.Visible = true;
                PreviousPageLinkButton2.Enabled = PreviousPageLinkButton.Enabled = !(ViewModel.pageNum == 1 || ViewModel.pageNum == 0);
            }
            else
            {
                PageDisplayPanel.Visible = false;
                PagingPanel1.Visible = false;
                PagingPanel2.Visible = false;
            }
        }
        #endregion

        private void QueryItems(bool refreshCount = false, int itmID = 0)
        {
            string searchString = ViewModel.GetDbSearchTerm();
            string where = GetWhereClause(itmID);

            EXBDbConnection db = new EXBDbConnection();
            string astWhere = "";
            //if (ViewModel.EXBAssetType == EXBAssetType.OCTG)
            //{
            //    astWhere = " AND astID = " + (int)EXBAssetType.OCTG + " ";
            //}
            if (ViewModel.EXBAssetType == EXBAssetType.OCTG && ViewModel.IsCasing)
            {
                //only show grds in the casing category?
                astWhere = " AND astID = " + (int)EXBAssetType.OCTG + " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("CasingCategory"), -1))) + " ) ";
            }
            else if (ViewModel.EXBAssetType == EXBAssetType.OCTG && ViewModel.IsTubing)
            {
                //only show grds in the casing category?
                astWhere = " AND astID = " + (int)EXBAssetType.OCTG + " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("TubingCategory"), -1))) + " ) ";
            }
            else if (ViewModel.EXBAssetType == EXBAssetType.OCTG && ViewModel.IsDrillPipe)
            {
                //only show grds in the casing category?
                astWhere = " AND astID = " + (int)EXBAssetType.OCTG + " AND grdID IN ( " + GetDBString(UtilityTools.GetGradesForCategoryId(WebConvert.ToInt32(AppConfig.GetValue("DrillPipeCategory"), -1))) + " ) ";
            }
            else if (ViewModel.iodID > 0)
            {
                astWhere = " AND astID = " + (int)EXBAssetType.LinePipe + " ";
            }
            else if (ViewModel.EXBAssetType == EXBAssetType.Equipment)
            {
                astWhere = " AND astID = " + (int)EXBAssetType.Equipment + " ";
            }
            //if (featuredOnly)
            //{
            //    where += " AND itmIsFeatured = 1 ";
            //}
            string countsql = "SELECT COUNT (*) FROM vwOpenItemsUserPortalNew WHERE cusID <> " + TheUser.CusID + astWhere + where;
            int numberOfItems = WebConvert.ToInt32(db.ExecuteScalar(countsql), 0);

            //change visibility of paging controls
            if (refreshCount)
            {
                SetUpPagination(numberOfItems);
            }

            //get page number, and then page count to select rows. add this to the where clause
            int rowCountBegin;
            int rowCountEnd;
            int pageNum = ViewModel.pageNum;
            //in case pagenum is too high
            pageNum = pageNum == 0 ? 1 : pageNum;
            //in case pagenum is too low
            int numberOfPages = numberOfItems / ViewModel.NumberOfItemsPerPage;
            if (numberOfItems % ViewModel.NumberOfItemsPerPage > 0)
            {
                numberOfPages++;
            }
            pageNum = pageNum > numberOfPages ? numberOfPages : pageNum;
            ViewModel.pageNum = pageNum;

            rowCountBegin = (pageNum - 1) * ViewModel.NumberOfItemsPerPage + 1;
            rowCountEnd = rowCountBegin + ViewModel.NumberOfItemsPerPage > numberOfItems + 1 ? numberOfItems + 1 : rowCountBegin + ViewModel.NumberOfItemsPerPage;
            PageDisplayString.Text = "Showing " + rowCountBegin + "-" + (rowCountEnd - 1) + " of " + numberOfItems + " results";

            var sqlStringPage = "SELECT * "
                + "FROM  ( Select ROW_NUMBER() OVER ( ORDER BY itmIsFeatured desc, grdID ) AS RowNum, * "
                + "FROM vwOpenItemsUserPortalNew "
                + "WHERE cusID <> " + TheUser.CusID + astWhere + where
                + ") AS RowConstrainedResult WHERE "
                + "RowNum >= " + rowCountBegin
                + "AND RowNum < " + rowCountEnd + "ORDER BY RowNum";

            DataTable items = db.Query(sqlStringPage);

            ItemRepeater.DataSource = items;
            ItemRepeater.DataBind();

            ItemProfilePanel.Visible = false;
            //if attributes are clicked (tagValue.Value.Length > 0) then show contact form
            if (items.Rows.Count < 1 && (searchString.Length == 0 || ViewModel.tagValue == null || ViewModel.tagValue.Length > 0))
            {
                NoCardsContent.Visible = true;
                SearchResultsPanel.Visible = false;
                ItemCardsPanel.Visible = false;
                EmailSentComplete.Visible = false;
                AttributeDescriptionTextBox.Text = GetAttributeText();
                SearchTermsLabel2.Text = searchString;
                WoopraReportAction("Attributes - No Items to Display");
            }
            else if (items.Rows.Count < 1 && searchString.Length > 0)
            {
                SearchResultsPanel.Visible = true;
                SearchTermsLabel.Text = searchString;
                AttributeDescriptionTextBox2.Text = GetAttributeText();
                NoCardsContent.Visible = false;
                ItemCardsPanel.Visible = false;
                EmailSentComplete.Visible = false;
                WoopraReportAction("SearchBox: No Search Results Found");
            }
            else
            {
                SearchResultsPanel.Visible = false;
                NoCardsContent.Visible = false;
                ItemCardsPanel.Visible = true;
                EmailSentComplete.Visible = false;
            }
        }
        protected void ShowItemProfile(string commandArgsString)
        {

            ItemProfilePanel.Visible = true;
            ItemCardsPanel.Visible = false;
            var commandArgs = commandArgsString.Split(',');
            var itmID = WebConvert.ToInt32(commandArgs[0], 0);
            if (commandArgs.Length > 1)
            {
                var iwaID = WebConvert.ToInt32(commandArgs[1], 0);
                ItemProfileControl.iwaID = iwaID;
            }
            ItemProfileControl.itmID = itmID;
            ItemProfileControl.UpdateFlag = true;
        }

        #region searchbox

        protected void SearchBox_OnTextChanged(object sender, EventArgs e)
        {
            TextBox SearchBox = sender as TextBox;
            EXBDbConnection db = new EXBDbConnection();
            SQLInjection si = new SQLInjection();
            ViewModel.SearchTerm = si.FormatStringForDb(SearchBox.Text);
            var where = GetWhereClause(0);
        }

        protected void RadComboBoxProduct_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
        {

            RadComboBox rcb = sender as RadComboBox;
            EXBDbConnection db = new EXBDbConnection();
            //need to get all items to calculate the sidebar filters and count each grade type. 
            //todo: optimize this query to get counts on sql side
            SQLInjection si = new SQLInjection();
            ViewModel.SearchTerm = si.FormatStringForDb(e.Text);
            var where = GetWhereClause(0);

            DataTable dtAll = db.Query("SELECT * FROM vwOpenItemsUserPortalNew WHERE cusID <> " + TheUser.CusID +
                where + " ORDER BY itmIsFeatured desc");

            foreach (DataRow dataRow in dtAll.Rows)
            {
                RadComboBoxItem item = new RadComboBoxItem();
                item.Text = "#" + WebConvert.ToString(dataRow["itmID"], "") + ". " + (string)dataRow["itmName"];
                item.Value = dataRow["itmID"].ToString();
                rcb.Items.Add(item);
                item.DataBind();
            }
        }
        void RadComboBoxItems_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox rcb = sender as RadComboBox;
            if (!string.IsNullOrWhiteSpace(e.Value))
            {
                int itmID;
                int.TryParse(e.Value, out itmID);
                //there's a selected item, show it in the list
                QueryItems(true, itmID);
                WoopraReportAction("SearchBoxItemClicked: " + itmID);
            }
            else if (RadComboBoxItems.Text != null)
            {
                SQLInjection si = new SQLInjection();
                string searchText = si.FormatStringForDb(rcb.Text);
                ViewModel.SearchTerm = searchText;
                QueryItems(true);
                WoopraReportAction("Searching: " + searchText);
            }
        }

        #endregion
        private void WoopraReportAction(string description)
        {
            if (AnalyticsInjection.IsWoopraTrackEnabled())
            {
                //the watchlist button has a different pattern to work
                bool trackWatchlist = false;
                if (description != null && description.Contains("Watch"))
                {
                    trackWatchlist = true;
                }
                string script = ("<script>woopra.track('userportal', {page: 'Inventory', description: '" + description
                    + "'});</script>");
                if (trackWatchlist)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "Javascript", script, false);
                }
                else
                {
                    Literal analyticsInjection = new Literal();
                    analyticsInjection.Text = script.ToString();
                    Page.Header.Controls.Add(analyticsInjection);
                }
            }
        }
        private string GetWhereClause(int itmID)
        {
            if (itmID > 0)
            {
                return "AND itmID = " + itmID;
            }
            string where = "";
            if (ViewModel.ListGradeIds.Any())
            {
                where += " AND grdID IN (" + GetDBString(ViewModel.ListGradeIds) + ")";
            }

            if (!string.IsNullOrWhiteSpace(ViewModel.GetDbSearchTerm()))
            {
                where += " AND (itmName like '%" + ViewModel.GetDbSearchTerm() + "%' OR ItmNameDescription like '%" + ViewModel.GetDbSearchTerm() + "%' OR itmID = " + WebConvert.ToInt32(ViewModel.GetDbSearchTerm(), 0) + " ) ";
            }
            if (ViewModel.tagValue != null && ViewModel.tagValue.Length > 0)
            {
                //there's an attributeValue collection here
                where += " AND itmID IN (" + ViewModel.tagValue + ") ";
            }
            if (ViewModel.iodID > 0)
            {
                where += " AND iodID = " + ViewModel.iodID + " ";
            }
            return where;

        }
        public string GetDBString(List<int> ints)
        {
            string returnValue = "";
            foreach (int grdID in ints)
            {
                returnValue += grdID + ",";
            }

            if (returnValue.Length > 0)
            {
                returnValue = returnValue.Remove(returnValue.Length - 1);
            }
            return returnValue;
        }
        private void SetWatchList()
        {
            ViewState["ActiveBuyerProjects"] = BuyerUtilities.GetAllActiveBuyerProjects(TheUser.UsrID, TheBuyer.BuyID);
        }
        protected void ItemRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            int itmID = WebConvert.ToInt32(dr["itmID"], 0);
            int iwaID = 0;
            List<ActiveBuyerProject> activeBuyerProjects = new List<ActiveBuyerProject>();
            string iwaIDQueryParameter = "";
            ActiveBuyerProject currentlySourcingProject = null;
            if (ViewState["ActiveBuyerProjects"] != null)
            {
                activeBuyerProjects = (List<ActiveBuyerProject>)ViewState["ActiveBuyerProjects"];
                currentlySourcingProject = activeBuyerProjects.OrderBy(target => (int)target.SourcingStage).FirstOrDefault(target => target.ItmID == itmID);
                if (currentlySourcingProject != null)
                {
                    iwaID = currentlySourcingProject.IwaID;
                    iwaIDQueryParameter = "&PID=" + iwaID;
                }
            }

            bool itemIsFeatured = WebConvert.ToBoolean(dr["itmIsFeatured"], false);
            //hide featured panel or not
            Panel featuredPanel = e.Item.FindControl("FeaturedPanel") as Panel;
            if (featuredPanel != null)
            {
                featuredPanel.Visible = itemIsFeatured;
            }
            EXBAssetType assetType = UtilityTools.ParseAssetType(WebConvert.ToInt32(dr["astID"], 0));
            //HtmlContainerControl askPriceSpan = e.Item.FindControl("askPriceSpan") as HtmlContainerControl;
            decimal itmIndexPrice = WebConvert.ToDecimal(dr["itmIndexPrice"], 0);
            decimal itmSellPrice = WebConvert.ToDecimal(dr["itmSellPrice"], 0);
            decimal itmQty = WebConvert.ToDecimal(dr["itmQty"], 0);
            int itmTotalFeet = WebConvert.ToInt32(dr["itmTotalFeet"], 0);
            decimal itmIndexPriceFT = itmTotalFeet > 0 ? Math.Round(itmIndexPrice * itmQty / itmTotalFeet, 2, MidpointRounding.AwayFromZero) : 0;
            decimal itmSellPriceFT = itmTotalFeet > 0 ? Math.Round(itmSellPrice * itmQty / itmTotalFeet, 2, MidpointRounding.AwayFromZero) : 0;
            Label quotedPriceAskingPriceLabel = e.Item.FindControl("QuotePriceAskPriceLabel") as Label;
            Literal itmIndexPriceLiteral = e.Item.FindControl("itmIndexPriceLiteral") as Literal;
            if (itmIndexPriceLiteral.Text != null)
            {
                Label OriginalPriceIndexPriceLabel = e.Item.FindControl("OriginalPriceIndexPriceLabel") as Label;
                if (assetType == EXBAssetType.LinePipe || assetType == EXBAssetType.OCTG)
                {
                    if (OriginalPriceIndexPriceLabel != null)
                    {
                        OriginalPriceIndexPriceLabel.Text = "Index Price";
                    }
                    itmIndexPriceLiteral.Text = itmIndexPriceFT > 0 ? String.Format("{0:C2}", itmIndexPriceFT) + " /ft" : "N/A";
                }
                else
                {
                    if (OriginalPriceIndexPriceLabel != null)
                    {
                        OriginalPriceIndexPriceLabel.Text = "Original Price";
                    }
                    if (itmQty == 1)
                    {
                        itmIndexPriceLiteral.Text = itmIndexPrice > 0 ? String.Format("{0:C0}", itmIndexPrice) : "N/A";
                    }
                    else
                    {
                        itmIndexPriceLiteral.Text = itmIndexPrice > 0 ? String.Format("{0:C0}", itmIndexPrice) + " /" + WebConvert.ToString(dr["uomName"], "").ToLower() : "N/A";
                    }
                }
            }




            Literal itmSellPriceLiteral = e.Item.FindControl("itmSellPriceLiteral") as Literal;

            if (itmSellPriceLiteral != null)
            {
                if (assetType == EXBAssetType.LinePipe || assetType == EXBAssetType.OCTG)
                {

                    //if the item is quoted in a project, then show the quote price
                    if (currentlySourcingProject != null && currentlySourcingProject.SourcingStage == EXBItemsWantedSourcingStage.CurrentlySourcing)
                    {
                        quotedPriceAskingPriceLabel.Text = "Quoted Price";
                        itmSellPriceLiteral.Text = currentlySourcingProject.IwbSellPriceFT > 0 ? String.Format("{0:C2}", currentlySourcingProject.IwbSellPriceFT) + " /ft" : "N/A";
                    }
                    else
                    {
                        quotedPriceAskingPriceLabel.Text = "Asking Price";
                        itmSellPriceLiteral.Text = itmSellPriceFT > 0 ? String.Format("{0:C2}", itmSellPriceFT) + " /ft" : "N/A";
                    }
                }
                else
                {
                    //if the item is quoted in a project, then show the quote price
                    if (currentlySourcingProject != null && currentlySourcingProject.SourcingStage == EXBItemsWantedSourcingStage.CurrentlySourcing)
                    {
                        quotedPriceAskingPriceLabel.Text = "Quoted Price";
                        if (itmQty == 1)
                        {
                            itmSellPriceLiteral.Text = currentlySourcingProject.IwbSellPrice > 0 ? String.Format("{0:C0}", currentlySourcingProject.IwbSellPrice) : "N/A";

                        }
                        else
                        {
                            itmSellPriceLiteral.Text = currentlySourcingProject.IwbSellPrice > 0 ? String.Format("{0:C0}", currentlySourcingProject.IwbSellPrice) + " /" + WebConvert.ToString(dr["uomName"], "").ToLower() : "N/A";
                        }

                    }
                    else
                    {
                        if (itmQty == 1)
                        {
                            itmSellPriceLiteral.Text = itmSellPrice > 0 ? String.Format("{0:C0}", itmSellPrice) : "N/A";

                        }
                        else
                        {
                            itmSellPriceLiteral.Text = itmSellPrice > 0 ? String.Format("{0:C0}", itmSellPrice) + " /" + WebConvert.ToString(dr["uomName"], "").ToLower() : "N/A";
                        }
                        quotedPriceAskingPriceLabel.Text = "Asking Price";
                    }
                }
            }

            Image itmImgMain = e.Item.FindControl("itmImgMain") as Image;

            if (itmImgMain != null)
            {

                if (WebConvert.ToInt32(dr["imgID"], 0) > 0)
                {
                    itmImgMain.ImageUrl = EXBImage.GetImageWebFilePath(WebConvert.ToInt32(dr["imgID"], 0), WebConvert.ToString(dr["imgExtension"], ".jpg"));
                }
                else
                {
                    itmImgMain.ImageUrl = "~/Images/image-unavailable2.png";
                }
            }
            LinkButton itmIDLink = e.Item.FindControl("itmIDLink") as LinkButton;
            if (itmIDLink != null)
            {
                itmIDLink.CommandArgument = itmID.ToString() + "," + iwaID.ToString();
            }

            Label gradeLabel = e.Item.FindControl("GradeLabel") as Label;
            if (gradeLabel != null)
            {
                gradeLabel.Text = WebConvert.ToString(dr["Grade"], "");
            }
            Label ODLabel = e.Item.FindControl("ODLabel") as Label;
            if (ODLabel != null)
            {
                if (assetType == EXBAssetType.LinePipe)
                {
                    ODLabel.Visible = true;
                    ODLabel.Text = WebConvert.ToString(dr["iodName"], "");
                }
            }

            HtmlContainerControl mainDiv = e.Item.FindControl("mainCardDiv") as HtmlContainerControl;
            if (mainDiv != null)
            {
                mainDiv.Attributes.Add("cmdargument", itmID.ToString() + "," + iwaID.ToString());
            }

            HyperLink viewProjectHyperLink = e.Item.FindControl("viewProjectHyperLink") as HyperLink;
            LinkButton addWatchlistHyperLink = e.Item.FindControl("addWatchlistHyperLink") as LinkButton;
            LinkButton removeWatchlistHyperLink = e.Item.FindControl("removeWatchlistHyperLink") as LinkButton;
            removeWatchlistHyperLink.CommandArgument = WebConvert.ToInt32(dr["bidID"], 0).ToString() + "," + WebConvert.ToInt32(dr["astID"], 0);
            addWatchlistHyperLink.CommandArgument = WebConvert.ToInt32(dr["bidID"], 0).ToString() + "," + WebConvert.ToInt32(dr["astID"], 0) + "," + WebConvert.ToInt32(dr["tagID"], 0);

            UpdatePanel itemRepeaterPanel = e.Item.FindControl("ItemRepeaterPanel") as UpdatePanel;
            if (currentlySourcingProject != null)
            {
                if (currentlySourcingProject.SourcingStage == EXBItemsWantedSourcingStage.Breakthrough)
                {
                    removeWatchlistHyperLink.Visible = true;
                    viewProjectHyperLink.Visible = false;
                    addWatchlistHyperLink.Visible = false;
                }
                else
                {
                    addWatchlistHyperLink.Visible = false;
                    removeWatchlistHyperLink.Visible = false;
                    viewProjectHyperLink.Visible = true;
                    viewProjectHyperLink.NavigateUrl = "MyProjectsNew.aspx?PID=" + currentlySourcingProject.IwaID;
                }
            }
            else
            {
                removeWatchlistHyperLink.Visible = false;
                viewProjectHyperLink.Visible = false;
                addWatchlistHyperLink.Visible = true;
                addWatchlistHyperLink.CommandArgument = WebConvert.ToInt32(dr["bidID"], 0).ToString() + "," + WebConvert.ToInt32(dr["astID"], 0) + "," + WebConvert.ToInt32(dr["tagID"], 0);
            }

            Label newLabel = e.Item.FindControl("NewLabel") as Label;
            Label usedLabel = e.Item.FindControl("UsedLabel") as Label;
            bool bothNotNull = newLabel != null && usedLabel != null;
            int icdID = WebConvert.ToInt32(dr["icdID"], 0);
            if (icdID == (int)EXBItemCondition.New && bothNotNull)
            {
                newLabel.Visible = true;
                usedLabel.Visible = false;
            }
            else if (bothNotNull)
            {
                newLabel.Visible = false;
                usedLabel.Visible = true;
            }

            Label itmQtyLabel = e.Item.FindControl("itmQtyLabel") as Label;
            string uom = WebConvert.ToString(dr["uomName"], "").ToLower();
            if (itmQtyLabel != null)
            {
                if (assetType == EXBAssetType.Equipment)
                {
                    itmQtyLabel.Text = itmQty > 0 ? String.Format("{0:N0}", itmQty) : "N/A";
                }
                else
                {
                    itmQtyLabel.Text = itmTotalFeet > 0 ? String.Format("{0:N0}", itmTotalFeet) + " ft" : "N/A";
                }

            }
            Literal viewProjectLiteral = e.Item.FindControl("viewProjectLiteral") as Literal;
            if (viewProjectLiteral != null)
            {
                if (UtilityTools.GetIsEnergyFlg())
                {
                    viewProjectLiteral.Text = "View Project";
                }
                else
                {
                    viewProjectLiteral.Text = "View Project";
                }
            }
        }
        protected void watchListAddButton_Command(object sender, CommandEventArgs e)
        {
            string commandArgVal = e.CommandArgument.ToString();
            string[] value = commandArgVal.Split(',');
            if (value.Count() < 2)
            {
                return;
            }
            EXBItemsWanted.UserPortalAddItemToWatchListProject(TheUser.UsrID, TheCustomer.CusID, WebConvert.ToInt32(value[0], 0), TheUser.PMAdmID, UtilityTools.ParseAssetType(WebConvert.ToInt32(value[1], (int)EXBItemsWantedType.LinePipe)), WebConvert.ToInt32(value[2], 0));
            WoopraReportAction("AddWatchlist, itmID: " + WebConvert.ToInt32(value[0], 0));
            SetWatchList();
            QueryItems();
        }
        protected void watchListRemoveButton_Command(object sender, CommandEventArgs e)
        {
            string commandArgVal = e.CommandArgument.ToString();
            string[] value = commandArgVal.Split(',');
            if (value.Count() < 2)
            {
                return;
            }
            EXBItemsWanted.UserPortalRemoveItemFromWatchList(TheBuyer.BuyID, WebConvert.ToInt32(value[0], 0), UtilityTools.ParseAssetType(WebConvert.ToInt32(value[1], (int)EXBItemsWantedType.LinePipe)));
            SetWatchList();
            QueryItems();
            WoopraReportAction("RemoveWatchlist, itmID: " + WebConvert.ToInt32(value[0], 0));
        }
        protected void Back_Button_OnCommand(object sender, CommandEventArgs e)
        {
            ItemProfilePanel.Visible = false;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            ItemCardsPanel.Visible = true;
            SetWatchList();
            QueryItems();
            WoopraReportAction("Item Profile - Back Button Pressed.");
        }
        protected void ClearSearch_Command(object sender, CommandEventArgs e)
        {
            //clear attributes.
            SetAttributesChecked(new Dictionary<int, bool>());
            ViewModel.tagValue = "";
            //roadmapHR.Visible = true;
            //ItmGradeFilterPanel.Visible = false;
            //ItmGradeFilterLine.Visible = false;
            AttributesPanel.Visible = true;
            SidePanel_Attributes.Visible = false;
            SidePanel_Category.Visible = true;
            ItemCardsPanel.Visible = false;
            ViewModel.SearchTerm = "";
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            UnCheckGrades();
            //clearButtonHR.Visible = true;
            WoopraReportAction("No Results, Clear Attribute Filters Pressed");
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            QueryItems(true);
            AttributesRepeater.DataBind();
        }
        protected void SendEmailLiteral_Command(object sender, CommandEventArgs e)
        {
            //get asset grade name
            string gradeName = "Asset Type: " + GradeTitleLabel.Text + "\n";

            string attributeText = "";
            foreach (RepeaterItem AttributeField in AttributesRepeater.Items)
            {
                HtmlContainerControl span = AttributeField.FindControl("tagName") as HtmlContainerControl;
                if (span == null)
                    return;
                string attributeName = span.InnerText + ": ";
                Repeater valuesRepeater = AttributeField.FindControl("AttributeValuesRepeater") as Repeater;
                if (valuesRepeater == null)
                    return;
                var foundAttributeChecked = false;
                foreach (RepeaterItem value in valuesRepeater.Items)
                {
                    CheckBox valueCheckBox = value.FindControl("valueCheckbox") as CheckBox;
                    if (valueCheckBox == null)
                        return;
                    if (valueCheckBox.Checked)
                    {
                        attributeName += valueCheckBox.Text + ", ";
                        foundAttributeChecked = true;
                    }
                }
                if (foundAttributeChecked)
                {
                    attributeName = attributeName.Remove(attributeName.Length - 2, 2);
                    attributeName += "\n";
                    attributeText += attributeName;
                }
            }

            //add from line
            string emailText = "A request for a project has been submitted via the user portal. \n";
            emailText += "User Name: " + TheUser.UsrFullName + "\n";
            emailText += "Buyer: " + TheCustomer.CusName + "\n";
            //emailText += "Customer ID: " + TheUser.CusID + "\n";
            emailText += gradeName + attributeText;
            SQLInjection si = new SQLInjection();
            var userText = si.RemoveInject(EmailTextBox.Text);
            emailText += "\n user text: " + userText;
            //get tag fields and values
            //string emailLinkText = "mailTo:" + admin.AdmEmail + "?subject=user portal: create a project&body=" + gradeName + attributeText + "\n";
            //ClientScript.RegisterStartupScript(this.GetType(), "mailto", "parent.location='" + emailLinkText + "'", true);
            WoopraReportAction("Email Sent to admin");
            EmailObject eo = new EmailObject();
            string emailTo = "customerservice@exchangebase.com";
            if (TheUser.PMAdmID > 0)
            {
                EXBAdministrator admin = new EXBAdministrator(TheUser.PMAdmID);
                if (admin.AdmID > 0)
                {
                    emailTo = admin.AdmEmail;
                }
            }
            eo.SendMailMessage("userportal@exchangebase.com", emailTo, "", "", "user portal: new project request",
                emailText + "\n", false);
            EmailSentComplete.Visible = true;
            NoCardsContent.Visible = false;
            EXBNotes2.GenerateUserPortalNote(TheUser.UsrID, TheUser.PMAdmID, emailText);
        }

        protected void ItemProfileControl_BackButton_Clicked(object sender, EventArgs e)
        {
            ItemProfilePanel.Visible = false;
            ItemProfileControl.UpdateFlag = false;
            ItemProfileControl.itmID = 0;
            ItemProfileControl.iwaID = 0;
            ItemCardsPanel.Visible = true;
            SetWatchList();
            QueryItems();
            WoopraReportAction("Item Profile - Back Button Pressed.");
        }

        protected void LinePipeGradeChanged(object sender, EventArgs e)
        {
            ViewModel.pageNum = 1;
            SetGradeFilter();
        }

        protected void UnCheckGrades()
        {
            foreach (RepeaterItem ri in LinePipeGradeRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField GrdHiddenID = ri.FindControl("GrdHiddenID") as HiddenField;
                    CheckBox ItmGradeCheckBox = ri.FindControl("valueCheckbox") as CheckBox;

                    if (ItmGradeCheckBox != null)
                    {
                        ItmGradeCheckBox.Checked = false;
                    }
                }
            }
        }

        private void SetGradeFilter()
        {
            ViewModel.grdIDFilterValue = "";

            string grdIDNames = "";
            ViewModel.ListGradeIds = new List<int>();
            foreach (RepeaterItem ri in LinePipeGradeRepeater.Items)
            {
                if (ri.ItemType == ListItemType.AlternatingItem || ri.ItemType == ListItemType.Item)
                {
                    HiddenField GrdHiddenID = ri.FindControl("GrdHiddenID") as HiddenField;
                    CheckBox ItmGradeCheckBox = ri.FindControl("valueCheckbox") as CheckBox;

                    if (ItmGradeCheckBox != null)
                    {
                        if (ItmGradeCheckBox.Checked)
                        {
                            grdIDNames += ItmGradeCheckBox.Text + ", ";
                            if (GrdHiddenID != null)
                            {
                                ViewModel.ListGradeIds.Add(WebConvert.ToInt32(GrdHiddenID.Value, 0));
                            }
                        }
                    }
                }
            }
            if (grdIDNames.Length > 1)
            {
                grdIDNames = grdIDNames.Remove(grdIDNames.Length - 2);
            }
            WoopraReportAction("Line Pipe Grade Clicked: " + grdIDNames);

            QueryItems(true);
        }

        protected void Go_To_DrillPipe(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int iodID = WebConvert.ToInt32(e.CommandArgument, 0);
            SidePanelTitleLabel.Text = "Drill Pipe " + (sender as LinkButton).Attributes["iodname"];
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            WoopraReportAction("Line Pipe - " + SidePanelTitleLabel.Text);
            ViewModel.iodID = iodID;
            ViewModel.IsDrillPipe = true;
            ViewModel.EXBAssetType = EXBAssetType.OCTG;
            SidePanel_Category.Visible = false;
            SidePanel_Attributes.Visible = true;
            LinePipeGradePanel.Visible = true;
            AttributesRepeater.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            LinePipeGradePanel.DataBind();
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }

        protected void Go_To_Casing(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int iodID = WebConvert.ToInt32(e.CommandArgument, 0);
            SidePanelTitleLabel.Text = "Casing " + (sender as LinkButton).Attributes["iodname"];
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            WoopraReportAction("Line Pipe - " + SidePanelTitleLabel.Text);
            ViewModel.iodID = iodID;
            ViewModel.IsCasing = true;
            ViewModel.EXBAssetType = EXBAssetType.OCTG;
            SidePanel_Category.Visible = false;
            SidePanel_Attributes.Visible = true;
            LinePipeGradePanel.Visible = true;
            AttributesRepeater.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            LinePipeGradePanel.DataBind();
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }
        protected void Go_To_Tubing(object sender, CommandEventArgs e)
        {
            ViewState["ViewModel"] = new InventoryViewModel();
            int iodID = WebConvert.ToInt32(e.CommandArgument, 0);
            SidePanelTitleLabel.Text = "Tubing " + (sender as LinkButton).Attributes["iodname"];
            GradeTitleLabel.Text = SidePanelTitleLabel.Text;
            WoopraReportAction("Line Pipe - " + SidePanelTitleLabel.Text);
            ViewModel.iodID = iodID;
            ViewModel.IsTubing = true;
            ViewModel.EXBAssetType = EXBAssetType.OCTG;
            SidePanel_Category.Visible = false;
            SidePanel_Attributes.Visible = true;
            LinePipeGradePanel.Visible = true;
            AttributesRepeater.Visible = false;
            ItemCardsPanel.Visible = true;
            ViewModel.SearchTerm = "";
            RadComboBoxItems.Text = "";
            RadComboBoxItems2.Text = "";
            LinePipeGradePanel.DataBind();
            NoCardsContent.Visible = false;
            SearchResultsPanel.Visible = false;
            EmailSentComplete.Visible = false;
            ItemProfilePanel.Visible = false;
            QueryItems(true);
        }

    }
    [Serializable]
    public class InventoryViewModel
    {
        public bool IsCasing { get; set; }
        public bool IsTubing { get; set; }
        public bool IsDrillPipe { get; set; }
        public int grdID { get; set; }
        public int iodID { get; set; }
        public EXBAssetType EXBAssetType { get; set; }
        public string SearchTerm { get; set; }
        public string GetDbSearchTerm()
        {
            SQLInjection si = new SQLInjection();
            return si.RemoveInject(SearchTerm);
        }
        public int pageNum { get; set; }
        public List<int> ListGradeIds { get; set; }
        public List<int> EquipmentTagIds { get; set; }
        public int NumberOfItemsPerPage { get; set; }
        public string tagValue { get; set; }
        public string grdIDFilterValue { get; set; }
        public InventoryViewModel()
        {
            NumberOfItemsPerPage = UtilityTools.GetItemsPerPage();
            ListGradeIds = new List<int>();
        }
    }

    [Serializable]
    public class SidePanelGrade
    {
        public int grdID { get; set; }
        public string grade { get; set; }
        public int iodID { get; set; }
        public string iodName { get; set; }
        public int Count { get; set; }
        public SidePanelGrade()
        {

        }
        public SidePanelGrade(vwOpenitemsUserPortalFilter item)
        {
            grdID = item.grdID;
            iodID = item.iodID;
            grade = item.grade;
            iodName = item.iodName;
        }
    }

    public class vwOpenitemsUserPortalFilter
    {
        public int iodID { get; set; }
        public int iodValue { get; set; }
        public string iodName { get; set; }
        public int grdID { get; set; }
        public string grade { get; set; }
        public int bstID { get; set; }
        public int astID { get; set; }
        public int cusID { get; set; }
        public bool itmIsFeatured { get; set; }
        public static List<vwOpenitemsUserPortalFilter> GetAll(int cusID, string andWhere = "")
        {
            List<vwOpenitemsUserPortalFilter> list = new List<vwOpenitemsUserPortalFilter>();

            EXBDbConnection db = new EXBDbConnection();
            DataTable dtAll = db.Query("SELECT * FROM vwOpenItemsUserPortalFilter WHERE cusID <> " + cusID + " " + andWhere);

            foreach (DataRow dr in dtAll.Rows)
            {
                vwOpenitemsUserPortalFilter vw = new vwOpenitemsUserPortalFilter();
                vw.iodID = SafeRead.ToInt32(dr, "iodID");
                vw.iodValue = SafeRead.ToInt32(dr, "iodValue");
                vw.iodName = SafeRead.ToString(dr, "iodName");
                vw.grdID = SafeRead.ToInt32(dr, "grdID");
                vw.grade = SafeRead.ToString(dr, "grade");
                vw.bstID = SafeRead.ToInt32(dr, "bstID");
                vw.astID = SafeRead.ToInt32(dr, "astID");
                vw.cusID = SafeRead.ToInt32(dr, "cusID");
                vw.itmIsFeatured = SafeRead.ToBoolean(dr, "itmIsFeatured");
                list.Add(vw);
            }

            return list;
        }
    }

    public static class SafeRead
    {
        public static int ToInt32(DataRow dr, string column)
        {
            if (dr.Table.Columns.Contains(column))
            {
                return WebConvert.ToInt32(dr[column], 0);
            }
            return 0;
        }
        public static string ToString(DataRow dr, string column)
        {
            if (dr.Table.Columns.Contains(column))
            {
                return WebConvert.ToString(dr[column], "");
            }
            return "";
        }
        public static bool ToBoolean(DataRow dr, string column)
        {
            if (dr.Table.Columns.Contains(column))
            {
                return WebConvert.ToBoolean(dr[column], false);
            }
            return false;
        }
        public static DateTime ToDateTime(DataRow dr, string column)
        {
            if (dr.Table.Columns.Contains(column))
            {
                return WebConvert.ToDateTime(dr[column], DateTime.MinValue);
            }
            return DateTime.MinValue;
        }
        public static decimal ToDecimal(DataRow dr, string column)
        {
            if (dr.Table.Columns.Contains(column))
            {
                return WebConvert.ToDecimal(dr[column], 0);
            }
            return 0;
        }
    }
}