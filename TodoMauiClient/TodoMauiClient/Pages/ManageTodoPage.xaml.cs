using System.Diagnostics;
using TodoMauiClient.DataServices;
using TodoMauiClient.Models;

namespace TodoMauiClient.Pages;

[QueryProperty(nameof(Todo),"Todo")]
public partial class ManageTodoPage : ContentPage
{
	private readonly IRestDataService _dataService;

	Todo _todo;
	bool _isNew;

	public Todo Todo
	{
		get => _todo;
		set
		{
			_isNew = IsNew(value);
			_todo = value;
			OnPropertyChanged();
		}
	}

	public ManageTodoPage(IRestDataService dataService)
	{
		InitializeComponent();
		_dataService = dataService;
		BindingContext = this;
	}

	bool IsNew(Todo todo)
	{
		if(todo.Id == 0)
		{
			return true;	
		}
		return false;
	}

	async void OnSaveButtonClicked(object sender,EventArgs e)
	{
		if(_isNew)
		{
			Debug.WriteLine("---> Add New Item");
			await _dataService.AddTodoAsync(Todo);            
        }
		else
		{
            Debug.WriteLine("---> Update the available Item");
            await _dataService.UpdateTodoAsync(Todo);
        }
        await Shell.Current.GoToAsync("..");
    }

	async void OnDeleteButtonClicked(object sender, EventArgs e)
	{
		await _dataService.DeleteTodoAsync(Todo.Id);
		await Shell.Current.GoToAsync("..");
	}

	async void OnCancelButtonClicked(object sender,EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}