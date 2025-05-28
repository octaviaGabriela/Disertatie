<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCompetition.aspx.cs" 
    Inherits="CharityAppProject.Views.Planner.CreateCompetition"
    MaintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add New Competition,View Existing Ones or Issue tickets</title>
    <link rel="stylesheet" type="text/css" href="CreateCompetitionStyles.css" />
</head>
<body>
    <form id="createCompetition" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container">

            <div class="button-group">
                <asp:Button ID="btnManage" runat="server" Text="Manage Competition Info" CssClass="action-button" OnClick="BtnManage_Click" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="action-button" OnClick="BtnLogout_Click" />
            </div>

            <header>
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
                <p>Welcome, you can now add new competitions, view and edit existing ones or issue tickets.</p>
                <asp:Label ID="lblUserId" runat="server" CssClass="user-id-label"></asp:Label>
            </header>

            <div class="competition-container">
                <h2>Existing Competitions</h2>
                <asp:UpdatePanel ID="updComp" runat="server">
                    <ContentTemplate>
                        <div class="competition-table-container">
                            <asp:GridView ID="gvCompetitions" runat="server" AutoGenerateColumns="False"
                                CssClass="competition-table" GridLines="None" AllowSorting="True"
                                OnRowDataBound="GvCompetitions_RowDataBound" OnRowCommand="GvCompetitions_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Update">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUpdateStatus" runat="server" CssClass="select-button"
                                                Text="🔔" ToolTip="Update this competition"
                                                CommandName="UpdateStatus"
                                                CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Competition_Name" HeaderText="Competition Name" />
                                    <asp:BoundField DataField="TicketFee" HeaderText="Ticket Fee" />
                                    <asp:BoundField DataField="RaisedMoney_C" HeaderText="Raised Money" />
                                    <asp:BoundField DataField="Competition_Date" HeaderText="Competition Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:BoundField DataField="Competition_Status" HeaderText="Status" />
                                    <asp:BoundField DataField="Sport_Name" HeaderText="Sport" />
                                    <asp:BoundField DataField="App_User_Name" HeaderText="Organizer" />
                                    <asp:BoundField DataField="Sponsor_Name" HeaderText="Sponsor" />
                                    <asp:BoundField DataField="Sponsor_Contribution" HeaderText="Sponsor Amount" />
                                    <asp:BoundField DataField="DonationMoney" HeaderText="Donation Amount" />
                                    <asp:BoundField DataField="CNP_CP" HeaderText="CNP" />
                                    <asp:BoundField DataField="Charity_Person_Name" HeaderText="Charity Person" />
                                    <asp:BoundField DataField="Cause" HeaderText="Cause" />
                                    <asp:BoundField DataField="IBAN" HeaderText="IBAN" />
                                    <asp:BoundField DataField="NeededMoney" HeaderText="Needed Money" />
                                    <asp:BoundField DataField="Association_Name" HeaderText="Association" />
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDeleteComp" runat="server" CssClass="select-button"
                                                Text="❌" ToolTip="Delete this competition"
                                                CommandName="DeleteCompetition"
                                                CommandArgument='<%# Container.DataItemIndex %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <asp:Label ID="lblErrorComp" runat="server" />
                        <div class="form-group">
                            <asp:Button ID="btnUpdateCompetitions" runat="server" CssClass="btnUpdateCompetitions" 
                                Text="Update the Status of Competitions" OnClick="BtnUpdateCompetitions_Click" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <div class="create-competition-container">
                <h2>Add New Competition</h2>
                <div class="form-group">
                    <label for="txtCompetitionName">Competition Name</label>
                    <asp:TextBox ID="txtCompetitionName" runat="server" CssClass="input-field" />
                </div>
                <div class="form-group">
                    <label for="txtTicketFee">Ticket Fee</label>
                    <asp:TextBox ID="txtTicketFee" runat="server" CssClass="input-field" TextMode="Number" />
                </div>
                <div class="form-group">
                    <label for="ddlSport">Select Sport</label>
                    <asp:DropDownList ID="ddlSport" runat="server" CssClass="input-field" />
                </div>
                <div class="form-group">
                    <label for="txtCompetitionDate">Competition Date</label>
                    <asp:TextBox ID="txtCompetitionDate" runat="server" CssClass="input-field" TextMode="DateTimeLocal" />
                </div>
                <div class="form-group">
                    <label for="ddlSponsor">Select Sponsor</label>
                    <asp:DropDownList ID="ddlSponsor" runat="server" CssClass="input-field" />
                </div>
                <div class="form-group">
                    <label for="ddlAssociation">Select Association</label>
                    <asp:DropDownList ID="ddlAssociation" runat="server" CssClass="input-field"
                        AutoPostBack="true" OnSelectedIndexChanged="DdlAssociation_SelectedIndexChanged" />
                </div>
                <div class="form-group">
                    <label for="ddlCharityPerson">Select Person from Association</label>
                    <asp:DropDownList ID="ddlCharityPerson" runat="server" CssClass="input-field" />
                </div>

                <asp:Label ID="lblErrorCreateComp" runat="server" />
                <div class="form-group">
                    <asp:Button ID="btnCreateComp" runat="server" CssClass="btnCreateComp" 
                        Text="Create Competition" OnClick="BtnCreateCompetition_Click" />
                </div>
            </div>

            <div class="competition-container">
                <h2>View Participant Tickets</h2>
                <div class="competition-table-container">
                    <asp:UpdatePanel ID="updTickets" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <asp:Timer ID="tmrAutoRefresh" runat="server" Interval="10000" OnTick="TmrAutoRefresh_Tick" />
                            <asp:GridView ID="gvTickets" runat="server" AutoGenerateColumns="False"
                                CssClass="competition-table" EmptyDataText="No competitions planned at this time."
                                ShowHeaderWhenEmpty="true" OnRowCommand="GvTickets_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="App_User_Name" HeaderText="Participant" />
                                    <asp:BoundField DataField="Competition_Name" HeaderText="Competition" />
                                    <asp:BoundField DataField="Competition_Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                    <asp:BoundField DataField="TicketFee" HeaderText="Fee" />
                                    <asp:TemplateField HeaderText="Ticket">
                                        <ItemTemplate>
                                            <asp:Button ID="btnShowTicket" runat="server" Text="🎟️ Issue ticket"
                                                CommandName="ShowTicket" CommandArgument='<%# Container.DataItemIndex %>'
                                                CssClass="select-button" />
                                            <asp:Label ID="lblGVT" runat="server" CssClass="gv-label" Visible="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

            <asp:UpdatePanel ID="UpdT" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
                <ContentTemplate>
                    <asp:MultiView ID="mvTicket" runat="server">
                        <asp:View ID="vwTicket" runat="server">
                            <div class="ticketContainer">
                                <asp:Button ID="btnCloseTicket" runat="server" Text="x" OnClick="BtnCloseTicket_Click" CssClass="btnC" />
                                <div class="ticketBox">
                                    <img src="../../Assets/logo.png" class="logo" />
                                    <h3>Tickets for Participants</h3>
                                    <p class="tichetD">Participant name: <asp:Label ID="lblTicketParticipant" runat="server" /></p>
                                    <p class="tichetD">Competition: <asp:Label ID="lblTicketCompetition" runat="server" /></p>
                                    <p class="tichetD">Date and time: <asp:Label ID="lblTicketDate" runat="server" /></p>
                                    <p class="tichetD">Ticket Fee: <asp:Label ID="lblTicketFee" runat="server" /></p>
                                </div>
                                <button onclick="window.print()" class="btnP">🖨️ Print the ticket</button>
                            </div>
                        </asp:View>
                    </asp:MultiView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
