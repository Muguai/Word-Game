using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public abstract class Event
    {
        public string EventDescription;
    }

    public class DebugEvent : Event
    {
    }

    public class DieEvent : Event
    {
        public GameObject UnitGameObject;
        public AudioClip UnitSound;
        public GameObject UnitParticle;
        public Vector2 deathContactPoint = Vector2.zero;
    }

    public class RestartEvent : Event
    {
        public int groupLenght;
    }
    
    public class UnitPlacedEvent : Event
    {
        public GameObject UnitPlaced;
        public char letterPlaced;
        public int boxPlacedIn;
    }
}
