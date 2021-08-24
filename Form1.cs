using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Windows.Forms.DataVisualization.Charting;

namespace PCA
{
    public partial class Form1 : Form
    {
        private Point mousepoint;
        private Boolean leftflag = false;//右击标志位
        private Boolean Send_flag =false;//发送的标志位
        Hardware Hardwaremonitor = new Hardware();
        StringBuilder CPU_tab_temp = new StringBuilder(10);
        StringBuilder CPU_tab_Load = new StringBuilder(10);
        StringBuilder CPU_tab_Fre = new StringBuilder(10);
        StringBuilder GPU_tab_temp = new StringBuilder(10);
        StringBuilder GPU_tab_Load = new StringBuilder(10);
        StringBuilder GPU_tab_Fre = new StringBuilder(10);
        StringBuilder Send_CPU_temp = new StringBuilder(3);
        StringBuilder Send_CPU_Load = new StringBuilder(3);
        StringBuilder Send_GPU_temp = new StringBuilder(3);
        StringBuilder Send_GPU_Load = new StringBuilder(3);
        StringBuilder Send_RAM_Load = new StringBuilder(3);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)//假如串口已经打开，点击的目的是关闭串口
                {
                    button1.Text = "扫描设备";
                    serialPort1.Close();
                    Send_flag =false;
                }
                else
                {
                    Send_flag =true;
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate =115200;//默认为115200
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    button1.Text ="关闭设备";
                    serialPort1.Open();
                    Send_flag = true;
                }
            }
            catch 
            {
                serialPort1 = new System.IO.Ports.SerialPort();//建立新的对象
                comboBox1.Items.Clear();//刷新
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());//给出新的串口
                button1.Text="扫描设备";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls =false;
            button1.Text ="扫描设备";
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
            label1.Text = Hardwaremonitor.Get_CPU_Name();
            label2.Text = Hardwaremonitor.Get_GPU_Name();
            label3.Text = "主板："+Hardwaremonitor.Get_borad();
            label4.Text = "可用内存：" + Convert.ToString(Hardwaremonitor.Get_RAM_Total())+"GB";
            label5.Text = label1.Text;
            label6.Text = Convert.ToString(CPU_tab_temp);
            label7.Text = Convert.ToString(CPU_tab_Load);
            label8.Text = Convert.ToString(CPU_tab_Fre);
            label9.Text = label2.Text;
            label10.Text = Convert.ToString(GPU_tab_temp);
            label11.Text = Convert.ToString(GPU_tab_Load);
            label12.Text = Convert.ToString(GPU_tab_Fre);
            Thread Get_task = new Thread(get_infor);//创建线程
            Get_task.IsBackground  =  true;
            Get_task.Start();
            notifyIcon1.Visible =false;
            // tabcontrol设置为可以自定义绘制标签内容
            this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl_DrawItem);
        }
        /// <summary>
        /// 一个用于刷新的任务的线程函数
        /// </summary>
        private void get_infor()
        {
            // 设置曲线的样式
            Series series = chart1.Series[0];
            int CPU_temp = 0;
            int CPU_Load =0;
            int GPU_temp =0;
            int GPU_Load =0;
            int RAM_Load =0;
            String infor = null;
            while (true)
            {
                CPU_temp = Hardwaremonitor.Get_CPU_Temp();
                CPU_Load = Hardwaremonitor.Get_CPU_Load();
                GPU_temp = Hardwaremonitor.Get_GPU_Temp();
                GPU_Load = Hardwaremonitor.Get_GPU_Load();
                RAM_Load = Hardwaremonitor.Get_RAM_Load();
                CPU_tab_temp.Append("温度："+Convert.ToString(CPU_temp)+"°C");
                CPU_tab_Load.Append("使用率：" + Convert.ToString(CPU_Load) +"%");
                CPU_tab_Fre.Append("频率："+Convert.ToString(Hardwaremonitor.Get_CPU_Clock()+"Mhz"));
                GPU_tab_temp.Append("温度：" + Convert.ToString(GPU_temp) + "°C");
                GPU_tab_Load.Append("使用率：" + Convert.ToString(GPU_Load) +"%");
                GPU_tab_Fre.Append("频率："+Convert.ToString(Hardwaremonitor.Get_GPU_Clock())+"Mhz");
                label6.Text = Convert.ToString(CPU_tab_temp);
                label7.Text = Convert.ToString(CPU_tab_Load);
                label8.Text = Convert.ToString(CPU_tab_Fre);
                label10.Text = Convert.ToString(GPU_tab_temp);
                label11.Text = Convert.ToString(GPU_tab_Load);
                label12.Text = Convert.ToString(GPU_tab_Fre);
                CPU_tab_temp.Clear();
                CPU_tab_Load.Clear();
                CPU_tab_Fre.Clear();
                GPU_tab_temp.Clear();
                GPU_tab_Load.Clear();
                GPU_tab_Fre.Clear();
                series.Points.AddY(CPU_temp);
                try
                {
                    if (Send_flag)//开始补位操作，数据帧的结构是3字节一个信息
                    {
                        
                        Send_CPU_temp.Append(Convert.ToString(CPU_temp));
                        if(Send_CPU_temp.Length==2) { Send_CPU_temp.Insert(0, '0'); }


                        Send_CPU_Load.Append(Convert.ToString(CPU_Load));
                        if (Send_CPU_Load.Length==2)
                        {
                            Send_CPU_Load.Insert(0, '0');
                        }
                        else if(Send_CPU_Load.Length == 1)
                        { Send_CPU_Load.Insert(0, '0'); Send_CPU_Load.Insert(1, '0'); }

                        Send_GPU_temp.Append(Convert.ToString(GPU_temp));
                        if (Send_GPU_temp.Length==2) { Send_GPU_temp.Insert(0,'0'); }
                        else if(Send_GPU_temp.Length == 1) { Send_GPU_temp.Insert(0, '0'); Send_GPU_temp.Insert(1, '0'); }
                    
                        Send_GPU_Load.Append(Convert.ToString(GPU_Load));
                        if (Send_GPU_Load.Length==2) { Send_GPU_Load.Insert(0,'0'); }
                        else if(Send_GPU_Load.Length==1)
                        { Send_GPU_Load.Insert(0,'0'); Send_GPU_Load.Insert(1, '0'); }


                        Send_RAM_Load.Append(Convert.ToString(RAM_Load));
                        if (Send_RAM_Load.Length == 2) { Send_RAM_Load.Insert(0,'0'); }
                        else if (Send_RAM_Load.Length == 1) { Send_RAM_Load.Insert(0, '0'); Send_RAM_Load.Insert(1, '0'); }


                        infor = Convert.ToString(Send_CPU_temp) + Convert.ToString(Send_CPU_Load) + Convert.ToString(Send_GPU_temp) + Convert.ToString(Send_GPU_Load) + Convert.ToString(Send_RAM_Load);

                        serialPort1.Write(infor);
                        //数据清洗
                        Send_CPU_temp.Clear();
                        Send_CPU_Load.Clear();
                        Send_GPU_temp.Clear();
                        Send_GPU_Load.Clear();
                        Send_RAM_Load.Clear();
                    }
                }
                catch
                {

                    Send_flag =false;
                }
                Thread.Sleep(2500);
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            {
                if (e.Button == MouseButtons.Left)//如果右键，则不移动
                {
                    mousepoint.X = e.X;
                    mousepoint.Y = e.Y;
                    leftflag = true;
                }

            }//  首先记下按下左键时的第一个鼠标位置
        }

        private void tabControl1_MouseUp(object sender, MouseEventArgs e)
        {
                leftflag = false;//松开，不移动
        }
        /// <summary>
        /// 用于窗体移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftflag)//如果鼠标右键了，则准备开始移动
            {
                Left = MousePosition.X - mousepoint.X;
                Top = MousePosition.Y - mousepoint.Y;
            }
        }
        /// <summary>
        /// 推出按钮定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            notifyIcon1.Visible = true;                 //小图标可见
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Left)
            {
                this.Visible = true;                        //窗体可见
                this.WindowState = FormWindowState.Normal;  //窗体默认大小
                this.notifyIcon1.Visible = true;            //设置图标可见
                notifyIcon1.Visible =false;                 //小图标不可见
            }
        }
        private void tabControl_DrawItem(object sender, DrawItemEventArgs e)
        {

            #region 改变工作区，工作区这里主要是背景色，因为我Form的背景色就是(45, 45, 48)，为了搭配起来好看
            //绑定工作区
            Rectangle rec01 = tabControl1.ClientRectangle;
            //设置工作区背景色
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(46, 51, 73)), rec01);
            #endregion

            #region 重绘标签头=======================
            //灰色背景
            SolidBrush back = new SolidBrush(Color.FromArgb(46, 51, 73));
            //蓝色字体
            SolidBrush white = new SolidBrush(Color.FromArgb(122, 193, 255));
            StringFormat sf = new StringFormat();
            //文本水平/垂直居中对齐
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            for (int i = 0; i < tabControl1.TabPages.Count; i++)
            {
                //绑定选项卡
                Rectangle rec = tabControl1.GetTabRect(i);
                //设置选项卡背景
                e.Graphics.FillRectangle(back, rec);

                //设置选项卡字体及颜色
                e.Graphics.DrawString(tabControl1.TabPages[i].Text, new Font("Nirmala UI", 9), white, rec, sf);
            }
            #endregion

        }


    }
}
