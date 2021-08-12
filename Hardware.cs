using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
namespace PCA
{
    class Hardware
    {
        public string Get_CPU_Name()
        {
            string CPU_name = null;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.Accept(updateVisitor);
            computer.CPUEnabled = true;
            CPU_name = computer.Hardware[0].Name;
            computer.Close();
            GC.Collect();
            return CPU_name;
        }
        public int Get_CPU_Temp()
        {
            int Temp = 0;
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
        public int Get_CPU_Load()
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
                if (computer.Hardware[0].Sensors[j].SensorType == SensorType.Load && computer.Hardware[0].Sensors[j].Name == "CPU Total")
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
        public string Get_GPU_Name()
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
        public int Get_GPU_Temp()
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
                        Temp = Convert.ToInt32(computer.Hardware[1].Sensors[i].Value);
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
        public int Get_GPU_Load()
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
        public int Get_RAM_Load()
        {
            int Load = 0;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            Load = Convert.ToInt32(computer.Hardware[0].Sensors[0].Value);
            computer.Close();
            GC.Collect();
            return Load;
        }
        public int Get_RAM_Total()
        {
            int Total;
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.RAMEnabled = true;
            computer.Accept(updateVisitor);
            Total = Convert.ToInt32(computer.Hardware[0].Sensors[1].Value + computer.Hardware[0].Sensors[2].Value);
            computer.Close();
            GC.Collect();
            return Total;
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
