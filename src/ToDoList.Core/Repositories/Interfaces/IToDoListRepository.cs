using System.Collections.Generic;
using ToDoList.Core.Definitions.DalModels;

namespace ToDoList.Core.Repositories.Interfaces
{
    public interface IToDoListRepository
    {
        public IEnumerable<ToDoListItemDalModel> GetItems(int offset, int limit);
        
        public void Update(ToDoListItemDalModel toDoListItem);
        
        public void Add(ToDoListItemDalModel toDoListItem);
        
        public void Delete(int toDoListItemId);
    }
}