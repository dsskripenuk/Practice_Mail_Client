using EASendMail;
using System.Windows;
using System.Diagnostics;
using Business_Logic_Layer;
using System.Windows.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System;

namespace Practice_Mail_Client
{
    public partial class MainWindow : Window
    {
        private IBLLClass _bll = null;
        string service = null;
        bool check;

        public MainWindow()
        {
            InitializeComponent();

            _bll = new BLLClass();

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("Chrome.exe", "https://accounts.google.com/signup/v2/webcreateaccount?service=mail&continue=https%3A%2F%2Fmail.google.com%2Fmail%2F&flowName=GlifWebSignIn&flowEntry=SignUp");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                EASendMail.SmtpServer server = new EASendMail.SmtpServer(service)
                {
                    Port = 465,
                    ConnectType = SmtpConnectType.ConnectSSLAuto,
                    User = loginTb.Text,
                    Password = passwordPB.Password
                };

                SmtpClient client = new SmtpClient();
                client.Connect(server);

                var user = new UserDTO()
                {
                    Login = loginTb.Text,
                    Password = passwordPB.Password,
                };


                foreach (var users in _bll.GetAllUsers())
                {
                    if(users.Login == user.Login)
                    {
                        check = true;
                    }
                }

                //adduser
                if(check != true)
                {
                    _bll.AddUser(user); 
                }
                

                WriteMail log = new WriteMail(loginTb.Text, passwordPB.Password, service);
                log.Show();
                this.Close();
            }
            catch
            {
                System.Windows.MessageBox.Show("Incorrect address or password!");
            }
        }

        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (serviceCB.SelectedItem.ToString() == "Gmail")
                service = "smtp.gmail.com";
            else if (serviceCB.SelectedItem.ToString() == "Yahoo")
                service = "smtp.mail.yahoo.com";
            else if (serviceCB.SelectedItem.ToString() == "Yandex")
                service = "smtp.yandex.ru";
            else
                service = "smtp.gmail.com";
        }

        private void LoginTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBox.Items.Clear();
            try
            {
                if (loginTb.Text != "")
                {
                    foreach (var users in _bll.GetAllUsers())
                    {
                        for (int i = 0; i < users.Login.Length; i++)
                        {
                            if (loginTb.Text[0] == users.Login[0])
                            {
                                listBox.Items.Add(users.Login);
                                break;
                            }
                        }
                    }
                }
            } catch(Exception ex) { MessageBox.Show(ex.Message); }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedItem != null)
            {
                loginTb.Text = listBox.SelectedItem.ToString();
            }
        }
    }
}
