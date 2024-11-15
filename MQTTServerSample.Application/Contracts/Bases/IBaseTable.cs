using MQTTServerSample.Domain.Entities.IdentityModels;
using System.ComponentModel.DataAnnotations;

namespace MQTTServerSample.Application.Contracts.Bases;

public interface IBaseTable
{
    [Key]
    Guid Id { get; set; }
    bool IsActive { get; set; }
    bool IsDeleted { get; set; }
    DateTime CreatedDate { get; set; }
    DateTime? ModifiedDate { get; set; }
    string UserId { get; set; }
    ApplicationUser User { get; set; }
    string? CreatorUserId { get; set; }
    ApplicationUser CreatorUser { get; set; }
}