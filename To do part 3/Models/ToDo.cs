using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace To_do_part_3;

public class ToDo : INotifyPropertyChanged
{
    private int _itemId;
    private string _itemName;
    private string _itemDescription;
    private string _status;
    private int _userId;
    private DateTime _timeModified;

    public int ItemId
    {
        get { return _itemId; }
        set { _itemId = value; OnPropertyChanged(nameof(ItemId)); }
    }

    public string ItemName
    {
        get { return _itemName; }
        set { _itemName = value; OnPropertyChanged(nameof(ItemName)); }
    }

    public string ItemDescription
    {
        get { return _itemDescription; }
        set { _itemDescription = value; OnPropertyChanged(nameof(ItemDescription)); }
    }

    public string Status
    {
        get { return _status; }
        set { _status = value; OnPropertyChanged(nameof(Status)); }
    }

    public int UserId
    {
        get { return _userId; }
        set { _userId = value; OnPropertyChanged(nameof(UserId)); }
    }

    public DateTime TimeModified
    {
        get { return _timeModified; }
        set { _timeModified = value; OnPropertyChanged(nameof(TimeModified)); }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}