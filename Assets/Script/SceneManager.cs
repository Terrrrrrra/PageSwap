using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] Text _inputNums;
    [SerializeField] Text _inputFrame;
    int _algorithm = 0;
    [SerializeField] GameObject _view;
    RectTransform _panel;
    float _panelX = 1920;
    float _panelY = 600;
    int _inputFrameCount;
    GridLayoutGroup _gridLayoutGroup;

    [SerializeField] Text _foult;

    [SerializeField] GameObject _childPrefab;

    [SerializeField] PageTableSO _pageTableSO;

    private void Awake()
    {
        _gridLayoutGroup = _view.GetComponent<GridLayoutGroup>();
    }

    public void SetPage()
    {
        for (int i = 0; i < _view.transform.childCount; i++)
        {
            Destroy(_view.transform.GetChild(i).gameObject);
        }

        StartCoroutine(Logic());

        IEnumerator Logic()
        {
            yield return null;
            string[] input = _inputNums.text.Split(',', '.', ' ', '\t', '\n', '\r').Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
            _inputFrameCount = int.Parse(_inputFrame.text);
            _pageTableSO.SetTable(input, _inputFrameCount, _algorithm);

            _gridLayoutGroup.cellSize = new Vector2(_panelX / input.Length, _panelY / (_inputFrameCount + 2));
            for (int i = 0; i < _pageTableSO._pageInfoArr.Length + 2 * _pageTableSO._row; i++)
            {
                Instantiate(_childPrefab, _view.transform);
            }

            for (int i = 0; i < input.Length; i++)
            {
                // 첫행
                _view.transform.GetChild(i).GetComponentInChildren<Text>().text = _pageTableSO._findPageNumArr[i];
                _view.transform.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color(0.9f, 0.9f, 1);

                // 중간행
                for (int j = 0; j < _inputFrameCount; j++)
                {
                    _view.transform.GetChild(i + (j + 1) * input.Length).GetComponentInChildren<Text>().text = _pageTableSO._pageInfoArr[i, j]._page;
                }

                // 끝행
                _view.transform.GetChild(i + (1 + _inputFrameCount) * input.Length).GetComponentInChildren<Text>().text
                    = _pageTableSO._isPageInArr[i] ? "O" : "X";
                _view.transform.GetChild(i + (1 + _inputFrameCount) * input.Length).GetChild(0).GetComponent<Image>().color = new Color(1, 0.9f, 0.9f);
            }

            int re = 0;
            for (int i = 0; i < _pageTableSO._isPageInArr.Length; i++) {
                if(_pageTableSO._isPageInArr[i] == false) { re++; }
            }
            _foult.text = $"페이지 부재 {re}회";
        }
    }

    public void SetAlgorithm(int algorithm)
    {
        _algorithm = algorithm;
    }

    public void SetOff()
    {
        Application.Quit();
    }
}
