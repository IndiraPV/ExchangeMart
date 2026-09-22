<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectProfile.ascx.cs" Inherits="Exchangebase.Com.Buyer.ProjectProfile" %>

    <style>
        @media (min-width: 1200px) {
            .col-xl-5e {
                flex: 0 0 20%;
                max-width: 20%;
            }
        }
        .col-fixed-143px {
            flex: 0 0 158px;
            max-width: 158px;
    padding-left: 15px;
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
            flex:1 1;
            padding-left:15px;
            padding-right:15px;
        }
        .mini-div {
            /*max-height:100px;
            overflow:hidden;*/
        }
                .flex-row {
            display: flex;
        }
        .card.shadow:hover {
    -webkit-box-shadow: 0 3px 10px 10px rgba(0,0,0,.05);
    box-shadow: 0 3px 10px 10px rgba(0,0,0,.05);
    -webkit-transition: all .4s ease;
    -o-transition: all .4s ease;
    transition: all .4s ease;
}
                .full-height {
        height:100%;
        }
                       .full-width {
        width:100%;
        }
        .card-img {
    width: 100%;
    height: 100%;
    border-radius: calc(.25rem - 1px);
    object-fit:contain;
}
                .no-padding {
        padding-left: 0px !important;
       padding-right: 0px !important;
        }
        .align-right {
            text-align:right;
        }
        .flex-200px {
            flex:0 0 200px;
        }
    </style>
<script>
    $(document).ready(function () {
        if ($('.show-more-box').height() > 100) {
            $('.show-more').removeClass('d-none');
            $('.show-btn-container').removeClass('d-none');
        }

        $('.show-more').on('click', function (e) {
            $('.show-more-box-container').removeClass('mini-div');
            $('.show-more').addClass('d-none');
            $('.show-less').removeClass('d-none');
        });

        $('.show-less').on('click', function (e) {
            $('.show-more-box-container').addClass('mini-div');
            $('.show-less').addClass('d-none');
            $('.show-more').removeClass('d-none');
        });
    });
</script>
<div class="container-fluid" id="profileView" style="background-color:unset">
    <div class="row">

        <div class="col-12 col-xl-9">
            <div class="row align-items-center align-items-lg-start d-none" id="actionRow">
                <div class="col-6 text-left">
                    <a href="/Buyer/MyProjectsNew.aspx" class="d-inline-block mx-2 my-2 btn btn-outline-secondary"><i class="fa fa-chevron-left align-middle mr-1" aria-hidden="true"></i><span class="d-none d-md-inline-block">Back</span></a>
                </div>
                <div class="col-6 text-right">
                    <div>
                        <asp:Literal ID="buyerRepLiteral" runat="server" />

                        <a style="display: none !important;" onclick="printDiv()" class="d-inline-block mx-2 my-2 btn btn-outline-primary" tabindex><i class="fa fa-print mr-md-2" aria-hidden="true"></i><span class="d-none d-md-inline-block">Print</span></a>
                    </div>
                </div>
            </div>

