using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views.Planner
{
    public partial class ManageCompetitionInfo : Page
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
                LoadCharityPeople();
                LoadAssociationsSelect();
            }
        }


        // metode de incarcare a Grid View-urilor prezente in pagina
        private void LoadSports()
        {
            gvSport.DataSource = Con.GetData(@"SELECT DISTINCT S.Sport_Id, S.Name_S FROM SPORT S
            WHERE S.Is_Active = 1 AND NOT EXISTS 
            (SELECT *
            FROM COMPETITION C
            WHERE C.Sport_Id = S.Sport_Id
            AND C.Is_Active = 1
            AND C.Competition_Date >= CAST(GETDATE() AS DATE))");
            gvSport.DataBind();
        }

        private void LoadSponsors()
        {
            gvSponsor.DataSource = Con.GetData(@"SELECT * FROM SPONSOR S
            WHERE S.Is_Active = 1 AND NOT EXISTS 
            (SELECT *
            FROM SPONSOR_OF SO
            JOIN COMPETITION C ON SO.Competition_Id = C.Competition_Id
            WHERE SO.Sponsor_Id = S.Sponsor_Id
            AND C.Is_Active = 1
            AND C.Competition_Date >= CAST(GETDATE() AS DATE))");
            gvSponsor.DataBind();
        }

        private void LoadAssociations()
        {
            gvAssociation.DataSource = Con.GetData(@"SELECT * FROM ASSOCIATION A
            WHERE A.Is_Active = 1 AND NOT EXISTS 
            (SELECT *
            FROM CHARITY_PERSON CP
            JOIN DONATION D ON CP.CNP_CP = D.CNP_CP
            JOIN COMPETITION C ON D.Competition_Id = C.Competition_Id
            WHERE CP.Association_Id = A.Association_Id
            AND CP.Is_Active = 1
            AND C.Is_Active = 1
            AND C.Competition_Date >= CAST(GETDATE() AS DATE))");
            gvAssociation.DataBind();
        }

        private void LoadCharityPeople()
        {
            gvCharityPerson.DataSource = Con.GetData(@"SELECT * FROM CHARITY_PERSON CP
            WHERE CP.Is_Active = 1 AND NOT EXISTS 
            (SELECT *
            FROM DONATION D
            JOIN COMPETITION C ON D.Competition_Id = C.Competition_Id
            WHERE D.CNP_CP = CP.CNP_CP
            AND C.Is_Active = 1
            AND C.Competition_Date >= CAST(GETDATE() AS DATE))");
            gvCharityPerson.DataBind();
        }

        // metoda pentru incarcarea Drop Down List-ului Asociatiilor pentru Add Charity Person
        private void LoadAssociationsSelect()
        {
            ddlAssociation.DataSource = Con.GetData("SELECT Association_Id, Name_A FROM ASSOCIATION WHERE Is_Active = 1");
            ddlAssociation.DataTextField = "Name_A";
            ddlAssociation.DataValueField = "Association_Id";
            ddlAssociation.DataBind();
        }





        // ------------ SPORT ----------------------------------------------------

        // metoda de obtinere si afisare a datelor din randul selectat din GvSport
        protected void GvSport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSport.Rows[rowIndex];
            string sportId = row.Cells[1].Text;
            string sportName = row.Cells[2].Text;

            if (e.CommandName == "Select")
            {
                labelSportId.Text = sportId;
                txtSportName.Text = sportName;
            }
        }

        // metode pentru butoanele Add, Edit si Delete pentru Sport
        protected void AddSport(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSportName.Text))
            {
                lblErrorSport.Text = "Please fill in all the required fields.";
                lblErrorSport.Visible = true;
                return;
            }

            string sportFound = "SELECT * FROM SPORT WHERE Name_S = @name AND Is_Active = 1";
            SqlParameter[] sportParams = {
             new SqlParameter("@name", txtSportName.Text)
            };
            DataTable dt = Con.GetData(sportFound, sportParams);

            if (dt.Rows.Count > 0)
            {
                lblErrorSport.Text = "A sport with this name already exists.";
                lblErrorSport.Visible = true;
                return;
            }

            try
            {
                int isActive = 1;
                string insertQuery = "INSERT INTO SPORT (Name_S, Is_active) VALUES (@name, @isActive)";
                SqlParameter[] parameters = {
                 new SqlParameter("@name", txtSportName.Text),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(insertQuery, parameters);

                LoadSports();
                lblErrorSport.Visible = false;
                labelSportId.Text = "";
                txtSportName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSport.Text = "Error while adding the sport: " + ex.Message;
                lblErrorSport.Visible = true;
            }

        }


        protected void EditSport(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSportName.Text))
            {
                lblErrorSport.Text = "Please fill in all the required fields.";
                lblErrorSport.Visible = true;
                return;
            }

            // verificare rand selectat din GvSport sau camp completat manual
            if (string.IsNullOrEmpty(labelSportId.Text))
            {
                lblErrorSport.Text = "Please select a valid row from the table before performing this action.";
                lblErrorSport.Visible = true;
                return;
            }

            string sportFound = "SELECT * FROM SPORT WHERE Name_S = @name AND Is_Active = 1";
            SqlParameter[] sportParams = {
             new SqlParameter("@name", txtSportName.Text)
            };
            DataTable dt = Con.GetData(sportFound, sportParams);

            if (dt.Rows.Count > 0)
            {
                lblErrorSport.Text = "A sport with this name already exists.";
                lblErrorSport.Visible = true;
                return;
            }
            
            try
            {
                string updateQuery = "UPDATE SPORT SET Name_S = @name WHERE Sport_Id = @sportId";
                SqlParameter[] parameters = {
                 new SqlParameter("@name", txtSportName.Text),
                 new SqlParameter("@sportId", int.Parse(labelSportId.Text))
                };
                Con.SetData(updateQuery, parameters);

                LoadSports();
                lblErrorSport.Visible = false;
                labelSportId.Text = "";
                txtSportName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSport.Text = "Error while updating the sport: " + ex.Message;
                lblErrorSport.Visible = true;
            }
        }


        protected void DeleteSport(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSportName.Text))
            {
                lblErrorSport.Text = "Please fill in all the required fields.";
                lblErrorSport.Visible = true;
                return;
            }

            // verificare rand selectat din GvSport sau camp completat manual
            if (string.IsNullOrEmpty(labelSportId.Text))
            {
                lblErrorSport.Text = "Please select a valid row from the table before performing this action.";
                lblErrorSport.Visible = true;
                return;
            }

            
            try
            {
                int isActive = 0;
                string deleteQuery = "UPDATE SPORT SET Is_Active = @isActive WHERE Sport_Id = @sportId";
                SqlParameter[] parameters = {
                 new SqlParameter("@sportId", int.Parse(labelSportId.Text)),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(deleteQuery, parameters);

                LoadSports();
                lblErrorSport.Visible = false;
                labelSportId.Text = "";
                txtSportName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSport.Text = "Error while deleting the sport: " + ex.Message;
                lblErrorSport.Visible = true;
            }

        }



        // ------------ SPONSOR ----------------------------------------------------

        // metoda de obtinere si afisare a datelor din randul selectat din GvSponsor
        protected void GvSponsor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSponsor.Rows[rowIndex];
            string sponsorId = row.Cells[1].Text;
            string sponsorName = row.Cells[2].Text;
            string sponsorAmount = row.Cells[3].Text;

            if (e.CommandName == "Select")
            {
                labelSponsorId.Text = sponsorId;
                txtSponsorName.Text = sponsorName;
                txtSponsorAmount.Text = sponsorAmount;
            }
        }

        // metode pentru butoanele Add, Edit si Delete pentru Sponsor
        protected void AddSponsor(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSponsorName.Text) || string.IsNullOrWhiteSpace(txtSponsorAmount.Text))
            {
                lblErrorSponsor.Text = "Please fill in all the required fields.";
                lblErrorSponsor.Visible = true;
                return;
            }

            string sponsorFound = "SELECT * FROM SPONSOR WHERE Name_S = @name AND Is_Active = 1";
            SqlParameter[] sponsorParams = {
             new SqlParameter("@name", txtSponsorName.Text)
            };
            DataTable dt = Con.GetData(sponsorFound, sponsorParams);

            if (dt.Rows.Count > 0)
            {
                lblErrorSponsor.Text = "A sponsor with this name already exists.";
                lblErrorSponsor.Visible = true;
                return;
            }

            try
            {
                int isActive = 1;
                string insertQuery = "INSERT INTO SPONSOR (Name_S, Money_S, Is_Active) VALUES (@name, @amount, @isActive)";
                SqlParameter[] parameters = {
                 new SqlParameter("@name", txtSponsorName.Text),
                 new SqlParameter("@amount", float.Parse(txtSponsorAmount.Text)),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(insertQuery, parameters);

                LoadSponsors();
                lblErrorSponsor.Visible = false;
                labelSponsorId.Text = "";
                txtSponsorName.Text = "";
                txtSponsorAmount.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSponsor.Text = "Error while adding the sponsor: " + ex.Message;
                lblErrorSponsor.Visible = true;
            }

        }

        protected void EditSponsor(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSponsorName.Text) || string.IsNullOrWhiteSpace(txtSponsorAmount.Text))
            {
                lblErrorSponsor.Text = "Please fill in all the required fields.";
                lblErrorSponsor.Visible = true;
                return;
            }

            // verificare rand selectat din GvSponsor sau camp completat manual
            if (string.IsNullOrEmpty(labelSponsorId.Text))
            {
                lblErrorSponsor.Text = "Please select a valid row from the table before performing this action.";
                lblErrorSponsor.Visible = true;
                return;
            }

            try
            {
                string updateQuery = "UPDATE SPONSOR SET Name_S = @name, Money_S = @amount WHERE Sponsor_Id = @sponsorId";
                SqlParameter[] parameters = {
                 new SqlParameter("@sponsorId", int.Parse(labelSponsorId.Text)),
                 new SqlParameter("@name", txtSponsorName.Text),
                 new SqlParameter("@amount", float.Parse(txtSponsorAmount.Text))
                };
                Con.SetData(updateQuery, parameters);

                LoadSponsors();
                lblErrorSponsor.Visible = false;
                labelSponsorId.Text = "";
                txtSponsorName.Text = "";
                txtSponsorAmount.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSponsor.Text = "Error while updating the sponsor: " + ex.Message;
                lblErrorSponsor.Visible = true;
            }

        }

        protected void DeleteSponsor(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtSponsorName.Text) || string.IsNullOrWhiteSpace(txtSponsorAmount.Text))
            {
                lblErrorSponsor.Text = "Please fill in all the required fields.";
                lblErrorSponsor.Visible = true;
                return;
            }

            // verificare rand selectat din GvSponsor sau camp completat manual
            if (string.IsNullOrEmpty(labelSponsorId.Text))
            {
                lblErrorSponsor.Text = "Please select a valid row from the table before performing this action.";
                lblErrorSponsor.Visible = true;
                return;
            }

            
            try
            {
                int isActive = 0;
                string deleteQuery = "UPDATE SPONSOR SET Is_Active = @isActive WHERE Sponsor_Id = @sponsorId";
                SqlParameter[] parameters = {
                 new SqlParameter("@sponsorId", int.Parse(labelSponsorId.Text)),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(deleteQuery, parameters);

                LoadSponsors();
                lblErrorSponsor.Visible = false;
                labelSponsorId.Text = "";
                txtSponsorName.Text = "";
                txtSponsorAmount.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorSponsor.Text = "Error while deleting the sponsor: " + ex.Message;
                lblErrorSponsor.Visible = true;
            }

        }



        // ------------ ASSOCIATION ----------------------------------------------------

        // metoda de obtinere si afisare a datelor din randul selectat din GvAssociation
        protected void GvAssociation_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvAssociation.Rows[rowIndex];
            string associationId = row.Cells[1].Text;
            string associationName = row.Cells[2].Text;

            if (e.CommandName == "Select")
            {
                labelAssociationId.Text = associationId;
                txtAssociationName.Text = associationName;
            }
        }

        // metode pentru butoanele Add, Edit si Delete pentru Association
        protected void AddAssociation(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtAssociationName.Text))
            {
                lblErrorAssociation.Text = "Please fill in all the required fields.";
                lblErrorAssociation.Visible = true;
                return;
            }

            string associationFound = "SELECT * FROM ASSOCIATION WHERE Name_A = @name AND Is_Active = 1";
            SqlParameter[] associationParams = {
             new SqlParameter("@name", txtAssociationName.Text)
            };
            DataTable dt = Con.GetData(associationFound, associationParams);

            if (dt.Rows.Count > 0)
            {
                lblErrorAssociation.Text = "An association with this name already exists.";
                lblErrorAssociation.Visible = true;
                return;
            }

            try
            {
                int isActive = 1;
                string insertQuery = "INSERT INTO ASSOCIATION (Name_A, Is_Active) VALUES (@name, @isActive)";
                SqlParameter[] parameters = {
                 new SqlParameter("@name", txtAssociationName.Text),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(insertQuery, parameters);

                LoadAssociations();
                LoadAssociationsSelect();
                lblErrorAssociation.Visible = false;
                labelAssociationId.Text = "";
                txtAssociationName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorAssociation.Text = "Error while adding the association: " + ex.Message;
                lblErrorAssociation.Visible = true;
            }

        }

        protected void EditAssociation(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtAssociationName.Text))
            {
                lblErrorAssociation.Text = "Please fill in all the required fields.";
                lblErrorAssociation.Visible = true;
                return;
            }

            // verificare rand selectat din GvAssociation sau camp completat manual
            if (string.IsNullOrEmpty(labelAssociationId.Text))
            {
                lblErrorAssociation.Text = "Please select a valid row from the table before performing this action.";
                lblErrorAssociation.Visible = true;
                return;
            }

            string associationFound = "SELECT * FROM ASSOCIATION WHERE Name_A = @name AND Is_Active = 1";
            SqlParameter[] associationParams = {
             new SqlParameter("@name", txtAssociationName.Text)
            };
            DataTable dt = Con.GetData(associationFound, associationParams);

            if (dt.Rows.Count > 0)
            {
                lblErrorAssociation.Text = "An association with this name already exists.";
                lblErrorAssociation.Visible = true;
                return;
            }

            try
            {
                string updateQuery = "UPDATE ASSOCIATION SET Name_A = @name WHERE Association_Id = @associationId";
                SqlParameter[] parameters = {
                 new SqlParameter("@associationId", int.Parse(labelAssociationId.Text)),
                 new SqlParameter("@name", txtAssociationName.Text)
                };
                Con.SetData(updateQuery, parameters);

                LoadAssociations();
                LoadAssociationsSelect();
                lblErrorAssociation.Visible = false;
                labelAssociationId.Text = "";
                txtAssociationName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorAssociation.Text = "Error while updating the association: " + ex.Message;
                lblErrorAssociation.Visible = true;
            }

        }

        protected void DeleteAssociation(object sender, EventArgs e)
        {
            // verificare campuri goale
            if (string.IsNullOrWhiteSpace(txtAssociationName.Text))
            {
                lblErrorAssociation.Text = "Please fill in all the required fields.";
                lblErrorAssociation.Visible = true;
                return;
            }

            // verificare rand selectat din GvAssociation sau camp completat manual
            if (string.IsNullOrEmpty(labelAssociationId.Text))
            {
                lblErrorAssociation.Text = "Please select a valid row from the table before performing this action.";
                lblErrorAssociation.Visible = true;
                return;
            }


            string associationFound2 = @"SELECT * FROM CHARITY_PERSON
            WHERE Association_Id = @associationId AND Is_Active = 1";
            
            SqlParameter[] associationParams2 = {
              new SqlParameter("@associationId", int.Parse(labelAssociationId.Text))
            };
            DataTable dt2 = Con.GetData(associationFound2, associationParams2);

            if (dt2.Rows.Count > 0)
            {
                lblErrorAssociation.Text = "Cannot delete: This association has an active charity person.";
                lblErrorAssociation.Visible = true;
                return;
            }



            try
            {
                int isActive = 0;
                string deleteQuery = "UPDATE ASSOCIATION SET Is_Active = @isActive WHERE Association_Id = @associationId";
                SqlParameter[] parameters = {
                 new SqlParameter("@associationId", int.Parse(labelAssociationId.Text)),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(deleteQuery, parameters);

                LoadAssociations();
                LoadAssociationsSelect();
                lblErrorAssociation.Visible = false;
                labelAssociationId.Text = "";
                txtAssociationName.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorAssociation.Text = "Error while deleting the association: " + ex.Message;
                lblErrorAssociation.Visible = true;
            }

        }




        // ------------ CHARITY PERSON ---------------------------------------------------

        // metoda de obtinere si afisare a datelor din randul selectat din GvCharityPerson
        protected void GvCharityPerson_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCharityPerson.Rows[rowIndex];
            string charityPersonCnp = row.Cells[1].Text;
            string charityPersonName = row.Cells[2].Text;
            string charityPersonIban = row.Cells[3].Text;
            string charityPersonCause = row.Cells[4].Text;
            string charityPersonNeededMoney = row.Cells[5].Text;

            if (e.CommandName == "Select")
            {
                labeEDCnp.Text = charityPersonCnp;
                txtEDName.Text = charityPersonName;
                txtEDIban.Text = charityPersonIban;
                txtEDCause.Text = charityPersonCause;
                txtEDNeededMoney.Text = charityPersonNeededMoney;
            }
        }

        // metode pentru butoanele Add, Edit si Delete pentru CharityPerson
        protected void AddCharityPerson(object sender, EventArgs e)
        {
            // verificare campuri goale 
            if (string.IsNullOrWhiteSpace(txtAddCnp.Text) ||
                string.IsNullOrWhiteSpace(txtAddName.Text) ||
                string.IsNullOrWhiteSpace(txtAddIban.Text) ||
                string.IsNullOrWhiteSpace(txtAddCause.Text) ||
                string.IsNullOrWhiteSpace(txtAddNeededMoney.Text))
            {
                lblErrorCharityPerson.Text = "Please fill in all the required fields.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            // verificare CNP - 13 caractere
            if (txtAddCnp.Text.Length != 13)
            {
                lblErrorCharityPerson.Text = "CNP must be exactly 13 characters long.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            // verificare IBAN - 22 caractere
            if (txtAddIban.Text.Length != 22)
            {
                lblErrorCharityPerson.Text = "IBAN must be exactly 22 characters long.";
                lblErrorCharityPerson.Visible = true;
                return;
            }



            string charityPersonFound1 = "SELECT * FROM CHARITY_PERSON WHERE CNP_CP = @cnp AND Is_Active = 1";
            SqlParameter[] charityPersonParams1 = {
             new SqlParameter("@cnp", txtAddCnp.Text)
            };
            DataTable dt1 = Con.GetData(charityPersonFound1, charityPersonParams1);

            if (dt1.Rows.Count > 0)
            {
                lblErrorCharityPerson.Text = "A charity person with this CNP already exists.";
                lblErrorCharityPerson.Visible = true;
                return;
            }



            string charityPersonFound2 = "SELECT * FROM CHARITY_PERSON WHERE Name_CP = @name AND Is_Active = 1";
            SqlParameter[] charityPersonParams2 = {
             new SqlParameter("@name", txtAddName.Text)
            };
            DataTable dt2 = Con.GetData(charityPersonFound2, charityPersonParams2);

            if (dt2.Rows.Count > 0)
            {
                lblErrorCharityPerson.Text = "A charity person with this name already exists.";
                lblErrorCharityPerson.Visible = true;
                return;
            }



            try
            {
                int isActive = 1;
                string insertQuery = @"INSERT INTO CHARITY_PERSON 
                (CNP_CP, Name_CP, IBAN, Cause, NeededMoney, Association_Id, Is_Active) 
                VALUES (@cnp, @name, @iban, @cause, @neededMoney, @associationId, @isActive)";
                SqlParameter[] parameters = {
                 new SqlParameter("@cnp", txtAddCnp.Text),
                 new SqlParameter("@name", txtAddName.Text),
                 new SqlParameter("@iban", txtAddIban.Text),
                 new SqlParameter("@cause", txtAddCause.Text),
                 new SqlParameter("@neededMoney", float.Parse(txtAddNeededMoney.Text)),
                 new SqlParameter("@associationId", int.Parse(ddlAssociation.SelectedValue)),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(insertQuery, parameters);

                LoadCharityPeople();
                lblErrorCharityPerson.Visible = false;
                txtAddCnp.Text = "";
                txtAddName.Text = "";
                txtAddIban.Text = "";
                txtAddCause.Text = "";
                txtAddNeededMoney.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorCharityPerson.Text = "An error occurred while adding the charity person: " + ex.Message;
                lblErrorCharityPerson.Visible = true;
            }

        }

        protected void EditCharityPerson(object sender, EventArgs e)
        {
            // verificare campuri goale 
            if (string.IsNullOrWhiteSpace(txtEDName.Text) ||
                string.IsNullOrWhiteSpace(txtEDIban.Text) ||
                string.IsNullOrWhiteSpace(txtEDCause.Text) ||
                string.IsNullOrWhiteSpace(txtEDNeededMoney.Text))
            {
                lblErrorCharityPerson.Text = "Please fill in all the required fields.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            // verificare IBAN - 22 caractere
            if (txtEDIban.Text.Length != 22)
            {
                lblErrorCharityPerson.Text = "IBAN must be exactly 22 characters long.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            // verificare rand selectat din GvCharityPerson sau camp completat manual
            if (string.IsNullOrEmpty(labeEDCnp.Text))
            {
                lblErrorCharityPerson.Text = "Please select a valid row from the table before performing this action.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            try
            {
                string updateQuery = @"UPDATE CHARITY_PERSON 
                SET Name_CP = @name, IBAN = @iban, Cause = @cause, NeededMoney = @neededMoney WHERE CNP_CP = @cnp";
                SqlParameter[] parameters = {
                 new SqlParameter("@cnp", labeEDCnp.Text),
                 new SqlParameter("@name", txtEDName.Text),
                 new SqlParameter("@iban", txtEDIban.Text),
                 new SqlParameter("@cause", txtEDCause.Text),
                 new SqlParameter("@neededMoney", float.Parse(txtEDNeededMoney.Text))
                };
                Con.SetData(updateQuery, parameters);

                LoadCharityPeople();
                lblErrorCharityPerson.Visible = false;
                labeEDCnp.Text = "";
                txtEDName.Text = "";
                txtEDIban.Text = "";
                txtEDCause.Text = "";
                txtEDNeededMoney.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorCharityPerson.Text = "An error occurred while updating the charity person: " + ex.Message;
                lblErrorCharityPerson.Visible = true;
            }

        }


        protected void DeleteCharityPerson(object sender, EventArgs e)
        {
            // verificare campuri goale 
            if (string.IsNullOrWhiteSpace(txtEDName.Text) ||
                string.IsNullOrWhiteSpace(txtEDIban.Text) ||
                string.IsNullOrWhiteSpace(txtEDCause.Text) ||
                string.IsNullOrWhiteSpace(txtEDNeededMoney.Text))
            {
                lblErrorCharityPerson.Text = "Please fill in all the required fields.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            // verificare rand selectat din GvCharityPerson sau camp completat manual
            if (string.IsNullOrEmpty(labeEDCnp.Text))
            {
                lblErrorCharityPerson.Text = "Please select a valid row from the table before performing this action.";
                lblErrorCharityPerson.Visible = true;
                return;
            }

            
            try
            {
                int isActive = 0;
                string deleteQuery = "UPDATE CHARITY_PERSON SET Is_Active = @isActive WHERE CNP_CP = @cnp";
                SqlParameter[] parameters = {
                 new SqlParameter("@cnp", labeEDCnp.Text),
                 new SqlParameter("@isActive", isActive)
                };
                Con.SetData(deleteQuery, parameters);

                LoadCharityPeople();
                lblErrorCharityPerson.Visible = false;
                labeEDCnp.Text = "";
                txtEDName.Text = "";
                txtEDIban.Text = "";
                txtEDCause.Text = "";
                txtEDNeededMoney.Text = "";
            }
            catch (Exception ex)
            {
                lblErrorCharityPerson.Text = "An error occurred while deleting the charity person: " + ex.Message;
                lblErrorCharityPerson.Visible = true;
            }

        }


        //buton manage info
        protected void BtnManage_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Planner/CreateCompetition.aspx"); 
        }


        //buton logout
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Views/LoginWF.aspx");
        }

    }

}
