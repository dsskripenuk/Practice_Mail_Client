using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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
using Business_Logic_Layer;
using EAGetMail;
using EASendMail;

namespace Practice_Mail_Client
{
    public partial class AddFolder : Window
    {
        static string login = null;
        static string password = null;
        static string service = null;

        MailServer server;
        MailClient client = new MailClient("TryIt");

        public AddFolder(string login_, string password_, string service_)
        {
            InitializeComponent();

            login = login_;
            password = password_;
            service = service_;

            server = new MailServer(
            service,
            login,
            password,
            EAGetMail.ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Connect(server);
            client.CreateFolder(null, tbFold.Text);
            MessageBox.Show("Folder create", "Create folder");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WriteMail wm = new WriteMail(login, password, service);
            wm.Show();
            this.Close();
        }
    }
}
