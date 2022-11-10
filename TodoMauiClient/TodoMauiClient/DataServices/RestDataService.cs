using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TodoMauiClient.Models;
using System.Diagnostics;

namespace TodoMauiClient.DataServices
{
    public class RestDataService : IRestDataService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseAddress;
        private readonly string _url;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public RestDataService(HttpClient httpClient)
        {
            //_httpClient = new HttpClient();
            _httpClient = httpClient;
            _baseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5125" : "https://localhost:7125";
            _url = $"{_baseAddress}/api";

            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
        }
        public async Task AddTodoAsync(Todo todo)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("---> No Internet access...");
                return;
            }
            try
            {
                string jsonTodo = JsonSerializer.Serialize<Todo>(todo,_jsonSerializerOptions);
                StringContent content = new StringContent(jsonTodo,Encoding.UTF8,"application/json");
                HttpResponseMessage response = await _httpClient.PostAsync($"{_url}/todos",content);

                if(response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Successfully created Todo");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("---> Non http 2xx response");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Whoops exception : {ex.Message}");
            }

        }

        public async Task DeleteTodoAsync(int id)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("---> No Internet access...");
                return;
            }
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"{_url}/todos/{id}");
                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Successfully deleted Todo");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("---> Non http 2xx response");
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Whoops exception : {e.Message}");
            }

        }

        public async Task<List<Todo>> GetAllTodosAsync()
        {
            List<Todo> todos = new List<Todo>();
            if(Connectivity.Current.NetworkAccess  != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("---> No Internet access...");
                return todos;
            }

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"{_url}/todos");
                if(response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    todos = JsonSerializer.Deserialize<List<Todo>>(content,_jsonSerializerOptions);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("---> Non http 2xx response");
                }
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Whoops exception : {e.Message}");
            }

            return todos;
        }

        public async Task UpdateTodoAsync(Todo todo)
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                System.Diagnostics.Debug.WriteLine("---> No Internet access...");
                return;
            }
            try
            {
                string jsonTodo = JsonSerializer.Serialize<Todo>(todo, _jsonSerializerOptions);
                StringContent content = new StringContent(jsonTodo, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PutAsync($"{_url}/todos/{todo.Id}", content);

                if (response.IsSuccessStatusCode)
                {
                    System.Diagnostics.Debug.WriteLine("Successfully updated Todo");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("---> Non http 2xx response");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Whoops exception : {ex.Message}");
            }
        }
    }
}
