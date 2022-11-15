using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class ParticleListener : MonoBehaviour
    {
        private Vector3 diePlace;
        // Start is called before the first frame update
        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.UNIT_DIED, OnDiePlayParticleEffect);

        }
        void OnDiePlayParticleEffect(Event eventInfo)
        {
            DieEvent unitDieEvent = (DieEvent)eventInfo;
            diePlace = unitDieEvent.UnitGameObject.transform.position;
            GameObject myParticle = unitDieEvent.UnitParticle;
            if(myParticle != null)
            {
                if(unitDieEvent.deathContactPoint != Vector2.zero)
                {
                    Instantiate(myParticle, unitDieEvent.deathContactPoint, myParticle.transform.rotation);
                }
                else
                {
                    Instantiate(myParticle, diePlace, myParticle.transform.rotation);
                }
            }

        }
    }
}
