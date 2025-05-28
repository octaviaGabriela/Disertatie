using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CharityAppProject
{
	public partial class StartPage : Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void BtnLogin1_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoginWF.aspx"); 
        }

    }
}
