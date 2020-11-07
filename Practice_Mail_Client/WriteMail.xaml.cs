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

        MailServer server;

        MailClient client = new MailClient("TryIt");

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
                client.Connect(server);

                var messages = client.GetMailInfos();

                foreach (var m in messages)
                {
                    EAGetMail.Mail message = client.GetMail(m);

                    listBox.Items.Add($"Index: {m.Index}{Environment.NewLine}\n" + $"From: {message.From}" + $"Date: {message.SentDate}");
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Login log = new Login(login, password, service);
            log.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
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

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    client.Connect(server);

            //    foreach (var folder in client.Imap4Folders)
            //    {
            //        foreach (var subfolder in folder.SubFolders)
            //        {
            //            if (subfolder.Name.Equals("Starred") || subfolder.Name.Equals("Отмеченные") || subfolder.Name.Equals("Відміченні"))
            //            {
            //                client.SelectFolder(subfolder);

            //                listBox.Items.Clear();
            //                listBox.Items.Add()
            //            }
            //        }
            //    }
            //}
            //catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void CbAllMails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            foreach (var users in _bll.GetAllUsers())
            {
                if (cbAllMails.SelectedItem != null)
                {
                    mw.Show();
                    mw.loginTb.Text = cbAllMails.SelectedItem.ToString();
                    this.Close();
                }
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var folder in client.Imap4Folders)
                {
                    foreach (var subfolder in folder.SubFolders)
                    {
                        if (subfolder.Name.Equals("Starred") || subfolder.Name.Equals("Отмеченные") || subfolder.Name.Equals("Відміченні"))
                        {
                            client.Move(GetMailByIndex(Convert.ToInt32(indexTB.Text)), subfolder);
                            MessageBox.Show("Moving to starred", "Message was moved to starred!");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var folder in client.Imap4Folders)
                {
                    foreach (var subfolder in folder.SubFolders)
                    {
                        if (subfolder.Name.Equals("Scheduled") || subfolder.Name.Equals("Отложенные") || subfolder.Name.Equals("Відложенні"))
                        {
                            client.Move(GetMailByIndex(Convert.ToInt32(indexTB.Text)), subfolder);
                            MessageBox.Show("Moving to scheduled", "Message was moved to scheduled!");
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
