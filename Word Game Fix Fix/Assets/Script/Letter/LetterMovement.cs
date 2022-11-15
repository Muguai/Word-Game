using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterMovement : MonoBehaviour
{
    public float speed = 1f;
    public float floatSpeed = 1f;
    public float getBackSpeed = 5;

    float yIncrease;
    Vector3 vectorY;

    Vector3 goUp = new Vector3(0, 0.5f, 0);
    Vector3 goDown = new Vector3(0, -0.5f, 0);

    float counter = 0.5f;

    [HideInInspector] public GameObject ghostLetter;

    public enum state
    {
        moving,
        dragging,
        stopDragging,
        gettingBackToGhost,
        inBox
    }

    public state letterState = state.moving;

    int upDown = 1;
    // Start is called before the first frame update
    void Start()
    {
        int coin = Random.Range(0, 1);
        if(coin == 0)
        {
            vectorY = goUp;

        }
        else
        {
            vectorY = goDown;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (letterState == state.moving)
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
        }else if (letterState == state.dragging)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = position;
        }else if (letterState == state.stopDragging)
        {
            letterState = state.gettingBackToGhost;
        }else if (letterState == state.gettingBackToGhost)
        {
            if(ghostLetter.activeSelf == true)
            {
                float step = getBackSpeed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, ghostLetter.transform.position, step);

                if (0.01f > Vector2.Distance(transform.position, ghostLetter.transform.position))
                {
                    ghostLetter.SetActive(false);
                    this.ghostLetter = null;
                    print("meet");
                    letterState = state.moving;
                }
            }
            else
            {
                this.ghostLetter = null;
                letterState = state.moving;
                this.gameObject.SetActive(false);
            }
           
        }else if(letterState == state.inBox)
        {

        }

    }
}
