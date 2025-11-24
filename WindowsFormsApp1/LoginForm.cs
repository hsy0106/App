using App.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class LoginForm : Form
    {
        public string UserName { get; private set; } = null;
        public bool IsLoginSuccess { get; private set; } = false;
        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = button1;
        }
        //登录
        private async void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }
            try
            {
                // 调用API验证用户名和密码
                bool isValid = await ValidateUserAsync(username, password);

                if (isValid)
                {
                    UserName = username;
                    IsLoginSuccess = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("用户名或密码错误", "登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox2.SelectAll();
                    textBox2.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"登录时发生错误: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                button1.Enabled = true;
                button1.Text = "登录";
                Cursor = Cursors.Default;
            }

        }

        private async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
        
                var response = await HttpUtilsServer.PostAsync(
                    "http://127.0.0.1:4523/m2/2728289-2828826-default/341070881",
                    new { Username = username, Password = password },
                    headers: new Dictionary<string, string> { { "Authorization", "Bearer token123" } }
                );

            
                dynamic result = JsonConvert.DeserializeObject(response);
                return result.result == true;
            }

            catch (Exception ex)
            {
                throw new Exception("验证用户时发生错误"+ ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
