using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
namespace PCA
{
    public partial class Form1 : Form
    {
        Hardware Hardwaremonitor = new Hardware();
        StringBuilder CPU_tab_temp = new StringBuilder(10);
        StringBuilder CPU_tab_Load = new StringBuilder(10);
        StringBuilder CPU_tab_Fre = new StringBuilder(10);
        StringBuilder GPU_tab_temp = new StringBuilder(10);
        StringBuilder GPU_tab_Load = new StringBuilder(10);
        StringBuilder GPU_tab_Fre = new StringBuilder(10);
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
                }
                else
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate =115200;//默认为115200
                    serialPort1.DataBits = 8;
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    button1.Text ="关闭设备";
                    serialPort1.Open();
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
            label1.Text = "CPU:"+Hardwaremonitor.Get_CPU_Name();
            label2.Text = "GPU:"+Hardwaremonitor.Get_GPU_Name();
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
            Thread task = new Thread(get_infor);//创建线程
            task.IsBackground  =  true;
            task.Start();
        }
        private void get_infor()
        {
            while (true)
            {
                CPU_tab_temp.Append("温度："+Convert.ToString(Hardwaremonitor.Get_CPU_Temp())+"°C");
                CPU_tab_Load.Append("使用率：" + Convert.ToString(Hardwaremonitor.Get_CPU_Load())+"%");
                CPU_tab_Fre.Append("频率："+Convert.ToString(Hardwaremonitor.Get_CPU_Clock()+"Mhz"));
                GPU_tab_temp.Append("温度：" + Convert.ToString(Hardwaremonitor.Get_GPU_Temp()) + "°C");
                GPU_tab_Load.Append("使用率：" + Convert.ToString(Hardwaremonitor.Get_GPU_Load())+"%");
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
            }
        }

    }
}
