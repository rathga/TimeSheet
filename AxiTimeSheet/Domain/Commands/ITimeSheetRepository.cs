namespace AxiTimeSheet.Domain.Commands;

public interface ITimeSheetRepository
{
    TimeSheet Get();
    void SaveChanges();
}