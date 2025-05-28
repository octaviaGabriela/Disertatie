<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateNewAccountWF.aspx.cs" Inherits="CharityAppProject.Views.CreateNewAccountWF" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Create new account</title>
    <link rel="stylesheet" type="text/css" href="CreateNewAccountStyles.css" />
</head>
<body>
    <form id="createNewAccountForm" runat="server">
        <div class="container">
            <div class="create-container">
                <h2>Create new account</h2>
                <div class="input-group">
                    <asp:TextBox ID="txtNewUsername" runat="server" CssClass="input-field" placeholder="NewUsername"></asp:TextBox>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="input-field" TextMode="Password" placeholder="NewPassword"></asp:TextBox>
                </div>
                <div>
                    <div class="checkbox-container">
                        <asp:CheckBox ID="chkIsPlanner" runat="server" CssClass="checkbox" Text="I am a planner" />
                    </div>
                </div>
                <asp:Label ID="lblError" runat="server" CssClass="error-message"></asp:Label>
                <asp:Button ID="btnCreate" runat="server" CssClass="create-account-button" Text="Create" OnClick="BtnCreateAccount_Click" />
            </div>
        </div>
    </form>
</body>
</html>
