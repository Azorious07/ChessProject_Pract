namespace Chess.View
{
    using System.Windows;
    using Chess.View.Login;
    using Microsoft.EntityFrameworkCore.Infrastructure;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DatabaseFacade facade = new DatabaseFacade(new UserDataContext());
            facade.EnsureCreated();
        }
    }
}