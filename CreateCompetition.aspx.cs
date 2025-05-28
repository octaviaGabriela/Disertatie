using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views.Planner
{
    public partial class CreateCompetition : Page
    {
        private DBCon Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new DBCon();

            // afisare denumire planner logat in aplicatie
            if (!IsPostBack)
            {
                if (Session["App_User_Name"] != null)
                {
                    lblUserName.Text = "Hello, " + Session["App_User_Name"].ToString();
                }
                else
                {
                    lblUserName.Text = "Hello, Guest!";
                }

                LoadSports();
                LoadSponsors();
                LoadAssociations();
                LoadCompetitions();
                LoadParticipantTickets();
            }
        }

        // metoda de incarcare a Grid View-ului Competitions
        private void LoadCompetitions()
        {
            gvCompetitions.DataSource = Con.GetData(@"SELECT 
            COMPETITION.Name_CO AS Competition_Name,
            COMPETITION.TicketFee,
            COMPETITION.RaisedMoney_C,
            COMPETITION.Competition_Date,
            COMPETITION.Competition_Status,

            SPORT.Name_S AS Sport_Name,

            APP_USER.App_User_Name,

            SPONSOR.Name_S AS Sponsor_Name,
            SPONSOR.Money_S AS Sponsor_Contribution,

            DONATION.DonationMoney,

            CHARITY_PERSON.Name_CP AS Charity_Person_Name,
            CHARITY_PERSON.CNP_CP,
            CHARITY_PERSON.IBAN,
            CHARITY_PERSON.Cause,
            CHARITY_PERSON.NeededMoney,

            ASSOCIATION.Name_A AS Association_Name

            FROM COMPETITION


            JOIN IS_ASSOCIATED_TO ON COMPETITION.Competition_Id = IS_ASSOCIATED_TO.Competition_Id
            JOIN APP_USER ON IS_ASSOCIATED_TO.App_User_Id = APP_USER.App_User_Id


            JOIN SPORT ON COMPETITION.Sport_Id = SPORT.Sport_Id


            JOIN SPONSOR_OF ON COMPETITION.Competition_Id = SPONSOR_OF.Competition_Id
            JOIN SPONSOR ON SPONSOR_OF.Sponsor_Id = SPONSOR.Sponsor_Id


            JOIN DONATION ON COMPETITION.Competition_Id = DONATION.Competition_Id


            JOIN CHARITY_PERSON ON DONATION.CNP_CP = CHARITY_PERSON.CNP_CP


            JOIN ASSOCIATION ON CHARITY_PERSON.Association_Id = ASSOCIATION.Association_Id


            WHERE COMPETITION.Is_Active = 1 AND APP_USER.Has_Full_Rights = 1
            ORDER BY COMPETITION.Competition_Date ASC");

            gvCompetitions.DataBind();

        }

        // metode pentru incarcarea Drop Down List-urilor 
        private void LoadSports()
        {
            ddlSport.DataSource = Con.GetData("SELECT Sport_Id, Name_S FROM SPORT WHERE Is_Active = 1");
            ddlSport.DataTextField = "Name_S";
            ddlSport.DataValueField = "Sport_Id";
            ddlSport.DataBind();
        }

        private void LoadSponsors()
        {
            ddlSponsor.DataSource = Con.GetData("SELECT Sponsor_Id, Name_S FROM SPONSOR WHERE Is_Active = 1");
            ddlSponsor.DataTextField = "Name_S";
            ddlSponsor.DataValueField = "Sponsor_Id";
            ddlSponsor.DataBind();
        }

        private void LoadAssociations()
        {
            ddlAssociation.DataSource = Con.GetData("SELECT Association_Id, Name_A FROM ASSOCIATION WHERE Is_Active = 1");
            ddlAssociation.DataTextField = "Name_A";
            ddlAssociation.DataValueField = "Association_Id";
            ddlAssociation.DataBind();
        }

        // metoda pentru incarcarea Drop Down List-ului Charity Person in functie de selectia Asociatiei
        protected void DdlAssociation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query = "SELECT CNP_CP, Name_CP FROM CHARITY_PERSON WHERE Association_Id = @AssociationId AND Is_Active = 1";
            SqlParameter[] parameters = {
            new SqlParameter("@AssociationId", ddlAssociation.SelectedValue)
            };
            ddlCharityPerson.DataSource = Con.GetData(query, parameters);
            ddlCharityPerson.DataTextField = "Name_CP";
            ddlCharityPerson.DataValueField = "CNP_CP";
            ddlCharityPerson.DataBind();
        }






        // creare competitie
        protected void BtnCreateCompetition_Click(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtCompetitionName.Text) ||
                string.IsNullOrWhiteSpace(txtTicketFee.Text) ||
                string.IsNullOrEmpty(ddlSport.SelectedValue) ||
                string.IsNullOrEmpty(ddlSponsor.SelectedValue) ||
                string.IsNullOrEmpty(ddlAssociation.SelectedValue) ||
                string.IsNullOrEmpty(ddlCharityPerson.SelectedValue))
            {
                lblErrorCreateComp.Text = "Please fill in all the required fields.";
                lblErrorCreateComp.Visible = true;
                return;
            }



            // verificare adaugare competitie cu acelasi nume
            string competitionFound = "SELECT * FROM COMPETITION WHERE Name_CO = @name AND (Is_Active = 1 OR Is_Active = 0) AND (Competition_Status = 'upcoming' OR Competition_Status = 'ended')";
            SqlParameter[] competitionFoundParams = {
             new SqlParameter("@name", txtCompetitionName.Text.Trim())
            };
            DataTable dt = Con.GetData(competitionFound, competitionFoundParams);
            if (dt.Rows.Count > 0)
            {
                lblErrorCreateComp.Text = "A competition with this name already exists.";
                lblErrorCreateComp.Visible = true;
                return;
            }



            DateTime competitionDate;
            bool isValidDate = DateTime.TryParse(txtCompetitionDate.Text, out competitionDate);

            // verificare data valida
            if (!isValidDate)
            {
                lblErrorCreateComp.Text = "Please enter a valid date.";
                return;
            }

            // verificare data in viitor
            if (competitionDate <= DateTime.Now)
            {
                lblErrorCreateComp.Text = "The date must be in the future.";
                return;
            }


            string name = txtCompetitionName.Text;
            float ticketFee = float.Parse(txtTicketFee.Text);
            int sportId = int.Parse(ddlSport.SelectedValue);
            int sponsorId = int.Parse(ddlSponsor.SelectedValue);
            string cnp = ddlCharityPerson.SelectedValue;
            int isActive = 1;
            string status = "upcoming";

            // obtinere Sponsor Amount - (creare competitie cu RaiseMoney_C = Sponsor Amount)
            string sponsorAmountFound = "SELECT Money_S FROM SPONSOR WHERE Sponsor_Id = @sponsorId";
            SqlParameter[] sponsorAmountFoundParams = {
             new SqlParameter("@sponsorId", sponsorId)
            };
            DataTable dtsponsorAmount = Con.GetData(sponsorAmountFound, sponsorAmountFoundParams);
            float sponsorAmount = Convert.ToSingle(dtsponsorAmount.Rows[0]["Money_S"]);


            try
            {
                string insertCompetitionQuery = @"INSERT INTO COMPETITION 
                (Name_CO, TicketFee, RaisedMoney_C, Competition_Status, Is_Active, Sport_Id, Competition_Date)
                VALUES (@Name_CO, @TicketFee, @RaisedMoney_C, @Competition_Status, @Is_Active, @Sport_Id, @Competition_Date)";

                SqlParameter[] competitionParams = {
                 new SqlParameter("@Name_CO", name),
                 new SqlParameter("@TicketFee", ticketFee),
                 new SqlParameter("@RaisedMoney_C", sponsorAmount),
                 new SqlParameter("@Competition_Status", status),
                 new SqlParameter("@Is_Active", isActive),
                 new SqlParameter("@Sport_Id", sportId),
                 new SqlParameter("@Competition_Date", competitionDate)
                };

                // verificare inserare in competitie pentru recuperarea id-ului competitiei
                if (Con.SetData(insertCompetitionQuery, competitionParams) > 0)
                {
                    int compId = Con.GetLastInsertedId("COMPETITION", "Competition_Id");

                    string insertSponsorOfQuery = "INSERT INTO SPONSOR_OF (Sponsor_Id, Competition_Id) VALUES (@Sponsor_Id, @Competition_Id)";
                    SqlParameter[] sponsorOfParams = {
                     new SqlParameter("@Sponsor_Id", sponsorId),
                     new SqlParameter("@Competition_Id", compId)
                    };
                    Con.SetData(insertSponsorOfQuery, sponsorOfParams);

                    string insertDonationQuery = "INSERT INTO DONATION (DonationMoney, CNP_CP, Competition_Id) VALUES (0, @CNP_CP, @Competition_Id)";
                    SqlParameter[] donationParams = {
                     new SqlParameter("@CNP_CP", cnp),
                     new SqlParameter("@Competition_Id", compId)
                    };
                    Con.SetData(insertDonationQuery, donationParams);

                    string insertIsAssociatedToQuery = "INSERT INTO IS_ASSOCIATED_TO (App_User_Id, Competition_Id) VALUES (@App_User_Id, @Competition_Id)";
                    SqlParameter[] isAssociatedToParams = {
                     new SqlParameter("@App_User_Id", Session["App_User_Id"]),
                     new SqlParameter("@Competition_Id", compId)
                    };
                    Con.SetData(insertIsAssociatedToQuery, isAssociatedToParams);

                    LoadCompetitions();
                    lblErrorCreateComp.Text = "Competition created successfully.";
                    txtCompetitionName.Text = "";
                    txtTicketFee.Text = "";
                    txtCompetitionDate.Text = "";
                    ddlSport.SelectedIndex = -1;
                    ddlSponsor.SelectedIndex = -1;
                    ddlAssociation.SelectedIndex = -1;
                    ddlCharityPerson.Items.Clear();
                }
                else
                {
                    lblErrorCreateComp.Text = "Failed to insert competition.";
                }
            }
            catch (Exception ex)
            {
                lblErrorCreateComp.Text = "Error: " + ex.Message;
            }
        }



        // buton actualizeaza competitii cu statusul -ended- pt competitiile ce au date de inceput in trecut
        protected void BtnUpdateCompetitions_Click(object sender, EventArgs e)
        {
            try
            {
                string updateQuery = @"UPDATE COMPETITION
                SET Competition_Status = 'ended'
                WHERE Competition_Date < GETDATE() 
                AND Competition_Status = 'upcoming'";

                int rowsAffected = Con.SetData(updateQuery);

                if (rowsAffected > 0)
                {
                    lblErrorComp.Text = rowsAffected + " competition(s) marked as 'ended'.";
                }
                else
                {
                    lblErrorComp.Text = "No competitions were updated.";
                }

                LoadCompetitions();

            }
            catch (Exception ex)
            {
                lblErrorComp.Text = "Error: " + ex.Message;
            }
        }




        // metoda pentru butoanele Update si Delete din cadrul GvCompetitions
        protected void GvCompetitions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCompetitions.Rows[rowIndex];
            string name = row.Cells[1].Text;
            float raisedMoney = float.Parse(row.Cells[3].Text);

            string selectQuery = "SELECT Competition_Id FROM COMPETITION WHERE Name_CO = @Name_CO";
            SqlParameter[] selectParams = {
             new SqlParameter("@Name_CO", name)
            };
            DataTable dt = Con.GetData(selectQuery, selectParams);

            try
            {
                if (dt.Rows.Count > 0)
                {
                    int compId = Convert.ToInt32(dt.Rows[0]["Competition_Id"]);

                    if (e.CommandName == "UpdateStatus")
                    {
                        string updateQuery = "UPDATE DONATION SET DonationMoney = @RaisedMoney WHERE Competition_Id = @Competition_Id";
                        SqlParameter[] updateParams = {
                         new SqlParameter("@RaisedMoney", raisedMoney),
                         new SqlParameter("@Competition_Id", compId)
                        };

                        if (Con.SetData(updateQuery, updateParams) > 0)
                        {
                            lblErrorComp.Text = "Donation updated.";
                            LoadCompetitions();
                        }

                    }
                    else if (e.CommandName == "DeleteCompetition")
                    {
                        DateTime endDate = DateTime.Now;
                        string status = "ended";
                        string deleteQuery = "UPDATE COMPETITION SET Is_Active = 0, Competition_Date = @EndDate, Competition_Status = @Status WHERE Competition_Id = @Competition_Id";
                        SqlParameter[] deleteParams = {
                         new SqlParameter("@Competition_Id", compId),
                         new SqlParameter("@EndDate", endDate),
                         new SqlParameter("@Status", status)
                        };

                        if (Con.SetData(deleteQuery, deleteParams) > 0)
                        {
                            lblErrorComp.Text = "Competition deleted.";
                            LoadCompetitions();
                        }

                    }
                    else
                    {
                        lblErrorComp.Text = "Competition could not be found.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorComp.Text = "Error: " + ex.Message;
            }

        }



        // colorare randuri in GvCompetitions in functie de conditii
        protected void GvCompetitions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime competitionDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Competition_Date"));
                string status = DataBinder.Eval(e.Row.DataItem, "Competition_Status").ToString();
                float donationMoney = Convert.ToSingle(DataBinder.Eval(e.Row.DataItem, "DonationMoney"));

                if (competitionDate < DateTime.Now && status == "upcoming")
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
                else if (donationMoney == 0 && status == "ended")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }

            }

        }




        //metoda de incarcare a Grid View-ului Tickets
        private void LoadParticipantTickets()
        {
            gvTickets.DataSource = Con.GetData(@"SELECT 
            APP_USER.App_User_Name,
            COMPETITION.Name_CO AS Competition_Name,
            COMPETITION.Competition_Date,
	        COMPETITION.TicketFee
            FROM 
            APP_USER 
            JOIN IS_ASSOCIATED_TO ON APP_USER.App_User_Id = IS_ASSOCIATED_TO.App_User_Id
            JOIN COMPETITION ON IS_ASSOCIATED_TO.Competition_Id = COMPETITION.Competition_Id
            WHERE 
            APP_USER.Is_Active = 1
            AND APP_USER.Has_Full_Rights = 0
            AND COMPETITION.Is_Active = 1
            AND GETDATE() >= DATEADD(HOUR, -1, COMPETITION.Competition_Date)
            AND GETDATE() <= DATEADD(HOUR, 1, COMPETITION.Competition_Date)");

            gvTickets.DataBind();
        }


        //metoda de reincarcare a Grid View-ului Tickets
        protected void TmrAutoRefresh_Tick(object sender, EventArgs e)
        {
            LoadParticipantTickets();// reincarcare la fiecare 10 secunde
        }


        // metoda pentru butonul ShowTicket din cadrul GvTickets
        protected void GvTickets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ShowTicket")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTickets.Rows[index];

                string participantName = row.Cells[0].Text;
                string competitionName = row.Cells[1].Text;
                string date = row.Cells[2].Text;
                string fee = row.Cells[3].Text;


                // obtinere ID pt Competitie si Participant + stare ticket
                string idFound = @"SELECT
                APP_USER.App_User_Id AS ParticipantId, COMPETITION.Competition_Id AS CompetitionId, IS_ASSOCIATED_TO.Ticket_Status AS TStatus
                FROM APP_USER 
                JOIN IS_ASSOCIATED_TO ON APP_USER.App_User_Id = IS_ASSOCIATED_TO.App_User_Id
                JOIN COMPETITION ON IS_ASSOCIATED_TO.Competition_Id = COMPETITION.Competition_Id
                WHERE APP_USER.App_User_Name = @participantName AND COMPETITION.Name_CO = @competitionName";

                SqlParameter[] idParams = {
                 new SqlParameter("@participantName", participantName),
                 new SqlParameter("@competitionName", competitionName)
                };
                DataTable dt = Con.GetData(idFound, idParams);
                int participantID = Convert.ToInt32(dt.Rows[0]["ParticipantId"]);
                int competitionID = Convert.ToInt32(dt.Rows[0]["CompetitionId"]);
                string ticketStatus = (dt.Rows[0]["TStatus"]).ToString();

                //verificare stare ticket
                if (ticketStatus == "Ticket received")
                {
                    Button ShowTicket = (Button)row.FindControl("btnShowTicket");
                    ShowTicket.Visible = false;

                    Label lbl = (Label)row.FindControl("lblGVT");
                    lbl.Text = "The ticket has already been issued.";
                    lbl.Visible = true;
                }
                else
                {
                    //setare 'Ticket received' 
                    string updateQuery = @"UPDATE IS_ASSOCIATED_TO 
                    SET Ticket_Status = 'Ticket received' 
                    WHERE App_User_Id = @App_User_Id AND Competition_Id = @Competition_Id";

                    SqlParameter[] updateParams = {
                     new SqlParameter("@App_User_Id", participantID),
                     new SqlParameter("@Competition_Id", competitionID)
                    };
                    Con.SetData(updateQuery, updateParams);

                    Button ShowTicket = (Button)row.FindControl("btnShowTicket");
                    ShowTicket.Visible = false;

                    Label lbl = (Label)row.FindControl("lblGVT");
                    lbl.Text = "Ticket just issued.";
                    lbl.Visible = true;


                    // creare ticket
                    lblTicketParticipant.Text = participantName;
                    lblTicketCompetition.Text = competitionName;
                    lblTicketDate.Text = date;
                    lblTicketFee.Text = fee;

                    mvTicket.SetActiveView(vwTicket);
                }

            }

        }

        //buton inchide bilet
        protected void BtnCloseTicket_Click(object sender, EventArgs e)
        {
            // Ascunde biletul
            mvTicket.ActiveViewIndex = -1;
        }

        //buton manage info
        protected void BtnManage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Planner/ManageCompetitionInfo.aspx"); 
        }


        //buton logout
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/LoginWF.aspx");
        }



    }

}
