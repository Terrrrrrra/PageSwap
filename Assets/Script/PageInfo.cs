public class PageInfo
{
    internal int _rank = -1;
    internal string _page = "";

    internal (int, string) GetInfo()
    {
        return (_rank, _page);
    }

    internal PageInfo() { }

    internal PageInfo((int, string) info)
    {
        _rank = info.Item1;
        _page = info.Item2;
    }
}
