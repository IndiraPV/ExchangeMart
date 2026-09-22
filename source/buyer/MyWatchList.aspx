<%@ Page Title="" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="MyWatchList.aspx.cs" Inherits="Exchangebase.Com.Buyer.MyWatchList" %>

<%@ Register TagPrefix="itemprofile" Src="~/Buyer/ItemProfileControl.ascx" TagName="itp2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="ContentPlaceHolder1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="iwaIDFiltervalue" Value="0" runat="server" />
    <asp:HiddenField ID="cmdArgumentFilterValue" Value="0" runat="server" />
    <asp:HiddenField ID="itmIDsFilterValue" Value="0" runat="server" />
    <asp:HiddenField ID="grdIDFilterValue" Value="0" runat="server" />
    <asp:Panel ID="watchListMainPanel" Style="width: 100%;" Visible="false" runat="server">

        <nav class="col-12 col-lg-3 col-xl-2 navbar-expand-lg sidebar my-lg-1 my-lg-auto bg-white">

            <div class="row py-2 d-lg-none" id="mobileFilter">
                <%-- <div class="col-9">
                    <form class="form-inline-block">
                        <input class="form-control py-1 px-2" type="text" placeholder="Search Within..">
                    </form>
                </div>--%>

                <div class="col-12 mx-auto text-center">
                    <button class="navbar-toggler white" type="button" data-toggle="collapse" data-target="#filterNav" aria-controls="filterNav" aria-expanded="false" aria-label="Toggle navigation">
                        <i class="fa fa-filter mr-2" aria-hidden="true"></i> Item Grades
                    </button>
                </div>
            </div>
            <div class="collapse d-lg-inline my-2 my-lg-auto" id="filterNav">
                <span class="filterHead p-2 d-none d-lg-flex">Watchlist<i class="fa fa-filter ml-auto align-self-center align-middle"></i></span>
                <div class="filter-text mt-2 mb-1 mx-2 pb-2 d-flex justify-content-between">
                    Item Grade
                </div>
                <div class="collapse show mb-3" id="sideNav">
                    <!-- SUB LOOP GRADES FOR CATEGORIES -->

                    <asp:Repeater Visible="true" runat="server" ID="grdRepeater">
                        <ItemTemplate>
                            <div class="row">
                                <div class="col-12">
                                    <div class="px-2 py-1">
                                        <asp:LinkButton CommandName="FilterRecord" runat="server" ID="grdLinkButton" Text='<%# Eval("Grade") %>'
                                            OnCommand="GrdLinkButton_Command" CommandArgument='<%# Eval("cmdArgument") %>' class="d-flex"></asp:LinkButton>
                                        <asp:HiddenField ID="cmdArgument" Value='<%# Eval("cmdArgument") %>' runat="server" />
                                        <asp:HiddenField ID="grdIDHidden" Value='<%# Eval("grdID") %>' runat="server" />
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                </div>
                <!-- END LOOP -->
            </div>
        </nav>
        <div class="col-12 ml-auto col-lg-9 col-xl-10 pt-0 pt-lg-3 pb-1 pb-lg-5 mt-2 mt-lg-auto" role="main">
            <div class="container-fluid" id="listView">


                <asp:Panel ID="ItemCardsPanel" ContentPlaceHolderID="list" runat="server">
                    <div class="row mb-2 pb-2 mb-lg-4 pb-lg-3" id="title-row">
                        <div class="col-12 col-md-6 align-self-end text-center text-lg-left">
                            <h1>
                                <asp:Label runat="server" ID="GradeTitleLabel"></asp:Label></h1>
                        </div>
                    </div>
                    <div class="row placeholders">
                        <asp:Repeater ID="ItmRepeater" runat="server">
                            <ItemTemplate>

                                <div class="col-12 col-md-6 col-lg-4 col-xl-3 mb-3 mb-lg-5" onclick="cardClicked(this)" id="mainCardDiv" runat="server">
                                    <div class="card shadow">
                                        <asp:Panel runat="server" ID="FeaturedPanel" class="card-header card-header-success">
                                            <i class="fa fa-star" aria-hidden="true"></i>Featured
                                        </asp:Panel>

                                        <div class="img text-center">
                                            <asp:Image ID="itmImgMain" Style="font-size: 14px;" onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" runat="server" class="card-img" />
                                        </div>
                                        <div class="card-body px-3 py-2">
                                            <h2 class="card-title my-1">
                                                <asp:Literal ID="itmNameLiteral" runat="server"></asp:Literal></h2>
                                        </div>
                                        <div class="card-footer px-3 py-2">
                                            <asp:Panel ID="itmIDPanel" runat="server"></asp:Panel>
                                            <asp:Panel ID="itmCondPanel" runat="server"></asp:Panel>
                                            <asp:Label runat="server" ID="ODLabel" Visible="false" CssClass="badge badge-primary"></asp:Label>
                                            <asp:Panel ID="gradeNamePanel" runat="server"></asp:Panel>
                                            <div class="row no-gutters detailRow">
                                                <div class="col-6 py-2">
                                                    <span class="details key quote-price" runat="server" id="askPriceSpan">Quoted Price</span>
                                                    <span class="details value quote-price">
                                                        <asp:Literal ID="itmSellPriceLiteral" runat="server" /></span>
                                                </div>
                                                <div class="col-6 py-2">
                                                    <span class="details key original-price">
                                                        <asp:Literal runat="server" ID="IndexOriginalPriceLiteral"></asp:Literal></span>
                                                    <span class="details value original-price">
                                                        <asp:Literal ID="itmIndexPriceLiteral" runat="server" /></span>
                                                </div>
                                            </div>
                                            <div class="row no-gutters detailRow">
                                                <div class="col-6 py-2">
                                                    <span class="details key">Quantity Available</span>
                                                    <span class="details value">
                                                        <asp:Literal runat="server" ID="itmQtyLiteral"></asp:Literal></span>
                                                </div>
                                                <div class="col-6 py-2">
                                                    <span class="details key">Location</span>
                                                    <span class="details value">
                                                        <asp:Literal runat="server" ID="facLocationNameLiteral"></asp:Literal></span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-subfooter">
                                            <asp:LinkButton OnCommand="watchListRemoveButton_Command" ID="watchListRemoveButton" runat="server" class="btn btn-outline-danger"><i class="fa fa-minus-circle" aria-hidden="true"></i> Watchlist</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </asp:Panel>
                <asp:Panel Visible="false" runat="server" ID="ItemProfilePanel">
                    <itemprofile:itp2 OnBackButton_Clicked="ItemProfileControl_BackButton_Clicked" ID="ItemProfileControl" runat="server"></itemprofile:itp2>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="watchListEmptyPanel" runat="server" class="col-12 ml-auto pt-0 pt-lg-3 pb-5" Visible="false" role="main">
        <div class="fade-in-no-movement" style="text-align: center; padding-top: 35px; padding-bottom: 150px; padding-left: 15px; padding-right: 15px; background:#fff; border-radius: 10px; border: 1px solid #d7dce1;">
            <h2>We didn't find any items in your watchlist.</h2>
            <img style="margin: 20px;" height="200" src="../Images/Buyer/watchlistempty.jpg" />
            <p>
                To add an item to your watchlist, you can click the <span>
                    <asp:LinkButton  runat="server" CssClass="btn btn-outline-success mx-2"> <span class="fa fa-plus-circle mr-2"></span> Watchlist</asp:LinkButton>
                </span>button on any item profile .
            </p>
            <p>Once an item is in your watchlist, you will see it here.</p>
            <div style="text-align: center; width: 50%; margin-left: 25%; margin-right: 25%; margin-top: 25px; margin-bottom: 25px; border-top: 1px solid lightgray"></div>
            <h4>To find the item you need, you can explore our <a href="/Buyer/Inventory.aspx">current inventory</a></h4>
        </div>
    </asp:Panel>
    <asp:Panel ID="MyWatchlistEmptyCantViewItems" runat="server" class="col-12 ml-auto pt-0 pt-lg-3 pb-5" Visible="false" role="main" >
        <div class="fade-in-no-movement"style="text-align: center; padding-top: 35px; padding-bottom: 150px; padding-left: 15px; padding-right: 15px; background:#fff; border-radius: 10px; border: 1px solid #d7dce1;">
            <h2>Tell us a little about yourself and your business.</h2>
            <img style="margin: 20px;" height="200" src="../Images/Buyer/watchlistempty.jpg" />
            <p>
                We will then suggest featured assets that you may be interested in purchasing. 
            </p>
            <div style="text-align: center; width: 50%; margin-left: 25%; margin-right: 25%; margin-top: 25px; margin-bottom: 25px; border-top: 1px solid lightgray"></div>
            <p>
                Please contact
                <asp:Literal ID="buyRepLiteral" runat="server"></asp:Literal>
                at <b>(440) 331-3600
                    <asp:Literal ID="buyRepExtLiteral" runat="server" /></b> to let them know what you are looking for.
            </p>
        </div>
    </asp:Panel>
    <script>
        function cardClicked(card) {
            //get the cmdArgument
            var cmdargument = card.getAttribute("cmdargument");
            __doPostBack('card', cmdargument);
        }</script>
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
</asp:Content>
