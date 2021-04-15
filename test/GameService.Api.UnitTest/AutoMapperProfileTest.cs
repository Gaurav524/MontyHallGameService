using AutoMapper;
using Xunit;

namespace GameService.Api.UnitTest
{
    public  class AutoMapperProfileTest
    {
        [Fact]
        public void AutoMapper_CorrectConfiguration_WillNotThrowException()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Converters.AutoMapperProfile>();
            });

            var mapper = config.CreateMapper();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}
