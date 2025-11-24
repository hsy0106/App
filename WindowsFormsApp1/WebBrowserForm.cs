using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class WebBrowserForm : Form
    {
        private Stack<string> backStack = new Stack<string>();
        private Stack<string> forwardStack = new Stack<string>();
        private string currentUrl = "home";

        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();
        private string currentText = "";
        private bool isTextChanging = false;

        









        public WebBrowserForm()
        {
            InitializeComponent();
            textBox1.Text = currentUrl;
        }



        public void Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(currentText);


                currentText = undoStack.Pop();
            }
        }

        public void Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(currentText);
                currentText = redoStack.Pop();
            }
        }
        public string GetText() => currentText;




        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isTextChanging)
            {
                undoStack.Push(richTextBox1.Text);
                redoStack.Clear();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if(undoStack.Count> 0)
            {
                isTextChanging = true;
                //Stack 后进先出 Pop是从栈顶 Peek是从返回栈顶元素然后不移除
                redoStack.Push(undoStack.Pop());
               richTextBox1.Text =  undoStack.Peek();
                isTextChanging = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (redoStack.Count > 0)
            {
                isTextChanging = true;
                string redoText = redoStack.Pop();
                undoStack.Push(redoText);
                richTextBox1.Text = redoText;
                isTextChanging = false;
            }
        }





        private void button1_Click(object sender, EventArgs e)
        {
            backStack.Push(currentUrl);
            currentUrl = textBox1.Text;
            forwardStack.Clear();
            label1.Text = $"当前页面：{currentUrl}";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(backStack.Count > 0)
            {
                forwardStack.Push(currentUrl);
                currentUrl = backStack.Pop();
                label1.Text = $"当前页面：{currentUrl}";
            }

        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (forwardStack.Count > 0)
            {
                backStack.Push(currentUrl);
                currentUrl = forwardStack.Pop();
                label1.Text = $"当前页面:{currentUrl}";
            }
        }

    
    }
}
