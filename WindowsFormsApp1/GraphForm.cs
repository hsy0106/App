using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class GraphForm : Form
    {
        class City
        {
            public string Name { get; set; }
            public Point Location { get; set; }
        }

        class Road
        {
            public City From { get; set; }
            public City To { get; set; }
            public int Distance { get; set; }
        }

        private List<City> cities = new List<City>();
        private List<Road> roads = new List<Road>();

        public GraphForm()
        {
            InitializeComponent();
            this.ClientSize = new Size(600, 400);

            // 初始化城市和道路
            InitializeCitiesAndRoads();

            // 添加控件
            ComboBox cbFrom = new ComboBox { Location = new Point(20, 20), Width = 150 };
            ComboBox cbTo = new ComboBox { Location = new Point(180, 20), Width = 150 };
            Button btnFindPath = new Button { Text = "查找最短路径", Location = new Point(340, 20) };
            Panel pnMap = new Panel { Location = new Point(20, 60), Size = new Size(560, 320), BackColor = Color.White };

            // 填充城市下拉框
            foreach (var city in cities)
            {
                cbFrom.Items.Add(city.Name);
                cbTo.Items.Add(city.Name);
            }
            cbFrom.SelectedIndex = 0;
            cbTo.SelectedIndex = 1;

            // 绘制地图
            pnMap.Paint += (s, e) => {
                Graphics g = e.Graphics;

                // 绘制道路
                foreach (var road in roads)
                {
                    g.DrawLine(Pens.LightGray, road.From.Location, road.To.Location);
                    Point midPoint = new Point(
                        (road.From.Location.X + road.To.Location.X) / 2,
                        (road.From.Location.Y + road.To.Location.Y) / 2);
                    g.DrawString(road.Distance.ToString(), this.Font, Brushes.Black, midPoint);
                }

                // 绘制城市
                foreach (var city in cities)
                {
                    g.FillEllipse(Brushes.Red, city.Location.X - 5, city.Location.Y - 5, 10, 10);
                    g.DrawString(city.Name, this.Font, Brushes.Black, city.Location.X + 10, city.Location.Y - 10);
                }
            };

            // 查找最短路径
            btnFindPath.Click += (s, e) => {
                string from = cbFrom.SelectedItem.ToString();
                string to = cbTo.SelectedItem.ToString();

                List<City> path = FindShortestPath(from, to);

                if (path != null)
                {
                    string message = "最短路径: ";
                    foreach (var city in path)
                    {
                        message += city.Name + " -> ";
                    }
                    message = message.Substring(0, message.Length - 4);
                    MessageBox.Show(message);

                    // 高亮显示路径
                    pnMap.Paint += (s2, e2) => {
                        Graphics g = e2.Graphics;
                        Pen highlightPen = new Pen(Color.Blue, 2);

                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            City city1 = path[i];
                            City city2 = path[i + 1];
                            g.DrawLine(highlightPen, city1.Location, city2.Location);
                        }
                    };
                    pnMap.Invalidate();
                }
                else
                {
                    MessageBox.Show("没有找到路径");
                }
            };

            this.Controls.Add(cbFrom);
            this.Controls.Add(cbTo);
            this.Controls.Add(btnFindPath);
            this.Controls.Add(pnMap);
        }

        private void InitializeCitiesAndRoads()
        {
            // 添加城市
            cities.Add(new City { Name = "北京", Location = new Point(100, 100) });
            cities.Add(new City { Name = "上海", Location = new Point(300, 150) });
            cities.Add(new City { Name = "广州", Location = new Point(200, 250) });
            cities.Add(new City { Name = "深圳", Location = new Point(250, 280) });
            cities.Add(new City { Name = "成都", Location = new Point(150, 200) });

            // 添加道路
            AddRoad("北京", "上海", 1200);
            AddRoad("北京", "成都", 1500);
            AddRoad("上海", "广州", 1400);
            AddRoad("广州", "深圳", 150);
            AddRoad("广州", "成都", 1300);
            AddRoad("深圳", "成都", 1400);
        }

        private void AddRoad(string from, string to, int distance)
        {
            City cityFrom = cities.Find(c => c.Name == from);
            City cityTo = cities.Find(c => c.Name == to);

            if (cityFrom != null && cityTo != null)
            {
                roads.Add(new Road { From = cityFrom, To = cityTo, Distance = distance });
                roads.Add(new Road { From = cityTo, To = cityFrom, Distance = distance }); // 双向道路
            }
        }

        // 使用Dijkstra算法查找最短路径
        private List<City> FindShortestPath(string from, string to)
        {
            City start = cities.Find(c => c.Name == from);
            City end = cities.Find(c => c.Name == to);

            if (start == null || end == null) return null;

            Dictionary<City, int> distances = new Dictionary<City, int>();
            Dictionary<City, City> previous = new Dictionary<City, City>();
            List<City> unvisited = new List<City>();

            // 初始化
            foreach (var city in cities)
            {
                distances[city] = int.MaxValue;
                previous[city] = null;
                unvisited.Add(city);
            }
            distances[start] = 0;

            while (unvisited.Count > 0)
            {
                // 找到未访问节点中距离最小的
                City current = null;
                int minDistance = int.MaxValue;
                foreach (var city in unvisited)
                {
                    if (distances[city] < minDistance)
                    {
                        minDistance = distances[city];
                        current = city;
                    }
                }

                if (current == null || current == end) break;
                unvisited.Remove(current);

                // 更新邻居距离
                foreach (var road in roads)
                {
                    if (road.From == current)
                    {
                        City neighbor = road.To;
                        int alt = distances[current] + road.Distance;
                        if (alt < distances[neighbor])
                        {
                            distances[neighbor] = alt;
                            previous[neighbor] = current;
                        }
                    }
                }
            }

            // 构建路径
            List<City> path = new List<City>();
            City temp = end;
            while (temp != null)
            {
                path.Insert(0, temp);
                temp = previous[temp];
            }

            return path[0] == start ? path : null;
        }
    }
}