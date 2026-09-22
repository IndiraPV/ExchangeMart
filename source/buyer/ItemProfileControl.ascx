<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemProfileControl.ascx.cs" Inherits="Exchangebase.Com.Buyer.ItemProfileControl" %>
<asp:HiddenField ID="ItmIDHiddenField" runat="server" />
<style>
    .table th, .table td {
        padding: 0.25rem !important;
        padding-left: 1rem !important;
    }

    .table td {
        font-weight: 400;
        font-size: 14px;
        color: #333333;
    }

    .table th {
        font-weight: 600;
        font-size: .8888888889rem;
    }

    .fake-card {
        background-color: #fff;
        background-clip: border-box;
        border: 1px solid rgba(0,0,0,.125);
        border-radius: .25rem;
    }

    .full-width {
        width: 100%;
    }

    .no-padding {
        padding-left: 0px !important;
        padding-right: 0px !important;
    }

    .align-right {
        text-align: right;
    }

    .flex-200px {
        flex: 0 0 200px;
    }

    .popover {
        max-width: unset;
        min-width: 500px;
    }
</style>
<script>
    $(document).ready(function () {
        $('#loadoutTable').DataTable();
    });
</script>
<script src="resources/jquery.dataTables.min.js"></script>
<div class="container-fluid" id="profileView" style="background-color: unset">
    <asp:Panel runat="server" ID="ItemView">

        <div class="row align-items-center align-items-lg-start mb-4" id="actionRow">
            <div class="col-6 text-left">
                <asp:LinkButton runat="server" OnCommand="Back_Button_Clicked" class="d-inline-block mx-2 mt-1 btn btn-outline-secondary"><i class="fa fa-chevron-left align-middle mr-1" aria-hidden="true"></i> Back</asp:LinkButton>
            </div>
            <div class="col-6 text-right align-self-center">

                <div>
                    <asp:Literal ID="buyerRepLiteral" runat="server" />

                    <%--<a style="display: none !important;" onclick="printDiv()" class="d-inline-block mx-2 my-2 btn btn-outline-primary" tabindex><i class="fa fa-print mr-md-2" aria-hidden="true"></i><span class="d-none d-md-inline-block">Print</span></a>--%>
                </div>

            </div>
        </div>

        <asp:Panel ID="QuotesDetailsPanel" runat="server">
            <div class="card p-3">
                <span class="details key mb-1" style="font-weight: 600">Service Name</span>
                <h1 class="mb-4" style="color: rgb(51, 51, 51);"><%= targetItem.itmName %></h1>
                <div class="row" id="Div2">
                    <div class="col-6 col-lg-3 my-2 my-lg-0">
                        <span class="details key price asking" style="font-weight: 600;">Price</span>
                        <span class="details value price asking" style="font-weight: 400; font-size: 14px;">
                            <asp:Literal runat="server" ID="ServicePriceLiteral1"></asp:Literal></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0">
                        <span class="details key" style="font-weight: 600;">Quote #</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= targetItem.ItmID %></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0">
                        <span class="details key" style="font-weight: 600;">Category</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= targetItem.AssetType %></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0" runat="server" id="GradeDiv">
                        <span class="details key" style="font-weight: 600;">Grade</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;"><%= targetItem.Grade.TagValue %></span>
                    </div>
                </div>
                <div class="row" id="Div3">
                    <div class="col-6 col-lg-3 my-2 my-lg-0" runat="server" id="Div1">
                        <span class="details key" style="font-weight: 600;">Qty Wanted</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                            <asp:Literal ID="QtyWantedLiteral" runat="server"></asp:Literal></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0" runat="server" id="serviceDate">
                        <span class="details key" style="font-weight: 600;">Service Date</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                            <asp:Literal ID="DateLiteral" runat="server"></asp:Literal></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0">
                        <span class="details key" style="font-weight: 600;">Service Location</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                            <asp:Literal ID="itmLocationLiteral2" runat="server"></asp:Literal></span>
                    </div>
                    <div class="col-6 col-lg-3 my-2 my-lg-0">
                        <span class="details key" style="font-weight: 600;">Service Name</span>
                        <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                            <asp:Literal ID="ServiceNameLiteral" runat="server"></asp:Literal></span>
                    </div>
                </div>

                <hr class="my-2 my-lg-4">

                <div class="row">
                    <div class="col-12 col-lg-8 my-2">
                        <h3 class="details key" style="font-weight: 600; font-size: .8888888889rem;">Additional Information</h3>
                        <p style="font-weight: 400; font-size: 14px; color: #333333;">
                            <%= targetItem.ItmDescription %>
                        </p>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel runat="server" ID="ItemImageAndDescriptionPanel1" class="row fade-in-no-movement">

            <div class="full-width fake-card" style="padding: 15px; margin: 0px 15px;">
                <div class="flex-row">
                    <div class="img" runat="server">

                        <div class="img-holder">
                            <a runat="server" id="mainImgLink" data-lightbox="productGallery">
                                <asp:Image CssClass=" fade-in-no-movement" runat="server" onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" ID="mainImg" Style="height: 248px; width: auto;" />
                                <i class="fa fa-search-plus img-zoom p-2" aria-hidden="true"></i>
                            </a>
                        </div>

                        <!-- IF ITEM IMGS > 1 -->
                        <div class="row mt-2" id="imagesGreaterThan1" style="max-width: 338px" runat="server">
                            <div class="col-1">
                            </div>
                            <div class="col-10">
                                <div class="slickOuter">
                                    <div class="slickInner">

                                        <!-- LOOP IMGS -->
                                        <asp:Repeater runat="server" ID="IimRepeater2">
                                            <ItemTemplate>
                                                <div class="slick-item">
                                                    <asp:HyperLink ID="hypImgDownload" runat="server" data-lightbox="productGallery">
                                                        <asp:Image ID="itmImageLogo" onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" runat="server" CssClass="img-thumbnail" />
                                                    </asp:HyperLink>
                                                </div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </div>
                                    <!-- END IMG LOOP -->
                                </div>
                            </div>
                            <div class="col-1">
                            </div>
                        </div>
                        <!-- ENDIF -->
                    </div>
                    <div style="flex: 1; padding-left: 15px; padding-right: 17px;">
                        <asp:Label class="badge badge-success mb-2 d-none" Visible="false" ID="ItmConditionNew" runat="server" Text="New"></asp:Label>
                        <asp:Label class="badge badge-warning mb-2 d-none" Visible="false" ID="ItmCondition1Used" runat="server" Text="Used"></asp:Label>
                        <span class="details key mb-1" style="font-weight: 600"></span>
                        <div>
                            <h1 class="mb-4" style="color: rgb(51, 51, 51);">
                                <asp:Literal runat="server" ID="ItemTitle"></asp:Literal></h1>
                        </div>
                        <asp:Panel class="row my-2 my-lg-4 " runat="server" ID="quotedPricing" Style="margin-bottom: 0 !important;">
                            <div class="col-3" runat="server" id="pricingDivProduct">
                                <div>
                                    <span class="details key asking" style="font-weight: 600;" runat="server">Product Price / UOM</span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingProductUOM"></asp:Literal>
                                    </span>
                                </div>
                            </div>
                            <div class="col-3" runat="server" id="pricingDivProduct2">
                                <div>
                                    <span class="details key asking" style="font-weight: 600;" runat="server">Product Price </span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingProduct"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="col-6" runat="server" id="pricingDivProductSpacer"></div>
                           <div class="col-3" runat="server" id="pricingDivFreight">
                                <div>
                                    <span class="details key asking" style="font-weight: 600;">Freight Price / UOM</span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingFreightUOM"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="col-3" runat="server" id="pricingDivFreight2">
                                <div>
                                    <span class="details  key asking" style="font-weight: 600;">Freight Price</span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingFreight"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="col-6" id="pricingDevFreightSpacer" runat="server"></div>
                            <div class="col-3">
                                <div>
                                    <span class="details key asking" style="font-weight: 600;" runat="server" id="totalPriceLabelUOM">Total Price / UOM</span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingTotalUOM"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="col-3">
                                <div>
                                    <span class="details key asking" style="font-weight: 600;" runat="server" id="totalPriceLabel">Total Price</span>
                                </div>
                                <div>
                                    <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal runat="server" ID="pricingTotal"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="col-6"></div>
                        </asp:Panel>
                        <div class="row" id="viewPricing" style="max-width: 600px;" runat="server" visible="false">
                            <asp:Panel ID="iwbSellPricePanel" runat="server" Visible="false" CssClass="col-6 col-lg-3 my-2 my-lg-0">
                                <span class="details price key asking" style="font-weight: 600;">Quoted Price</span>
                                <span class="details price value asking" style="font-weight: 400; font-size: 14px;">
                                    <asp:Literal runat="server" ID="iwbSellPriceLiteral"></asp:Literal></span>
                            </asp:Panel>
                            <asp:Panel ID="itmSellPricePanel" runat="server" Visible="false" CssClass="col-6 col-lg-3 my-2 my-lg-0">
                                <span class="details price key original" style="font-weight: 600;">Asking Price</span>
                                <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="itmSellPriceLiteral"></asp:Literal>
                                </span>
                            </asp:Panel>
                            <asp:Panel ID="TotalSellPricePanel" runat="server" Visible="false" CssClass="col-6 col-lg-3 my-2 my-lg-0">
                                <span class="details price key original" style="font-weight: 600;">Total Price</span>
                                <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="totalSellPriceLiteral"></asp:Literal>
                                </span>
                            </asp:Panel>
                            <asp:Panel Visible="false" ID="itmSellPricePanel_Small" runat="server" CssClass="col-6 col-lg-3 my-2 my-lg-0">
                                <span class="details price key original" style="font-weight: 600;">Asking Price</span>
                                <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="itmSellPriceLiteral2"></asp:Literal></span>
                            </asp:Panel>
                            <div class="col-6 col-lg-3 my-2 my-lg-0 d-none">
                                <asp:Label runat="server" ID="OriginalPriceIndexPriceLiteral" class="price key original" Style="font-weight: 600;">Original Price</asp:Label>
                                <span class="details price value original" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="itmIndexPriceLiteral"></asp:Literal></span>
                            </div>
                        </div>

                        <hr />

                        <div class="row " id="viewOverview" style="width: 400px;">
                            <asp:Panel class="detail-color" ID="iwbQtyPanel" runat="server" Visible="false" CssClass="col-6  my-lg-0">
                                <span class="details key" style="font-weight: 600;">Quantity Wanted</span>
                                <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="iwbQtyLiteral"></asp:Literal></span>
                            </asp:Panel>
                            <div class="col-6 my-lg-0">
                                <span class="details key" style="font-weight: 600;">Quantity Available</span>
                                <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="itmQtyLiteral"></asp:Literal></span>
                            </div>

                        </div>
                        <div class="row " id="Div4" style="width: 400px;">
                            <div class="col-6 my-2 my-lg-0">
                                <span class="details key" style="font-weight: 600;">Location</span>
                                <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal ID="itmLocationLiteral" runat="server"></asp:Literal></span>
                            </div>
                            <div class="col-6 ">
                                <span class="details key" style="font-weight: 600;">Asset #</span>
                                <span class="details value" style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal runat="server" ID="itmIDLiteral"></asp:Literal></span>
                            </div>
                        </div>
                        <div class="row d-none">
                            <div class="col-12 my-5">
                                <span class="contact text p-3 d-block"><i class="fa fa-question-circle mr-1" aria-hidden="true"></i>Questions? Contact
                                    at <span class="contact number"><strong>(440) 331 - 3600
                                        <asp:Literal ID="buyerRepExtLiteral" runat="server" /></strong></span></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <div class="row fade-in-no-movement" id="viewDetails">
            <div class="col-12 my-3" id="tabs">
                <asp:Panel runat="server" ID="ItemTabs">
                    <ul class="nav nav-tabs responsive-tabs flex-nowrap" data-tabs="tabs" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link active mx-1" data-toggle="tab" href="#description" role="tab">Description</a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link mx-1" data-toggle="tab" href="#documents" role="tab">Documents <span class="badge badge-pill badge-primary ml-1">
                                <asp:Label runat="server" ID="itemDocumentCountLabel"></asp:Label></span></a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link mx-1" data-toggle="tab" href="#pictures" role="tab">Pictures <span class="badge badge-pill badge-primary ml-1">
                                <asp:Label runat="server" ID="itemImageCountLabel"></asp:Label></span></a>
                        </li>
                        <li class="nav-item" runat="server" id="loadoutTab" visible="false">
                            <a class="nav-link mx-1" data-toggle="tab" href="#loadout" role="tab">Load Out<span class="badge badge-pill badge-primary ml-1">
                                <asp:Label runat="server" ID="itemLoadOutLabel" Text="0"></asp:Label></span></a>
                        </li>
                    </ul>
                </asp:Panel>
                <asp:Panel runat="server" ID="QuoteTabs">
                    <ul class="nav nav-tabs responsive-tabs flex-nowrap" role="tablist">
                        <li class="nav-item">
                            <a class="nav-link mx-1" runat="server" id="documentsA" data-toggle="tab" href="#documents" role="tab">Documents <span class="badge badge-pill badge-primary ml-1">
                                <asp:Label runat="server" ID="itemQuoteDocumentCountLabel"></asp:Label></span></a>
                        </li>
                        <li class="nav-item">
                            <a class="nav-link mx-1" runat="server" id="loadoutTab2" visible="false" data-toggle="tab" href="#loadout" role="tab">Load Out <span class="badge badge-pill badge-primary ml-1">
                                <asp:Label runat="server" ID="itemLoadOutLabel2"></asp:Label></span></a>
                        </li>
                    </ul>
                </asp:Panel>
                <div class="tab-content">
                    <div class="tab-pane active" runat="server" clientidmode="Static" id="description" role="tabpanel">
                        <div class="row">
                            <div class="col-12 col-lg-4 my-4">
                                <table class="table table-bordered">
                                    <tbody>
                                        <asp:Panel runat="server" ID="LinePipeOCTGPanel" Visible="false">
                                            <tr>
                                                <th>Grade</th>
                                                <td>
                                                    <asp:Literal ID="itmPipeGradeLiteral" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <th>OD (in)</th>
                                                <td>
                                                    <asp:Literal ID="itmODLiteral" runat="server" />
                                                </td>
                                            </tr>
                                            <asp:Panel runat="server" ID="LinePipeRows" Visible="false">
                                                <tr>
                                                    <th>Wall Thickness</th>
                                                    <td>
                                                        <asp:Literal ID="itmWTLiteral" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>Coating</th>
                                                    <td>
                                                        <asp:Literal ID="itmHasCoatingLiteral" runat="server" />
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Panel runat="server" ID="OCTGRows" Visible="false">
                                                <tr>
                                                    <th>Weight (lbs/ft)</th>
                                                    <td>
                                                        <asp:Literal ID="itmWeightLiteral" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <th>End Connection</th>
                                                    <td>
                                                        <asp:Literal ID="itmEndConnectionLiteral" runat="server" />
                                                    </td>
                                                </tr>
                                            </asp:Panel>
                                            <tr>
                                                <th>Seam Type</th>
                                                <td>
                                                    <asp:Literal ID="itmSeamTypeLiteral" runat="server" />
                                                </td>
                                            </tr>

                                        </asp:Panel>
                                        <asp:Panel runat="server" ID="EquipmentGrade" Visible="false">
                                            <tr>
                                                <th>Grade</th>
                                                <td>
                                                    <asp:Literal ID="ItmGradeLiteral" runat="server" /></td>
                                            </tr>
                                        </asp:Panel>
                                        <tr>
                                            <th>Condition</th>
                                            <td>
                                                <asp:Literal ID="ItmConditionLiteral" runat="server" /></td>
                                        </tr>
                                        <asp:Repeater runat="server" ID="AttributesRepeater" Visible="true">
                                            <ItemTemplate>
                                                <tr>
                                                    <th><%# Eval("TagParentValue") %></th>
                                                    <td><%# Eval("TagValue") %></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <tr>
                                            <th>Certifications</th>
                                            <td>
                                                <asp:Literal ID="ItmMTRLiteral" runat="server" /></td>
                                        </tr>

                                        <tr>
                                            <th>Manufacturer</th>
                                            <td>
                                                <asp:Literal ID="ItmMillLiteral" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <th>Country of Origin</th>
                                            <td>
                                                <asp:Literal ID="ItmCountryOfOriginLiteral" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <th>Manufactured Year</th>
                                            <td>
                                                <asp:Literal ID="ItmManufactureYearLiteral" runat="server" /></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-12 col-lg-8 my-4">
                                <h3 class="details key" style="font-weight: 600; font-size: .8888888889rem;">Details</h3>
                                <p style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal ID="basicInfo" runat="server"></asp:Literal>
                                </p>
                                <h3 class="details key" style="font-weight: 600; font-size: .8888888889rem;">Additional Information</h3>
                                <p style="font-weight: 400; font-size: 14px; color: #333333;">
                                    <asp:Literal ID="itmDescriptionLiteral" runat="server"></asp:Literal>
                                </p>
                                <asp:Panel runat="server" ID="commentsPanel">
                                    <h3 class="details key" style="font-weight: 600; font-size: .8888888889rem;">Comments</h3>
                                    <p style="font-weight: 400; font-size: 14px; color: #333333;">
                                        <asp:Literal ID="commentLiteral" runat="server"></asp:Literal>
                                    </p>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="documents" runat="server" clientidmode="Static" role="tabpanel">
                        <div class="row">
                            <div class="col-12 my-4">
                                <table class="table table-hover table-responsive table-bordered">
                                    <thead class="thead-default">
                                        <tr>
                                            <th>Type</th>
                                            <th>File Name</th>
                                            <th>File Size</th>
                                            <th style="width: 130px;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="IdcRepeater">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="vertical-align: middle;">
                                                        <asp:HyperLink ID="hypDocType" runat="server" /></td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:HyperLink ID="hypDocFilename" runat="server" /></td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:HyperLink ID="hypDocSize" runat="server" ForeColor="Black" /></td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:HyperLink runat="server" ID="downloadDocumentButton" Style="width: 120px;" class="btn btn-primary d-block"><i class="fa fa-download" style="margin-right:5px;"></i>Download</asp:HyperLink>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <asp:Panel Visible="false" runat="server" ID="IdcRepeaterEmpty">
                                            <tr id="Tr1" runat="server" class="alt">
                                                <td colspan="4">This item has no documents.</td>
                                            </tr>
                                        </asp:Panel>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="pictures" role="tabpanel">
                        <div class="row">
                            <div class="col-12 my-4">
                                <table class="table table-hover table-responsive table-bordered">
                                    <thead class="thead-default">
                                        <tr>
                                            <th>Image</th>
                                            <th>Name</th>
                                            <th style="width: 130px;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:Repeater runat="server" ID="IimRepeater">
                                            <ItemTemplate>
                                                <tr>
                                                    <td style="vertical-align: middle;">
                                                        <asp:Image Height="40" onerror="this.onerror=null;this.src='/Images/image-unavailable2.png'" ID="itmImageLogo" runat="server" class="img-fluid" /></td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:Literal runat="server" ID="imgCaption"></asp:Literal></td>
                                                    <td style="vertical-align: middle;">
                                                        <asp:HyperLink ID="hypImgDownload" runat="server" ToolTip="Download" CssClass="btn btn-primary"><i class="fa fa-download mr-2" aria-hidden="true"></i> Download</asp:HyperLink></td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <tr id="trEmptyRow" runat="server" visible="false" class="alt">
                                                    <td colspan="3">This item has no pictures.</td>
                                                </tr>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>

                    <div class="tab-pane" id="loadout" runat="server" clientidmode="Static" role="tabpanel">
                        <div class="row">
                            <div runat="server" id="noLoadoutsDiv" visible="false" style="margin: 20px;">No records found.</div>
                            <div class="col-12 my-4" runat="server" id="yesLoadoutsDiv" visible="false">
                                <h5>Load Out Overview</h5>
                                <div class="row no-gutters detailRow my-1 fake-card" style="padding: 10px; margin-bottom: 10px !important">
                                    <div class="col-3">
                                        <asp:Label ID="Label5" runat="server" class="details key original-price" Style="font-weight: 600;">Total Loads Shipped</asp:Label>
                                        <div>
                                            <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                <asp:Literal runat="server" ID="itmLoadShippedLabel"></asp:Literal></span>
                                        </div>
                                    </div>
                                    <div class="col-3">
                                        <asp:Label ID="Label6" runat="server" class="details key original-price" Style="font-weight: 600;">Total Quantity Shipped</asp:Label>
                                        <div>
                                            <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                <asp:Literal runat="server" ID="itmTotalFeetShippedLabel"></asp:Literal></span>
                                        </div>
                                    </div>

                                </div>
                                <h5 style="margin-bottom: 10px;">Load Out Details</h5>

                                <table id="loadoutTable" class="table table-hover table-responsive table-bordered">
                                    <asp:Repeater ID="LoadoutRepeater" runat="server" OnItemDataBound="LoadoutRepeater_ItemDataBound">
                                        <HeaderTemplate>
                                            <thead class="thead-default">
                                                <tr>
                                                    <th>Shipping Date</th>
                                                    <th>Mode of Transportation</th>
                                                    <th>Load&nbsp;#</th>
                                                    <th>Bill of Lading&nbsp;#</th>
                                                    <th>Vehicle&nbsp;#</th>
                                                    <th>Qty&nbsp;Shipped</th>
                                                    <th>Details</th>
                                                    <th>Documents</th>
                                                </tr>
                                            </thead>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="Tr1">
                                                <td><%# Eval("iblShippingDate","{0:MM/dd/yyyy}") %></td>
                                                <td><%# Eval("motName") %></td>
                                                <td><%# Eval("iblLoadNo") %></td>
                                                <td><%# Eval("iblBillOfLadingNo") %></td>
                                                <td><%# Eval("iblTransportationNo") %></td>
                                                <td><%# Eval("qtyShipped") %></td>
                                                <td><%# Eval("iblNote") %></td>
                                                <td style="text-align: center; width: 100px;">
                                                    <button
                                                        type="button"
                                                        style="width: 80px;"
                                                        class="btn btn-primary btn-sm"
                                                        title="Documents"
                                                        data-container="body"
                                                        data-toggle="popover"
                                                        data-placement="bottom"
                                                        data-html="true"
                                                        data-content='<%# Eval("DocumentHtml") %>'>
                                                        View <span class="badge badge-light ml-2"><%# Eval("iblDocCount") %></span>
                                                    </button>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <script>

</script>
    </asp:Panel>

</div>
