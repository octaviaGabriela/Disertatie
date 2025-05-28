using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views
{
    public partial class CreateNewAccountWF : Page
    {
        private DBCon Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new DBCon();
        }

        // buton creare cont nou
        protected void BtnCreateAccount_Click(object sender, EventArgs e)
        {
            string username = txtNewUsername.Text.Trim();
            string password = txtNewPassword.Text.Trim();
            bool isPlanner = chkIsPlanner.Checked;

            // verificare campuri goale
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Username and password cannot be empty.";
                lblError.Visible = true;
                return;
            }

            // verificare parola minim 6 caractere
            if (password.Length <= 6)
            {
                lblError.Text = "Password must be at least 6 characters long.";
                lblError.Visible = true;
                return;
            }

            // verificare cont deja existent
            string checkQuery = "SELECT * FROM APP_USER WHERE App_User_Name = @username";
            SqlParameter[] checkParams = {
                new SqlParameter("@username", username)
            };

            DataTable dt = Con.GetData(checkQuery, checkParams);

            if (dt.Rows.Count > 0)
            {
                lblError.Text = "Username already exists. Please choose a different one.";
                lblError.Visible = true;
                return;
            }

            try
            {
                int isPlannerValue;

                if (isPlanner)
                {
                    isPlannerValue = 1;
                }
                else
                {
                    isPlannerValue = 0;
                }

                int isActive = 1;
                string insertQuery = @"INSERT INTO APP_USER 
                (App_User_Name, App_User_Password, Has_Full_Rights, Is_Active) 
                VALUES (@username, @password, @rights, @active)";

                SqlParameter[] insertParams = {
                 new SqlParameter("@username", username),
                 new SqlParameter("@password", password),
                 new SqlParameter("@rights", isPlannerValue),
                 new SqlParameter("@active", isActive)
                };

                Con.SetData(insertQuery, insertParams);
                Response.Redirect("LoginWF.aspx");
            }
            catch (Exception ex)
            {
                lblError.Text = "Error: " + ex.Message;
                lblError.Visible = true;
            }
        }
    }
}
