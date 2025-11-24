using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace WindowsFormsApp1
{
    public partial class HeadlinesForm : Form
    {
        private string articleContent = "";
        private string articleTitle = "";

        public HeadlinesForm()
        {
            InitializeComponent();
        }

        private void HeadlinesForm_Load(object sender, EventArgs e)
        {
            // 初始化界面
            txtUrl.Text = "https://bbs.hupu.com/634948645.html";
            //txtApiKey.Text = "密钥";
            txtResult.ScrollBars = ScrollBars.Vertical;
            txtResult.WordWrap = true;
        }

        private async void 获取文章_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("请输入有效的URL地址", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 显示加载状态
                txtResult.Text = "正在获取文章内容，请稍候...";
                Application.DoEvents();

                // 获取文章内容
                string htmlContent = await FetchWebContent(url);

                // 提取文章内容
                ExtractArticleContent(htmlContent);

                // 显示提取的内容
                txtResult.Text = $"文章标题: {articleTitle}\r\n\r\n文章内容:\r\n{articleContent}";

                MessageBox.Show("文章获取成功!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取文章失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtResult.Text = $"获取文章时发生错误: {ex.Message}";
            }
        }

        private async Task<string> FetchWebContent(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // 设置请求头，模拟浏览器访问
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        private void ExtractArticleContent(string htmlContent)
        {
            // 提取标题 - 针对网易体育新闻
            Match titleMatch = Regex.Match(htmlContent, @"<h1[^>]*>(.*?)</h1>", RegexOptions.IgnoreCase);
            if (!titleMatch.Success)
            {
                titleMatch = Regex.Match(htmlContent, @"<title[^>]*>(.*?)</title>", RegexOptions.IgnoreCase);
            }

            if (titleMatch.Success)
            {
                articleTitle = WebUtility.HtmlDecode(titleMatch.Groups[1].Value).Trim();
            }

            // 针对网易体育新闻的特定内容提取
            StringBuilder contentBuilder = new StringBuilder();

            // 方法1: 尝试提取正文内容区域
            Match contentMatch = Regex.Match(htmlContent, @"<div[^>]*class\s*=\s*[""']content[""'][^>]*>(.*?)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (contentMatch.Success)
            {
                // 从内容区域提取段落
                ExtractParagraphsFromContent(contentMatch.Groups[1].Value, contentBuilder);
            }
            else
            {
                // 方法2: 备用方案 - 直接提取所有段落
                MatchCollection contentMatches = Regex.Matches(htmlContent, @"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase);

                foreach (Match match in contentMatches)
                {
                    string paragraph = WebUtility.HtmlDecode(Regex.Replace(match.Groups[1].Value, "<[^>]+>", ""));
                    paragraph = paragraph.Trim();

                    // 过滤掉过短或无关的段落
                    if (paragraph.Length > 20 &&
                        !paragraph.Contains("function(") &&
                        !paragraph.Contains("var ") &&
                        !paragraph.Contains("广告") &&
                        !paragraph.Contains("举报"))
                    {
                        contentBuilder.AppendLine(paragraph);
                        contentBuilder.AppendLine(); // 空行分隔段落
                    }
                }
            }

            articleContent = contentBuilder.ToString();

            // 如果内容为空，尝试其他提取方法
            if (string.IsNullOrEmpty(articleContent))
            {
                // 方法3: 尝试提取包含中文文本的div
                ExtractChineseContent(htmlContent, contentBuilder);
                articleContent = contentBuilder.ToString();
            }

            // 如果内容过长，截取部分
            //if (articleContent.Length > 1000)
            //{
            //    articleContent = articleContent.Substring(0, 1000) + "...";
            //}
        }

        private void ExtractParagraphsFromContent(string contentHtml, StringBuilder builder)
        {
            MatchCollection paragraphMatches = Regex.Matches(contentHtml, @"<p[^>]*>(.*?)</p>", RegexOptions.IgnoreCase);

            foreach (Match match in paragraphMatches)
            {
                string paragraph = WebUtility.HtmlDecode(Regex.Replace(match.Groups[1].Value, "<[^>]+>", ""));
                paragraph = paragraph.Trim();

                if (paragraph.Length > 10 && !paragraph.Contains("广告"))
                {
                    builder.AppendLine(paragraph);
                    builder.AppendLine();
                }
            }
        }

        private void ExtractChineseContent(string htmlContent, StringBuilder builder)
        {
            // 提取包含中文字符的内容块
            Regex chineseRegex = new Regex(@">[^<]*[\u4e00-\u9fa5]+[^<]*<", RegexOptions.IgnoreCase);
            MatchCollection chineseMatches = chineseRegex.Matches(htmlContent);

            foreach (Match match in chineseMatches)
            {
                string text = match.Value.Trim('>', '<', ' ', '\t', '\n', '\r');
                text = WebUtility.HtmlDecode(text);

                if (text.Length > 20 && text.Contains("。") && !text.Contains("function"))
                {
                    builder.AppendLine(text);
                }
            }
        }


        private async void 生成文章_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(articleContent))
            {
                MessageBox.Show("请先获取文章内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                newtestResult.Text = "正在生成新文章，请稍候...";
                Application.DoEvents();

                // 构建AI提示
               string prompt = $"请根据以下文章内容，生成一篇新的体育新闻报道。要求：\n" +
                               $"1. 保持原文核心事实不变\n" +
                               $"2. 优化语言表达，使其更加生动有趣\n" +
                               $"3. 调整文章结构，使其更具吸引力\n" +
                               $"4. 适当增加一些背景信息和分析\n\n" +
                               $"原文内容：\n{articleContent}";

                // 调用免费的DeepSeek API
                string newArticle = await CallDeepSeekAPI(prompt);

                // 显示生成的文章
                newtestResult.Text = $"生成的新文章:\r\n\r\n{newArticle}";

                MessageBox.Show("文章生成成功!", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成文章失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                newtestResult.Text = $"生成文章时发生错误: {ex.Message}";
            }
        }

        private async Task<string> CallDeepSeekAPI(string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                // DeepSeek API端点 (免费)
                string apiUrl = "https://api.deepseek.com/v1/chat/completions";

                // 设置请求头
                client.DefaultRequestHeaders.Add("Authorization", "Bearer sk-free");
                client.DefaultRequestHeaders.Add("Accept", "application/json");

                // 构建请求数据
                var requestData = new
                {
                    model = "deepseek-chat", // 免费模型
                    messages = new[]
                    {
                new { role = "user", content = prompt }
            },
                    max_tokens = 2000,
                    temperature = 0.7,
                    stream = false
                };

                string jsonData = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // 发送请求
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                // 解析响应
                string responseJson = await response.Content.ReadAsStringAsync();
                dynamic responseData = JsonConvert.DeserializeObject(responseJson);

                // 提取生成的文本
                return responseData.choices[0].message.content.ToString();
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(newtestResult.Text))
            {
                Clipboard.SetText(newtestResult.Text);
                MessageBox.Show("内容已复制到剪贴板", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}