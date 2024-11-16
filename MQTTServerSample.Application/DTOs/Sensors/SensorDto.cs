using MQTTServerSample.Application.DTOs.Bases;
using MQTTServerSample.Domain.Enums;

namespace MQTTServerSample.Application.DTOs.Sensors;

public class SensorDto: BaseDto
{
    public string SensorName { get; set; }
    public SensorType SensorType { get; set; }
    public string SensorIp { get; set; }
}
