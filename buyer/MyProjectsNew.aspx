<%@ Page Title="" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="MyProjectsNew.aspx.cs" Inherits="Exchangebase.Com.Buyer.MyProjectsNew" %>

<%@ Register Src="~/Buyer/ProjectProfile.ascx" TagPrefix="uc1" TagName="ProjectProfile" %>

<%@ Register Src="~/Buyer/ItemProfileControl.ascx" TagPrefix="uc1" TagName="ItemProfileControl" %>
<%@ Register Src="~/buyer/LoadOutProfile.ascx" TagPrefix="uc1" TagName="LoadOutProfileControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="sidebar">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.2.0/css/all.css" integrity="sha384-hWVjflwFxL6sNzntih27bfxkr27PmbbK/iSvJ+a4+0owXq79v+lsFkW54bOGbiDQ" crossorigin="anonymous">
    <link href="resources/jquery.dataTables.min.css" rel="stylesheet" />

    <style>
        table.dataTable thead .sorting {
            background: url('images/sort_both.png') no-repeat center right !important;
        }

        table.dataTable thead .sorting_asc {
            background: url('images/sort_asc.png') no-repeat center right !important;
        }

        table.dataTable thead .sorting_desc {
            background: url('images/sort_desc.png') no-repeat center right !important;
        }

        table.dataTable thead .sorting_asc_disabled {
            background: url('images/sort_asc_disabled.png') no-repeat center right !important;
        }

        table.dataTable thead .sorting_desc_disabled {
            background: url('images/sort_desc_disabled.png') no-repeat center right !important;
        }

        .btn-orange {
            color: white;
            border-color: #ea571a;
            box-shadow: 0 5px 16px 0 rgba(0,0,0,0.2), 0 3px 20px 0 rgba(0,0,0,0.19);
            background: linear-gradient(#ea571ac4, #ea571a);
        }

            .btn-orange:hover, .btn-orange:active {
                background: linear-gradient(#ea571ac4, #ea571a);
                box-shadow: 0 5px 16px 0 rgba(0,0,0,0.35), 0 3px 20px 0 rgba(0,0,0,0.3);
                border-color: #ea571a;
                color: white;
            }

        .lg-btn-text {
            text-transform: uppercase;
            font-size: 14px;
            font-weight: 600;
            text-align: left;
            white-space: normal;
        }

        .btn-icon-holder {
            height: 30px;
            padding-right: 10px;
            margin: auto;
        }

        div.small-block {
            color: #737373;
            font-weight: 600;
        }

        .inline-flex {
            display: inline-flex;
        }

        .d-block.active {
            font-weight: 600;
            background-color: #ea571a1f;
        }

        .highly-active {
            background-color: #ea571a1f;
        }

        .d-block.active .small-block {
            font-weight: 800;
        }

        @media (min-width: 1200px) {
            .col-xl-5e {
                flex: 0 0 20%;
                max-width: 20%;
            }
        }

        .col-fixed-143px {
            flex: 0 0 158px;
            max-width: 158px;
        }

        .col-fixed-view {
            flex: 0 0 90px;
            max-width: 90px;
            padding-right: 15px;
        }

        .col-1e {
            flex: 0 0 12.666667%;
            max-width: 12.666667%;
            padding-right: 15px;
            padding-left: 15px;
        }

        .col-11e {
            flex: 0 0 87.33333%;
            max-width: 87.33333%;
            padding-right: 15px;
            padding-left: 15px;
        }

        .flex-1 {
            flex: 1 1;
            padding-left: 15px;
            padding-right: 15px;
        }

        .mini-div {
            max-height: 190px;
            overflow: hidden;
        }

        .flex-row {
            display: flex;
        }

        .no-padding {
            padding-left: 0px !important;
            padding-right: 0px !important;
        }

        .full-height {
            height: 100%;
        }

        .card-img {
            width: 100%;
            height: 100%;
            border-radius: calc(.25rem - 1px);
            object-fit: contain;
        }

        .card.shadow:hover {
            -webkit-box-shadow: 0 3px 10px 10px rgba(0,0,0,.05);
            box-shadow: 0 3px 10px 10px rgba(0,0,0,.05);
            -webkit-transition: all .4s ease;
            -o-transition: all .4s ease;
            transition: all .4s ease;
        }

        .top-align-header th {
            vertical-align: top !important;
        }
    </style>
    <script>
        $(document).ready(function () {
            $("#upArrow").toggle();
            $("#sidebar_closedProjectTitle").click(function () {
                $("#closedProjectDiv").toggleClass("d-none");
                $("#downArrow").toggleClass("d-none");
                $("#upArrow").toggleClass("d-none");
            });
                        $("#sidebar_soldItemsTitle").click(function () {
                $("#soldItemsDivClient").toggleClass("d-none");
                $("#downArrow2").toggleClass("d-none");
                $("#upArrow2").toggleClass("d-none");
            });
            $(function () {
                $('[data-toggle="popover"]').popover()
            })
            $(document).on('click', function (e) {
                $('[data-toggle="popover"],[data-original-title]').each(function () {
                    //the 'is' for buttons that trigger popups
                    //the 'has' for icons within a button that triggers a popup
                    if (!$(this).is(e.target) && $(this).has(e.target).length === 0 && $('.popover').has(e.target).length === 0) {
                        (($(this).popover('hide').data('bs.popover') || {}).inState || {}).click = false  // fix for BS 3.3.6
                    }

                });
            });
        });
    </script>
    <asp:HiddenField ID="soldItemFilterValue" Value="0" runat="server" />
    <asp:HiddenField ID="iwaIDFilterValue" Value="0" runat="server" />
    <asp:Panel runat="server" ID="SideBarPanel" CssClass="col-12 col-lg-auto no-gutters">

        <nav class="col-12 col-lg-3 col-xl-2 navbar-expand-lg sidebar my-lg-1 my-lg-auto bg-white">
            <div class="row py-2 mb-3 d-lg-none" id="mobileFilter">
                <div class="col-12 mx-auto text-center">
                    <button class="navbar-toggler white" type="button" data-toggle="collapse" data-target="#filterNav" aria-controls="filterNav" aria-expanded="false" aria-label="Toggle navigation">
                        <i class="fa fa-clipboard mr-2" aria-hidden="true"></i><asp:Literal runat="server" ID="mobileNavbarTitle"></asp:Literal>
                    </button>
                </div>
            </div>
            <div class="collapse d-lg-inline" id="filterNav">
                <a runat="server" visible="false" style="display: none;" class="filterHead p-2 d-none d-lg-flex" href="/Buyer/MyProjectsNew.aspx">
                    <asp:Literal runat="server" ID="myprojectsLiteral"></asp:Literal>
                    <i class="fa fa-clipboard ml-auto align-self-center align-middle"></i></a>
                <div class="filter-text mt-2 mb-1 mx-2 pb-2 d-flex justify-content-between" runat="server" id="featuredAssetDiv1" style="color: #ea571a;">
                    Featured
                </div>
                <div class="mx-2" runat="server" id="featuredAssetDiv2" >
                    <asp:LinkButton OnCommand="IwaLinkButton_Command" CssClass="full-width" CommandName="FilterRecord" runat="server" ID="highlyDiscountedLabel1">
                        <asp:Panel runat="server" id="highlyDiscountedDiv1" visible="false" class="filter-text mb-1 pb-2 d-flex justify-content-between d-none" style="color: #ea571a; border-bottom: none;">
                            <div class="px-3 active" >
                                <div runat="server" id="wefwef">
                                        <div runat="server" id="Div1" class="small-block pt-2">
                                           Available Assets
                                        </div>
                                </div>
                            </div>
                        </asp:Panel>
                    </asp:LinkButton>
                </div>


                <asp:Repeater Visible="false" runat="server" ID="breakThroughProjectsRepeater" OnItemDataBound="ProjectNamesRepeater_ItemDataBound">
                    <ItemTemplate>
                        <div class="row pl-3">
                            <div class="col-12">
                                <div class="px-2 py-1">
                                    <asp:LinkButton ID="IwaLinkButton" runat="server"
                                        CommandName="FilterRecord"
                                        CommandArgument='<%# Eval("iwaID") %>'
                                        OnCommand="IwaLinkButton_Command">
                                        <div runat="server" id="divForActive">
                                            <div runat="server" id="projectNumberLabel" class="small-block">
                                                Project #<%# Eval("iwaID") %>
                                                <div class="pull-right"><span class="badge badge-success d-none">Open</span></div>
                                            </div>
                                            Highly Discounted Assets For your current & future projects
                                            <%--<hr style="margin-top: 0.5rem; margin-bottom: 0rem;" />--%>
                                        </div>
                                    </asp:LinkButton>
                                    <asp:HiddenField ID="IwaIDHidden" runat="server" Value='<%# Eval("iwaID") %>' />
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="filter-text mt-2 mb-1 mx-2 pb-2 d-flex justify-content-between" runat="server" id="titleCurrentProjects"  style="color:#ea571a;">
                    <asp:Literal runat="server" ID="ProjectNameLiteral"></asp:Literal>
                </div>
                <asp:Repeater runat="server" ID="ProjectNamesRepeater" OnItemDataBound="ProjectNamesRepeater_ItemDataBound">
                    <ItemTemplate>
                                <div class="mx-2">
                                    <asp:LinkButton ID="IwaLinkButton" runat="server"
                                        CommandName="FilterRecord"
                                        CommandArgument='<%# Eval("iwaID") %>'
                                        OnCommand="IwaLinkButton_Command">
                                        <div runat="server" id="divForActive">
                                                                                            <div class="px-3">
                                            <div runat="server" id="projectNumberLabel" class="small-block pt-2">
                                                Project #<%# Eval("iwaID") %>
                                                <div class="pull-right"><span class="badge badge-success d-none">Open</span></div>
                                            </div>
                                            <%#  Eval("iwaRefName").ToString()  %>
                                                                                                </div>
                                            <hr style="margin-top: 0.5rem; margin-bottom: 0rem;" />
                                        </div>
                                    </asp:LinkButton>
                                    <asp:HiddenField ID="IwaIDHidden" runat="server" Value='<%# Eval("iwaID") %>' />
                                </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div runat="server" id="closedProjectTitle" class="filter-text mt-2 mb-1 mx-2 pb-2 d-flex justify-content-between"  style="color:#ea571a;">
                    <asp:Literal runat="server" ID="Literal1">Your Orders <div class="pull-right"><i id="downArrow" class="fa fa-arrow-circle-down" aria-hidden="true"></i><i id="upArrow" class="fa fa-arrow-circle-up d-none" aria-hidden="true"></i></div></asp:Literal>
                </div>
                <div id="closedProjectDiv" class="">
                    <asp:Repeater runat="server" ID="ProjectNamesRepeaterClosed">
                        <ItemTemplate>
                                 <div class="mx-2">
                                        <asp:LinkButton ID="IwaLinkButton" runat="server"
                                            CommandName="FilterRecord"
                                            CommandArgument='<%# Eval("iwaID") %>'
                                            OnCommand="IwaLinkButton_Command">
                                            <div runat="server" id="divForActive">
                                                <div class="px-3">
                                                <div class="small-block pt-2">Project #<%# Eval("iwaID") %> </div>
                                                <%#  Eval("iwaRefName").ToString()  %>
                                                                                                </div>
                                                <hr style="margin-top: 0.5rem; margin-bottom: 0rem;" />
                                            </div>
                                        </asp:LinkButton>
                                        <asp:HiddenField ID="IwaIDHidden" runat="server" Value='<%# Eval("iwaID") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                 <div runat="server" id="soldItemsTitle" class="filter-text mt-2 mb-1 mx-2 pb-2 d-flex justify-content-between"  style="color:#ea571a;">
                    <asp:Literal runat="server" ID="Literal2">Your Sold Items<div class="pull-right"><i id="downArrow2" class="fa fa-arrow-circle-down" aria-hidden="true"></i><i id="upArrow2" class="fa fa-arrow-circle-up d-none" aria-hidden="true"></i></div></asp:Literal>
                </div>
                <div id="soldItemsDiv" runat="server" class="">
                    <div id="soldItemsDivClient"  class="">
                    <asp:Repeater runat="server" ID="SoldItemsRepeater">
                        <ItemTemplate>
                            <div class="mx-2">
                                <asp:LinkButton ID="SoldItemLinkButton" runat="server"
                                    CommandName="FilterRecord"
                                    CommandArgument='<%# Eval("itmID") %>'
                                    OnCommand="SoldItemLinkButton_Command">
                                    <div runat="server" id="divForActive">
                                        <div class="px-3">
                                        <div class="small-block pt-2">Asset #<%# Eval("itmID") %> </div>
                                            <%#  Eval("itmName").ToString()  %>
                                        </div>
                                        <hr style="margin-top: 0.5rem; margin-bottom: 0rem;" />
                                    </div>
                                </asp:LinkButton>
                                <asp:HiddenField ID="IwaIDHidden" runat="server" Value='<%# Eval("itmID") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    </div>
                </div>
            </div>
        </nav>
    </asp:Panel>
</asp:Content>
<asp:Content ContentPlaceHolderID="body" runat="server">
    <asp:Panel ID="ItemsPanel" runat="server" Visible="true" class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-5" role="main">
        <div  class="row" style="padding-left: 15px;">
<%--            <div style="padding-top: 15px; text-align:center;" class="col-12">
                <img src="images/banner-userportal.png" style="width:100%;" class="center" />
            </div>--%>
            <%--<div class="col-1 d-sm-none d-md-none d-lg-none d-xl-block"></div>--%>
            <div runat="server" id="featuredItemsDiv" Visible="false" style="" class="col-xl-8 col-lg-12">
            <hr style="padding-bottom:5px;padding-top:5px;" />

                <div>
                    <h1 style="font-weight: 700; font-size: 1.2222222222rem;">Highly Discounted Assets and Emerging Technologies</h1>
                    <div class="my-4 row placeholders">
                        <asp:Repeater ID="featuredAssetsRepeater" runat="server" OnItemDataBound="featuredAssetsRepeater_ItemDataBound">
                            <ItemTemplate>
                                <div class="col-12 mb-2" id="mainCardDiv" runat="server" iwaid='<%# Eval("iwaID") %>' itmid='<%# Eval("itmID") %>' onclick="itmClicked(this)">
                                    <div class="card shadow full-height">
                                        <asp:Panel runat="server" ID="FeaturedPanel" style="background-color:#1b6b2d" class="card-header card-header-success">
                                            <i style="color:white;" class="fa fa-star" aria-hidden="true"></i><span style="color:white;">Featured</span>
                                        </asp:Panel>
                                        <div class="flex-row full-height">
                                            <div class="img col-fixed-143px full-height" style="border-bottom:unset;">
                                                <asp:Image runat="server" ID="itmImgMain" style="font-size: 14px;" alt="no image found"
                                                    onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" class="card-img" />
                                            </div>
                                            <div class="flex-1 no-padding">
                                                <div class="card-body px-3 py-2 row">
                                                    <div class="flex-1">

                                                        <h2 class="card-title my-1 details value" style="font-weight: 400; font-size: 14px; color: #333333;"><a><%# Eval("Name") %></a></h2>
                                                    </div>
                                                    <div class="flex-1">
                                                        <asp:Panel runat="server" Visible="false" ID="PurchasedPanel" iwaid='<%# Eval("iwaID") %>' itmid='<%# Eval("itmID") %>' onclick="itmClicked(this)" class="btn btn-success d-block" Style="padding: .2rem .75rem">Purchased</asp:Panel>
                                                        <asp:Panel runat="server" Visible="false" ID="SoldPanel" class="btn btn-danger disabled d-block" Style="padding: .2rem .75rem">Sold</asp:Panel>
                                                        <asp:Panel runat="server" ID="ButtonPanel" iwaid='<%# Eval("iwaID") %>' itmid='<%# Eval("itmID") %>' onclick="itmClicked(this)" class="btn btn-primary d-block" style="padding:.2rem .75rem"><i class="fa fa-link""></i>View</asp:Panel>
                                                    </div>
                                                </div>

                                                <div class="card-footer px-3 py-2">
                                                    <span class="badge badge-secondary">
                                                        <asp:Label ID="Label1" runat="server"><%# Eval("Type") %></asp:Label>
                                                        #<%# Eval("itmID") %></span>
                                                    <asp:Label runat="server" Visible="false" ID="UsedLabel"
                                                        class="badge badge-warning"><%# Eval("icdName") %></asp:Label>
                                                    <asp:Label runat="server" Visible="false" ID="NewLabel"
                                                        class="badge badge-success"><%# Eval("icdName") %></asp:Label>
                                                   
                                                    <asp:Label runat="server" ID="ODLabel" Visible="false" CssClass="badge badge-primary"><%# Eval("itmIodValue") %></asp:Label>
                                                    <span class="badge badge-primary" runat="server" id="gradeLabelSpan">
                                                        <asp:Label runat="server" ID="gradeLabel"></asp:Label></span>
                                                     <asp:Label runat="server" ID="InterestLevelHot" class="badge badge-danger">Hot</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelWarm" class="badge badge-warm">Warm</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelNeutral" class="badge badge-primary">Neutral</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelCold" class="badge badge-info">Cold</asp:Label>
                                                    <div class="row no-gutters detailRow">
                                                        <asp:Panel CssClass="d-none" runat="server" ID="AskPricePanel" Visible="true" class="col-3 py-2">
                                                            <asp:Label runat="server" ID="QuotePriceAskPriceLabel" class="details key" Style="font-weight: 600;">Qty Wanted</asp:Label>
                                                            <span class="details value quote-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                                <asp:Label runat="server" ID="qtyWanted"></asp:Label></span>
                                                        </asp:Panel>
                                                        <div class="col-4 py-2 ">
                                                            <asp:Label ID="OriginalPriceIndexPriceLabel" runat="server" class="details key original-price" Style="font-weight: 600;">Qty Available</asp:Label>
                                                            <div class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                                <asp:Label runat="server" ID="qtyAvailable"></asp:Label></div>
                                                        </div>

                                                        <div class="col-4 py-2">
                                                            <div class="details key" style="font-weight: 600;">Quote Price</div>
                                                            <asp:Label runat="server" class="details value" ID="IwbSellPriceLabel" Style="font-weight: 400; font-size: 14px; color: #333333;"></asp:Label>

                                                        </div>
                                                        <div class="col-4 py-2">
                                                            <div class="details key" style="font-weight: 600;">Total Price</div>
                                                            <div class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><span class="money"><%# (decimal)Eval("TotalPrice") != 0 ? Eval("TotalPrice", "{0:C0}") : "N/A" %></span></div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
            
            <%--<div style="padding-top: 40px" class="col-6">
                    <img src="https://media.istockphoto.com/vectors/oil-derricks-and-financial-data-vector-id585804658" />

                </div>--%>
            <div class="container-fluid d-none">
                <div class="row mb-2 pb-2 mb-lg-4 pb-lg-3" id="title-row">
                    <div class="col-12 col-md-6 align-self-end text-center text-lg-left">
                        <h1>
                            <asp:Literal runat="server" ID="myProjectsTitle"></asp:Literal>
                        </h1>
                    </div>
                </div>
            </div>
            <div class="row placeholders d-none">
                <asp:Repeater runat="server" ID="ProjectCardsRepeater">
                    <ItemTemplate>
                        <div class="col-12 col-md-6 col-lg-4 col-xl-3 my-2" cmdargument='<%# Eval("ProjectId") %>' onclick="cardClicked(this)">
                            <div class="card shadow">
                                <div class="card-body px-3 py-2">
                                    <h2 class="card-title my-1"><%# Eval("ProjectName") %></h2>
                                </div>
                                <div class="card-footer px-3 py-2">
                                    <span class="badge badge-secondary">#<%# Eval("ProjectId") %></span>
                                    <asp:Label Visible="false" runat="server" ID="ContractReviewStatusLabel" class="badge badge-danger">Contract Review</asp:Label>
                                    <asp:Label Visible="false" runat="server" ID="CompleteStatusLabel" class="badge badge-success">Closed</asp:Label>
                                    <%--<asp:Label Visible="false" runat="server" ID="CurrentlySourcingStatusLabel" class="badge badge-warning d-none">Open</asp:Label>--%>
                                    <span class="badge badge-primary"><%# Eval("IodName") %></span>
                                    <span class="badge badge-primary"><%# Eval("Grade") %></span>
                                    <div class="row no-gutters detailRow">
                                        <div class="col-6 text-left py-2">
                                            <span class="details key">Best Option Items</span>
                                            <span class="details value"><%# Eval("BestOptionItemsCount") %></span>
                                        </div>
                                        <div class="col-6 text-left py-2">
                                            <span class="details key">Quoted Items</span>
                                            <span class="details value"><%# Eval("QuotedItems") %></span>
                                        </div>
                                    </div>
                                    <div class="row no-gutters detailRow">
                                        <div class="col-6 text-left py-2">
                                            <asp:Label runat="server" ID="DecisionDateLabel" class="details key">Due Date</asp:Label>
                                            <span class="details value"><%# Eval("DueDate") %></span>
                                        </div>
                                        <div class="col-6 text-left py-2">
                                            <asp:Label runat="server" ID="DeliveryLabel" class="details key">Delivery Location</asp:Label>
                                            <span class="details value"><%# Eval("DeliveryLocation") %></span>

                                        </div>
                                    </div>
                                </div>
                                <div class="card-subfooter">
                                    <div runat="server" class="btn btn-outline-primary">
                                        <i class="fa fa-link" aria-hidden="true"></i>
                                        <asp:Literal runat="server" ID="viewProjectLiteral"></asp:Literal>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="ProjectProfilePanel" Visible="false" class="col-12 ml-auto col-lg-9 col-xl-10 pb-5" role="main">

               <div style="text-align:left;padding: 3px 15px;" class="col-12 col-xl-10">
                   <div class="alert alert-info" role="alert" style="font-weight:500;border: 1.4px solid;border-color: #78aedc;background: rgb(232, 242, 251);">
               <asp:Literal runat="server" ID="welcomeText1"></asp:Literal>
</div>

            </div>
        <uc1:ProjectProfile runat="server" ID="ProjectProfile" />
    </asp:Panel>
    <asp:Panel runat="server" ID="ItemProfilePanel" Visible="false" class="col-12 ml-auto col-lg-9 col-xl-10 pb-5" role="main">
        <uc1:ItemProfileControl runat="server" OnBackButton_Clicked="ItemProfileControl_BackButton_Clicked" ID="ItemProfileControl" />
    </asp:Panel>
    <asp:Panel ID="myProjectEmptyPanel" runat="server" class="col-12 ml-auto col-lg-12 col-xl-12 pb-5" style="text-align:center;"  Visible="false" role="main">
        <div class="alert alert-info" role="alert" style="font-weight:500;border: 1.4px solid;border-color: #78aedc;background: rgb(232, 242, 251);">
                <asp:Literal runat="server" ID="welcomeText2"></asp:Literal>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="myLoadoutPanel"  Visible="false" class="col-12 ml-auto col-lg-9 col-xl-10 pb-5" role="main">
        <uc1:LoadOutProfileControl id="LoadOutProfileControl" runat="server" ></uc1:LoadOutProfileControl>
    </asp:Panel>
    <footer class="py-3 text-center" style="display: none;">
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
            var cmdargument = card.getAttribute("cmdargument");

            if (typeof woopra != "undefined" && woopra) {

                woopra.track('userportal', { page: 'My Projects', description: 'View Project Card' + cmdargument });
            }

            window.location = window.location.origin + "/Buyer/MyProjectsNew.aspx?PID=" + cmdargument;
        }
        function itmClicked(itm) {
            var itmid = itm.getAttribute("itmid");
            var iwaid = itm.getAttribute("iwaid");
            var iwbid = itm.getAttribute("iwbid");
            if (typeof woopra != "undefined" && woopra) {
                woopra.track('userportal', { page: 'Project Profile ', description: 'project item clicked ' });
            }
            window.location = "/Buyer/MyProjectsNew.aspx?PID=" + iwaid + "&IID=" + itmid + "&iwbID=" + iwbid;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
