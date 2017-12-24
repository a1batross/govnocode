using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using OOP_ExtraTaskShared;
namespace OOP_ExtraTask
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public int sessionId = 0;

        private byte[] ConnectSendRecv(ClientProtocol msg, byte[] data, ServerProtocol waitMsg = ServerProtocol.svc_noop )
        {
            TcpClient client = new TcpClient();

            client.Connect(IPAddress.Parse("127.0.0.1"), 29015);
            if (!client.Connected)
            {
                MessageBox.Show("Вы не видите у нас обед?");
                return null;
            }
            NetworkStream ns = client.GetStream();

            // craft message
            byte[] msgData = new byte[data.Length + 1];
            msgData[0] = (byte)msg;
            data.CopyTo(msgData, 1);

            ns.Write(msgData, 0, msgData.Length);

            byte[] response = new byte[4096];

            

            if( waitMsg != (byte)ServerProtocol.svc_noop )
            {
                int len = ns.Read(response, 0, response.Length);
                if( len > 0 )
                {
                    if( response[0] == (byte)waitMsg )
                    {
                        return response.Skip(1).ToArray();
                    }
                }
            }


            ns.Close();
            client.Close();

            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = Passwd.Text;
            string user = UserName.Text;

            if (password.Length == 0 || user.Length == 0)
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            LoginCredentials creds = new LoginCredentials()
            {
                login = user,
                passwd = password
            };

            byte []answer = ConnectSendRecv(ClientProtocol.clc_login, creds.SerializeToByteArray(), ServerProtocol.svc_login);

            if( answer != null )
            {
                long size;
                LoginResponse resp = answer.DeserializeObject<LoginResponse>( out size);

                switch( resp.error )
                {
                    case LoginError.LOGIN_ALREADYLOGIN:
                        MessageBox.Show("Вас много, а я одна!");
                        break;
                    case LoginError.LOGIN_INVALIDPASSWD:
                        MessageBox.Show("Вот где открывали счёт, туда и обращайтесь!");
                        break;
                    case LoginError.LOGIN_SUCCESS:
                        statusText.Text = String.Format("Connected as {0}", user);
                        sessionId = resp.sessionId;
                        Disconnect.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = true;
                        break;
                }
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private void Disconnect_Click(object sender, EventArgs e)
        {
            if( sessionId == 0 )
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            ConnectSendRecv(ClientProtocol.clc_disconnect, sessionId.SerializeToByteArray());
            statusText.Text = "Disconnected";
            label1.Text = "";
            Disconnect.Enabled = button2.Enabled = button3.Enabled = button4.Enabled = button5.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";

            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (sessionId == 0)
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            byte[] answer = ConnectSendRecv(ClientProtocol.clc_getaccounthistory, sessionId.SerializeToByteArray(), ServerProtocol.svc_accounthistory);

            if (answer != null)
            {
                long size;
                List<AccountChange> resp = answer.DeserializeObject<List<AccountChange>>(out size);

                DataTable dt = new DataTable();
                DataColumn dc = dt.Columns.Add("From/to");
                dc = dt.Columns.Add("Change");

                foreach( AccountChange change in resp )
                {
                    DataRow dr = dt.NewRow();

                    if (change.loginId.Length == 0)
                        dr["From/to"] = "Withdraw";
                    else
                        dr["From/to"] = change.loginId;

                    dr["Change"] = change.account;

                    dt.Rows.Add(dr);
                }

                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (sessionId == 0)
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            byte[] answer = ConnectSendRecv(ClientProtocol.clc_getaccountvalue, sessionId.SerializeToByteArray(), ServerProtocol.svc_accountvalue);

            if (answer != null)
            {
                long size;
                long value = answer.DeserializeObject<long>(out size);

                label1.Text = "Account : " + value;
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (sessionId == 0 || Convert.ToInt64(textBox1.Text) <= 0 )
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            SendMoney money = new SendMoney
            {
                sessionId = sessionId,
                to = "",
                value = Convert.ToInt64(textBox1.Text)
            };


            byte[] answer = ConnectSendRecv(ClientProtocol.clc_send, money.SerializeToByteArray(), ServerProtocol.svc_sendresponse);

            if (answer != null)
            {
                long size;
                SendResponseError value = answer.DeserializeObject<SendResponseError>(out size);
                
                switch( value )
                {
                    case SendResponseError.SEND_INVAL_ID:
                        MessageBox.Show("Нет такого счёта");
                        break;
                    case SendResponseError.SEND_NOTENOUGH:
                        MessageBox.Show("На какие шиши выводить?");
                        break;
                    case SendResponseError.SEND_SUCCESS:
                        MessageBox.Show("Исполнено");
                        break;
                }
            }
            else
            {
                MessageBox.Show("error");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (sessionId == 0 || Convert.ToInt64(textBox1.Text) <= 0 || textBox2.Text.Length == 0 )
            {
                MessageBox.Show("Ашыпка!");
                return;
            }

            SendMoney money = new SendMoney
            {
                sessionId = sessionId,
                to = textBox2.Text,
                value = Convert.ToInt64(textBox1.Text)
            };


            byte[] answer = ConnectSendRecv(ClientProtocol.clc_send, money.SerializeToByteArray(), ServerProtocol.svc_sendresponse);

            if (answer != null)
            {
                long size;
                SendResponseError value = answer.DeserializeObject<SendResponseError>(out size);

                switch (value)
                {
                    case SendResponseError.SEND_INVAL_ID:
                        MessageBox.Show("Нет такого счёта");
                        break;
                    case SendResponseError.SEND_NOTENOUGH:
                        MessageBox.Show("На какие шиши выводить?");
                        break;
                    case SendResponseError.SEND_SUCCESS:
                        MessageBox.Show("Исполнено");
                        break;
                }
            }
            else
            {
                MessageBox.Show("error");
            }
        }
    }
}
