using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameranglechange : MonoBehaviour
{
    public Camera MainCamera;
    public Camera CaveCamera;

    void Start()
    {
        MainCamera.enabled = true;
        CaveCamera.enabled = false;
    }

    public void ShowCaveCamera()
    {
        MainCamera.enabled = false;
        CaveCamera.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pirate_man"))
        {
            ShowCaveCamera();
            SimpleController simpleController = other.gameObject.GetComponent<SimpleController>();
            simpleController.CaveControls = true;

        }
        else
        {
            Debug.LogWarning("Object with tag 'pirate_man' not found.");
        }
    }

}