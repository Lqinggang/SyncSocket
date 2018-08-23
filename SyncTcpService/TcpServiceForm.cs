using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;

namespace SyncTcpService
{
    public partial class TcpServiceForm : Form
    {
        private Socket socket = null;
        private Socket clientSocket = null;
        private Thread receviethread = null;

        private delegate void recevieFromClient(byte[] receivebytes);

        public TcpServiceForm()
        {
            InitializeComponent();

        }

        //窗体加载事件
        private void TcpServiceForm_Load(object sender, EventArgs e)
        {
            try
            {
                //从配置文件获取信息
                String ip = ConfigurationManager.AppSettings["serviceIp"].ToString();
                String port = ConfigurationManager.AppSettings["port"].ToString();

                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(iep); //绑定，同一个ip+port只能有一个?
                socket.Listen(100); //监听，最大连接数100

                clientSocket = socket.Accept(); //阻塞，等待Client连接,Bug:造成在Client没有连接之前窗口不显示
                String message = "Connected Success!!";
                byte[] msg = System.Text.Encoding.UTF8.GetBytes(message); //转码
                int successSendBytes = clientSocket.Send(msg, msg.Length, SocketFlags.None); //发送数据到客户端
                //Thread acceptThread = new Thread(accept);//创建一个等待连接的线程
                //acceptThread.Start();

                receviethread = new Thread(recevie); //创建一个接收信息的线程
                receviethread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ////等待连接
        //private void accept() {
        //    clientSocket = socket.Accept(); //阻塞，等待Client连接
        //    String message = "Connected Success!!\n";
        //    byte[] msg = System.Text.Encoding.UTF8.GetBytes(message); //转码
        //    clientSocket.Send(msg, msg.Length, SocketFlags.None); //发送数据到客户端
        //}

        //发送按钮点击事件
        private void send_button_Click(object sender, EventArgs e)
        {
            if (!"".Equals(this.send_textBox.Text))
            {
                try
                {
                    String message = this.send_textBox.Text;
                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(message); //转码
                    int successSendBytes = clientSocket.Send(msg, msg.Length, SocketFlags.None); //发送数据到客户端
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //发送窗口按键事件
        private void send_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {  //按下回车
                send_button_Click(sender, e);
            }
        }

        //窗口关闭事件
        private void TcpServiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            receviethread.Abort(); //结束线程
            //Application.Exit(); //程序退出
        }

        //从接收端接收数据
        private void recevie()
        {
            while (clientSocket.Connected)
            {
                try
                {
                    while (clientSocket.Available > 0)
                    {
                        byte[] receivebytes = new byte[1024];//接收缓冲区，每次都重新new的原因：防止接收到的数据受到污染(短数据只覆盖长数据的前部分)
                        clientSocket.Receive(receivebytes); //接收数据
                        this.Invoke(new recevieFromClient(invoke), receivebytes); //解决多线程下控件操作问题，委托
                        //委托传值：delegate,parameter list
                        //invoke需要一个byte[]参数，receivebytes是转给invoke的参数
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //使用委托解决跨线程操作控件问题
        private void invoke(byte[] receivebytes)
        {
            String NewLine = "";
            if (!"".Equals(this.msg_textBox.Text.Trim()))
            { //首行不换行
                NewLine += Environment.NewLine;
            }
            this.msg_textBox.AppendText(NewLine + System.Text.Encoding.UTF8.GetString(receivebytes));
        }

    }
}
