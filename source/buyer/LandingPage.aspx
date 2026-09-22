<%@ Page Title="" Language="C#" MasterPageFile="~/buyer/Buyer.master" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="Exchangebase.Com.buyer.LandingPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sidebar" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="body" runat="server">
    <style type="text/css">
        .technology {
            padding: 30px 0;
        }

            .technology .row {
                justify-content: space-between;
            }

                .technology .row > div {
                    width: 48%;
                }

        .tech-box {
            box-shadow: 0 0 10px rgba(0,0,0,0.5);
            margin: 0 10px;
            text-align: center;
            padding-bottom: 30px;
        }

            .tech-box img {
                height: 115px;
                width: 100%;
                object-fit: cover;
            }

            .tech-box p {
                text-align: center;
                font-weight: 600;
                font-size: 22px;
                color: #215ea2;
                margin-top: 20px;
                margin-bottom: 25px;
            }

            .tech-box a {
                background-color: #3581ff;
                padding: 7px 25px;
                border-radius: 5px;
                color: white;
                font-weight: 600;
                line-height: 0;
                transition: 0.3s;
            }

                .tech-box a:hover {
                    text-decoration: none;
                    color: white;
                    box-shadow: 0 0 10px rgba(0,0,0,0.3);
                }

        @media screen and (max-width: 767px) {
            .technology .row {
                flex-wrap: wrap;
            }

                .technology .row > div {
                    width: 100%;
                    margin-bottom: 30px;
                }

                    .technology .row > div:last-child {
                        margin-bottom: 0;
                    }
        }
    </style>
    <%-- <div class="container">
        <div class="row p-5">
            <div class="col-md-8 col-lg-6 col-s-12 col-xs-12 bg-white p-5 align-self-center mx-auto align-middle" style="border-radius: 10px; border: 1px solid #d7dce1;">
                <h1>EXB Energy</h1>
                <div class="form-block" style="overflow:hidden;">
                    <asp:Image runat="server" Height="100" ImageUrl="https://www.exchangebase.com/wp-content/uploads/2017/02/industry-midstream-banner.jpg" />
                    <asp:Button ID="EnergyButton" runat="server" Text="Go To the Energy Customer Portal" CssClass="btn btn-primary d-block col-12" />
                </div>
            </div>
            <div class="col-md-8 col-lg-6 col-s-12 col-xs-12 bg-white p-5 align-self-center mx-auto align-middle" style="border-radius: 10px; border: 1px solid #d7dce1;">
                <h1>EXB Ventures</h1>
                <div class="form-block">
                    <asp:Button ID="VenturesButton" runat="server" Text="Go To the Ventures Portal" CssClass="btn btn-primary d-block col-12" />
                </div>
            </div>
        </div>
    </div>--%>
    <div class="technology" style="width:100%">
        <div class="container">
            <div class="row">
                <div class="coll-lg-3 coll-md-3">
                    <div class="tech-box">
                        <img src="http://www.exchangebase.com/wp-content/uploads/2017/02/industry-midstream-nav-2.jpg" alt="" />
                        <p>EXB Energy - Oil & Gas Division</p>
                        <a href="https://tubulars.exchangebase.com/Buyer/Login.aspx">View Division</a>
                    </div>
                </div>
                <div class="coll-lg-3 coll-md-3">
                    <div class="tech-box">
                        <img src="http://www.exchangebase.com/wp-content/uploads/2017/12/iStock-542071270-nav.png" alt="" />
                        <p>EXB Ventures</p>
                        <a href="https://energy.exchangebase.com/Buyer/Login.aspx">View Division</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="list" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
