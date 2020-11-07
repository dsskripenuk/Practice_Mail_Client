using Business_Logic_Layer;
using EAGetMail;
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
    public partial class WriteMail : Window
    {
        static string login = null;
        static string password = null;
        static string service = null;
        string SMTPservice = null;
        private IBLLClass _bll = null;

        MailClient mailClient = new MailClient("TryIt");
        MailServer server;



        public WriteMail(string login_, string password_, string service_)
        {
            InitializeComponent();
            _bll = new BLLClass();

            foreach (var users in _bll.GetAllUsers())
            {
                cbAllMails.Items.Add(users.Login);
            }

            login = login_;
            password = password_;
            SMTPservice = service_;

            if (service_ == "smtp.gmail.com")
                service = "imap.gmail.com";
            else if (service_ == "smtp.mail.yahoo.com")
                service = "imap.mail.yahoo.com";
            else if (service_ == "smtp.yandex.ru")
                service = "imap.yandex.ru";

            server = new MailServer(
            service,
            login,
            password,
            EAGetMail.ServerProtocol.Imap4)
            {
                SSLConnection = true,
                Port = 993
            };

            ShowMails();
        }

        private void ShowMails()
        {
            try
            {
                mailClient.Connect(server);

                var messages = mailClient.GetMailInfos();

                foreach (var m in messages)
                {
                    EAGetMail.Mail message = mailClient.GetMail(m);

                    listBox.Items.Add($"{m.Index}{Environment.NewLine}\n" + $"From: {message.From}" + $"Date: {message.SentDate}");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private MailInfo GetMailByIndex(int index)
        {
            MailInfo mail = new MailInfo();

            try
            {
                mailClient.Connect(server);

                var messages = mailClient.GetMailInfos();

                foreach (var m in messages)
                {
                    EAGetMail.Mail message = mailClient.GetMail(m);

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login(login, password, service);
            log.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var folder in mailClient.Imap4Folders)
                {
                    foreach (var subfolder in folder.SubFolders)
                    {
                        if (subfolder.Name.Equals("Trash") || subfolder.Name.Equals("Кошик") || subfolder.Name.Equals("Корзина"))
                        {
                            int index = GetSelectedIndex(listBox.SelectedItem.ToString());

                            mailClient.Move(GetMailByIndex(Convert.ToInt32(indexTB.Text)), subfolder);
                            MessageBox.Show("Moving to sent", "Message was moved to sent!");
                            break;
                        }
                    }
                }
            }
            catch (Exception)
            { }

            //try
            //{
            //    int index = GetSelectedIndex(listBox.SelectedItem.ToString());
            //    MailInfo client = GetMailByIndex(index);

            //    MessageBox.Show(client.ToString());

            //    mailClient.Connect(server);
            //    mailClient.Delete(client);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message);
            //}
        }

        private int GetSelectedIndex(string selectedMail)
        {
            var split = selectedMail.Split('\n', '\r');
            int index = Convert.ToInt32(split[0]);

            return index;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                mailClient.Connect(server);

                if (GetMailByIndex(Convert.ToInt32(indexTB.Text)).Read == false)
                    mailClient.MarkAsRead(GetMailByIndex(Convert.ToInt32(indexTB.Text)), true);
                else
                    mailClient.MarkAsRead(GetMailByIndex(Convert.ToInt32(indexTB.Text)), false);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                mailClient.Connect(server);

                MailInfo mail = GetMailByIndex(Convert.ToInt32(indexTB.Text));

                Info inf = new Info(mail, login, password, service);
                inf.Show();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

        }
    }
}
