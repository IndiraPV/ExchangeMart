<%@ Page Title="ExchangeBase.com - Login" Language="C#" MasterPageFile="~/Buyer/Buyer.master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Exchangebase.Com.Login" %>

<%@ MasterType VirtualPath="~/Buyer/Buyer.master" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="head1" runat="server" ContentPlaceHolderID="head">
<%--    <style>
        body {
            background: #fafafa !important;
        }

        .wrapper {
            margin-top: 80px;
            margin-bottom: 80px;
        }

        .form-signin {
            max-width: 380px;
            padding: 15px 35px 45px;
            margin: 0 auto;
            background-color: #fff;
            border: 1px solid rgba(0, 0, 0, 0.1);
        }

            .form-signin .form-signin-heading,
            .form-signin .checkbox {
                margin-bottom: 30px;
            }

            .form-signin .checkbox {
                font-weight: normal;
            }

            .form-signin .form-control {
                position: relative;
                font-size: 16px;
                height: auto;
                padding: 10px;
            }

                .form-signin .form-control:focus {
                    z-index: 2;
                }

            .form-signin input[type="text"] {
                margin-bottom: -1px;
                border-bottom-left-radius: 0;
                border-bottom-right-radius: 0;
            }

            .form-signin input[type="password"] {
                margin-bottom: 20px;
                border-top-left-radius: 0;
                border-top-right-radius: 0;
            }

        #bodywrapper {
            background-color: #eee;
            padding: 0;
        }

        .modal {
            text-align: center;
            padding: 0!important;
        }

            .modal:before {
                content: '';
                display: inline-block;
                height: 100%;
                vertical-align: middle;
                margin-right: -4px;
            }

        .modal-dialog {
            display: inline-block;
            text-align: left;
            vertical-align: middle;
        }

        #passwordResetToast {
            visibility: hidden;
            min-width: 200px;
            margin-left:-250px;
            background-color: #333;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            left: 50%;
            bottom: 30px;
            font-size: 17px;
        }

            #passwordResetToast.show {
                visibility: visible;
                -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
                animation: fadein 0.5s, fadeout 0.5s 2.5s;
            }

        @-webkit-keyframes fadein {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @keyframes fadein {
            from {
                bottom: 0;
                opacity: 0;
            }

            to {
                bottom: 30px;
                opacity: 1;
            }
        }

        @-webkit-keyframes fadeout {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }

        @keyframes fadeout {
            from {
                bottom: 30px;
                opacity: 1;
            }

            to {
                bottom: 0;
                opacity: 0;
            }
        }
    </style>--%>
    <%--<script type="text/javascript" src="../Scripts/bootstrap-modal.js"></script>--%>

    <style type="text/css">
        #passwordResetToast {
            visibility: hidden;
            min-width: 200px;
            margin-left:-250px;
            background-color: #333;
            color: #fff;
            text-align: center;
            border-radius: 2px;
            padding: 16px;
            position: fixed;
            z-index: 1;
            left: 50%;
            bottom: 30px;
            font-size: 17px;
        }

            #passwordResetToast.show {
                visibility: visible;
                -webkit-animation: fadein 0.5s, fadeout 0.5s 2.5s;
                animation: fadein 0.5s, fadeout 0.5s 2.5s;
            }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#resetPasswordButton").on("click", function () {
                if (typeof woopra != "undefined" && woopra) {
                    woopra.track('userportal', { page: 'Login', description: 'Reset Password' });
                }
                var email = String($("#txtForgotPassword").val());

                if (email.length < 1) {
                    $("#resetpassworderrorlabel").text("Enter an email address.");
                    return;
                }

                $.ajax({
                    url: 'Login.aspx/ResetPassword',
                    type: "POST",
                    data: "{'email':'" + escape(email) + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        if (data.d == "0" || data.d == "2") {
                            //alert("Your password has been reset and an email has been sent to the address provided.  Please check your inbox for further instruction.");
                            $('#passwordModal').modal('hide');
                            $("#txtForgotPassword").val("");
                            showPasswordResetSuccess();
                        } else if (data.d == "1") {
                            $("#resetpassworderrorlabel").text("Enter an email address.");

                            //alert("Enter an email address");
                        } else if (data.d == "2") {
                            //removing because this is an security flaw where an attacker could find out a list of all known users
                            //$("#resetpassworderrorlabel").text("The specified email address was not found.");
                            //alert("The specified email address was not found.");
                        } else {
                            alert("An unknown error occurred while attempting to send the message.");
                        }
                    }
                });
            });
        });
        function showPasswordResetSuccess() {
            var x = document.getElementById("passwordResetToast")
            x.className = "show";
            setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">

    <div id="passwordResetToast">
        <p>Your password has been reset and an email has been sent to the address provided. </p>
        <p>Please check your inbox for further instruction...</p>
    </div>
    <div id="passwordModal" class="modal  fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog" style="max-width:400px;text-align: center;">
            <div class="modal-content">
                <button type="button" class="close text-right px-4 pt-2 pb-0" data-dismiss="modal" aria-hidden="true">x</button>
                <div class="modal-header col-12 d-flex align-items-center text-center">
                    
                    <h4 class="mx-auto"><i class="fa fa-lock mr-2"></i> Forgot Password?</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="input-group">
                            <span class="input-group-addon"><i class="fa fa-envelope"></i></span>
                            <asp:TextBox ID="txtForgotPassword" CssClass="form-control" placeholder="Email" runat="server" ClientIDMode="Static" />
                        </div>
                    </div>
                    <div id="resetpassworderrorlabel" style="color: red;"></div>
                </div>
                <div class="modal-footer">
                    <a id="resetPasswordButton" class="btn btn-primary" tabindex>Reset password</a>
                    <a class="btn btn-outline-danger" data-dismiss="modal" aria-hidden="true" tabindex>Cancel</a>
                    
                </div>
            </div>
        </div>
    </div>
    <div class="container">
        <div class="row p-5">
            <div class="col-md-8 col-lg-6 col-s-12 col-xs-12 bg-white p-5 align-self-center mx-auto align-middle" style="border-radius: 10px; border: 1px solid #d7dce1;">
               <h1><%= System.Configuration.ConfigurationManager.AppSettings["BuyerLoginBanner"] %></h1>
                <asp:Panel ID="LoginPanel" runat="server" DefaultButton="OkButton">
                    <asp:TextBox required="" placeholder="Email" ID="UsrEmailTextBox" CssClass="form-control my-3" runat="server" />
                    <asp:TextBox required="" placeholder="Password" ID="UsrPasswordTextBox" runat="server" TextMode="Password" CssClass="form-control my-3" />
                    <asp:Panel ID="PanelLoginError" runat="server" ForeColor="Red" Style="padding-top: 4px;">
                        <asp:Label ID="lblLoginError" runat="server" />
                    </asp:Panel>
                    <div class="form-block">
                        <asp:Button ID="OkButton" runat="server" Text="Log In" CssClass="btn btn-primary d-block col-12" />
                        <asp:CheckBox Visible="false" ID="ChkRememberMe" runat="server" Text="&nbsp;&nbsp;Remember Me" />
                    </div>
                    <div style="margin-top: 20px; text-align: center;"><a href="#passwordModal" data-toggle="modal">Forgot Your Password?</a></div>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
