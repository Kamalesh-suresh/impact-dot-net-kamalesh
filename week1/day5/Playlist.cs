using System;

public class Playlist
{
    private readonly string[] _tracks = new string[5];

    public string this[int index]
    {
        get
        {
            if(index<0 || index >= _tracks.Length)
                throw new IndexOutOfRangeException();
            return _tracks[index];

        }
        set
        {
            if (index < 0 || index >= _tracks.Length)
                throw new IndexOutOfRangeException();
            _tracks[index]=value;
        }
    }

    public string this[string trackname] => Array.Find(_tracks, testc => testc == trackname) ?? "Not Found";


}
