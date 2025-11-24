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
    public partial class CinemaSeatsForm : Form
    {
        #region 电影院座位
        private int[,] cinemaSeats;
        private const int ROWS = 5;
        private const int COLS = 10;
        private List<Button> selectedSeats = new List<Button>();

        #endregion
        public CinemaSeatsForm()
        {
            InitializeComponent();
            cinemaSeats = new int[ROWS, COLS];
            Init();
        }
        private void Init()
        {
            panel1.Controls.Clear();
            panel1.AutoScroll = true; 

            int seatSize = 40;
            int margin = 5;

            int totalWidth = COLS * (seatSize + margin);
            int totalHeight = ROWS * (seatSize + margin);

            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    Button seat = new Button
                    {
                        Text = $"{row + 1}-{col + 1}",
                        Tag = new Point(row, col),
                        Size = new Size(50, 50),
                        Location = new Point(60 + col * 50, 60 + row * 50),
                        BackColor = Color.LightGreen
                    };

                    seat.Click += (s, e) => {
                        Button btn = (Button)s;
                        Point pos = (Point)btn.Tag;

                        //if (cinemaSeats[pos.X, pos.Y] == 0)
                        //{
                        //    cinemaSeats[pos.X, pos.Y] = 1;
                        //    btn.BackColor = Color.Red;
                        //    MessageBox.Show($"成功预订座位 {btn.Text}");
                        //}
                        //else
                        //{
                        //    MessageBox.Show($"座位 {btn.Text} 已被预订");
                        //}

                        if(cinemaSeats[pos.X, pos.Y] == 0)
                        {
                            if (btn.BackColor == Color.LightGreen)
                            {
                                btn.BackColor = Color.Yellow;
                                selectedSeats.Add(btn);
                            }
                            else if (btn.BackColor == Color.Yellow)
                            {
                                btn.BackColor = Color.LightGreen; 
                                selectedSeats.Remove(btn);
                            }
                        }
                        else
                        {
                            MessageBox.Show($"座位 {btn.Text} 已被预订！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    };

                    panel1.Controls.Add(seat);
                }
            }
            label1.Text = $"剩余{CountAvailableSeats()}个座位";

        }
        private int CountAvailableSeats()
        {
            int count = 0;
            for (int row = 0; row < ROWS; row++)
            {
                for (int col = 0; col < COLS; col++)
                {
                    if (cinemaSeats[row, col] == 0) count++;
                }
            }
            return count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedSeats.Count == 0)
            {
                MessageBox.Show("请先选择座位！", "提示");
                return;
            }

            foreach(var set in selectedSeats)
            {
                Point point = (Point)set.Tag;
                cinemaSeats[point.X, point.Y] = 1;
                set.BackColor = Color.Red;
            }
            selectedSeats.Clear();
            MessageBox.Show("选中座位已成功预订！", "成功");
            label1.Text = $"剩余{CountAvailableSeats()}个座位";
        }
    }
}