using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace WayOfBlood.PauseManagement
{
	public class PauseManager : MonoBehaviour
	{
		public delegate void PauseDelegateAction(bool paused);

		public static event PauseDelegateAction PauseAction;

        public static bool IsPaused { get; set; }

        /// <summary>
        /// Use Unity's timeScale to stop time when paused ?
        /// </summary>
        public bool useTimeScale = true;

		/// <summary>
		/// The Input Action Asset's reference to apply to pauseInputAction
		/// </summary>
		public InputActionReference pauseActionReference = null;

		/// <summary>
		/// Events triggered when paused
		/// </summary>
		[SerializeField]
		private UnityEvent pauseEvent = null;

		/// <summary>
		/// Events triggered when resumed
		/// </summary>
		[SerializeField]
		private UnityEvent resumeEvent = null;

		private bool m_ExecuteEvents = true;

		private bool m_ExecuteDelegateActions = true;


		void Awake()
		{
			if (pauseActionReference)
				pauseActionReference.action.performed += _ => TogglePause();

			IsPaused = false;
		}

		void OnApplicationPause(bool pause)
		{
			if (pause && !IsPaused)
				Pause();
		}

		public void TogglePause()
		{
			if (!IsPaused)
				Pause();
			else
				Resume();
		}

		public void Pause()
		{
			if (useTimeScale)
				StopTime();

			IsPaused = true;

			if (m_ExecuteEvents)
				pauseEvent.Invoke();

			if (m_ExecuteDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void Resume()
		{
			ResetTime();

			IsPaused = false;

			if (m_ExecuteEvents)
				resumeEvent.Invoke();

			if (m_ExecuteDelegateActions && PauseAction != null)
				PauseAction.Invoke(IsPaused);
		}

		public void StopTimeDelayed(float time)
		{
			Invoke(nameof(StopTime), time);
		}

		public void StopTime()
		{
			Time.timeScale = 0;
		}

		public void ResetTimeDelayed(float time)
		{
			Invoke(nameof(ResetTime), time);
		}

		public void ResetTime()
		{
			Time.timeScale = 1;
		}
	}
}