<%@ Page Title="" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="BuyerUserSettingsEdit.aspx.cs" Inherits="Exchangebase.Com.Buyer.BuyerUserSettingsEdit" %>
<%@ Register TagPrefix="exb" TagName="AccountSettings" Src="~/Controls/AccountSettings-Edit.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <exb:AccountSettings ID="AccountSettings1" runat="server" />
</asp:Content>
