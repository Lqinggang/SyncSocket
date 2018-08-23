namespace SyncSocket
{
    partial class TcpClientForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.msg_textBox = new System.Windows.Forms.TextBox();
            this.send_textBox = new System.Windows.Forms.TextBox();
            this.send_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // msg_textBox
            // 
            this.msg_textBox.Enabled = false;
            this.msg_textBox.Location = new System.Drawing.Point(8, 12);
            this.msg_textBox.Multiline = true;
            this.msg_textBox.Name = "msg_textBox";
            this.msg_textBox.Size = new System.Drawing.Size(457, 323);
            this.msg_textBox.TabIndex = 0;
            // 
            // send_textBox
            // 
            this.send_textBox.Location = new System.Drawing.Point(8, 342);
            this.send_textBox.Multiline = true;
            this.send_textBox.Name = "send_textBox";
            this.send_textBox.Size = new System.Drawing.Size(457, 81);
            this.send_textBox.TabIndex = 1;
            this.send_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.send_textBox_KeyDown);
            // 
            // send_button
            // 
            this.send_button.Location = new System.Drawing.Point(378, 429);
            this.send_button.Name = "send_button";
            this.send_button.Size = new System.Drawing.Size(75, 23);
            this.send_button.TabIndex = 2;
            this.send_button.Text = "Send";
            this.send_button.UseVisualStyleBackColor = true;
            this.send_button.Click += new System.EventHandler(this.send_button_Click);
            // 
            // TcpClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 464);
            this.Controls.Add(this.send_button);
            this.Controls.Add(this.send_textBox);
            this.Controls.Add(this.msg_textBox);
            this.Name = "TcpClientForm";
            this.Text = "TcpClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TcpClientForm_FormClosed);
            this.Load += new System.EventHandler(this.TcpClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox msg_textBox;
        private System.Windows.Forms.TextBox send_textBox;
        private System.Windows.Forms.Button send_button;
    }
}

