using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;
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
            label1.Text =Get_CPU_Name();
            label2.Text =Get_GPU_Name();
            label3.Text =Convert.ToString(Get_RAM_Total());
        }
        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        private string Get_CPU_Name()
        {
            string CPU_name =null;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.Accept(updateVisitor);
            computer.CPUEnabled =true;
            CPU_name = computer.Hardware[0].Name;
            computer.Close();
            GC.Collect();
            return CPU_name;
        }
        private int Get_CPU_Temp()
        {
            int Temp=0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);
            //computer.Open();
            for (int j = 0; j < computer.Hardware[0].Sensors.Length; j++)
            {
                        //找到温度传感器
                if (computer.Hardware[0].Sensors[j].SensorType == SensorType.Temperature && computer.Hardware[0].Sensors[j].Name == "CPU Package")
                {
                  Temp = Convert.ToInt32(computer.Hardware[0].Sensors[j].Value);
                }
            }
            computer.Close();
            GC.Collect();
            return Temp;
        }
        private int Get_CPU_Load()
        {
            int Load = 0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);

           for (int j = 0; j < computer.Hardware[0].Sensors.Length; j++)
             {
                        //找到温度传感器
               if (computer.Hardware[0].Sensors[j].SensorType == SensorType.Load && computer.Hardware[0].Sensors[j].Name =="CPU Total")
                  {
                    Load = Convert.ToInt32(computer.Hardware[0].Sensors[j].Value);
                  }
             }
            computer.Close();
            GC.Collect();
            return Load;
        }
        /// <summary>
        /// 获取GPU信息
        /// </summary>
        /// <returns></returns>
        private string Get_GPU_Name()
        {
            string GPU_Name = null;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);
            try
            {
                GPU_Name = computer.Hardware[1].Name;
            }
            catch
            {

                GPU_Name = computer.Hardware[0].Name;
            }
            return GPU_Name;
        }
        private int Get_GPU_Temp()
        {
            int Temp = 0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);
            try
            {
                for (int i = 0; i < computer.Hardware[1].Sensors.Length; i++)
                {
                    if (computer.Hardware[0].Sensors[i].SensorType == SensorType.Temperature)
                    {
                        Temp =Convert.ToInt32(computer.Hardware[1].Sensors[i].Value);
                    }
                }
            }
            catch
            {
                for (int i = 0; i < computer.Hardware[0].Sensors.Length; i++)
                {
                    if (computer.Hardware[0].Sensors[i].SensorType == SensorType.Temperature)
                    {
                        Temp = Convert.ToInt32(computer.Hardware[1].Sensors[i].Value);
                    }
                }
            }
            computer.Close();
            GC.Collect();
            return Temp;
        }
        private int Get_GPU_Load()
        {
            int Load = 0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);
            try
            {
                for (int i = 0; i < computer.Hardware[1].Sensors.Length; i++)
                {
                    if (computer.Hardware[1].Sensors[i].SensorType == SensorType.Load)
                    {
                        Load = Convert.ToInt32(computer.Hardware[1].Sensors[i].Value);
                    }
                }
            }
            catch 
            {

                for (int i = 0; i < computer.Hardware[0].Sensors.Length; i++)
                {
                    if (computer.Hardware[0].Sensors[i].SensorType == SensorType.Load)
                    {
                        Load = Convert.ToInt32(computer.Hardware[0].Sensors[i].Value);
                    }
                }
            }
            computer.Close();
            GC.Collect();
            return Load;
        }
        /// <summary>
        /// 获取RAM信息
        /// </summary>
        /// <returns></returns>
        private int Get_RAM_Load()
        {
            int Load = 0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            Load =Convert.ToInt32(computer.Hardware[0].Sensors[0].Value);
            computer.Close();
            GC.Collect();
            return Load;
        }
        private int Get_RAM_Total()
        {
            int Total;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            Total =Convert.ToInt32(computer.Hardware[0].Sensors[1].Value+ computer.Hardware[0].Sensors[2].Value);
            computer.Close();
            GC.Collect();
            return Total;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text =Convert.ToString(Get_CPU_Temp());
        }
    }
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
}
