using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace To_do_part_3;

public class ToDo
{
    private int _id;
    private string _task;
    private string _description;
    private bool _isDone;

    public int Id
    {
        get => _id;
        set => _id = value;
    }
    public string Task
    {
        get => _task;
        set => _task = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public bool IsDone
    {
        get => _isDone;
        set => _isDone = value;
    }

    public ToDo()
    {
        _task = "";
        _description = "";
        _isDone = false;
    }

    public ToDo(int id,string task, string description, bool isDone)
    {   
        _id = id;
        _task = task;
        _description = description;
        _isDone = isDone;
    }
}