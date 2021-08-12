using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCA
{
    public partial class Form1 : Form
    {
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
            button1.Text ="扫描设备";
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }
    }
}
