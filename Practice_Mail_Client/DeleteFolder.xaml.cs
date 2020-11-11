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
using EAGetMail;

namespace Practice_Mail_Client
{
    /// <summary>
    /// Логика взаимодействия для DeleteFolder.xaml
    /// </summary>
    public partial class DeleteFolder : Window
    {
        static string login = null;
        static string password = null;
        static string service = null;

        MailServer server;
        MailClient client = new MailClient("TryIt");

        public DeleteFolder(string login_, string password_, string service_)
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WriteMail wm = new WriteMail(login, password, service);
            wm.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Connect(server);
            client.DeleteFolder(new Imap4Folder(tbFold.Text));
            MessageBox.Show("Folder delete", "Delete folder");
        }
    }
}
