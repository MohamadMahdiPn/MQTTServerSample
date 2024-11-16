using MQTTServerSample.Domain.Entities.Sensors;

namespace MQTTServerSample.Application.DTOs.Sensors;

public class NameIdSensorRedis
{
    public NameIdSensorRedis()
    {

    }
    public NameIdSensorRedis(Sensor sensor)
    {
        Id = sensor.Id;
        SensorName = sensor.SensorName;
    }
    public Guid Id { get; set; }
    public string SensorName { get; set; }
}
