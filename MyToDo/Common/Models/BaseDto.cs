using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Common.Models
{
    public class BaseDto : BindableBase
    {
        /// <summary>
        /// 公共实体类
        /// </summary>
        /// 
        //定义标识id属性
        private int id;
        public int Id 
        { 
            get { return id; }
            set { id = value; }
        }

        //定义时间属性
        private DateTime updateDate;
        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        private DateTime creatDate;
        public DateTime CreatDate
        {
            get { return creatDate; }
            set { creatDate = value; }
        }
    }
}
