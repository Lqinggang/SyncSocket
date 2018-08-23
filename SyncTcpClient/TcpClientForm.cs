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

namespace SyncSocket
{
    public partial class TcpClientForm : Form
    {
        private Socket clientSocket;
        private delegate void recevieFromService(byte[] receivebytes);
        Thread receviethread;

        public TcpClientForm()
        {
            InitializeComponent();
        }

        //窗体加载事件
        private void TcpClientForm_Load(object sender, EventArgs e)
        {
            try
            {
                //从配置文件获取信息
                String ip = ConfigurationManager.AppSettings["serviceIp"].ToString();
                String port = ConfigurationManager.AppSettings["port"].ToString();

                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); //创建Socket
                clientSocket.Connect(iep);//尝试连接
                receviethread = new Thread(recevie); //创建一个接收数据的线程
                receviethread.Start(); //线程开始
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //发送按钮点击事件
        private void send_button_Click(object sender, EventArgs e)
        {
            try
            {
                String sendMessage = this.send_textBox.Text.Trim(); //获取需要发送的数据
                if (!"".Equals(sendMessage))
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sendMessage); //转码
                    int successSendBytes = clientSocket.Send(buffer); //发送数据到服务端
                }
                else
                {
                    throw new Exception("发送的数据不能为空！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void send_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                send_button_Click(sender, e);
            }
        }


        //窗口关闭事件
        private void TcpClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            receviethread.Abort(); //结束线程
            //Application.Exit(); //程序退出
        }

        //接收数据
        private void recevie()
        {
            while (clientSocket.Connected) //连接有效
            {
                try
                {
                    while (clientSocket.Available > 0) //尚有数据接收
                    {

                        byte[] receivebytes = new byte[1024];//接收缓冲区，每次都重新new的原因：防止接收到的数据受到污染(短数据只覆盖长数据的前部分)
                        clientSocket.Receive(receivebytes); //接收数据
                        this.Invoke(new recevieFromService(invoke), receivebytes);//解决多线程下控件操作问题，委托
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        //Invoke，解决跨线程操作控件问题
        private void invoke(byte[] receivebytes)
        {
            String NewLine = "";
            if (!"".Equals(this.msg_textBox.Text.Trim()))
            { //首行不换行
                NewLine += Environment.NewLine;
            }
            this.msg_textBox.AppendText(NewLine + System.Text.Encoding.UTF8.GetString(receivebytes)); //将数据追加到TextBox
        }


    }
}
