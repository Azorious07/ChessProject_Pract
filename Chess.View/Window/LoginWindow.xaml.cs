namespace Chess.View.Window
{
    using Chess.View.Login;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            this.InitializeComponent();
        }

        public void GrantAccess()
        {
            MainWindow main = new MainWindow();
            main.Show();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserLoginBox.Text;
            string userPassword = UserPasswordBox.Password;

            using (UserDataContext context = new UserDataContext())
            {
                bool loginExists = context.Users.Any(user => user.Name == userName);

                if (!Validator.RegisterSymbolsValid(userName))
                {
                    MessageBox.Show("Unacceptable login");
                }
                else
                {
                    if (loginExists)
                    {
                        MessageBox.Show("User with this login already exists");
                    }
                    else
                    {
                        User registeredUser = new User();
                        registeredUser.Name = userName;
                        registeredUser.Password = userPassword;
                        context.Users.Add(registeredUser);
                        context.SaveChanges();
                        MessageBox.Show("Registration successful");
                    }
                }
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = UserLoginBox.Text;
            string userPassword = UserPasswordBox.Password;

            using (UserDataContext context = new UserDataContext())
            {
                bool userFound = context.Users.Any(user => user.Name == userName
                                                   && user.Password == userPassword);
                if (userFound)
                {
                    GrantAccess();
                    Close();
                }
                else
                {
                    MessageBox.Show("User not found, please register");
                }
            }
        }
    }
}
