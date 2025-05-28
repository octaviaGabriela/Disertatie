<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginWF.aspx.cs" Inherits="CharityAppProject.Views.LoginWF" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charity Platform Login</title>
    <link rel="stylesheet" type="text/css" href="LoginStyles.css" />
</head>
<body>
    <form id="loginForm" runat="server">
        <section class="container">
            <header>
                <img src="../Assets/logo.png" class="logo" />
                <h1>Join Charity Sports Events</h1>
                <p class="subtitle">Login to participate in events and help those in need.</p>
            </header>

            <div class="login-box">
                <h2>Login</h2>
                <p class="info-text">Enter your credentials to continue.</p>

                <div class="input-group">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username"></asp:TextBox>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="Password"></asp:TextBox>
                    <asp:Button ID="btnLogin" runat="server" CssClass="login-btn" Text="Login" OnClick="BtnLogin_Click" /> 
                    <asp:Button ID="btnForgotPassword" runat="server" CssClass="create-btn" Text="Forgot password?" OnClick="BtnForgotPassword_Click" />
                    <asp:Label ID="lblError" runat="server" CssClass="error-message"></asp:Label>
                </div>
            </div>

            <div class="create">
                <p class="info-text">New here? Create an account to start donating!</p>
                <asp:Button ID="btnCreateAccount" runat="server" CssClass="create-btn" Text="Create New Account" OnClick="BtnCreateAccount_Click" />
            </div>
          </section>
    </form>
</body>
</html>