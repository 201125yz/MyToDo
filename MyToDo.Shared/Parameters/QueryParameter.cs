using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Shared.Parameters
{
    /// <summary>
    /// 用于待办与备忘添加查询的实体类的定义
    /// </summary>
    public class QueryParameter
    {
        public int PageIndex {  get; set; }//页码
        public int PageSize { get; set; }//页数(返回的数据数量)
        public string? Search {  get; set; }//通用查询属性
    }
}
