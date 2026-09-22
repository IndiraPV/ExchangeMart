<%@ Page Title="" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="BuyerUserSettings.aspx.cs" Inherits="Exchangebase.Com.Buyer.BuyerUserSettings" %>
<%@ Register TagPrefix="exb" TagName="AccountSettings" Src="~/Controls/AccountSettings.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <exb:AccountSettings ID="AccountSettings1" runat="server" />
</asp:Content>
