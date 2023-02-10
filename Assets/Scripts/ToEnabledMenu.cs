using UnityEngine;
using System.Collections.Generic;

public class ToEnabledMenu : MonoBehaviour
{
    [field: SerializeField]
    public List<Material> Materials { private set; get; }

    private void Start()
    {
        if (Materials.Count != 0)
        {
            return;
        }

        Materials = new List<Material>();

        if (transform.TryGetComponent(out MeshRenderer meshRenderer))
        {
            foreach (var material in meshRenderer.materials)
            {
                Materials.Add(material);
            }
        }
        else
        {
            Debug.LogError("Materials field is null!");
        }
    }

    public void ChangeTransparencyValue(float value)
    {
        foreach (var material in Materials)
        {
            var color = material.color;
            color.a = value;
            material.color = color;
        }
    }
}
