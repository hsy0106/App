using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace App.Server
{
    public static class ConfigServer
    {

        public static IHostApplicationBuilder AddIniConfiguration(this IHostApplicationBuilder builder, string path)
        {
            builder.Configuration.AddIniFile(
                Path.Combine(Environment.CurrentDirectory, path)
            );
            return builder;
        }
        /// <summary>
        /// 添加配置选项
        /// </summary>
        /// <typeparam name="TOptions">选项类型</typeparam>
        /// <param name="builder"></param>
        /// <param name="path">配置路径 默认为类名(去除Options后缀)</param>
        /// <returns>当前配置结果</returns>
        public static TOptions AddOptions<TOptions>(this IHostApplicationBuilder builder, string path = null) where TOptions : class, new()
        {
            if (string.IsNullOrEmpty(path))
            {
                path = typeof(TOptions).Name.Replace("Options", "");
            }

            IConfiguration section = builder.Configuration.GetSection(path);
            builder.Services.Configure<TOptions>(section);
            builder.Services.AddSingleton(provider =>
            {
                var options = provider.GetRequiredService<IOptions<TOptions>>();
                return options.Value;
            });

            TOptions result = new TOptions();
            section.Bind(result);
            return result;
        }
    }
}
