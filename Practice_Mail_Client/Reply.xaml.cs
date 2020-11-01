using EASendMail;
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

namespace Practice_Mail_Client
{
    public partial class Reply : Window
    {
        string login = null;
        string password = null;
        string from = null;
        string service = null;

        public Reply(string login_, string password_, string from_, string service_)
        {
            InitializeComponent();

            login = login_;
            password = password_;
            from = from_;
            service = service_;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            EASendMail.SmtpServer server = new EASendMail.SmtpServer(service)
            {
                Port = 465,
                ConnectType = SmtpConnectType.ConnectSSLAuto,
                User = login,
                Password = password
            };

            EASendMail.SmtpMail message = new EASendMail.SmtpMail("TryIt") // trial licence
            {
                From = login,
                To = from,
                Subject = messageTB.Text,
                TextBody = bodyTB.Text,
                Priority = EASendMail.MailPriority.High
            };

            foreach (var item in listBox.Items)
                message.AddAttachment(item.ToString());

            SmtpClient client = new SmtpClient();
            client.Connect(server);

            try
            {
                System.Windows.MessageBox.Show("Try to send mail...");

                client.SendMail(message);

                System.Windows.MessageBox.Show("Message is sent");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFile = new System.Windows.Forms.OpenFileDialog();

            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                foreach (var item in openFile.FileNames)
                    listBox.Items.Add(item);
        }
    }
}
