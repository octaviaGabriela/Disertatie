using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CharityAppProject.Models;

namespace CharityAppProject.Views
{
	public partial class ForgotPassword : Page
	{
        private DBCon Con;

        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new DBCon();
        }


        // buton save
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string passwordOld = txtPassword.Text;
            string passwordNew = txtNewPassword.Text;

            // verificare campuri goale
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwordOld) || string.IsNullOrEmpty(passwordNew))
            {
                lblError.Text = "Fields cannot be empty";
                lblError.Visible = true;
                return;
            }

            // verificare parola minim 6 caractere
            if (passwordNew.Length <= 6)
            {
                lblError.Text = "New password must be at least 6 characters long.";
                lblError.Visible = true;
                return;
            }

            try
            {
                // obtinere detalii user pe baza username-ului si parolei
                string userFound = "SELECT App_User_Id FROM APP_USER WHERE App_User_Name = @Username AND App_User_Password = @Password AND Is_Active = 1";
                SqlParameter[] userParams = {
                 new SqlParameter("@Username", txtUsername.Text),
                 new SqlParameter("@Password", txtPassword.Text)
                };

                DataTable dt = Con.GetData(userFound, userParams);

                if (dt.Rows.Count > 0)
                {
                    int userId = Convert.ToInt32(dt.Rows[0]["App_User_Id"]);

                    //schimbare parola
                    string updateQuery = @"UPDATE APP_USER SET App_User_Password = @newPassword WHERE App_User_Id = @userId";

                    SqlParameter[] updateParams = {
                     new SqlParameter("@userId", userId),
                     new SqlParameter("@newPassword", passwordNew)
                    };

                    Con.SetData(updateQuery, updateParams);
                    Response.Redirect("LoginWF.aspx");
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
    }
}