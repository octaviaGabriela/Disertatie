<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StartPage.aspx.cs" Inherits="CharityAppProject.StartPage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charity Through Sports</title>
    <link rel="stylesheet" type="text/css" href="StartPageStyles.css" />
</head>
<body>
    <form id="startPageForm" runat="server">
        <section class="container">

            <header>
                <img src="../Assets/logo.png" alt="Charity Logo" class="logo" />
                <h1>Transform Lives Through Sports</h1>
                <p class="subtitle">Your passion for sports can change the world.</p>
            </header>

            <section class="join-compete-donate">
                <h2>Join. Compete. Donate.</h2>
                <p>Every race, every match, every step you take helps someone in need. Sign up, stay active, and make a real difference!</p>
                <asp:Button ID="btnLogin1" runat="server" CssClass="btn-1" Text="Login & Get Started" OnClick="BtnLogin1_Click" />
            </section>

            <section class="info">
                <h2>Why Should You Join?</h2>
                <p>Participating in a charity sports competition benefits both you and those in need. While you push your limits and improve your health, you are also donating to help individuals facing hardship.</p>
            </section>

            <section class="impact">
                <h2>Your Impact</h2>
                <div class="impact-box3">
                    <div class="impact-box">
                        <h3>🏃‍♂️ Stay Active</h3>
                        <p>Regular exercise reduces stress and keeps you fit. Join a competition that keeps you moving!</p>
                    </div>
                    <div class="impact-box">
                        <h3>💖 Help Others</h3>
                        <p>Your registration fee directly funds essential aid for those in need.</p>
                    </div>
                    <div class="impact-box">
                        <h3>🌍 Be Part of Change</h3>
                        <p>Every participant contributes to a larger movement of kindness and generosity.</p>
                    </div>
                </div>
            </section>

            <section class="how-it-works">
                <h2>How It Works</h2>
                <ul>
                    <li>Sign up for a competition with a minimum donation.</li>
                    <li>Train, compete, and challenge yourself.</li>
                    <li>See your contribution make a real difference.</li>
                </ul>
            </section>

            <section class="did-you-know">
                <h2>Did You Know?</h2>
                <p>Studies show that helping others increases happiness, improves mental health, and boosts personal satisfaction. At the same time, exercising regularly reduces stress, enhances mood, and strengthens the immune system. Why not combine both?</p>
            </section>

            <footer>
                <p>Be the reason someone smiles today. Join a competition and make a difference!</p>
                <asp:Button ID="btnLogin2" runat="server" CssClass="btn-2" Text="Login & Get Started" OnClick="BtnLogin1_Click" />
            </footer>

        </section>
    </form>
</body>
</html>
