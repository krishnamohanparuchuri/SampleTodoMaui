using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoMauiClient.Models;

namespace TodoMauiClient.DataServices
{
    public interface IRestDataService
    {
        Task<List<Todo>> GetAllTodosAsync();
        Task AddTodoAsync(Todo todo);
        Task DeleteTodoAsync(int id);
        Task UpdateTodoAsync(Todo todo);
    }
}
