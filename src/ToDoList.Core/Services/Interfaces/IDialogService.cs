using System.Threading.Tasks;

namespace ToDoList.Core.Services.Interfaces
{
    public interface IDialogService
    {
        Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    }
}
