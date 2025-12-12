using AutoMapper.Configuration;
using MyToDo.Api.Context;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using AutoMapper;

namespace MyToDo.Api.Extensions
{
    /// <summary>
    /// 配置映射关系，即ToDo与ToDoDto自动转换,Memo与MemoDto自动转换,User与UserDto自动转换
    /// </summary>
    public class AutoMapperProFile : Profile
    {
        public AutoMapperProFile() 
        {
            CreateMap<ToDo, ToDoDto>().ReverseMap();
            CreateMap<Memo, MemoDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
