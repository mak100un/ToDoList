using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using ToDoList.Core.Definitions.DalModels;
using ToDoList.Core.Repositories.Interfaces;

namespace ToDoList.Core.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly Lazy<ILiteDatabase> _liteDatabase;

        private ILiteCollection<ToDoListItemDalModel> _dbCollection;

        private ILiteCollection<ToDoListItemDalModel> DbCollection => _dbCollection ??= _liteDatabase.Value.GetCollection<ToDoListItemDalModel>();

        public ToDoListRepository(Lazy<ILiteDatabase> liteDatabase)
        {
            _liteDatabase = liteDatabase;
        }

        public IEnumerable<ToDoListItemDalModel> GetItems(int offset, int limit) => DbCollection.FindAll().OrderByDescending(item => item.CreatedAt).Skip(offset).Take(limit).ToArray();

        public void Update(ToDoListItemDalModel toDoListItem) => DbCollection.Update(toDoListItem);

        public void Add(ToDoListItemDalModel toDoListItem) => DbCollection.Insert(toDoListItem);

        public void Delete(int toDoListItemId) => DbCollection.Delete(toDoListItemId);
    }
}
