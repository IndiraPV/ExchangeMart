<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadOutProfile.ascx.cs" Inherits="Exchangebase.Com.buyer.LoadOutProfile" %>

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
        flex: 1 1;
        padding-left: 15px;
        padding-right: 15px;
    }

    .mini-div {
        max-height: 100px;
        overflow: hidden;
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
        height: 100%;
    }

    .full-width {
        width: 100%;
    }

    .card-img {
        width: 100%;
        height: 100%;
        border-radius: calc(.25rem - 1px);
        object-fit: contain;
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
        $('#loadOut').DataTable();
    });
</script>

<script src="resources/jquery.dataTables.min.js"></script>
<div class="container-fluid" id="profileView" style="background-color: unset">
    <div class="row">
        <%--<asp:Literal></asp:Literal>--%>
        <div class="col-12">

            <div class="row">
                <div class="col-9 mb-3">
                    <div class="card full-height">
                        <div class="flex-row  full-height">
                            <div class="flex-1 no-padding no-padding">
                                <div class="card-body px-3 py-2 row">
                                    <div class="flex-1">
                                        <span class="details key mb-1" style="font-weight: 600">Asset Name
                                        </span>
                                        <h1>
                                            <asp:Literal runat="server" ID="itmNameLabel"></asp:Literal></h1>
                                        <h2 class="card-title my-1 details value" style="font-weight: 400; font-size: 14px; color: #333333;"></h2>

                                    </div>
                                    <div class="flex-200px align-right pr-2">
                                    </div>
                                </div>

                                <div class="card-footer px-3 py-2" style="background-color: unset;">
                                    <div class="row no-gutters detailRow my-1">
                                        <div class="col-3">
                                            <asp:Label ID="Label4" runat="server" class="details key original-price" Style="font-weight: 600;">Asset #</asp:Label>
                                            <div>
                                                <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                    <asp:Literal runat="server" ID="itmIDLabel"></asp:Literal></span>
                                            </div>
                                        </div>
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
                                    <div class="row no-gutters detailRow my-1" id="explanationTextDiv" runat="server">
                                        <div>
                                            <asp:Label ID="Label2" runat="server" class="details key original-price" Style="font-weight: 600;">Explanation</asp:Label>
                                            <div>
                                                <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                    <asp:Literal runat="server" ID="explanation"></asp:Literal></span>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row no-gutters detailRow my-1" style="display: none;">
                                        <div>
                                            <asp:Label ID="OriginalPriceIndexPriceLabel" runat="server" class="details key original-price" Style="font-weight: 600;">Item Details</asp:Label>
                                            <div>
                                                <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                    <asp:Literal runat="server" ID="itmBasicInfo"></asp:Literal></span>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="row no-gutters detailRow my-2">
                                        <div>
                                            <asp:Label ID="Label1" runat="server" class="details key original-price" Style="font-weight: 600;">Additional Information</asp:Label>
                                            <div>
                                                <span class="details value original-price" style="font-weight: 400; font-size: 14px; color: #333333;">
                                                    <asp:Literal runat="server" ID="itmDescriptionLabel"></asp:Literal></span>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <table id="loadOut" class="table table-hover table-responsive table-bordered">
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
