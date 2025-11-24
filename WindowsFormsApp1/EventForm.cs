using App;
using MessagePublic;
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
    public partial class EventForm : Form
    {
        public EventForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 存
            var person = new Person { Name = "唐三", Age = 20 };
            IEvent.Instance.SetValue("Person", person);

            // 取（非泛型）
            var obj = IEvent.Instance.GetRetainValue("Person");
            Console.WriteLine(((Person)obj).Name);
            richTextBox1.AppendText("Name" + ((Person)obj).Name);
            // 取（泛型）
            var p = IEvent.Instance.GetRetainValue<Person>("Person");
            Console.WriteLine(p.Age);
            richTextBox1.AppendText("p.Age" + p.Age.ToString());
        }
        public class Person
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

    }
}
