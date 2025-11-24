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
    public partial class TreeForm : Form
    {

        class Employee
        {
            public string Name { get; set; }
            public string Position { get; set; }
            public List<Employee> Subordinates { get; set; } = new List<Employee>();
        }
        private Employee orgStructure;
        public TreeForm()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            orgStructure = new Employee { Name = "SB", Position = "CEO" };
            var cto = new Employee { Name = "CTO1",Position = "CTO" };
            var cfo = new Employee { Name = "CFO1", Position = "CFO" };

            orgStructure.Subordinates.Add(cto);
            orgStructure.Subordinates.Add(cfo);

            var devManager = new Employee { Name = "赵主管", Position = "开发主管" };
            var testManager = new Employee { Name = "钱主管", Position = "测试主管" };

            cto.Subordinates.Add(devManager);
            cto.Subordinates.Add(testManager);

            devManager.Subordinates.Add(new Employee { Name = "孙开发", Position = "高级开发" });
            devManager.Subordinates.Add(new Employee { Name = "周开发", Position = "初级开发" });


            BuildTreeView(treeView1.Nodes, orgStructure);

        }

        private void BuildTreeView(TreeNodeCollection nodes, Employee employee)
        {
            TreeNode node = new TreeNode($"{employee.Name} ({employee.Position})");
            nodes.Add(node);

            foreach (var subordinate in employee.Subordinates)
            {
                BuildTreeView(node.Nodes, subordinate);
            }
        }



    }
}
