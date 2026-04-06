using AutoMapper;
using CheriesBlog.Domain.Dtos;
using CheriesBlog.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CheriesBlog.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BlogPost, PostToEditDto>().ReverseMap();
    }
}
