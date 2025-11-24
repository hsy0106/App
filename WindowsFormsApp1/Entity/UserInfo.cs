using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Entity
{
    [SugarTable("TestResult")]
    public  class UserInfo
    {

        [SugarColumn(ColumnName = "Id", IsPrimaryKey = true, IsIdentity = true)]
        public long Id { get; set; }
        [SugarColumn(ColumnName = "UserName")]
        public string UserName { get; set; }
        [SugarColumn(ColumnName = "PassWord")]
        public string PassWord { get; set; }

        [SugarColumn(ColumnName = "CurrentIp")]
        public string CurrentIp { get; set; }

    }
}
