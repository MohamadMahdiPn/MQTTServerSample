using AutoMapper;
using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Entities.Sensors;

namespace MQTTServerSample.Application.Mapper;

public class MappingProfile:Profile
{
	public MappingProfile()
	{
        CreateMap<Sensor, SensorDto>().ReverseMap();
        
    }
}
