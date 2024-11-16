using MQTTServerSample.Application.DTOs.Sensors;
using MQTTServerSample.Domain.Responses;

namespace MQTTServerSample.Application.Contracts.Sensors;

public interface ISensorService
{
    Task<BaseResponse<SensorDto>> AddNew(SensorDto sensorDto);
    Task<BaseResponse<SensorDto>> CheckExists(string sensorName);
}
