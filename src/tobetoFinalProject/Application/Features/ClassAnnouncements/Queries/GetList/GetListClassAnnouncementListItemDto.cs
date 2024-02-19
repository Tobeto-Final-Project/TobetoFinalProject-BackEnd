using Core.Application.Dtos;

namespace Application.Features.ClassAnnouncements.Queries.GetList;

public class GetListClassAnnouncementListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid AnnouncementId { get; set; }
    public string AnnouncementName { get; set; }
    public string AnnouncementDescription { get; set; }
    public DateTime AnnouncementCreatedDate { get; set; }
}