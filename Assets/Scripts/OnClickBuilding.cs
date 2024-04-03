using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickBuilding : MonoBehaviour
{
    public GameObject removeButton;

    void OnMouseDown()
    {
        if (removeButton != null)
        {
            if (!removeButton.activeSelf)
            {
                removeButton.SetActive(true);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
