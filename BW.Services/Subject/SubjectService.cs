using BW.Data;
using BW.Data.Entities;
using BW.Models.Subject;

namespace BW.Services.Subject;

public class SubjectService : ISubjectService
{
    private readonly ApplicationDbContext _dbContext;
    
    public SubjectService(ApplicationDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<SubjectListItem?> CreateSubject(SubjectCreate request)
    {
        SubjectEntity entity = new(){
            Name = request.Name
        };

        _dbContext.Subjects.Add(entity);

        var numOfChanges = await _dbContext.SaveChangesAsync();
        if(numOfChanges != 1) {
            return null;
        }

        SubjectListItem response = new() {
            Id = entity.Id,
            Name = entity.Name
        };
        return response;

    }
}
