using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    private bool isOn = false;

    [SerializeField]
    private GameObject flashlight;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
        {
            if (!isOn)
            {
                isOn = true;
                this.gameObject.SetActive(true);
            }
            else if (isOn)
            {
                isOn = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
