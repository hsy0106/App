using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class ItechForm : Form
    {
        // TCP 客户端
        private TcpClient tcpClient;
        private NetworkStream networkStream;
        private string ipAddress = "192.168.200.100";  
        private int port = 30000;  

        public ItechForm()
        {
            InitializeComponent();
        }

    
        private void ConnectToDevice()
        {
            try
            {
                
                tcpClient = new TcpClient(ipAddress, port);
                networkStream = tcpClient.GetStream();
               
                richTextBox1.AppendText("Connected to device at " + ipAddress + ":" + port);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to device: " + ex.Message);
            }
        }

        private void SendCommand(string command)
        {
            try
            {
          
                byte[] commandBytes = Encoding.ASCII.GetBytes(command);
                networkStream.Write(commandBytes, 0, commandBytes.Length);
               // MessageBox.Show("Command sent: " + command);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending command: " + ex.Message);
            }
        }

        private string ReadResponse()
        {
            try
            {
                byte[] dataBuffer = new byte[1024];
                int bytesRead = networkStream.Read(dataBuffer, 0, dataBuffer.Length);
                string response = Encoding.ASCII.GetString(dataBuffer, 0, bytesRead);
                return response;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading response: " + ex.Message);
                return string.Empty;
            }
        }

        //关闭输入
       private void OUTPUT()
        {
            try
            {
                string OutPutCommand = "OUTP 0\n";
                SendCommand(OutPutCommand);
            }catch(Exception ex)
            {
                MessageBox.Show("Eroor CLOSE OUTP ",ex.Message);
            }
        }

        //打开输入
        private void INPUT()
        {
            try
            {
                string InputCommand = "OUTP 1\n";
                SendCommand(InputCommand);
            }catch(Exception ex)
            {
                MessageBox.Show("Error OPEN OUTP ",ex.Message);
            }
        }

        //断开连接
        private void Disconnect()
        {
            try
            {
                networkStream.Close();
                tcpClient.Close();
                richTextBox1.AppendText("Disconnected from device.");
                MessageBox.Show("Disconnected from device.");
            }
            catch (Exception ex)
            {

                richTextBox1.AppendText("Error disconnecting: " + ex.Message);
                

            }
        }
        //设置远程
        private void Remote()
        {
            string setRemote = "SYST:REM\n";

            SendCommand(setRemote);


        }

        //设置恒流源模式
        private void SetCURR()
        {
            string setCurr = "FUNC CURR\n";
            SendCommand(setCurr);
        }

        //设置电流具体值
        private void SetCurrentValue()
        {
            if (int.TryParse(textBox1.Text, out int currentValue))
            {


                string setCurrentCommand = $"CURR {currentValue}\n";
                SendCommand(setCurrentCommand);
            }
            else
            {
                MessageBox.Show("Invalid current value entered.");
            }
        }
        //设置电流
        private void button1_Click(object sender, EventArgs e)
        {
            //先设置远程模式
            Remote();

            //设置恒流源模式
            SetCURR();

            //电压设置最大值
            SetVoltMax();

            //设置电流具体值
            SetCurrentValue();

            System.Threading.Thread.Sleep(2000);

            //输入电流值
            INPUT();
        }

        //读取电流
        private void button2_Click(object sender, EventArgs e)
        {

            MeasureCurr();
        }


        //设置电压最大值
        private void SetVoltMax()
        {
            string setVolatMaxCommand =  ":VOLT:LIM MAX\n";
            SendCommand(setVolatMaxCommand);
        }

        //读取平均电流值
         private void MeasureCurr()
        {
            string readCurrentCommand = "MEAS:CURR?\n";
            SendCommand(readCurrentCommand);
            string response = ReadResponse();
            richTextBox1.AppendText("当前设备电流" + response);
        }
     
        private void Form1_Load(object sender, EventArgs e)
        {
          
            ConnectToDevice();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Disconnect();
        }
        //断开输入
        private void button4_Click(object sender, EventArgs e)
        {
            OUTPUT();
        }
    }
}