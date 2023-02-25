using AutoMapper;
using InstagramClone.Api.Mappings;

namespace InstagramClone.UnitTests.Helpers;

public class AutomapperProfileHelper
{
    private static IMapper _mapper;

    public static IMapper CreateMapper()
    {
        if (_mapper != null)
        {
            return _mapper;
        }

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });

        _mapper = mappingConfig.CreateMapper();

        return _mapper;
    }
}