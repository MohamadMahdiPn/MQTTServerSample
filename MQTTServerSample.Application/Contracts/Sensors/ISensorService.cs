using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Responses;

namespace MQTTServerSample.Application.Contracts.Sensors;

public interface ISensorService
{
    Task<BaseResponse<NameIdSensorRedis>> SaveListToRedis();
    Task<BaseResponse<NameIdSensorRedis>> ReadListFromRedis();
    Task<BaseResponse<NameIdSensorRedis>> ReadIdFromNameRedis(string sensorName);
    Task<BaseResponse<SensorDto>> AddNew(SensorDto sensorDto);
    Task<BaseResponse<SensorDto>> CheckExists(string sensorName);

    Task<BaseResponse<SensorMessageDto>> AddNewMessage(SensorMessageDto sensorMessageDto);
}
