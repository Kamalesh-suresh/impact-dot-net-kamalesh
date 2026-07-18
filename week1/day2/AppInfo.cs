using System;

public class AppInfo
{
    #region Fields
    private string _version;
    #endregion

    #region Properties
    public string Version
    {
        get => _version;
        set => _version = value;
    }
    #endregion

    #region Constructors
    public AppInfo(string version)
    {
        _version = version;
    }
    #endregion

    #region Methods
    public void PrintInfo()
    {
        Console.WriteLine($"App version :{_version}");

    }
    #endregion

}