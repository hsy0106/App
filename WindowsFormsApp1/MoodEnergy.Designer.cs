using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    partial class MoodEnergy
    {
        private System.ComponentModel.IContainer components = null;
        private Label lbl标题;
        private Label lbl个人信息;
        private Label lbl姓名标签;
        private Label lbl姓名;
        private Label lbl心情标签;
        private Label lbl心情;
        private Label lbl精力标签;
        private TrackBar lbl精力;
        private Label lbl精力文本;
        private TextBox txt状态显示;
        private ListBox lst事件记录;
        private Button btn改变心情;
        private Button btn改变精力;
        private Button btn增加精力;
        private Button btn减少精力;
        private Button btn添加心情观察者;
        private Button btn添加精力观察者;
        private Button btn移除观察者;
        private GroupBox grp控制面板;
        private GroupBox grp显示面板;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lbl标题 = new System.Windows.Forms.Label();
            this.lbl个人信息 = new System.Windows.Forms.Label();
            this.lbl姓名标签 = new System.Windows.Forms.Label();
            this.lbl姓名 = new System.Windows.Forms.Label();
            this.lbl心情标签 = new System.Windows.Forms.Label();
            this.lbl心情 = new System.Windows.Forms.Label();
            this.lbl精力标签 = new System.Windows.Forms.Label();
            this.lbl精力 = new System.Windows.Forms.TrackBar();
            this.lbl精力文本 = new System.Windows.Forms.Label();
            this.txt状态显示 = new System.Windows.Forms.TextBox();
            this.lst事件记录 = new System.Windows.Forms.ListBox();
            this.btn改变心情 = new System.Windows.Forms.Button();
            this.btn改变精力 = new System.Windows.Forms.Button();
            this.btn增加精力 = new System.Windows.Forms.Button();
            this.btn减少精力 = new System.Windows.Forms.Button();
            this.btn添加心情观察者 = new System.Windows.Forms.Button();
            this.btn添加精力观察者 = new System.Windows.Forms.Button();
            this.btn移除观察者 = new System.Windows.Forms.Button();
            this.grp控制面板 = new System.Windows.Forms.GroupBox();
            this.grp显示面板 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.lbl精力)).BeginInit();
            this.grp控制面板.SuspendLayout();
            this.grp显示面板.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl标题
            // 
            this.lbl标题.AutoSize = true;
            this.lbl标题.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl标题.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.lbl标题.Location = new System.Drawing.Point(15, 16);
            this.lbl标题.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl标题.Name = "lbl标题";
            this.lbl标题.Size = new System.Drawing.Size(205, 30);
            this.lbl标题.TabIndex = 0;
            this.lbl标题.Text = "👀 观察者模式演示";
            // 
            // lbl个人信息
            // 
            this.lbl个人信息.AutoSize = true;
            this.lbl个人信息.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl个人信息.Location = new System.Drawing.Point(15, 56);
            this.lbl个人信息.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl个人信息.Name = "lbl个人信息";
            this.lbl个人信息.Size = new System.Drawing.Size(102, 22);
            this.lbl个人信息.TabIndex = 1;
            this.lbl个人信息.Text = "👤 个人信息";
            // 
            // lbl姓名标签
            // 
            this.lbl姓名标签.AutoSize = true;
            this.lbl姓名标签.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl姓名标签.Location = new System.Drawing.Point(22, 88);
            this.lbl姓名标签.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl姓名标签.Name = "lbl姓名标签";
            this.lbl姓名标签.Size = new System.Drawing.Size(44, 17);
            this.lbl姓名标签.TabIndex = 2;
            this.lbl姓名标签.Text = "姓名：";
            // 
            // lbl姓名
            // 
            this.lbl姓名.AutoSize = true;
            this.lbl姓名.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl姓名.Location = new System.Drawing.Point(75, 88);
            this.lbl姓名.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl姓名.Name = "lbl姓名";
            this.lbl姓名.Size = new System.Drawing.Size(32, 17);
            this.lbl姓名.TabIndex = 3;
            this.lbl姓名.Text = "小高";
            // 
            // lbl心情标签
            // 
            this.lbl心情标签.AutoSize = true;
            this.lbl心情标签.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl心情标签.Location = new System.Drawing.Point(22, 112);
            this.lbl心情标签.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl心情标签.Name = "lbl心情标签";
            this.lbl心情标签.Size = new System.Drawing.Size(44, 17);
            this.lbl心情标签.TabIndex = 4;
            this.lbl心情标签.Text = "心情：";
            // 
            // lbl心情
            // 
            this.lbl心情.AutoSize = true;
            this.lbl心情.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl心情.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.lbl心情.Location = new System.Drawing.Point(75, 112);
            this.lbl心情.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl心情.Name = "lbl心情";
            this.lbl心情.Size = new System.Drawing.Size(32, 17);
            this.lbl心情.TabIndex = 5;
            this.lbl心情.Text = "开心";
            // 
            // lbl精力标签
            // 
            this.lbl精力标签.AutoSize = true;
            this.lbl精力标签.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl精力标签.Location = new System.Drawing.Point(22, 136);
            this.lbl精力标签.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl精力标签.Name = "lbl精力标签";
            this.lbl精力标签.Size = new System.Drawing.Size(44, 17);
            this.lbl精力标签.TabIndex = 6;
            this.lbl精力标签.Text = "精力：";
            // 
            // lbl精力
            // 
            this.lbl精力.Location = new System.Drawing.Point(75, 136);
            this.lbl精力.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lbl精力.Maximum = 100;
            this.lbl精力.Name = "lbl精力";
            this.lbl精力.Size = new System.Drawing.Size(150, 45);
            this.lbl精力.TabIndex = 7;
            this.lbl精力.TickStyle = System.Windows.Forms.TickStyle.None;
            this.lbl精力.Value = 100;
            // 
            // lbl精力文本
            // 
            this.lbl精力文本.AutoSize = true;
            this.lbl精力文本.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl精力文本.Location = new System.Drawing.Point(232, 136);
            this.lbl精力文本.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl精力文本.Name = "lbl精力文本";
            this.lbl精力文本.Size = new System.Drawing.Size(29, 17);
            this.lbl精力文本.TabIndex = 8;
            this.lbl精力文本.Text = "100";
            // 
            // txt状态显示
            // 
            this.txt状态显示.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.txt状态显示.Location = new System.Drawing.Point(8, 20);
            this.txt状态显示.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txt状态显示.Multiline = true;
            this.txt状态显示.Name = "txt状态显示";
            this.txt状态显示.ReadOnly = true;
            this.txt状态显示.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt状态显示.Size = new System.Drawing.Size(528, 219);
            this.txt状态显示.TabIndex = 0;
            // 
            // lst事件记录
            // 
            this.lst事件记录.FormattingEnabled = true;
            this.lst事件记录.ItemHeight = 17;
            this.lst事件记录.Location = new System.Drawing.Point(5, 264);
            this.lst事件记录.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lst事件记录.Name = "lst事件记录";
            this.lst事件记录.Size = new System.Drawing.Size(531, 106);
            this.lst事件记录.TabIndex = 1;
            // 
            // btn改变心情
            // 
            this.btn改变心情.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btn改变心情.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn改变心情.Location = new System.Drawing.Point(96, 33);
            this.btn改变心情.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn改变心情.Name = "btn改变心情";
            this.btn改变心情.Size = new System.Drawing.Size(75, 24);
            this.btn改变心情.TabIndex = 0;
            this.btn改变心情.Text = "随机心情";
            this.btn改变心情.UseVisualStyleBackColor = false;
            // 
            // btn改变精力
            // 
            this.btn改变精力.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btn改变精力.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn改变精力.ForeColor = System.Drawing.Color.White;
            this.btn改变精力.Location = new System.Drawing.Point(178, 33);
            this.btn改变精力.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn改变精力.Name = "btn改变精力";
            this.btn改变精力.Size = new System.Drawing.Size(75, 24);
            this.btn改变精力.TabIndex = 1;
            this.btn改变精力.Text = "随机精力";
            this.btn改变精力.UseVisualStyleBackColor = false;
            // 
            // btn增加精力
            // 
            this.btn增加精力.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btn增加精力.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn增加精力.ForeColor = System.Drawing.Color.White;
            this.btn增加精力.Location = new System.Drawing.Point(260, 33);
            this.btn增加精力.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn增加精力.Name = "btn增加精力";
            this.btn增加精力.Size = new System.Drawing.Size(75, 24);
            this.btn增加精力.TabIndex = 2;
            this.btn增加精力.Text = "增加精力";
            this.btn增加精力.UseVisualStyleBackColor = false;
            // 
            // btn减少精力
            // 
            this.btn减少精力.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.btn减少精力.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn减少精力.ForeColor = System.Drawing.Color.White;
            this.btn减少精力.Location = new System.Drawing.Point(343, 33);
            this.btn减少精力.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn减少精力.Name = "btn减少精力";
            this.btn减少精力.Size = new System.Drawing.Size(75, 24);
            this.btn减少精力.TabIndex = 3;
            this.btn减少精力.Text = "减少精力";
            this.btn减少精力.UseVisualStyleBackColor = false;
            // 
            // btn添加心情观察者
            // 
            this.btn添加心情观察者.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(66)))), ((int)(((byte)(193)))));
            this.btn添加心情观察者.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn添加心情观察者.ForeColor = System.Drawing.Color.White;
            this.btn添加心情观察者.Location = new System.Drawing.Point(96, 94);
            this.btn添加心情观察者.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn添加心情观察者.Name = "btn添加心情观察者";
            this.btn添加心情观察者.Size = new System.Drawing.Size(98, 24);
            this.btn添加心情观察者.TabIndex = 4;
            this.btn添加心情观察者.Text = "添加心情观察者";
            this.btn添加心情观察者.UseVisualStyleBackColor = false;
            // 
            // btn添加精力观察者
            // 
            this.btn添加精力观察者.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(126)))), ((int)(((byte)(20)))));
            this.btn添加精力观察者.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn添加精力观察者.ForeColor = System.Drawing.Color.White;
            this.btn添加精力观察者.Location = new System.Drawing.Point(200, 94);
            this.btn添加精力观察者.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn添加精力观察者.Name = "btn添加精力观察者";
            this.btn添加精力观察者.Size = new System.Drawing.Size(98, 24);
            this.btn添加精力观察者.TabIndex = 5;
            this.btn添加精力观察者.Text = "添加精力观察者";
            this.btn添加精力观察者.UseVisualStyleBackColor = false;
            // 
            // btn移除观察者
            // 
            this.btn移除观察者.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btn移除观察者.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn移除观察者.ForeColor = System.Drawing.Color.White;
            this.btn移除观察者.Location = new System.Drawing.Point(306, 94);
            this.btn移除观察者.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btn移除观察者.Name = "btn移除观察者";
            this.btn移除观察者.Size = new System.Drawing.Size(112, 24);
            this.btn移除观察者.TabIndex = 6;
            this.btn移除观察者.Text = "移除最后一个观察者";
            this.btn移除观察者.UseVisualStyleBackColor = false;
            // 
            // grp控制面板
            // 
            this.grp控制面板.Controls.Add(this.btn移除观察者);
            this.grp控制面板.Controls.Add(this.btn添加精力观察者);
            this.grp控制面板.Controls.Add(this.btn添加心情观察者);
            this.grp控制面板.Controls.Add(this.btn减少精力);
            this.grp控制面板.Controls.Add(this.btn增加精力);
            this.grp控制面板.Controls.Add(this.btn改变精力);
            this.grp控制面板.Controls.Add(this.btn改变心情);
            this.grp控制面板.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grp控制面板.Location = new System.Drawing.Point(15, 546);
            this.grp控制面板.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grp控制面板.Name = "grp控制面板";
            this.grp控制面板.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grp控制面板.Size = new System.Drawing.Size(536, 144);
            this.grp控制面板.TabIndex = 10;
            this.grp控制面板.TabStop = false;
            this.grp控制面板.Text = "🎮 控制面板";
            // 
            // grp显示面板
            // 
            this.grp显示面板.Controls.Add(this.txt状态显示);
            this.grp显示面板.Controls.Add(this.lst事件记录);
            this.grp显示面板.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.grp显示面板.Location = new System.Drawing.Point(15, 168);
            this.grp显示面板.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grp显示面板.Name = "grp显示面板";
            this.grp显示面板.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.grp显示面板.Size = new System.Drawing.Size(547, 374);
            this.grp显示面板.TabIndex = 9;
            this.grp显示面板.TabStop = false;
            this.grp显示面板.Text = "📊 观察者反馈";
            // 
            // 观察者模式
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(618, 701);
            this.Controls.Add(this.grp控制面板);
            this.Controls.Add(this.grp显示面板);
            this.Controls.Add(this.lbl精力文本);
            this.Controls.Add(this.lbl精力);
            this.Controls.Add(this.lbl精力标签);
            this.Controls.Add(this.lbl心情);
            this.Controls.Add(this.lbl心情标签);
            this.Controls.Add(this.lbl姓名);
            this.Controls.Add(this.lbl姓名标签);
            this.Controls.Add(this.lbl个人信息);
            this.Controls.Add(this.lbl标题);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "观察者模式";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "观察者模式演示 - 小高状态监控系统";
            ((System.ComponentModel.ISupportInitialize)(this.lbl精力)).EndInit();
            this.grp控制面板.ResumeLayout(false);
            this.grp显示面板.ResumeLayout(false);
            this.grp显示面板.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}