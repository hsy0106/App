using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{



    public partial class Form1 : Form
    {
        #region 银行叫号
        private Queue<Dictionary<string,Customer>> customerQueue = new Queue<Dictionary<string, Customer>>();
        private int ticketNumber = 1;
        #endregion



        public Form1()
        {
            InitializeComponent();

          
        }

        
        private class Customer
        {
            public string CustomerName { get; set; }

            public string TicketName { get; set; }

        }




        private void button1_Click(object sender, EventArgs e)
        {
            string ticketName = $"A{ticketNumber++ }";
            string customerName = $"客户{new Random().Next(1000, 9999)}";
            Customer newCustomer = new Customer
            {
                CustomerName = customerName,
                TicketName = ticketName
            };

            customerQueue.Enqueue(new Dictionary<string, Customer> { { newCustomer.TicketName, newCustomer } });
            listBox1.Items.Add(ticketName);
            Log($"客户：{newCustomer.CustomerName} 的票号数是： {newCustomer.TicketName}已放入堆栈");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(customerQueue.Count > 0)
            {
                Dictionary<string, Customer> nextCustomer = customerQueue.Dequeue();
                var ticketName = nextCustomer.Keys.First();
                listBox1.Items.Remove(ticketName);
                MessageBox.Show($"请{ ticketName} 号到窗口办理业务");
                Log($"请 {nextCustomer.Keys} 号到窗口办理业务");
            }
        }

        private void Log(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                richTextBox1.Invoke(new Action<string>(Log), message);
            }
            else
            {
                richTextBox1.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
