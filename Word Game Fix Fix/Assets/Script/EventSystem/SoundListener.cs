using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class SoundListener : MonoBehaviour
    {
        private int NoOfcurrentlyPlayingSounds = 0;
        [SerializeField] private int maxNoOfSimultaneousSounds;
    
        // Start is called before the first frame update
        void Start()
        {
            EventSystem.Current.RegisterListener(EVENT_TYPE.UNIT_DIED, OnDiePlaySound);
        }

        void OnDiePlaySound(Event eventInfo)
        {
            DieEvent dieEventInfo = (DieEvent)eventInfo;
            AudioClip dieSound = dieEventInfo.UnitSound;

            if (NoOfcurrentlyPlayingSounds < maxNoOfSimultaneousSounds && dieSound != null)
            {
                NoOfcurrentlyPlayingSounds++;
                GameObject unit = dieEventInfo.UnitGameObject;
                AudioSource.PlayClipAtPoint(dieSound, unit.transform.position);
                Invoke("SubtractCurrentlyPlayingSounds", dieSound.length);
                
                
            }
        }

        public void SubtractCurrentlyPlayingSounds()
        {
            NoOfcurrentlyPlayingSounds--;
        }
    }
}
