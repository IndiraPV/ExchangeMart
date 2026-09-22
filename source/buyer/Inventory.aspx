<%@ Page Title="" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="Inventory.aspx.cs" Inherits="Exchangebase.Com.Buyer.Inventory" %>

<%@ Register Src="~/Buyer/ProjectProfile.ascx" TagPrefix="uc1" TagName="ProjectProfile" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="itemprofile" Src="~/Buyer/ItemProfileControl.ascx" TagName="itp2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .expandable {
            cursor: pointer;
        }
        .category-pill {
            margin-right:15px;
            padding-top:5px;
        }
        .category-toggle {
            position:absolute;
            width:100%;
            top:-40px;
        }
            .category-toggle a {
                padding-right:10px;
                text-align:right;
                /*right:45px;*/
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid">
        <div class="row">
            <nav class="col-12 col-lg-3 col-xl-2 navbar-expand-lg sidebar my-lg-1 my-lg-auto bg-white">
                <div class="row py-2 d-lg-none" id="mobileFilter">
                    <div class="col-9">
                        <div class="form-inline-block">
                            <telerik:RadComboBox RenderMode="Lightweight"
                                InputCssClass="form-control col-12 col-lg-12 py-1 px-2"
                                DropDownCssClass="btn dropdown"
                                ID="RadComboBoxItems2" CssClass="search-form custom-search-input .search-query"
                                runat="server" Height="280" Width="100%"
                                DropDownWidth="315" EmptyMessage="Search Item # or Description..."
                                ShowDropDownOnTextboxClick="false"
                                HighlightTemplatedItems="true" AutoPostBack="true"
                                EnableLoadOnDemand="true"
                                ShowMoreResultsBox="true"
                                ShowToggleImage="false"
                                OnItemsRequested="RadComboBoxProduct_ItemsRequested">
                                <ClientItemTemplate>
                                <table cellspacing="0" cellpadding="0" border="0">
							        <tr>
								        <td style="vertical-align:middle;"><span>#= Text #</span></td>
							        </tr>
						        </table>
                                </ClientItemTemplate>
                            </telerik:RadComboBox>
                        </div>
                    </div>

                    <div class="col-3 mx-auto text-center">
                        <button class="navbar-toggler white" type="button" data-toggle="collapse"
                            data-target="#filterNav" aria-controls="filterNav" aria-expanded="false" aria-label="Toggle navigation">
                            <i class="fa fa-filter" aria-hidden="true"></i>
                        </button>
                    </div>
                </div>

                <div class="collapse d-lg-inline my-2 my-lg-auto" id="filterNav">
                    <span class="filterHead p-2 d-none d-lg-flex">Refine Results <i class="fa fa-filter ml-auto align-self-center align-middle"></i></span>
                    <div class="form-block mx-2 my-3 d-none d-lg-block">
                        <telerik:RadComboBox RenderMode="Lightweight"
                            InputCssClass="form-control col-12 col-lg-12 py-1 px-2"
                            DropDownCssClass="btn dropdown"
                            ID="RadComboBoxItems" CssClass="form-block d-none d-lg-block"
                            runat="server" Height="280" Width="100%"
                            DropDownWidth="315" EmptyMessage="Search Item # or Description..."
                            ShowDropDownOnTextboxClick="false"
                            HighlightTemplatedItems="true" AutoPostBack="true"
                            EnableLoadOnDemand="true"
                            ShowMoreResultsBox="true"
                            ShowToggleImage="false"
                            OnItemsRequested="RadComboBoxProduct_ItemsRequested">
                        </telerik:RadComboBox>
                    </div>

                    <asp:Panel runat="server" ID="SidePanel_Category" Visible="true">
                        <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                            Midstream Equipment <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="midstreamCountLiteral"></asp:Literal></span>
                        </div>
                        <div class="collapse show mb-3" id="equipNav" style="position:relative">
                            <asp:Repeater runat="server" ID="EquipmentRepeater">
                                <ItemTemplate>
                                    <div class="row subItems">
                                        <div class="col-12">
                                            <div class="px-2 py-1">
                                                <asp:LinkButton ID="LinkButton1" OnCommand="Go_To_Attributes" runat="server"
                                                    CommandArgument='<%# Eval("grdID") %>' CssClass="d-flex" gradename='<%# Eval("grade") %>'>
                                                    <%# Eval("grade") %>
                                                    <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>

                            <div id="showMore" class="category-toggle">
                                <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                    <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                    <span class="aria-true"> <i class="fa fa-angle-double-up align-middle"></i></span>
                                </a>
                            </div>
                        </div>
                        <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                            Line Pipe  <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="linePipeLiteral"></asp:Literal></span>
                        </div>
                        <div class="collapse show m-0" id="pipeNav" style="position:relative">
                            <asp:Repeater runat="server" ID="LinePipeIODRepeater">
                                <ItemTemplate>
                                    <div class="row subItems">
                                        <div class="col-12">
                                            <div class="px-2 py-1">
                                                <asp:LinkButton OnCommand="Go_To_LinePipeGrades"
                                                    CommandArgument='<%# Eval("iodID") %>' runat="server" ID="LinePipeIODLinkButton" class="d-flex"
                                                    iodname='<%# Eval("iodName") %>'><%# Eval("iodName") %>
                                                    <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                </asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div id="showMore" class="category-toggle">
                                <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                    <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                    <span class="aria-true"> <i class="fa fa-angle-double-up align-middle"></i></span>
                                </a>
                            </div>
                        </div>
                     

                       

                        <asp:Panel ID="OCTGPanel" runat="server" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                <%--Casing & Tubing (OCTG)--%>
                                OCTG
                            </div>
                            <div class="collapse show m-0"  id="octgNav" style="position:relative">
                                <asp:Repeater runat="server" ID="OCTGRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                    <asp:LinkButton OnCommand="Go_To_CasingGrades"
                                                        CommandArgument='<%# Eval("iodID") %>' runat="server" ID="LinePipeIODLinkButton" class="d-flex"
                                                        iodname='<%# Eval("iodName") %>'><%# Eval("iodName") %>
                                                    <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div id="showMore" class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"> <i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>


                        <asp:Panel runat="server" ID="ShipsGradePanel" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Ships  <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="shipsLiteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="shipsDiv" style="position:relative">
                                <asp:Repeater runat="server" ID="ShipsGradeRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                     <asp:LinkButton ID="LinkButton1" OnCommand="Go_To_Attributes" runat="server"
                                                        CommandArgument='<%# Eval("grdID") %>' CssClass="d-flex" gradename='<%# Eval("grade") %>'>
                                                        <%# Eval("grade") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                              <div id="showMore" class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"><i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>

                         <asp:Panel runat="server" ID="RigsPanel" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Rigs & Platforms <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="rigsliteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="rigsDiv" style="position:relative">
                                <asp:Repeater runat="server" ID="RigsRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                     <asp:LinkButton ID="LinkButton1" OnCommand="Go_To_Attributes" runat="server"
                                                        CommandArgument='<%# Eval("grdID") %>' CssClass="d-flex" gradename='<%# Eval("grade") %>'>
                                                        <%# Eval("grade") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <div id="showMore"  class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"> <i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>
                           <asp:Panel ID="UpstreamPanel" runat="server" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Upstream Equipment <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="upstreamliteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="equipUpNav" style="position:relative">
                                <asp:Repeater runat="server" ID="UpstreamEquipmentRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                    <asp:LinkButton ID="LinkButton1" OnCommand="Go_To_Attributes" runat="server"
                                                        CommandArgument='<%# Eval("grdID") %>' CssClass="d-flex" gradename='<%# Eval("grade") %>'>
                                                        <%# Eval("grade") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <div id="showMore"  class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"> <i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"><i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>
                       
                         <asp:Panel ID="CasingPanel" runat="server" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Casing <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="casingliteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="casingDiv" style="position:relative">
                                <asp:Repeater runat="server" ID="CasingRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                   <asp:LinkButton OnCommand="Go_To_Casing"
                                                        CommandArgument='<%# Eval("iodID") %>' runat="server" ID="LinePipeIODLinkButton" class="d-flex"
                                                        iodname='<%# Eval("iodName") %>'><%# Eval("iodName") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <div id="showMore" class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"><i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"><i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>
                         <asp:Panel ID="TubingPanel" runat="server" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Tubing <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="tubingliteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="tubingDiv" style="position:relative">
                                <asp:Repeater runat="server" ID="TubingRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                   <asp:LinkButton OnCommand="Go_To_Tubing"
                                                        CommandArgument='<%# Eval("iodID") %>' runat="server" ID="LinePipeIODLinkButton" class="d-flex"
                                                        iodname='<%# Eval("iodName") %>'><%# Eval("iodName") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <div id="showMore" class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"><i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"><i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>
                         <asp:Panel ID="DrillPipePanel" runat="server" Visible="false">
                            <div class="filter-text mx-2 mt-2 mb-1 pb-2 d-flex justify-content-between">
                                Drill Pipe <span class="badge badge-primary badge-pill category-pill"><asp:Literal runat="server" id="drillpipeliteral"></asp:Literal></span>
                            </div>
                            <div class="collapse show mb-3" id="drillPipeDiv" style="position:relative">
                                <asp:Repeater runat="server" ID="DrillPipeRepeater">
                                    <ItemTemplate>
                                        <div class="row subItems">
                                            <div class="col-12">
                                                <div class="px-2 py-1">
                                                   <asp:LinkButton OnCommand="Go_To_DrillPipe"
                                                        CommandArgument='<%# Eval("iodID") %>' runat="server" ID="LinePipeIODLinkButton" class="d-flex"
                                                        iodname='<%# Eval("iodName") %>'><%# Eval("iodName") %>
                                                        <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    </asp:LinkButton>
                                                </div>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>

                                <div id="showMore" class="category-toggle">
                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                        <span class="aria-false"><i class="fa fa-angle-double-down align-middle"></i></span>
                                        <span class="aria-true"><i class="fa fa-angle-double-up align-middle"></i></span>
                                    </a>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="SidePanel_Attributes" Visible="false">
                        <h5>
                            <asp:LinkButton ID="LinkButton2" runat="server" OnCommand="Go_To_Categories" class="badge badge-pill badge-primary white mx-2">
                                <asp:Label runat="server" ID="SidePanelTitleLabel"></asp:Label><i class="fa fa-close ml-2"></i>
                            </asp:LinkButton>
                        </h5>

                        <asp:Panel runat="server" ID="AttributesPanel" Visible="true" CssClass="">
                            <div>
                                <asp:Repeater ID="AttributesRepeater" runat="server" Visible="false" DataSource='<%# GetAttributeFields() %>'>
                                    <ItemTemplate>
                                        <span runat="server" id="tagName" class="filter-text mx-2 mt-2 mb-1 py-2 d-flex justify-content-between"><%# Eval("tagName") %></span>
                                        <div class="collapse show mb-2">
                                            <div id="AttributesRepeater_Div" class="row" runat="server">
                                                <asp:Repeater ID="AttributeValuesRepeater" runat="server" DataSource='<%# Eval("GradeAttributeValues") %>'>
                                                    <ItemTemplate>
                                                        <div class="col-12 subItems">
                                                            <div class="px-2 py-1">
                                                                <asp:CheckBox ID="valueCheckbox" tagid='<%# Eval("tagID") %>'
                                                                    OnCheckedChanged="Attribute_Check_Changed" AutoPostBack="true"
                                                                    CssClass="" runat="server" Text='<%# Eval("tagName") %>' />
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>

                                                </asp:Repeater>
                                                <div id="showMore" class="col-12">
                                                    <a href="javascript:void(0);" class="showMore d-block py-2 ml-2 expandable" data-toggle="collapse">
                                                        <span class="aria-false">Show More <i class="fa fa-angle-double-down align-middle"></i></span>
                                                        <span class="aria-true">Show Less <i class="fa fa-angle-double-up align-middle"></i></span>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>

                                    </ItemTemplate>
                                </asp:Repeater>

                                <asp:Panel runat="server" ID="LinePipeGradePanel" Visible="false">
                                    <span runat="server" id="tagName" class="filter-text mx-2 py-2 d-flex justify-content-between">Grade</span>

                                    <asp:Repeater ID="LinePipeGradeRepeater" runat="server" DataSource='<%# GetLinePipeGrades() %>'>
                                        <ItemTemplate>
                                            <div class="col-12">
                                                <div class="py-1 d-flex">
                                                    <asp:CheckBox ID="valueCheckbox" tagid='<%# Eval("grdID") %>'
                                                        OnCheckedChanged="LinePipeGradeChanged" AutoPostBack="true"
                                                        CssClass="" runat="server" Text='<%# Eval("grade") %>' />
                                                    <span class="badge badge-pill badge-secondary ml-auto align-self-center"><%# Eval("Count") %></span>
                                                    <asp:HiddenField ID="GrdHiddenID" Value='<%# Eval("grdID") %>' runat="server" />
                                                </div>
                                            </div>
                                        </ItemTemplate>
                                    </asp:Repeater>
                            </div>
                        </asp:Panel>
                </div>
            </asp:Panel>
                    </asp:Panel>
        </div>
        </nav>
            
            <asp:Panel Visible="false" runat="server" ID="ItemProfilePanel" class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-5" role="main">
                <itemprofile:itp2 ID="ItemProfileControl" runat="server" OnBackButton_Clicked="ItemProfileControl_BackButton_Clicked"></itemprofile:itp2>
            </asp:Panel>
        <asp:Panel runat="server" ID="SearchResultsPanel" class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-5" Visible="false" role="main">
            <div class="container-fluid pt-2" id="listView">
                <div class="row mb-2 pb-2 mb-lg-4 pb-lg-3" id="title-row">
                    <div class="col-12 col-md-6 align-self-end text-center text-lg-left">
                        <h1><i class="fa fa-exclamation-circle" aria-hidden="true"></i>No Matching Assets!</h1>
                    </div>
                </div>
                <div class="row bg-white p-4">
                    <div class="col-12 col-lg-5">
                        <h3>Dont Worry!</h3>
                        <p>
                            Our supply chain team is here to find assets that match your needs.

                        </p>
                        <p>
                            Please provide us with any additional information below that can help us facilitate your request. For example, tell us when and where your project will be taking place and when you will need to make a decision on assets.
                        </p>
                        <h4 class="h4 pb-2 mb-3">Search Details</h4>
                        <div class="row">
                            <div class="col-6">
                                <asp:Label Style="" runat="server" ID="AttributeDescriptionTextBox2"></asp:Label>
                            </div>
                            <div class="col-6">
                                <label for="ContentPlaceHolder1_EmailTextBox" class="d-block bold">Additional Information</label>
                                <asp:TextBox Rows="6" CssClass="col-12 d-block" runat="server" TextMode="MultiLine" ID="TextBox1"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 my-2">
                                <asp:LinkButton OnCommand="SendEmailLiteral_Command" runat="server" ID="LinkButton5" class="btn btn-success d-inline-block mr-3"><i class="fa fa-paper-plane-o mr-2" aria-hidden="true"></i> Request Item</asp:LinkButton>
                                <asp:LinkButton OnCommand="ClearSearch_Command" runat="server" ID="LinkButton6" class="btn btn-outline-danger d-inline-block"><i class="fa fa-times-circle mr-2" aria-hidden="true"></i> Clear Filters</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-lg-2 align-self-center align-items-center text-center">
                        <span class="orCallout bold">OR </span>
                    </div>
                    <div class="col-12 col-lg-5 align-self-center align-items-center">
                        <h3>Try Again...</h3>
                        <p runat="server" visible="false">
                            Your search -
                            <asp:Label runat="server" class="bold" ID="SearchTermsLabel"></asp:Label>
                            - did not match any items in our current inventory.
                        </p>
                        <p>
                            Consider the following:
                                <ul>
                                    <li>Make sure keywords are spelled correctly</li>
                                    <li>Try fewer keywords</li>
                                    <li>Try different keywords</li>
                                    <li>Clear any selected attributes</li>
                                    <li>Clear any selected asset types</li>
                                    <li>Still not seeing what you want? Contact Us!</li>
                                </ul>
                        </p>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="NoCardsContent" class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-5" Visible="false" role="main">
            <div class="container-fluid pt-2" id="listView">
                <div class="row mb-2 pb-2 mb-lg-4 pb-lg-3" id="title-row">
                    <div class="col-12 col-md-6 align-self-end text-center text-lg-left">
                        <h1><i class="fa fa-exclamation-circle" aria-hidden="true"></i>No Matching Assets!</h1>
                    </div>
                </div>
                <div class="row bg-white p-4">
                    <div class="col-12 col-lg-5">
                        <h3>Dont Worry!</h3>
                        <p>
                            Our supply chain team is here to find assets that match your needs.

                        </p>
                        <p>
                            Please provide us with any additional information below that can help us facilitate your request. For example, tell us when and where your project will be taking place and when you will need to make a decision on assets.
                        </p>
                        <h4 class="h4 pb-2 mb-3">Search Details</h4>
                        <div class="row">
                            <div class="col-6">
                                <asp:Label Style="" runat="server" ID="AttributeDescriptionTextBox"></asp:Label>
                            </div>
                            <div class="col-6">
                                <label for="ContentPlaceHolder1_EmailTextBox" class="d-block bold">Additional Information</label>
                                <asp:TextBox Rows="6" CssClass="col-12 d-block" runat="server" TextMode="MultiLine" ID="EmailTextBox"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-12 my-2">
                                <asp:LinkButton OnCommand="SendEmailLiteral_Command" runat="server" ID="LinkButton4" class="btn btn-success d-inline-block mr-3"><i class="fa fa-paper-plane-o mr-2" aria-hidden="true"></i> Request Item</asp:LinkButton>
                                <asp:LinkButton OnCommand="ClearSearch_Command" runat="server" ID="LinkButton7" class="btn btn-outline-danger d-inline-block"><i class="fa fa-times-circle mr-2" aria-hidden="true"></i> Clear Filters</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="col-12 col-lg-2 align-self-center align-items-center text-center">
                        <span class="orCallout bold">OR </span>
                    </div>
                    <div class="col-12 col-lg-5 align-self-center align-items-center">
                        <h3>Try Again...</h3>
                        <p runat="server" visible="false">
                            Your search -
                            <asp:Label runat="server" class="bold" ID="SearchTermsLabel2"></asp:Label>
                            - did not match any items in our current inventory.
                        </p>
                        <p>
                            Consider the following:
                                <ul>
                                    <li>Make sure keywords are spelled correctly</li>
                                    <li>Try fewer keywords</li>
                                    <li>Try different keywords</li>
                                    <li>Clear any selected attributes</li>
                                    <li>Clear any selected asset types</li>
                                    <li>Still not seeing what you want? Contact Us!</li>
                                </ul>
                        </p>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="EmailSentComplete" Style="padding-left: 15px;" class="col-md-9 main-content" Visible="false">
            <div class="fade-in-no-movement" style="text-align: center; padding-top: 35px; padding-bottom: 150px; padding-left: 15px; padding-right: 15px;">
                <div class="col-xs-2 hidden-sm hidden-xs"></div>
                <div class=" col-xs-9 " style="max-width: 500px;">
                    <img style="margin: 20px; margin-bottom: 5px;" height="150" src="../Images/Buyer/watchlistempty.jpg" />
                    <p>
                        Thank you for your request.
                    </p>
                    <p>
                        Your Project Manager will contact you shortly.
                    </p>
                    <p></p>
                    <div class="col-xs-12;">
                        <asp:Label runat="server" ID="Label1"></asp:Label>
                    </div>
                    <div class="col-xs-12;">
                        <asp:LinkButton runat="server" OnCommand="ClearSearch_Command" ID="LinkButton3" Style="color: #5bc0de; border-color: #5bc0de;" class="btn btn-default"><span class="" style="margin-right: 5px;"></span>Clear Filters</asp:LinkButton>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="ItemCardsPanel" Visible="false" Style="width: 100%;">
            <div class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-1 pb-lg-5 mt-2 mt-lg-auto" role="main">
                <div class="container-fluid" id="listView">

                    <div class="row mb-2 pb-2 mb-lg-4 pb-lg-3" id="title-row">
                        <div class="col-12 col-md-6 align-self-end text-center text-lg-left">
                            <h1>
                                <asp:Label runat="server" ID="GradeTitleLabel">Current Inventory</asp:Label></h1>
                        </div>
                        <asp:Panel runat="server" Visible="False" ID="PageDisplayPanel"
                            class="col-12 col-md-6  align-self-center align-self-md-end text-center text-md-right my-1 mt-lg-3 mt-lg-auto">
                            <span class="pagination-text align-bottom mx-md-3 d-block d-md-inline-block my-2 my-md-auto">
                                <strong class="align-bottom">Showing:</strong>
                                <asp:Label class="fade-in-no-movement" ID="PageDisplayString" runat="server"></asp:Label>
                            </span>
                            <nav class="navbar-pagination">
                                <ul class="pagination" visible="false" runat="server" id="PagingPanel1">
                                    <li class="page-item">
                                        <asp:LinkButton runat="server" ID="PreviousPageLinkButton"
                                            OnCommand="PreviousPage_Command" class="page-link p-2">
                                                <i class="fa fa-chevron-left align-middle mx-1" aria-hidden="true"></i>
                                                Previous</asp:LinkButton></li>
                                    <li class="page-item">
                                        <asp:LinkButton runat="server" ID="NextPageLinkButton"
                                            OnCommand="NextPage_Command" class="page-link p-2">
                                                Next <i class="fa fa-chevron-right align-middle mx-1" aria-hidden="true"></i></asp:LinkButton></li>
                                </ul>
                            </nav>
                        </asp:Panel>
                    </div>
                    <div class="row placeholders">
                        <asp:Repeater runat="server" ID="ItemRepeater" OnItemDataBound="ItemRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div class="col-12 col-md-6 col-lg-4 col-xl-3 mb-3 mb-lg-5" id="mainCardDiv" runat="server" onclick="cardClicked(this)">
                                    <div class="card shadow">
                                        <asp:Panel runat="server" ID="FeaturedPanel" class="card-header card-header-success">
                                            <i class="fa fa-star" aria-hidden="true"></i>Featured
                                        </asp:Panel>
                                        <div class="img text-center">
                                            <asp:Image runat="server" ID="itmImgMain" Style="font-size: 14px;" alt="no image found"
                                                onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" class="card-img" />
                                        </div>
                                        <div class="card-body px-3 py-2">
                                            <h2 class="card-title my-1"><a><%# Eval("itmName") %></a></h2>
                                        </div>
                                        <div class="card-footer px-3 py-2">
                                            <span class="badge badge-secondary">#<%# Eval("itmID") %></span>
                                            <asp:Label runat="server" Visible="false" ID="UsedLabel"
                                                class="badge badge-warning"><%# Eval("icdName") %></asp:Label>
                                            <asp:Label runat="server" Visible="false" ID="NewLabel"
                                                class="badge badge-success"><%# Eval("icdName") %></asp:Label>
                                            <asp:Label runat="server" ID="ODLabel" Visible="false" CssClass="badge badge-primary"></asp:Label>
                                            <span class="badge badge-primary">
                                                <asp:Label runat="server" ID="GradeLabel"></asp:Label></span>
                                            <div class="row no-gutters detailRow">
                                                <asp:Panel runat="server" ID="AskPricePanel" Visible="true" class="col-6 py-2">
                                                    <asp:Label runat="server" ID="QuotePriceAskPriceLabel" class="details key quote-price"></asp:Label>
                                                    <span class="details value quote-price">
                                                        <asp:Literal runat="server" ID="itmSellPriceLiteral"></asp:Literal></span>
                                                </asp:Panel>
                                                <div class="col-6 py-2">
                                                    <asp:Label ID="OriginalPriceIndexPriceLabel" runat="server" class="details key original-price">Original Price</asp:Label>
                                                    <span class="details value original-price">
                                                        <asp:Literal runat="server" ID="itmIndexPriceLiteral"></asp:Literal></span>
                                                </div>
                                            </div>
                                            <div class="row no-gutters detailRow">
                                                <div class="col-6 py-2">
                                                    <span class="details key">Quantity Available</span>
                                                    <asp:Label runat="server" ID="itmQtyLabel"
                                                        class="details value"><%# Eval("itmQty") %></asp:Label>
                                                </div>
                                                <div class="col-6 py-2">
                                                    <span class="details key">Location</span>
                                                    <span class="details value"><%# Eval("facLocation") %></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-subfooter">
                                            <asp:LinkButton Visible="false" ID="removeWatchlistHyperLink"
                                                OnCommand="watchListRemoveButton_Command" runat="server"
                                                class="btn btn-outline-danger">
                                                <i class="fa fa-minus-circle mr-2" aria-hidden="true"></i>
                                                Watchlist </asp:LinkButton>
                                            <asp:HyperLink ID="viewProjectHyperLink" runat="server"
                                                Visible="false" class="btn btn-outline-primary"> 
                                                <i class="fa fa-link mr-2" aria-hidden="true"></i> 
                                                <asp:Literal runat="server" ID="viewProjectLiteral"></asp:Literal></asp:HyperLink>
                                            <asp:LinkButton ID="addWatchlistHyperLink" runat="server" Visible="false"
                                                OnCommand="watchListAddButton_Command" class="btn btn-outline-success"> 
                                                <i class="fa fa-plus-circle mr-2" aria-hidden="true"></i> Watchlist </asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                    <hr class="mt-2 mt-lg-4">
                    <div class="col-12 align-self-end text-center text-md-right">
                        <nav class="navbar-pagination">
                            <ul class="pagination" visible="false" runat="server" id="PagingPanel2">
                                <li class="page-item">
                                    <asp:LinkButton runat="server" ID="PreviousPageLinkButton2"
                                        OnCommand="PreviousPage_Command" class="page-link"><i class="fa fa-chevron-left" aria-hidden="true"></i>
                                    Previous</asp:LinkButton></li>
                                <li class="page-item">
                                    <asp:LinkButton runat="server" ID="NextPageLinkButton2"
                                        OnCommand="NextPage_Command" class="page-link">
                                    Next <i class="fa fa-chevron-right" aria-hidden="true"></i></asp:LinkButton></li>
                            </ul>
                        </nav>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </div>
    <footer class="py-3 text-center" runat="server" style="display: none;" id="UnauthorizedAccessProhibitedFooter">
        <div class="container-fluid">
            <div class="row">
                <div class="col-lg-2"></div>
                <div class="col-12 col-lg-10 pl-lg-5">
                    <span class="footerCallout d-block my-2">Unauthorized Access is Strictly Prohibited
                    </span>
                    <span class="copyright d-block my-2">&copy; Copyright 2018 ExchangeBase, LLC. All rights reserved.
                    </span>
                </div>
            </div>
        </div>
    </footer>
    <script>
        function cardClicked(card) {
            //get the cmdArgument
            var cmdargument = card.getAttribute("cmdargument");
            __doPostBack('card', cmdargument);

        }

        $(document).ready(function () {

            $("div.form-block > div").removeClass("RadComboBox");
            $("div.form-block > div").removeClass("RadComboBox_Default");
            $("div.form-inline-block > div").removeClass("RadComboBox");
            $("div.form-inline-block > div").removeClass("RadComboBox_Default");
            $("input[type=checkbox]").addClass("align-middle mr-2");

            var attItemCountHidden = 8;
            var attShowMoreHidden = 9;
            var catItemCountHidden = 0;
            var catShowMoreHidden = 0;
            var OCTGItemCountHidden = 0;
            var OCTGShowMoreHidden = 0;

            $("div[id*=AttributesRepeater_Div]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    if (i > attItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    }
                    totalItems++;
                });
                if (totalItems > attShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More attributes sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                        }
                    });

                } else {
                    $this.find("#showMore").hide();
                }

            });

            $("div[id*=equipNav]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More equipment sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarMid", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarMid", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=equipUpNav]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More equipment sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarUp", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarUp", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=shipsDiv]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                    $(this).hide();
                    $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More ships sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarShips", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarShips", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=rigsDiv]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                    $(this).hide();
                    $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More rigs sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarRigs", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarRigs", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=pipeNav]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More line pipe OD' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarPipe", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarPipe", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=octgNav]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > OCTGItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > OCTGShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More OCTG OD' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarOCTG", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarOCTG", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

            $("div[id*=drillPipeDiv]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More drill pipe sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarDrillPipe", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarDrillPipe", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });
            $("div[id*=tubingDiv]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More tubing sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarTubing", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarTubing", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });
            $("div[id*=casingDiv]").each(function () {
                var $this = $(this);
                var totalItems = 0;
                $this.children('.subItems').each(function (i) {
                    //if (i > catItemCountHidden) {
                        $(this).hide();
                        $(this).addClass("hidden");
                    //}
                    totalItems++;
                });
                if (totalItems > catShowMoreHidden) {
                    $this.find("#showMore").show();
                    $this.find("#showMore a").on("click", function (e) {
                        if (typeof woopra != "undefined" && woopra) {
                            woopra.track('userportal', { page: 'Inventory', description: 'Show More casing sidepanel' });
                        }
                        if ($(this).hasClass("expanded")) {
                            $(this).removeClass("expanded");
                            $(this).addClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarCasing", "False");
                        } else {
                            $(this).addClass("expanded");
                            $(this).removeClass("expandable");
                            $this.children('.subItems.hidden').slideToggle();
                            sessionStorage.setItem("sessionVarCasing", "True");
                        }
                    });

                } else {
                    $this.find("#showMore").show();
                }

            });

        });

        $(document).ready(function () {
            var sessionVarEquip = sessionStorage.getItem("sessionVarEquip");
            var sessionVarPipe = sessionStorage.getItem("sessionVarPipe");
            var sessionVarOCTG = sessionStorage.getItem("sessionVarOCTG");
            var sessionVarMid = sessionStorage.getItem("sessionVarMid");
            var sessionVarUp = sessionStorage.getItem("sessionVarUp");
            var sessionVarCasing = sessionStorage.getItem("sessionVarCasing");
            var sessionVarTubing = sessionStorage.getItem("sessionVarTubing");
            var sessionVarDrillPipe = sessionStorage.getItem("sessionVarDrillPipe");
            var sessionVarRigs = sessionStorage.getItem("sessionVarRigs");
            var sessionVarShips = sessionStorage.getItem("sessionVarShips");

            if (sessionVarMid == "True") {
                $("#equipNav .subItems.hidden").slideDown(0);
                $("#equipNav #showMore a").addClass("expanded");
                $("#equipNav #showMore a").removeClass("expandable");
            }
            if (sessionVarPipe == "True") {
                $("#pipeNav .subItems.hidden").slideDown(0);
                $("#pipeNav #showMore a").addClass("expanded");
                $("#pipeNav #showMore a").removeClass("expandable");
            }
            if (sessionVarOCTG == "True") {
                $("#octgNav .subItems.hidden").slideDown(0);
                $("#octgNav #showMore a").addClass("expanded");
                $("#octgNav #showMore a").removeClass("expandable");
            }
            if (sessionVarMid == "True") {
                $("#equipNav .subItems.hidden").slideDown(0);
                $("#equipNav #showMore a").addClass("expanded");
                $("#equipNav #showMore a").removeClass("expandable");
            }
            if (sessionVarUp == "True") {
                $("#equipUpNav .subItems.hidden").slideDown(0);
                $("#equipUpNav #showMore a").addClass("expanded");
                $("#equipUpNav #showMore a").removeClass("expandable");
            }
            if (sessionVarCasing == "True") {
                $("#casingDiv .subItems.hidden").slideDown(0);
                $("#casingDiv #showMore a").addClass("expanded");
                $("#casingDiv #showMore a").removeClass("expandable");
            }
            if (sessionVarTubing == "True") {
                $("#tubingDiv .subItems.hidden").slideDown(0);
                $("#tubingDiv #showMore a").addClass("expanded");
                $("#tubingDiv #showMore a").removeClass("expandable");
            }
            if (sessionVarDrillPipe == "True") {
                $("#drillPipeDiv .subItems.hidden").slideDown(0);
                $("#drillPipeDiv #showMore a").addClass("expanded");
                $("#drillPipeDiv #showMore a").removeClass("expandable");
            }
            if (sessionVarRigs == "True") {
                $("#rigsDiv .subItems.hidden").slideDown(0);
                $("#rigsDiv #showMore a").addClass("expanded");
                $("#rigsDiv #showMore a").removeClass("expandable");
            }
            if (sessionVarShips == "True") {
                $("#shipsDiv .subItems.hidden").slideDown(0);
                $("#shipsDiv #showMore a").addClass("expanded");
                $("#shipsDiv #showMore a").removeClass("expandable");
            }
        })
    </script>
</asp:Content>