<%--            <hr style="margin-top:4px">--%>

 <div class="card py-2">
                <div class="col-12 row">
                    <div class="col-12 col-md-12">                    
                        <span class="details key mb-1" style="font-weight: 600">
                        <asp:Literal runat="server" ID="projectNameLiteral"></asp:Literal></span>
                    <h1 class="mb-4"><%= ViewModel.iwaRefName %> </h1></div>
                   
                </div>

                <div class="col-12 col-xl-12">
                    <asp:Panel runat="server" class="row" ID="equipmentOverview">
                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">
                                <asp:Literal runat="server" ID="projectNumLiteral"></asp:Literal></span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.iwaID %></span>
                        </div>
                       <%-- <div class="col-3 my-1 my-lg-0">
                            <span class="details key" style="font-weight: 600;">Grade</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.Grade %></span>
                        </div>--%>
                        <%--<div class="col-3 my-1 my-lg-0">
                            <span class="details key" style="font-weight: 600;">Due Date</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DueDate %></span>
                        </div>--%>
                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Quantity Needed</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= String.Format("{0:n0}", ViewModel.iwaQty) %></span>
                        </div>
                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Delivery Location</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DeliveryLocation %></span>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" class="row" ID="linepipeOverview">

                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Project #</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.ProjectId %></span>
                        </div>
                      <%--  <div class="col-4 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">OD</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.iodName %></span>
                        </div>
                        <div class="col-4 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Grade</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.Grade %></span>
                        </div>--%>
                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Quantity Needed</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                <% if (ViewModel.iwaTotalFeet > 0)
                                   { %>
                                <%= String.Format("{0:n0}", ViewModel.iwaTotalFeet)  %> ft</span>
                            <% }
                                   else
                                   { %>
                            <%= "N/A" %>
                            <% } %>
                        </div>
                       <%-- <div class="col-4 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Due Date</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DueDate %></span>
                        </div>--%>
                        <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Delivery Location</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DeliveryLocation %></span>
                        </div>
                    </asp:Panel>
                    <asp:Panel runat="server" class="row" ID="DatesPanel">
                          <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Decision Date</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DecisionDateDisplay %></span>
                        </div>
                          <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Delivery Date</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.DeliveryDateDisplay %></span>
                        </div>
                          <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Sourcing Date Start</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.SourceDateStartDisplay %></span>
                        </div>
                          <div class="col-3 col-md-2 my-1">
                            <span class="details key" style="font-weight: 600;">Sourcing Date End</span>
                            <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.SourceDateEndDisplay %></span>
                        </div>
                    </asp:Panel>
                </div>
                <%--<div style="border-left: 1px solid #d7dce1; margin: 4px 0px; flex: 0 0 60px;"></div>--%>
                <div class="col-12 mt-2" runat="server" id="additionalInformationDiv">

                    <div class="row">
                        <div class="col-12">
                            <span class="details key" style="font-weight: 600">Project Details</span>
                            <div class="show-more-box-container mini-div">
                            <span class="details value show-more-box fade-in "  style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.BasicInformation %></span>
                                <br />
                            <span class="details key" style="font-weight: 600">Additional Information</span>
                            <div class="show-more-box-container mini-div">
                            <span class="details value show-more-box fade-in "  style="font-weight: 400; font-size: 14px; color: #333333;"><%= ViewModel.AdditionalInformation%></span>
                            </div>
                                <div class="show-btn-container d-none" style="height:0px;">
                                <button type="button" class="btn btn-sm btn-outline-secondary show-more fade-in-no-movement d-none " style="font-size:12px;line-height: 1;">Show More</button>
                                <button type="button" class="btn btn-sm  btn-outline-secondary show-less fade-in-no-movement d-none" style="font-size:12px;line-height: 1;">Show Less</button>
                                </div>
                    </div>

                    <div class="row d-none">
                        <div class="col-12 my-5">
                            <span class="contact text p-3 d-block"><i class="fa fa-question-circle mr-1" aria-hidden="true"></i></span>
                                Questions? Contact <span style="color: #1280f9; font-weight: 600" class="">
                                    <b><asp:Literal ID="buyerRepExtLiteral" runat="server" /></b>
                                </span>
                        </div>
                    </div>

                </div>
        </div>
    </div>
     
     </div>
    <hr runat="server" id="lineBreakHR" style="margin: 30px 0px;" />
    <div class="row" id="viewDetails">
        <div class="col-12" id="tabs">
            <h1 runat="server" id="BestOptionsLabel" style="margin-bottom:0px;margin-left:5px">Options for this Project</h1>
            <%-- <ul class="nav nav-tabs flex-nowrap" role="tablist">
                <li class="nav-item">
                    <a class="nav-link mx-1 active" data-toggle="tab" href="#best" role="tab">
                        <asp:Literal runat="server" ID="bestOptionsLabel"></asp:Literal><span class="badge badge-pill badge-primary ml-1"><%= ViewModel.BestOptionsCount %></span></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link mx-1" data-toggle="tab" href="#quoted" role="tab">Quoted Items <span class="badge badge-pill badge-primary ml-1"><%= ViewModel.QuotedItemsCount%></span></a>
                </li>

            </ul>--%>

            <div class="card my-2 p-2" runat="server" visible="false" id="noOptionsText"><span>There are no items to show here.</span></div>

            <div class="tab-content">
                <div class="tab-pane active" id="best" role="tabpanel">
                    <asp:Panel runat="server" ID="BestOptionTablePanel" class="row">
                        <div class="my-4 placeholders full-width">
                            <asp:Repeater runat="server" ID="BestOptionsRepeater" OnItemDataBound="QuotedItemsRepeater_ItemDataBound">
                                <ItemTemplate>
                                    <div class="col-12 mb-2" id="mainCardDiv" runat="server" iwaid='<%# Eval("iwaID") %>' itmid='<%# Eval("itmID") %>' iwbid='<%# Eval("iwbID") %>' onclick="itmClicked(this)">
                                        <div class="card shadow full-height">
                                            <asp:Panel runat="server" ID="FeaturedPanel" class="card-header card-header-success">
                                                <i class="fa fa-star" aria-hidden="true" style="color: #ea591c;margin-right:8px;"></i>Featured
                                            </asp:Panel>
                                            <div class="flex-row  full-height">
                                            <div class="img col-fixed-143px full-height">
                                                <asp:Image runat="server" ID="itmImgMain" Style="font-size: 14px;" alt="no image found"
                                                    onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" class="card-img" />
                                            </div>
                                              <div class="flex-1 no-padding no-padding">
                                            <div class="card-body px-3 py-2 row">
                                                <div class="flex-1">
                                                <h2 class="card-title my-1 details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                    <a> 
                                                    <span class="badge badge-secondary mb-1" style="margin-right: 8px;font-size: 90%"><asp:Label ID="Label1" runat="server">
                                                       Quote</asp:Label> #<%# Eval("iwbID") %></span>
                                                        <br class="d-xl-none d-lg-none d-md-none"/>
                                                        <%# Eval("Name") %>
                                                    </a>

                                                </h2>

                                                </div>
                                                <div class="flex-200px align-right pr-2">
                                                <asp:Panel runat="server" Visible="false" ID="PurchasedPanel" iwaid='<%# Eval("iwaID") %>' itmid='<%# Eval("itmID") %>' iwbid='<%# Eval("iwbID") %>' onclick="itmClicked(this)" class="btn btn-success" style="padding:.2rem .75rem"><i class="fas fa-check-double" style="font-size: 14px;margin-right: 8px;"></i>Purchased</asp:Panel>
                                                <asp:Panel runat="server" Visible="false" ID="SoldPanel" class="btn btn-danger disabled" style="padding:.2rem .75rem"><i class="fab fa-creative-commons-nc" style="font-size: 14px;margin-right: 8px;"></i>Sold</asp:Panel>
                                                    <asp:Panel runat="server" ID="ButtonPanel" iwbid='<%# Eval("iwbID") %>' iwaid="<%# ViewModel.iwaID %>" itmid='<%# Eval("itmID") %>' onclick="itmClicked(this)" class="btn btn-primary " style="padding:.2rem .75rem"><i class="fa fa-link" style="font-size: 14px;margin-right: 8px;"></i>View</asp:Panel>
                                            </div>
                                                </div>

                                            <div class="card-footer px-3 py-2" style="background-color:unset;">
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
                                                    <asp:Panel runat="server" ID="AskPricePanel" Visible="true" class="col-6 col-md-3 py-2">
                                                        <asp:Label runat="server" ID="QuotePriceAskPriceLabel" class="details key" style="font-weight: 600;s">Qty Wanted</asp:Label>
                                                        <span class="details value quote-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                            <asp:Label runat="server" ID="qtyWanted" ></asp:Label></span>
                                                    </asp:Panel>
                                                    <div class="col-6 col-md-3 py-2">
                                                        <asp:Label ID="OriginalPriceIndexPriceLabel" runat="server" class="details key original-price" style="font-weight: 600;">Qty Available</asp:Label>
                                                        <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                            <asp:Label runat="server" ID="qtyAvailable"></asp:Label></span>
                                                    </div>

                                                    <div class="col-6 col-md-3 py-2">
                                                        <span class="details key" style="font-weight: 600;" runat="server" id="QuotePriceAskingPriceLabel">Quote Price</span>
                                                        <asp:Label runat="server" class="details value" ID="IwbSellPriceLabel" style="font-weight: 400; font-size: 14px; color: #333333;"></asp:Label>

                                                    </div>
                                                    <div class="col-6 col-md-3 py-2" runat="server" id="TotalPriceDiv">
                                                        <span class="details key" style="font-weight: 600;">Total Price</span>
                                                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><span class="money"><%# (decimal)Eval("ProductCostTotal") != 0 ? Eval("ProductCostTotal", "{0:C2}") : "N/A" %></span></span>
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
                    </asp:Panel>

                </div>
                <div class="tab-pane" id="quoted" role="tabpanel">

                    <asp:Panel runat="server" ID="QuotedTablePanel" class="row">
                        <div class="col-12 my-4">
                            <table class="table table-hover table-responsive table-bordered">
                                <thead class="thead-default">
                                    <tr>
                                        <th>Type</th>
                                        <th>ID</th>
                                        <th>Name</th>
                                        <th runat="server" id="ODHeader1" style="display: none;">OD</th>
                                        <th>Grade</th>
                                        <th runat="server" id="QtyWantedHeader1">Qty Wanted</th>
                                        <th runat="server" id="QtyAvailHeader1">Qty Available</th>
                                        <th runat="server" id="QtyQuotePriceHeader1">Quoted Price</th>
                                        <th>Total Quote</th>
                                        <th>Your Interest</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater runat="server" ID="QuotedItemsRepeater" OnItemDataBound="QuotedItemsRepeater_ItemDataBound">
                                        <ItemTemplate>
                                            <tr runat="server" id="TableRow" iwaid="<%# ViewModel.iwaID %>" itmid='<%# Eval("itmID") %>' iwbid='<%# Eval("iwbID") %>' onclick="itmClicked(this)">
                                                <td><%# Eval("Type") %></td>
                                                <td>#<%# Eval("itmID") %></td>
                                                <td><%# Eval("Name") %></td>
                                                <td runat="server" id="ODTD" style="display: none;"><%# Eval("itmIodValue") %></td>
                                                <td>
                                                    <asp:Label runat="server" ID="gradeLabel"></asp:Label></td>
                                                <td>
                                                    <asp:Label runat="server" ID="qtyWanted"></asp:Label></td>
                                                <td>
                                                    <asp:Label runat="server" ID="qtyAvailable"></asp:Label></td>
                                                <td>
                                                    <asp:Label runat="server" ID="IwbSellPriceLabel"></asp:Label></td>
                                                <td>
                                                    <%# (decimal)Eval("TotalPrice") != 0 ? Eval("TotalPrice", "{0:C0}") : "N/A" %>
                                                </td>
                                                <td>
                                                    <asp:Label runat="server" ID="InterestLevelHot" class="badge badge-danger">Hot</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelWarm" class="badge badge-warm">Warm</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelNeutral" class="badge badge-primary">Neutral</asp:Label>
                                                    <asp:Label runat="server" ID="InterestLevelCold" class="badge badge-info">Cold</asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Panel runat="server" ID="ButtonPanel" iwaid="<%# ViewModel.iwaID %>" itmid='<%# Eval("itmID") %>' iwbid='<%# Eval("iwbID") %>' onclick="itmClicked(this)" class="btn btn-primary d-block"><i class="fa fa-link"></i>View</asp:Panel>
                                                    <asp:Panel runat="server" Visible="false" ID="PurchasedPanel" iwaid="<%# ViewModel.iwaID %>" itmid='<%# Eval("itmID") %>' iwbid='<%# Eval("iwbID") %>' onclick="itmClicked(this)" class="btn btn-success d-block">Purchased</asp:Panel>
                                                    <asp:Panel runat="server" Visible="false" ID="SoldPanel" class="btn btn-danger disabled d-block">Sold</asp:Panel>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr runat="server" id="QuotedEmptyPanelTR">
                                        <td colspan="11">
                                            <asp:Panel CssClass="row" Style="margin-left: 0px;" Visible="false" runat="server" ID="QuotedEmptyPanel">
                                                There are no quoted items for this project.
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>


                    </asp:Panel>
                </div>
            </div>
        </div>
       
    </div>
</div>
            </div></div>


<script type="text/javascript">
    function itmClicked(itm) {
        var itmid = itm.getAttribute("itmid");
        var iwaid = itm.getAttribute("iwaid");
        if (typeof woopra != "undefined" && woopra) {
            woopra.track('userportal', { page: 'Project Profile ', description: 'project item clicked ' });
        }
        window.location = "/Buyer/MyProjectsNew.aspx?PID=" + iwaid + "&IID=" + itmid;
    }
    var sum = 0;

    $("td.cost span.money").each(function () {
        var $this = $(this).text().replace(/[^0-9.-]+/g, '');
        if ($(this).text() != "N/A") {
            sum += parseFloat($this);
        }
        //sum += parseInt($(this).text());
    });

    $("th#totalCost").html("$" + sum.toFixed(2).replace(/\d(?=(?:\d{3})+\.)/g, '$&,').replace('.00', ' '));



</script>

