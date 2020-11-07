using EASendMail;
using System;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms;
using MaterialDesignThemes.Wpf;
using EAGetMail;

namespace Practice_Mail_Client
{
    public partial class Login : Window
    {
        string login = null;
        string password = null;
        string service = null;
        MailClient client = new MailClient("TryIt");

        public Login(string login_, string password_, string service_)
        {
            InitializeComponent();

            login = login_;
            password = password_;
            service = service_;
        }

        private void openFileBtn_Click(object sender, RoutedEventArgs e)
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
                To = toTb.Text,
                Subject = messageTb.Text,
                TextBody = bodyTb.Text,
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            messageTb.FontStyle = FontStyles.Normal;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            messageTb.FontStyle = FontStyles.Italic;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            messageTb.TextDecorations = TextDecorations.Underline;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            messageTb.TextDecorations = TextDecorations.Strikethrough;
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            messageTb.FontWeight = FontWeights.Bold;
        }
    }
}
