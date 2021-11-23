using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Dropdown ObjectDropdown;
    private ARPlacement placement;

    // Start is called before the first frame update
    void Start()
    {
        ObjectDropdown.onValueChanged.AddListener(ChangeObjectPrefab);
        placement = FindObjectOfType<ARPlacement>();
    }

    private void ChangeObjectPrefab(int val)
    {
        placement.ChangeImagePrefab(val);
    }
}
