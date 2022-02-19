using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour
{

    public GameObject BG1;
    public GameObject BG2;

    private float ScrollingSpeed = 2f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float B1pos_y = BG1.transform.position.y - ScrollingSpeed * Time.deltaTime;
        float B2pos_y = BG2.transform.position.y - ScrollingSpeed * Time.deltaTime;
        
        BG1.transform.position = new Vector3(0, B1pos_y, 0);
        BG2.transform.position = new Vector3(0, B2pos_y, 0);

        if ( BG1.transform.position.y < -7 ) {
            BG1.transform.position = new Vector3(0, 7, 2);
        }

        if ( BG2.transform.position.y < -7 ) {
            BG2.transform.position = new Vector3(0, 7, 2);
        }
        
    }
}
