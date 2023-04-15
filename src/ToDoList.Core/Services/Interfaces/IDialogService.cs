using System.Threading.Tasks;

namespace ToDoList.Core.Services.Interfaces
{
    public interface IDialogService
    {
        // TODO What if more arguments?
        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    }
}