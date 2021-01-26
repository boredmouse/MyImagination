using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WHGame
{
    public class EventTriggerLisener : EventTrigger
    {
		public delegate void EventDelegate( GameObject go, PointerEventData ev );

		public EventDelegate OnDragBegin;
		public EventDelegate OnDragEvent;
		public EventDelegate OnDragEnd;

        public static EventTriggerLisener Get(GameObject go)
        {
            if (!go)
            {
                Debug.LogError("EventTriggerLisener Get(GameObject gameObject) null");
            }
            EventTriggerLisener listener = go.GetComponent<EventTriggerLisener>();
            if (listener == null)
            {
                listener = go.AddComponent<EventTriggerLisener>();
            }
            return listener;
        }

        public override void OnBeginDrag( PointerEventData event_data )
		{
			if( this.OnDragBegin != null )
				this.OnDragBegin( this.gameObject, event_data );
		}

		public override void OnDrag( PointerEventData event_data )
		{
			if( this.OnDragEvent != null )
				this.OnDragEvent( this.gameObject, event_data );
		}

		public override void OnEndDrag( PointerEventData event_data )
		{
			if( this.OnDragEnd != null )
				this.OnDragEnd( this.gameObject, event_data );
		}

    }
}

