namespace MQTTServerSample.Domain.Responses;

public class BaseResponse<TEntity>
{

    public TEntity? DataItem { get; set; }
    public List<TEntity>? DataItems { get; set; }

    public List<string>? Errors { get; set; }
    public bool IsSucceeded { get; set; }
    public Guid? Id { get; set; }
}