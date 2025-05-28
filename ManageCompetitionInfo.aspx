<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageCompetitionInfo.aspx.cs"
    Inherits="CharityAppProject.Views.Planner.ManageCompetitionInfo"
    MaintainScrollPositionOnPostBack="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Competition Information</title>
    <link rel="stylesheet" href="ManageCompetitionInfoStyles.css" />
</head>
<body>
    <form id="manageCompetitionInfo" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />

        <div class="container">

            <div class="button-group">
                <asp:Button ID="btnManage" runat="server" Text="Competitions" CssClass="action-button" OnClick="BtnManage_Click" />
                <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="action-button" OnClick="BtnLogout_Click" />
            </div>

            <header>
                <asp:Label ID="lblUserName" runat="server"></asp:Label>
                <p>Welcome, you can now manage competition information.</p>
            </header>

            <!-- Sport Table -->
            <div class="info-container">
                <asp:UpdatePanel ID="updSport" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>Sport Table</h2>
                            <asp:GridView ID="gvSport" runat="server" AutoGenerateColumns="False" OnRowCommand="GvSport_RowCommand" CssClass="table">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelectSport" runat="server" Text="Select" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" CssClass="select-btn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sport_Id" HeaderText="Sport ID" />
                                    <asp:BoundField DataField="Name_S" HeaderText="Sport Name" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-container">
                            <h3>Edit Sport Information</h3>
                            <asp:Label ID="labelSportId" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <label for="txtSportName" class="input-label">Sport Name</label>
                                <asp:TextBox ID="txtSportName" runat="server" CssClass="input-field" />
                            </div>
                            <div class="button-container">
                                <asp:Button ID="btnAddSport" runat="server" Text="Add" OnClick="AddSport" CssClass="button" />
                                <asp:Button ID="btnEditSport" runat="server" Text="Edit" OnClick="EditSport" CssClass="button" />
                                <asp:Button ID="btnDeleteSport" runat="server" Text="Delete" OnClick="DeleteSport" CssClass="button" />
                            </div>
                            <asp:Label ID="lblErrorSport" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Sponsor Table -->
            <div class="info-container">
                <asp:UpdatePanel ID="updSponsor" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>Sponsor Table</h2>
                            <asp:GridView ID="gvSponsor" runat="server" AutoGenerateColumns="False" OnRowCommand="GvSponsor_RowCommand" CssClass="table">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelectSponsor" runat="server" Text="Select" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" CssClass="select-btn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Sponsor_Id" HeaderText="Sponsor ID" />
                                    <asp:BoundField DataField="Name_S" HeaderText="Sponsor Name" />
                                    <asp:BoundField DataField="Money_S" HeaderText="Amount" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-container">
                            <h3>Edit Sponsor Information</h3>
                            <asp:Label ID="labelSponsorId" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <label for="txtSponsorName" class="input-label">Sponsor Name</label>
                                <asp:TextBox ID="txtSponsorName" runat="server" CssClass="input-field" />
                                <label for="txtSponsorAmount" class="input-label">Sponsor Amount</label>
                                <asp:TextBox ID="txtSponsorAmount" runat="server" CssClass="input-field" TextMode="Number" />
                            </div>
                            <div class="button-container">
                                <asp:Button ID="btnAddSponsor" runat="server" Text="Add" OnClick="AddSponsor" CssClass="button" />
                                <asp:Button ID="btnEditSponsor" runat="server" Text="Edit" OnClick="EditSponsor" CssClass="button" />
                                <asp:Button ID="btnDeleteSponsor" runat="server" Text="Delete" OnClick="DeleteSponsor" CssClass="button" />
                            </div>
                            <asp:Label ID="lblErrorSponsor" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Association Table -->
            <div class="info-container">
                <asp:UpdatePanel ID="updAssociation" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>Association Table</h2>
                            <asp:GridView ID="gvAssociation" runat="server" AutoGenerateColumns="False" OnRowCommand="GvAssociation_RowCommand" CssClass="table">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelectAssociation" runat="server" Text="Select" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" CssClass="select-btn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Association_Id" HeaderText="Association ID" />
                                    <asp:BoundField DataField="Name_A" HeaderText="Association Name" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-container">
                            <h3>Edit Association Information</h3>
                            <asp:Label ID="labelAssociationId" runat="server" Visible="false"></asp:Label>
                            <div class="form-group">
                                <label for="txtAssociationName" class="input-label">Association Name</label>
                                <asp:TextBox ID="txtAssociationName" runat="server" CssClass="input-field" />
                            </div>
                            <div class="button-container">
                                <asp:Button ID="btnAddAssociation" runat="server" Text="Add" OnClick="AddAssociation" CssClass="button" />
                                <asp:Button ID="btnEditAssociation" runat="server" Text="Edit" OnClick="EditAssociation" CssClass="button" />
                                <asp:Button ID="btnDeleteAssociation" runat="server" Text="Delete" OnClick="DeleteAssociation" CssClass="button" />
                            </div>
                            <asp:Label ID="lblErrorAssociation" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

            <!-- Charity Person Table -->
            <div class="info-container">
                <asp:UpdatePanel ID="updCharityPerson" runat="server">
                    <ContentTemplate>
                        <div class="table-container">
                            <h2>Charity Person Table</h2>
                            <asp:GridView ID="gvCharityPerson" runat="server" AutoGenerateColumns="False" OnRowCommand="GvCharityPerson_RowCommand" CssClass="table">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnSelectCharityPerson" runat="server" Text="Select" CommandName="Select" CommandArgument="<%# Container.DataItemIndex %>" CssClass="select-btn" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="CNP_CP" HeaderText="CNP" />
                                    <asp:BoundField DataField="Name_CP" HeaderText="Charity Person Name" />
                                    <asp:BoundField DataField="IBAN" HeaderText="IBAN" />
                                    <asp:BoundField DataField="Cause" HeaderText="Cause" />
                                    <asp:BoundField DataField="NeededMoney" HeaderText="Needed Money" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-container">
                            <h3>Edit Charity Person Information</h3>
                            <div class="form-subcontainer">
                                <!-- Add -->
                                <div class="add-container">
                                    <div class="form-group">
                                        <label for="ddlAssociation" class="input-label">Select Association</label>
                                        <asp:DropDownList ID="ddlAssociation" runat="server" CssClass="input-field" />
                                        <label for="txtAddCnp" class="input-label">CNP</label>
                                        <asp:TextBox ID="txtAddCnp" runat="server" CssClass="input-field" />
                                        <label for="txtAddName" class="input-label">Charity Person Name</label>
                                        <asp:TextBox ID="txtAddName" runat="server" CssClass="input-field" />
                                        <label for="txtAddIban" class="input-label">IBAN</label>
                                        <asp:TextBox ID="txtAddIban" runat="server" CssClass="input-field" />
                                        <label for="txtAddCause" class="input-label">Cause</label>
                                        <asp:TextBox ID="txtAddCause" runat="server" CssClass="input-field" />
                                        <label for="txtAddNeededMoney" class="input-label">Needed Money</label>
                                        <asp:TextBox ID="txtAddNeededMoney" runat="server" CssClass="input-field" TextMode="Number" />
                                        <asp:Button ID="btnAddCharityPerson" runat="server" Text="Add" OnClick="AddCharityPerson" CssClass="button" />
                                    </div>
                                </div>

                                <!-- Edit/Delete -->
                                <div class="edit-delete-container">
                                    <asp:Label ID="labeEDCnp" runat="server" Visible="false"></asp:Label>
                                    <div class="form-group">
                                        <label for="txtEDName" class="input-label">Charity Person Name</label>
                                        <asp:TextBox ID="txtEDName" runat="server" CssClass="input-field" />
                                        <label for="txtEDIban" class="input-label">IBAN</label>
                                        <asp:TextBox ID="txtEDIban" runat="server" CssClass="input-field" />
                                        <label for="txtEDCause" class="input-label">Cause</label>
                                        <asp:TextBox ID="txtEDCause" runat="server" CssClass="input-field" />
                                        <label for="txtEDNeededMoney" class="input-label">Needed Money</label>
                                        <asp:TextBox ID="txtEDNeededMoney" runat="server" CssClass="input-field" TextMode="Number" />
                                    </div>
                                    <div class="button-container">
                                        <asp:Button ID="btnEditCharityPerson" runat="server" Text="Edit" OnClick="EditCharityPerson" CssClass="button" />
                                        <asp:Button ID="btnDeleteCharityPerson" runat="server" Text="Delete" OnClick="DeleteCharityPerson" CssClass="button" />
                                    </div>
                                </div>
                            </div>
                            <asp:Label ID="lblErrorCharityPerson" runat="server" CssClass="error-label" Visible="false"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
