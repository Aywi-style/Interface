using UnityEngine;
using UnityEngine.UI;
using System;

public class UiSelectableObject : MonoBehaviour
{
    public Action<int, bool> OnSelected;

    public int Id { private set; get; }

    [field: SerializeField]
    public Text NameField { private set; get; }

    [field: SerializeField]
    public Toggle SelectToggle { private set; get; }

    [field: SerializeField]
    public Image VisibleIndication { private set; get; }

    public void Init(int id, string objectName)
    {
        Id = id;
        NameField.text = objectName;
    }

    public void Select()
    {
        OnSelected?.Invoke(Id, SelectToggle.isOn);
    }

    public void ToggleVisibleIndication(bool activeSelf)
    {
        if (activeSelf)
        {
            VisibleIndication.color = ColorTheme.ENABLED_OBJECT;
        }
        else
        {
            VisibleIndication.color = ColorTheme.DISABLED_OBJECT;
        }
    }
}
