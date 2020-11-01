using EAGetMail;
using System;
using System.Linq;
using System.Windows;

namespace Practice_Mail_Client
{
    public partial class Info : Window
    {
        string login;
        string password;
        MailInfo mailInfo;
        string service;

        public Info(MailInfo mail_, string login_, string password_, string service_)
        {
            InitializeComponent();

            login = login_;
            password = password_;
            mailInfo = mail_;
            service = service_;

            ViewInfo(mail_);
        }

        private void ViewInfo(MailInfo mail)
        {
            MailServer server = new MailServer(
                 service,
                 login,
                 password,
                 EAGetMail.ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };

            MailClient client = new MailClient("TryIt"); // trial version

            try
            {
                client.Connect(server);

                infolb.Items.Add($"Index: {mail.Index}{Environment.NewLine}Size: {mail.Size}\n");

                EAGetMail.Mail message = client.GetMail(mail);

                infolb.Items.Add($"From: {message.From}\n\n\t{message.Subject}");
                infolb.Items.Add($"Date: {message.SentDate}\tAttachments: {message.Attachments.Count()}");
                infolb.Items.Add($"Body: {new string(message.TextBody.ToArray())}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MailServer server = new MailServer(
               service,
               login,
               password,
               EAGetMail.ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };

            MailClient client = new MailClient("TryIt"); // trial version

            MailAddress from = null;

            try
            {
                client.Connect(server);

                EAGetMail.Mail message = client.GetMail(mailInfo);
                from = message.From;

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            Reply reply = new Reply(login, password, from.Address, service);
            reply.Show();
        }
    }
}
