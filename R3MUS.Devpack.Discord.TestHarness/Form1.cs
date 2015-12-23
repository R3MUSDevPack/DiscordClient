using R3MUS.Devpack.Slack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3MUS.Devpack.Discord.TestHarness
{
    public partial class Form1 : Form
    {
        Client client;
        long lastMessageId = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoWork();
        }

        private void DoWork()
        {
            if ((client == null) && (textBox1.Text != string.Empty) && (textBox2.Text != string.Empty))
            {
                client = new Client() { UserName = textBox1.Text, Password = textBox2.Text };
                if (!client.Logon())
                {
                    MessageBox.Show("Authentication failed.");
                    return;
                }
            }
            else
            {
                MessageBox.Show("You need credentials to do this.");
                return;
            }
            try
            {
                var messages = client.GetMessages(Convert.ToInt64(textBox4.Text), lastMessageId);
                lastMessageId = Convert.ToInt64(messages.Last().id);
                messages.Where(msg => (DateTime.Now - msg.timestamp).Minutes < 5).ToList().ForEach(msg => {
                    var payload = new MessagePayload();
                    payload.Attachments = new List<MessagePayloadAttachment>();
                    payload.Attachments.Add(new MessagePayloadAttachment()
                    {
                        AuthorName = msg.author.username, AuthorIcon = msg.author.avatar,
                         Text = msg.content, Title = string.Format("{0}: Message from {1}", msg.timestamp.ToString("yyyy-MM-dd HH:mm:ss"), msg.author.username)
                    });
                    Plugin.SendToRoom(payload, "it_testing", textBox3.Text, "DiscordBot");
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show("Could not get messages from Discord. Was your channel ID correct?");
                return;
            }
            try
            {
                client.LogOut();
            }
            catch(Exception ex)
            {
            }
        }
    }
}
