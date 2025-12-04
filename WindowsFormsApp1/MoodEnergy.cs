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
    public partial class MoodEnergy : Form
    {
        private Person _小高;
        private List<IObserver> _observers;

        public MoodEnergy()
        {
            InitializeComponent();
            InitializePerson();
            InitializeObservers();

            // 手动绑定事件（确保事件正确绑定）
            BindEvents();
        }

        private void BindEvents()
        {
            this.btn改变心情.Click += new EventHandler(btn改变心情_Click);
            this.btn改变精力.Click += new EventHandler(btn改变精力_Click);
            this.btn增加精力.Click += new EventHandler(btn增加精力_Click);
            this.btn减少精力.Click += new EventHandler(btn减少精力_Click);
            this.btn添加心情观察者.Click += new EventHandler(btn添加心情观察者_Click);
            this.btn添加精力观察者.Click += new EventHandler(btn添加精力观察者_Click);
            this.btn移除观察者.Click += new EventHandler(btn移除观察者_Click);
            this.FormClosing += new FormClosingEventHandler(观察者模式_FormClosing);
        }

        private void InitializePerson()
        {
            _小高 = new Person
            {
                Name = "小高",
                Mood = "开心",
                Energy = 90
            };
        }

        private void InitializeObservers()
        {
            _observers = new List<IObserver>();

            // 创建不同类型的观察者
            var moodObserver = new MoodObserver("心情监控器", txt状态显示, lst事件记录);
            var energyObserver = new EnergyObserver("精力监控器", txt状态显示, lst事件记录);
            var comprehensiveObserver = new ComprehensiveObserver("综合监控器", txt状态显示, lst事件记录);

            // 注册观察者
            _observers.Add(moodObserver);
            _observers.Add(energyObserver);
            _observers.Add(comprehensiveObserver);

            // 附加到被观察者
            foreach (var observer in _observers)
            {
                _小高.Attach(observer);
            }

            UpdatePersonInfo();

            // 显示观察者信息
            foreach (var observer in _observers.OfType<IDisplay>())
            {
                observer.Display();
            }
        }

        private void UpdatePersonInfo()
        {
            // 确保在UI线程上更新
            if (lbl姓名.InvokeRequired)
            {
                this.Invoke(new Action(UpdatePersonInfo));
                return;
            }

            lbl姓名.Text = _小高.Name;
            lbl心情.Text = _小高.Mood;
            lbl精力.Value = _小高.Energy;  
            lbl精力文本.Text = _小高.Energy.ToString();

            // 根据精力值改变颜色
            var colorRules = new Dictionary<Func<int, bool>, Color>
            {
                { e => e >= 80, Color.Green },
                { e => e >= 60, Color.Blue },
                { e => e >= 40, Color.Orange },
                { e => e >= 20, Color.OrangeRed }
            };

            lbl精力文本.ForeColor = Color.Black;
        }

        #region 接口定义
        // 被观察者接口
        public interface IObservable
        {
            //订阅
            void Attach(IObserver observer);
            //取消订阅
            void Detach(IObserver observer);
            void Notify();
        }

        // 观察者接口
        public interface IObserver
        {
            void Update(IObservable observable);
        }

        // 显示接口
        public interface IDisplay
        {
            void Display();
        }
        #endregion

        #region 具体被观察者类
        // 人物类 - 被观察者
        public class Person : IObservable
        {
            private List<IObserver> _observers = new List<IObserver>();
            private string _name;
            private string _mood;
            private int _energy;

            public string Name
            {
                get => _name;
                set
                {
                    if (_name != value)
                    {
                        _name = value;
                        Notify();
                    }
                }
            }

            public string Mood
            {
                get => _mood;
                set
                {
                    if (_mood != value)
                    {
                        _mood = value;
                        Notify();
                    }
                }
            }

            public int Energy
            {
                get => _energy;
                set
                {
                    if (_energy != value)
                    {
                        _energy = value;
                        Notify();
                    }
                }
            }

            public void Attach(IObserver observer)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }
            }

            public void Detach(IObserver observer)
            {
                _observers.Remove(observer);
            }

            public void Notify()
            {
                foreach (var observer in _observers)
                {
                    observer.Update(this);
                }
            }

            public override string ToString()
            {
                return $"{Name} [心情: {Mood}, 精力: {Energy}]";
            }
        }
        #endregion

        #region 具体观察者类
        // 基础观察者（可被继承）
        public abstract class BaseObserver : IObserver
        {
            protected string _name;
            protected TextBox _displayTextBox;
            protected ListBox _eventListBox;

            public BaseObserver(string name, TextBox displayTextBox, ListBox eventListBox)
            {
                _name = name;
                _displayTextBox = displayTextBox;
                _eventListBox = eventListBox;
            }

            public abstract void Update(IObservable observable);

            protected void AddMessage(string message)
            {
                if (_displayTextBox.InvokeRequired)
                {
                    _displayTextBox.Invoke(new Action(() =>
                    {
                        _displayTextBox.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\r\n");
                        _displayTextBox.SelectionStart = _displayTextBox.Text.Length;
                        _displayTextBox.ScrollToCaret();
                    }));
                }
                else
                {
                    _displayTextBox.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\r\n");
                    _displayTextBox.SelectionStart = _displayTextBox.Text.Length;
                    _displayTextBox.ScrollToCaret();
                }
            }

            protected void AddEvent(string eventMessage)
            {
                if (_eventListBox.InvokeRequired)
                {
                    _eventListBox.Invoke(new Action(() =>
                    {
                        _eventListBox.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} - {eventMessage}");
                        if (_eventListBox.Items.Count > 50) _eventListBox.Items.RemoveAt(50);
                    }));
                }
                else
                {
                    _eventListBox.Items.Insert(0, $"{DateTime.Now:HH:mm:ss} - {eventMessage}");
                    if (_eventListBox.Items.Count > 50) _eventListBox.Items.RemoveAt(50);
                }
            }
        }

        // 心情观察者
        public class MoodObserver : BaseObserver, IDisplay
        {
            private string _lastMood;

            public MoodObserver(string name, TextBox displayTextBox, ListBox eventListBox)
                : base(name, displayTextBox, eventListBox)
            {
            }

            public override void Update(IObservable observable)
            {
                if (observable is Person person)
                {
                    if (_lastMood != person.Mood)
                    {
                        string message = $"[{_name}] 检测到心情变化: {_lastMood} -> {person.Mood}";
                        AddMessage(message);
                        AddEvent(message);
                        _lastMood = person.Mood;

                        Display();
                    }
                }
            }

            public void Display()
            {
                AddMessage($"[{_name}] 我是专门监控心情的观察者！");
            }
        }

        // 精力观察者
        public class EnergyObserver : BaseObserver, IDisplay
        {
            private int _lastEnergy;

            public EnergyObserver(string name, TextBox displayTextBox, ListBox eventListBox)
                : base(name, displayTextBox, eventListBox)
            {
            }

            public override void Update(IObservable observable)
            {
                //类型模式 + 声明模式
                if (observable is Person person)
                {
                    if (_lastEnergy != person.Energy)
                    {
                        string energyStatus = GetEnergyStatus(person.Energy);
                        string message = $"[{_name}] 精力变化: {_lastEnergy} -> {person.Energy} ({energyStatus})";
                        AddMessage(message);
                        AddEvent(message);
                        _lastEnergy = person.Energy;

                        Display();
                    }
                }
            }

            private string GetEnergyStatus(int energy)
            {
                switch (energy)
                {
                    case var e when e >= 80:
                        return "精力充沛";
                    case var e when e >= 60:
                        return "状态良好";
                    case var e when e >= 40:
                        return "有些疲惫";
                    case var e when e >= 20:
                        return "非常疲惫";
                    default:
                        return "精疲力尽";
                }
            }

            public void Display()
            {
                AddMessage($"[{_name}] 我专门监控精力水平！");
            }
        }

        // 综合观察者
        public class ComprehensiveObserver : BaseObserver, IDisplay
        {
            //构造函数新写法
            public ComprehensiveObserver(string name, TextBox displayTextBox, ListBox eventListBox)
                : base(name, displayTextBox, eventListBox)
            {
            }

            public override void Update(IObservable observable)
            {
                if (observable is Person person)
                {
                    string message = $"[{_name}] 综合报告: {person}";
                    AddMessage(message);
                    AddEvent(message);

                    Display();
                }
            }

            public void Display()
            {
                AddMessage($"[{_name}] 我是综合监控观察者！");
            }
        }
        #endregion

        #region 事件处理方法
        private void btn改变心情_Click(object sender, EventArgs e)
        {
            string[] moods = { "开心", "生气", "悲伤", "兴奋", "平静", "紧张", "困惑", "满足" };
            var random = new Random();
            _小高.Mood = moods[random.Next(moods.Length)];
            UpdatePersonInfo();
        }

        private void btn改变精力_Click(object sender, EventArgs e)
        {
            var random = new Random();
            _小高.Energy = random.Next(0, 101);
            UpdatePersonInfo();
        }

        private void btn增加精力_Click(object sender, EventArgs e)
        {
            _小高.Energy = Math.Min(100, _小高.Energy + 10);
            UpdatePersonInfo();
        }

        private void btn减少精力_Click(object sender, EventArgs e)
        {
            _小高.Energy = Math.Max(0, _小高.Energy - 10);
            UpdatePersonInfo();
        }

        private void btn添加心情观察者_Click(object sender, EventArgs e)
        {
            var newObserver = new MoodObserver($"心情观察者{_observers.Count + 1}", txt状态显示, lst事件记录);
            _observers.Add(newObserver);
            _小高.Attach(newObserver);
            newObserver.Display();
        }

        private void btn添加精力观察者_Click(object sender, EventArgs e)
        {
            var newObserver = new EnergyObserver($"精力观察者{_observers.Count + 1}", txt状态显示, lst事件记录);
            _observers.Add(newObserver);
            _小高.Attach(newObserver);
            newObserver.Display();
        }

        private void btn移除观察者_Click(object sender, EventArgs e)
        {
            if (_observers.Count > 3) // 保留基本的3个观察者
            {
                var lastObserver = _observers.Last();
                _小高.Detach(lastObserver);
                _observers.Remove(lastObserver);

                AddMessage($"移除了观察者，当前观察者数量: {_observers.Count}");
            }
        }

        private void AddMessage(string message)
        {
            if (txt状态显示.InvokeRequired)
            {
                txt状态显示.Invoke(new Action(() =>
                {
                    txt状态显示.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\r\n");
                    txt状态显示.SelectionStart = txt状态显示.Text.Length;
                    txt状态显示.ScrollToCaret();
                }));
            }
            else
            {
                txt状态显示.AppendText($"{DateTime.Now:HH:mm:ss} - {message}\r\n");
                txt状态显示.SelectionStart = txt状态显示.Text.Length;
                txt状态显示.ScrollToCaret();
            }
        }

        private void 观察者模式_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (var observer in _observers)
            {
                _小高.Detach(observer);
            }
        }
        #endregion
    }
}