using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Dtos;
using Data.Entities;

namespace Business.Mapper;

public class ApplicationMapper : AutoMapper.Mapper
{
    public ApplicationMapper() : base(new ProvaConfigurationProvider())
    {
    }

    private class ProvaConfigurationProvider : MapperConfiguration
    {
        public ProvaConfigurationProvider() :
            base(cfg =>
            {
                var testmapping = cfg.CreateMap<Part, PartDto>();
                testmapping.ForMember
                (
                    dto => dto.Size2,
                    options => options.MapFrom(entity => entity.Size1 * 100)
                );
            }
            )
        {
        }
    }
}
