using EAGetMail;
using System;
using System.Windows;

namespace Practice_Mail_Client
{
    public partial class Show : Window
    {
        string login = null;
        string password = null;
        string service = null;
        string SMTPservice = null;

        public Show(string login_, string password_, string service_)
        {
            InitializeComponent();

            login = login_;
            password = password_;
            SMTPservice = service_;

            if (service_ == "smtp.gmail.com")
                service = "imap.gmail.com";
            else if (service_ == "smtp.mail.yahoo.com")
                service = "imap.mail.yahoo.com";
            else if (service_ == "smtp.yandex.ru")
                service = "imap.yandex.ru";

            ShowMails();
        }

        private void ShowMails()
        {
            MailServer server = new MailServer(
                 "imap.gmail.com",
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

                var messages = client.GetMailInfos();

                foreach (var m in messages)
                {
                    EAGetMail.Mail message = client.GetMail(m);

                    listBox.Items.Add($"Index: {m.Index}{Environment.NewLine}Size: {m.Size}\n" + $"From: {message.From}" + $"Date: {message.SentDate}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private MailInfo GetMailByIndex(int index)
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
            MailInfo mail = new MailInfo();

            try
            {
                client.Connect(server);

                var messages = client.GetMailInfos();

                foreach (var m in messages)
                {
                    EAGetMail.Mail message = client.GetMail(m);

                    if (m.Index == Convert.ToInt32(indexTB.Text))
                        mail = m;
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }

            return mail;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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

            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(server);

                MailInfo mail = GetMailByIndex(Convert.ToInt32(indexTB.Text));

                Info inf = new Info(mail, login, password, service);
                inf.Show();

            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
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

            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(server);

                if (GetMailByIndex(Convert.ToInt32(indexTB.Text)).Read == false)
                    client.MarkAsRead(GetMailByIndex(Convert.ToInt32(indexTB.Text)), true);
                else
                    client.MarkAsRead(GetMailByIndex(Convert.ToInt32(indexTB.Text)), false);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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

            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(server);
                client.Delete(GetMailByIndex(Convert.ToInt32(indexTB.Text)));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Login log = new Login(login, password, SMTPservice);
            log.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
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

            MailClient client = new MailClient("TryIt");

            try
            {
                client.Connect(server);

                client.Move(GetMailByIndex(Convert.ToInt32(indexTB.Text)), new Imap4Folder("INBOX"));
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
    }
}
