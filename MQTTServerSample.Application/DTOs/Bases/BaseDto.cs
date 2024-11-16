namespace MQTTServerSample.Application.DTOs.Bases;

public abstract class BaseDto
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
