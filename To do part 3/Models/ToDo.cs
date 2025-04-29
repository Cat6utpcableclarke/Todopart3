using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace To_do_part_3;

public class ToDo : INotifyPropertyChanged
{
    private int _itemId;
    private string _itemName = string.Empty; // Initialize to avoid nullability warnings
    private string _itemDescription = string.Empty; // Initialize to avoid nullability warnings
    private string _status = string.Empty; // Initialize to avoid nullability warnings
    private int _userId;
    private DateTime _timeModified;

    public ToDo(int itemId, string itemName, string itemDescription, string status, int userId, DateTime timeModified)
    {
        ItemId = itemId;
        ItemName = itemName;
        ItemDescription = itemDescription;
        Status = status;
        UserId = userId;
        TimeModified = timeModified;
    }

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

    public event PropertyChangedEventHandler? PropertyChanged; // Mark as nullable to avoid nullability warnings

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
