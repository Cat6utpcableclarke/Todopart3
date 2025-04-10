
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace To_do_part_3;

public partial class ToDoPage : ContentPage
{
	private ObservableCollection<ToDo> toDoList = new ObservableCollection<ToDo>();

    public ToDoPage()
	{
		InitializeComponent();
        Shell.SetNavBarIsVisible(this, false);
        //toDoList.Add(new ToDo (1,"Task1","task1", false));
        //toDoList.Add(new ToDo (2,"Task2","task2", false));
        //toDoList.Add(new ToDo (3,"Task3", "task3", false));
        //ToDos.ItemsSource = toDoList;
    }

	

	private async void Edit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new EditToDo(), true);
        Debug.WriteLine("Edit Clicked");

    }

	private void Delete_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Delete Clicked");
    }

    private async void Add_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Add Clicked");
        //await Shell.Current.GoToAsync("///AddToDo");
        await Navigation.PushModalAsync(new AddToDoPage(), true);
    }

    private void Done_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Done Clicked");
        var button = (Button)sender;
        var toDo = (ToDo)button.CommandParameter;
        //Debug.WriteLine(toDo.Task);
    }
}