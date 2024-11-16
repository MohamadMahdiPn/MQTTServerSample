using MQTTServerSample.Application.DTOs.Bases;
using MQTTServerSample.Domain.Entities.Sensors;

namespace MQTTServerSample.Application.DTOs.Sensors;

public class SensorMessageDto : BaseDto
{
    public virtual string Value { get; set; }
    public virtual Guid? SensorId { get; set; }
    public string Payload { get; set; }
}