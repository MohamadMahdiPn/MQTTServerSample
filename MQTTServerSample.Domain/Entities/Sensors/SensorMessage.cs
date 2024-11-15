using MQTTServerSample.Domain.Bases;
using MQTTServerSample.Domain.Entities.IdentityModels;

namespace MQTTServerSample.Domain.Entities.Sensors;

public class SensorMessage : IBaseTable
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

    public virtual string Value { get; set; }
    public virtual Guid? SensorId { get; set; }
    public virtual Sensor Sensor { get; set; }

  

}