using MQTTServerSample.Domain.Bases;
using MQTTServerSample.Domain.Entities.IdentityModels;
using MQTTServerSample.Domain.Enums;

namespace MQTTServerSample.Domain.Entities.Sensors;

public class Sensor:IBaseTable
{
    public virtual Guid Id { get; set; }
    public virtual bool IsActive { get; set; }
    public virtual bool IsDeleted { get; set; }
    public virtual DateTime CreatedDate { get; set; }
    public virtual DateTime? ModifiedDate { get; set; }
    public virtual string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }
    public virtual string? CreatorUserId { get; set; }
    public virtual ApplicationUser CreatorUser { get; set; }

    public string SensorName { get; set; }
    public SensorType SensorType { get; set; }
    public string SensorIp { get; set; }

}