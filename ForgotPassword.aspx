<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="CharityAppProject.Views.ForgotPassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <link rel="stylesheet" type="text/css" href="ForgotPasswordStyles.css" />
</head>
<body>
    <form id="forgotPassword" runat="server">
                <div class="container">
            <div class="change-container">
                <h2>Change your password</h2>
                <div class="input-group">
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="input-field" placeholder="Username"></asp:TextBox>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="OldPassword"></asp:TextBox>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="NewPassword"></asp:TextBox>
                </div>
                
                <asp:Label ID="lblError" runat="server" CssClass="error-message"></asp:Label>
                <asp:Button ID="btnSave" runat="server" CssClass="save-button" Text="Save" OnClick="BtnSave_Click" />
            </div>
        </div>
    </form>
</body>
</html>
