using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;
using TMPro;
public class SelectDragLetter : MonoBehaviour
{
    public GameObject ghostLetter;
    private GameObject letter = null;
    [SerializeField]
    private ContactFilter2D contactFilter = new ContactFilter2D();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D col = Physics2D.OverlapArea(position, position);
            if (col != null)
            {
                if(col.tag != "Letter")
                {
                    return;
                }
                letter = col.gameObject;
                LetterMovement letterMovement = letter.GetComponent<LetterMovement>();

                if(letterMovement.letterState == LetterMovement.state.inBox)
                {
                    return;
                }

                TrailRenderer trail = ghostLetter.GetComponent<TrailRenderer>();
                trail.Clear();
                letterMovement.letterState = LetterMovement.state.dragging;
                ghostLetter.SetActive(true);
                ghostLetter.transform.position = letter.transform.position;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            if(letter != null)
            {
                bool found = false;
                Collider2D[] colliders = new Collider2D[5];
                Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Collider2D col = letter.GetComponent<PolygonCollider2D>();
                int colInt = col.OverlapCollider(contactFilter, colliders);
                LetterMovement letterMovement = letter.GetComponent<LetterMovement>();

                if (colliders.Length > 0)
                {
                    foreach(Collider2D c in colliders)
                    {
                        if(c.tag == "LetterBox")
                        {
                            found = true;
                            letter.transform.SetParent(c.transform);
                            letterMovement.letterState = LetterMovement.state.inBox;
                            UnitPlacedEvent upe = new UnitPlacedEvent();
                            upe.UnitPlaced = letter;
                            upe.letterPlaced = char.Parse(letter.GetComponentInChildren<TextMeshPro>().text);
                            upe.boxPlacedIn = int.Parse(c.name);
                            EventSystem.Current.FireEvent(EVENT_TYPE.UNIT_PLACED, upe);
                            break;
                        }
                    }
                }

                if (found == false)
                {
                    letterMovement.ghostLetter = ghostLetter;
                    letterMovement.letterState = LetterMovement.state.stopDragging;
                }
                
            }
            letter = null;
        }
    }
}
