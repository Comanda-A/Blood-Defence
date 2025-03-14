using UnityEngine;
using UnityEngine.Events;

namespace PauseManagement
{
	public class PauseEventHandler : MonoBehaviour
	{
		[SerializeField] private UnityEvent pauseEvent = null;
	
		[SerializeField] private UnityEvent resumeEvent = null;

		private bool _subscribed = false;

        private void OnEnable()
		{
			if (!_subscribed)
			{
				PauseManager.PauseAction += PauseHandler;
				_subscribed = true;
			}
		}

		private void OnDisable()
		{
            if (_subscribed)
            {
                PauseManager.PauseAction -= PauseHandler;
                _subscribed = false;
            }
		}

		void PauseHandler(bool paused)
		{
			if (paused)
				pauseEvent.Invoke();
			else
				resumeEvent.Invoke();
		}

        private void OnDestroy()
        {
			pauseEvent = null;
			resumeEvent = null;

            if (_subscribed)
            {
                PauseManager.PauseAction -= PauseHandler;
                _subscribed = false;
            }
        }
    }
}