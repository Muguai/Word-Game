using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGhostMovement : MonoBehaviour
{
    public float speed = 1f;
    public float floatSpeed = 1f;

    float yIncrease;
    Vector3 vectorY;

    Vector3 goUp = new Vector3(0, 0.5f, 0);
    Vector3 goDown = new Vector3(0, -0.5f, 0);
    float counter = 0.5f;

    private bool moving = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            counter += Time.deltaTime;
            transform.position += vectorY * floatSpeed * Time.deltaTime;

            if (counter > 1f)
            {
                if (vectorY.Equals(goUp))
                {
                    vectorY = goDown;
                }
                else
                {
                    vectorY = goUp;
                }

                counter = 0f;
            }
        }
    }
}
