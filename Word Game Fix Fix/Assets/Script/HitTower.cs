using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventCallbacks;

public class HitTower : MonoBehaviour
{
    [SerializeField] private GameObject deathParticle;
    [SerializeField] private AudioClip deathSound;
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Letter")
        {
            col.gameObject.SetActive(false);
            DieEvent dieEvent = new DieEvent();
            dieEvent.UnitGameObject = col.gameObject;
            dieEvent.UnitParticle = deathParticle;
            dieEvent.UnitSound = deathSound;
            RaycastHit2D hit = (Physics2D.Raycast(col.gameObject.transform.position, transform.right));
            if (hit != false)
            {
                dieEvent.deathContactPoint = hit.point;
            }
            EventSystem.Current.FireEvent(EVENT_TYPE.UNIT_DIED, dieEvent);
        }      
    }
}
