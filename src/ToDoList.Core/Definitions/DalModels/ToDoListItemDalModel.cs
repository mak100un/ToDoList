using System;
using ToDoList.Core.Definitions.Enums;

namespace ToDoList.Core.Definitions.DalModels;

public class ToDoListItemDalModel
{
    public int Id { get; set; }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public ToDoTaskStatus Status { get; set; }
    
    // TODO Required? Notnull
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}