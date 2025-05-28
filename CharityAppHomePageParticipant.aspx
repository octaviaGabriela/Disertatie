<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CharityAppHomePageParticipant.aspx.cs" Inherits="CharityAppProject.Views.Participant.CharityAppHomePageParticipant" MaintainScrollPositionOnPostBack="true" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charity app home page</title>
    <link rel="stylesheet" href="CharityAppHomePageParticipant.css" />
</head>
<body>
    <form id="charityAppHomePageParticipant" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container">
            <div class="top-bar">
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="logout-button" OnClick="BtnLogout_Click" />
            </div>
            <img src="../../Assets/logo.png" class="logo" />
            <header>
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
                <p class="intro">Welcome, you can register for competitions, withdraw from them if necessary, and view the funds raised in the charity events you participated in. However, we hope you won’t need to withdraw and will continue to support the cause!</p>
                <p class="important">Important: All registrations and withdrawals must be completed at least one full day before the competition date.</p>
                <p class="dmy">Don’t miss your chance — sign up in time and be part of the change!</p>
            </header>

            <div class="info-container">
                <asp:UpdatePanel ID="updNewCompetitions" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>New Competitions</h2>
                            <asp:GridView ID="gvNewCompetitions" runat="server" AutoGenerateColumns="False" OnRowCommand="GvNewCompetitions_RowCommand" OnRowDataBound="GvNewCompetitions_RowDataBound" CssClass="table">
                                <Columns>
                                    <asp:TemplateField HeaderText="Register">
                                        <ItemTemplate>
                                            <asp:Button ID="btnRegisterToCompetition" runat="server" Text="🏁" ToolTip="Sign up now" CommandName="RegisterToCompetition" CommandArgument='<%# Container.DataItemIndex %>' CssClass="button" />
                                            <asp:Label ID="lblGVN" runat="server" Text="Registration deadline has passed" CssClass="gv-label" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Competition_Name" HeaderText="Competition Name" />
                                    <asp:BoundField DataField="TicketFee" HeaderText="Ticket Fee" />
                                    <asp:BoundField DataField="Competition_Date" HeaderText="Competition Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:BoundField DataField="Competition_Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Sport_Name" HeaderText="Sport" />
                                    <asp:BoundField DataField="Sponsor_Name" HeaderText="Sponsor" />
                                    <asp:BoundField DataField="Charity_Person_Name" HeaderText="Charity Person" />
                                    <asp:BoundField DataField="Cause" HeaderText="Cause" />
                                    <asp:BoundField DataField="NeededMoney" HeaderText="Needed Money" />
                                    <asp:BoundField DataField="Association_Name" HeaderText="Association" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lblErrorNew" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="info-container">
                <asp:UpdatePanel ID="updAssociatedCompetitions" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>My Competitions</h2>
                            <asp:GridView ID="gvAssociatedCompetitions" runat="server" AutoGenerateColumns="False" OnRowCommand="GvAssociatedCompetitions_RowCommand" OnRowDataBound="GvAssociatedCompetitions_RowDataBound" CssClass="table" EmptyDataText="You haven't signed up for any competitions." ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Withdraw">
                                        <ItemTemplate>
                                            <asp:Button ID="btnWithdrawFromCompetition" runat="server" Text="❌" ToolTip="Cancel my participation in the competition" CommandName="WithdrawFromCompetition" CommandArgument='<%# Container.DataItemIndex %>' CssClass="button" />
                                            <asp:Label ID="lblGVA" runat="server" CssClass="gv-label" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Competition_Name" HeaderText="Competition Name" />
                                    <asp:BoundField DataField="TicketFee" HeaderText="Ticket Fee" />
                                    <asp:BoundField DataField="RaisedMoney_C" HeaderText="Raised Money" />
                                    <asp:BoundField DataField="Competition_Date" HeaderText="Competition Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:BoundField DataField="Competition_Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Sport_Name" HeaderText="Sport" />
                                    <asp:BoundField DataField="DonationMoney" HeaderText="Donation Amount" />
                                    <asp:BoundField DataField="Charity_Person_Name" HeaderText="Charity Person" />
                                    <asp:BoundField DataField="Cause" HeaderText="Cause" />
                                    <asp:BoundField DataField="NeededMoney" HeaderText="Needed Money" />
                                    <asp:BoundField DataField="Association_Name" HeaderText="Association" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <asp:Label ID="lblErrorAssociated" runat="server" Visible="false"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
