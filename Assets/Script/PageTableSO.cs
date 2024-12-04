using UnityEngine;

[CreateAssetMenu(fileName = "PageTable", menuName = "Scriptable Objects/PageTable")]
public class PageTableSO : ScriptableObject
{
    internal int _row;

    internal string[] _findPageNumArr;
    internal PageInfo[,] _pageInfoArr;
    internal bool[] _isPageInArr;

    internal GameObject[][] _pageTableObj;
    internal int[] _pageStay;

    internal void SetTable(string[] pages, int frame, int algorithm)
    {
        // 기초 세팅
        _row = pages.Length;
        _findPageNumArr = new string[_row];
        _pageInfoArr = new PageInfo[_row, frame];
        _isPageInArr = new bool[_row];

        for(int i=0; i< frame; i++)
        {
            for (int j = 0; j < _row; j++)
            {
                _pageInfoArr[j, i] = new PageInfo();
            }
        }

        for (int column = 0; column < _findPageNumArr.Length; column++)
        {
            _findPageNumArr[column] = pages[column];
            if (column == 0)
            {
                _pageInfoArr[column, 0]._rank = column;
                _pageInfoArr[column, 0]._page = _findPageNumArr[column];
            }
            else
            {
                int bestIndex = 0;
                int bestRank = -1;
                bool isPageIn = false;

                for (int row = 0; row < frame; row++)
                {
                    _pageInfoArr[column, row] = new PageInfo(_pageInfoArr[column - 1, row].GetInfo());

                    if (_pageInfoArr[column, row]._page.Equals(_findPageNumArr[column]))
                    {
                        isPageIn = true;
                        bestIndex = row;
                    }

                    if(isPageIn) { continue; }

                    if (row == 0)
                    {
                        bestRank = _pageInfoArr[column, row]._rank;
                    }
                    else
                    {
                        if (bestRank > _pageInfoArr[column, row]._rank)
                        {
                            bestRank = _pageInfoArr[column, row]._rank;
                            bestIndex = row;
                        }
                    }
                }

                if (algorithm == 0)
                {
                    if (isPageIn)
                    {
                        _isPageInArr[column] = true;
                    }
                    else
                    {
                        _pageInfoArr[column, bestIndex]._page = _findPageNumArr[column];
                        _pageInfoArr[column, bestIndex]._rank = column;
                    }
                }
                else if (algorithm == 1)
                {
                    if (isPageIn)
                    {
                        _isPageInArr[column] = true;
                        _pageInfoArr[column, bestIndex]._rank = column;
                    }
                    else
                    {
                        _pageInfoArr[column, bestIndex]._page = _findPageNumArr[column];
                        _pageInfoArr[column, bestIndex]._rank = column;
                    }
                }
            }
        }
        //for (int j = 0; j < _row; j++) Debug.Log(_pageInfoArr[j, 0]._page + ", " + _pageInfoArr[j, 1]._page + ", " + _pageInfoArr[j, 2]._page);
    }
}
