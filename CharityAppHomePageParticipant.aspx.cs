using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views.Participant
{
    public partial class CharityAppHomePageParticipant : Page
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

                LoadNewCompetitions();
                LoadAssociatedCompetitions();
            }
        }

        // metoda de incarcare a Grid View-ului NewCompetitions
        private void LoadNewCompetitions()
        {
            gvNewCompetitions.DataSource = Con.GetData(@"SELECT 
            COMPETITION.Name_CO AS Competition_Name,
            COMPETITION.TicketFee,
            COMPETITION.Competition_Date,
            COMPETITION.Competition_Status,
            SPORT.Name_S AS Sport_Name,
            SPONSOR.Name_S AS Sponsor_Name,
            CHARITY_PERSON.Name_CP AS Charity_Person_Name,
            CHARITY_PERSON.Cause,
            CHARITY_PERSON.NeededMoney,
            ASSOCIATION.Name_A AS Association_Name

            FROM COMPETITION


            JOIN SPORT ON COMPETITION.Sport_Id = SPORT.Sport_Id


            JOIN SPONSOR_OF ON COMPETITION.Competition_Id = SPONSOR_OF.Competition_Id
            JOIN SPONSOR ON SPONSOR_OF.Sponsor_Id = SPONSOR.Sponsor_Id


            JOIN DONATION ON COMPETITION.Competition_Id = DONATION.Competition_Id


            JOIN CHARITY_PERSON ON DONATION.CNP_CP = CHARITY_PERSON.CNP_CP


            JOIN ASSOCIATION ON CHARITY_PERSON.Association_Id = ASSOCIATION.Association_Id


            WHERE COMPETITION.Is_Active = 1 AND CAST(COMPETITION.Competition_Date AS DATE) > CAST(GETDATE() AS DATE) AND COMPETITION.Competition_Status = 'upcoming'

            ORDER BY COMPETITION.Competition_Date ASC");

            gvNewCompetitions.DataBind();

        }

        //metoda pentru butonul Register GvNewCompetitions
        protected void GvNewCompetitions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvNewCompetitions.Rows[rowIndex];
            string name = row.Cells[1].Text;

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

                    if (e.CommandName == "RegisterToCompetition")
                    {
                        string checkQuery = "SELECT * FROM IS_ASSOCIATED_TO WHERE App_User_Id = @App_User_Id AND Competition_Id = @Competition_Id";
                        SqlParameter[] checkParams = {
                         new SqlParameter("@App_User_Id", Session["App_User_Id"]),
                         new SqlParameter("@Competition_Id", compId)
                        };
                        DataTable check = Con.GetData(checkQuery, checkParams);

                        if (check.Rows.Count > 0)
                        {
                            lblErrorNew.Text = "You are already registered for this competition.";
                            lblErrorNew.Visible = true;
                        }
                        else
                        { 
                            string insertQuery = "INSERT INTO IS_ASSOCIATED_TO (App_User_Id, Competition_Id) VALUES (@App_User_Id, @Competition_Id)";
                            SqlParameter[] insertParams = {
                             new SqlParameter("@App_User_Id", Session["App_User_Id"]),
                             new SqlParameter("@Competition_Id", compId)
                            };


                            if (Con.SetData(insertQuery, insertParams) > 0)
                            {
                                lblErrorAssociated.Visible = false;

                                lblErrorNew.Text = "You have registered for a new competition. The competition has been added to the My Competitions table below.";
                                lblErrorNew.Visible = true;
                                LoadAssociatedCompetitions();

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorNew.Text = "Error: " + ex.Message;
                lblErrorNew.Visible = true;
            }

        }


        // metoda pt logica afisarii buton Register / label Too late
        protected void GvNewCompetitions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime competitionDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Competition_Date"));
                DateTime cutoffDate = competitionDate.AddDays(-1);

                Button btnRegister = (Button)e.Row.FindControl("btnRegisterToCompetition");
                Label lbl = (Label)e.Row.FindControl("lblGVN");

                if (DateTime.Now > cutoffDate)
                {
                    // prea tarziu pentru inregistrare
                    btnRegister.Visible = false;
                    lbl.Visible = true;
                }
            }
        }







        // metoda de incarcare a Grid View-ului AssociatedCompetitions
        private void LoadAssociatedCompetitions()
        {
            string associatedCompetitionsQuery = @"SELECT 
            COMPETITION.Name_CO AS Competition_Name,
            COMPETITION.TicketFee,
            COMPETITION.RaisedMoney_C,
            COMPETITION.Competition_Date,
            COMPETITION.Competition_Status,
            SPORT.Name_S AS Sport_Name,
            DONATION.DonationMoney,
            CHARITY_PERSON.Name_CP AS Charity_Person_Name,
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


            WHERE COMPETITION.Is_Active = 1 AND APP_USER.App_User_Id = @App_User_Id

            ORDER BY COMPETITION.Competition_Date ASC";

            SqlParameter[] associatedCompetitionsParams = {
             new SqlParameter("@App_User_Id", Session["App_User_Id"])
            };
            DataTable dt = Con.GetData(associatedCompetitionsQuery, associatedCompetitionsParams);
            gvAssociatedCompetitions.DataSource = dt;
            gvAssociatedCompetitions.DataBind();
        }

        // metoda pentru butonul Withdraw GvAssociatedCompetitions
        protected void GvAssociatedCompetitions_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAssociatedCompetitions.Rows[rowIndex];
            string name = row.Cells[1].Text;

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

                    if (e.CommandName == "WithdrawFromCompetition")
                    {
                        string deleteQuery = "DELETE FROM IS_ASSOCIATED_TO WHERE App_User_Id = @App_User_Id AND Competition_Id = @Competition_Id";
                        SqlParameter[] deleteParams = {
                             new SqlParameter("@App_User_Id", Session["App_User_Id"]),
                             new SqlParameter("@Competition_Id", compId)
                        };

                        if (Con.SetData(deleteQuery, deleteParams) > 0)
                        {
                            lblErrorNew.Visible = false;

                            lblErrorAssociated.Text = "You have withdrawn from a competition.";
                            lblErrorAssociated.Visible = true;
                            LoadAssociatedCompetitions();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblErrorAssociated.Text = "Error: " + ex.Message;
                lblErrorAssociated.Visible = true;
            }
        }




        // metoda pt logica afisarii buton Withdrawn / label Thank you for participating / label It is too late to withdraw
        protected void GvAssociatedCompetitions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DateTime competitionDate = Convert.ToDateTime(DataBinder.Eval(e.Row.DataItem, "Competition_Date"));
                DateTime cutoffDate = competitionDate.AddDays(-1);

                Button btnWithdrawn = (Button)e.Row.FindControl("btnWithdrawFromCompetition");
                Label lbl = (Label)e.Row.FindControl("lblGVA");

                if (DateTime.Now > competitionDate)
                {
                    // competitia s-a incheiat
                    btnWithdrawn.Visible = false;
                    lbl.Text = "Thank you for participating.";
                    lbl.Visible = true;
                }
                else if (DateTime.Now > cutoffDate)
                {
                    // prea tarziu pentru retragere
                    btnWithdrawn.Visible = false;
                    lbl.Text = "It is too late to withdraw.";
                    lbl.Visible = true;
                }
            }
        }

        //buton delogare
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/LoginWF.aspx");

        }
    }
}