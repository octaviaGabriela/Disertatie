using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views
{
    public partial class LoginWF : Page
    {
        private DBCon Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new DBCon();
        }


        // buton login
        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // verificare campuri goale
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Username and password cannot be empty.";
                lblError.Visible = true;
                return;
            }

            try
            {
                // obtinere detalii user pe baza username-ului si parolei
                string userFound = "SELECT App_User_Id, App_User_Name, Has_Full_Rights FROM APP_USER WHERE App_User_Name = @Username AND App_User_Password = @Password AND Is_Active = 1";
                SqlParameter[] userParams = {
                 new SqlParameter("@Username", txtUsername.Text),
                 new SqlParameter("@Password", txtPassword.Text)
                };

                DataTable dt = Con.GetData(userFound, userParams);

                if (dt.Rows.Count > 0) 
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["App_User_Id"]);
                    int rights = Convert.ToInt32(dt.Rows[0]["Has_Full_Rights"]);
                    string userName = dt.Rows[0]["App_User_Name"].ToString();

                    // salvam in sesiunea curenta
                    Session["App_User_Id"] = userId;
                    Session["App_User_Name"] = userName;


                    // redirectionare conform privilegiilor
                    if (rights == 1)
                    {
                        Response.Redirect("Planner/CreateCompetition.aspx");
                    }
                    else
                    {
                        Response.Redirect("Participant/CharityAppHomePageParticipant.aspx");
                    }
                }
                else
                {
                    lblError.Text = "Invalid username or password.";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }



        // buton creare cont nou
        protected void BtnCreateAccount_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateNewAccountWF.aspx");
        }

        // buton schimba parola
        protected void BtnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }
    }
}
