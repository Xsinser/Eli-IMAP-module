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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;
using System.Speech.Synthesis;
using System.Windows.Threading;
namespace Eli_IMAP_module
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int count = 0;
        void sin()
        {
            using (var client = new ImapClient())
            {
                // For demo-purposes, accept all SSL certificates
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("imap.mail.ru", 993, true);
                
                client.Authenticate("xsenotest@mail.ru", "7579916q");

                // The Inbox folder is always available on all IMAP servers...
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var z = inbox.Count;
                if((z>count)&&(count!=0))
               // for (int i = 0; i < inbox.Count; i++)
                {
                 
                    Informing((count-z).ToString(), "xsenotest@mail.ru");
                    count = z;
                    //     var message = inbox.GetMessage(i);
                    //       Console.WriteLine("Subject: {0}", message.Subject);
                }
                else if(
                    count==0)
                {
                    count = z;
                }

                client.Disconnect(true);
            }
        }
        public void Informing(string countM, string typeSN)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();

            synthesizer.Volume = 100;
            synthesizer.Rate = 0;
            synthesizer.Speak($"У вас обнаружено {countM} не прочитанное сообщение в {typeSN}");
        }

        private void timer_Tick(object sender, EventArgs e)
        {
              sin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
            sin();
        }
    }
}
