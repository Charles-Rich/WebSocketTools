using DotNetty.Socket;
using System.Text;

namespace WebSocketTools
{
    public partial class MainForm : Form
    {
        private IWebSocketClient webSocketClient;

        private int connectStatus = 0;
        private StringBuilder sbData = new StringBuilder();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (this.btnConnect.Text.Equals("����")) 
            {
                WebSocketConnect();
            }
        }

        private async Task WebSocketConnect() 
        {
            webSocketClient = await SocketBuilderFactory.GetWebSocketClientBuilder(this.txtUrl.Text, int.Parse(txtPort.Text))
                .OnClientStarted(client =>
                {
                    Console.WriteLine($"�ͻ�������");
                    connectStatus = 1;
                })
                .OnClientClose(client =>
                {
                    Console.WriteLine($"�ͻ��˹ر�");
                    connectStatus = 0;
                })
                .OnException(ex =>
                {

                })
                .OnRecieve((client, bytes) =>
                {
                    Console.WriteLine($"�ͻ���:�յ�{client.Ip}����:{bytes}");
                    sbData.Append($"{bytes}\r\n");
                })
                .OnSend((client, bytes) =>
                {
                    Console.WriteLine($"�ͻ���:��������:{bytes}");
                })
                .BuildAsync();

            webSocketClient.Send("WS_GET_CONNECT_ID");
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            switch (connectStatus) 
            {
                case 0:
                    this.btnConnect.Text = "����";

                    this.txtRequest.Enabled = false;
                    this.btnSend.Enabled = false;
                    this.txtResponse.Enabled = false;
                    this.btnClear.Enabled = false;
                    this.btnCLose.Enabled = false;
                    break;
                case 1:
                    this.btnConnect.Text = "������";

                    this.txtRequest.Enabled = true;
                    this.btnSend.Enabled = true;
                    this.txtResponse.Enabled = true;
                    this.btnClear.Enabled = true;
                    this.btnCLose.Enabled = true;
                    break;
            }

            this.txtResponse.Text = sbData.ToString();
            this.tslblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (null == webSocketClient) return;
            webSocketClient.Send(this.txtRequest.Text);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            sbData.Clear();
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            webSocketClient.Close();
        }

        private void btnGetConnectId_Click(object sender, EventArgs e)
        {
            webSocketClient.Send("WS_GET_CONNECT_ID");
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtUrl.Text = ConfigUtils.GetConfig("IPAddress");
            txtPort.Text = ConfigUtils.GetConfig("Port");
            txtRequest.Text = ConfigUtils.GetConfig("Request");
            txtResponse.Text = ConfigUtils.GetConfig("Response");
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ConfigUtils.SetConfig("IPAddress", this.txtUrl.Text);
            ConfigUtils.SetConfig("Port", this.txtPort.Text);
            ConfigUtils.SetConfig("Request", this.txtRequest.Text);
            ConfigUtils.SetConfig("Response", this.txtResponse.Text);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.Modifiers == ((int)Keys.Control + (int)Keys.Alt) && e.KeyCode == Keys.C)
            {
                this.btnConnect.PerformClick();
            }

            if ((int)e.Modifiers == ((int)Keys.Control + (int)Keys.Alt) && e.KeyCode == Keys.Q) 
            {
                this.btnCLose.PerformClick();
            }

            //if (e.KeyData == (Keys.Control | Keys.Enter))
            if ((int)e.Modifiers == ((int)Keys.Control + (int)Keys.Alt) && e.KeyCode == Keys.S)
            {
                this.btnSend.PerformClick();
            }

            if ((int)e.Modifiers == ((int)Keys.Control + (int)Keys.Alt) && e.KeyCode == Keys.W)
            {
                this.btnClear.PerformClick();
            }
        }

        private void btnRepeatSend_Click(object sender, EventArgs e)
        {
            if (null == webSocketClient) return;
            for(var i = 0; i <= numericUpDown.Value; i++)
            {
                webSocketClient.Send(this.txtRequest.Text);
            }
        }
    }
}