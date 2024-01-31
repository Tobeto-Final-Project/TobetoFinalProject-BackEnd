using Core.Application.Dtos;

namespace Application.Features.StudentExperiences.Queries.GetList;

public class GetListStudentExperienceListItemDto : IDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public string CompanyName { get; set; }
    public string Sector { get; set; }
    public string Position { get; set; }
    public string StudentFirstName { get; set; }
    public string StudentLastName { get; set; }
    public string StudentEmail { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }
    public string CityName { get; set; }
}