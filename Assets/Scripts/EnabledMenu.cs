using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnabledMenu : MonoBehaviour
{
    private RectTransform _selfRectTransform;
    [SerializeField]
    private Toggle _toggleAllSelect;
    [SerializeField]
    private GameObject _contentField;
    [SerializeField]
    private UiSelectableObject _contentPrefab;

    #region Arrays
    [SerializeField]
    private ToEnabledMenu[] _allObjects;
    private UiSelectableObject[] _allContents;
    private List<UiSelectableObject> _selectedContents;
    private List<UiSelectableObject> _needToSelect;
    #endregion

    private bool _menuIsOn = true;
    private float _menuAnchorSizeX;

    void Start()
    {
        if (transform.TryGetComponent(out RectTransform rectTransform))
        {
            _selfRectTransform = rectTransform;
        }

        CalculateAnchorSize();

        CreateStartContent();
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _allContents.Length; i++)
        {
            _allContents[i].OnSelected -= SelectContent;
        }
    }

    private void CalculateAnchorSize()
    {
        Vector2 currentMinAnchor = _selfRectTransform.anchorMin;
        Vector2 currentMaxAnchor = _selfRectTransform.anchorMax;

        _menuAnchorSizeX = currentMaxAnchor.x - currentMinAnchor.x;
    }

    private void CreateStartContent()
    {
        _allContents = new UiSelectableObject[_allObjects.Length];
        _selectedContents = new List<UiSelectableObject>(_allObjects.Length);
        _needToSelect = new List<UiSelectableObject>(_allObjects.Length);

        for (int i = 0; i < _allObjects.Length; i++)
        {
            var content = Instantiate(_contentPrefab, _contentField.transform);
            _allContents[i] = content;
            content.Init(i, _allObjects[i].name);
            content.OnSelected += SelectContent;
            content.ToggleVisibleIndication(_allObjects[i].gameObject.activeSelf);
        }
    }

    public void ToggleMenu()
    {
        Vector2 currentMinAnchor = _selfRectTransform.anchorMin;
        Vector2 currentMaxAnchor = _selfRectTransform.anchorMax;

        if (_menuIsOn)
        {
            currentMinAnchor.x -= _menuAnchorSizeX;
            currentMaxAnchor.x -= _menuAnchorSizeX;
        }
        else
        {
            currentMinAnchor.x += _menuAnchorSizeX;
            currentMaxAnchor.x += _menuAnchorSizeX;
        }

        _selfRectTransform.anchorMin = currentMinAnchor;
        _selfRectTransform.anchorMax = currentMaxAnchor;

        _menuIsOn = !_menuIsOn;
    }

    private void SelectContent(int Id, bool select)
    {
        var selectedContent = _allContents[Id];

        if (select)
        {
            _selectedContents.Add(selectedContent);
        }
        else
        {
            _selectedContents.Remove(selectedContent);
        }
    }

    public void ToggleVisibility()
    {
        foreach (var selectedContent in _selectedContents)
        {
            var selectedObject = _allObjects[selectedContent.Id].gameObject;
            selectedObject.SetActive(!selectedObject.activeSelf);

            selectedContent.ToggleVisibleIndication(selectedObject.activeSelf);
        }
    }

    public void ToggleAllSelect()
    {
        if (_toggleAllSelect.isOn)
        {
            foreach (var content in _allContents)
            {
                if (!_selectedContents.Contains(content))
                {
                    _needToSelect.Add(content);
                }
            }
        }
        else
        {
            foreach (var content in _allContents)
            {
                if (_selectedContents.Contains(content))
                {
                    _needToSelect.Add(content);
                }
            }
        }

        foreach (var content in _needToSelect)
        {
            content.SelectToggle.isOn = !content.SelectToggle.isOn;
        }

        _needToSelect.Clear();
    }

    public void ChangeTransparencyValue(float value)
    {
        foreach (var selectedContent in _selectedContents)
        {
            _allObjects[selectedContent.Id].ChangeTransparencyValue(value);
        }
    }
}
