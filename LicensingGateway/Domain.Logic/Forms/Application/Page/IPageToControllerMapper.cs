using Domain.Models.Database;

namespace Domain.Logic.Forms.Application.Page;
public interface IPageToControllerMapper
{
    string Map(ApplicationPage page);
}
