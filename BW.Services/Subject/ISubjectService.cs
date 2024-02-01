using BW.Models.Subject;

namespace BW.Services.Subject;

public interface ISubjectService
{
    public Task<SubjectListItem?> CreateSubject(SubjectCreate request);
}
