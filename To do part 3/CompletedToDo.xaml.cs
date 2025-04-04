using System.Collections.ObjectModel;
using System.Diagnostics;

namespace To_do_part_3;

public partial class CompletedToDo : ContentPage
{
    private ObservableCollection<ToDo> toDoList = new ObservableCollection<ToDo>();

    public CompletedToDo()
	{
		InitializeComponent();
        toDoList.Add(new ToDo(1, "Task1", "task1", true));
        toDoList.Add(new ToDo(2, "Task2", "task2", true));
        toDoList.Add(new ToDo(3, "Task3", "task3", true));
        CToDos.ItemsSource = toDoList;
    }


    private void Delete_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine("Delete Clicked");
    }
}