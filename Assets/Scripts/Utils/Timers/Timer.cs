using System;
using UnityEngine;

namespace StardustInteractive.Tools
{
    [Serializable]
    public class Timer : MonoBehaviour
    {
        #region Fields
        private float m_TimerDuration;
        private float m_CurrentTime = 0;
        private bool m_TimerPaused = false;
        public event Action ActionToPerform;
        private string m_TimerID; // used for identifying and getting a timer
        private bool m_CanHaveMultipleInstances;
        private GameObject m_Source; //What initiated this timer
        private bool m_multipleInstancesFromSource;
        private bool m_TimerCompleted;
        private int m_Repeats;

        private int m_CurrentRepeats = 0;
        #endregion

        #region Properties
        /// <summary>
        /// An instance of a Timer
        /// </summary>
        /// <param name="timerID">The ID of this timer</param>
        /// <param name="timerDuration">The max duration of this timer</param>
        /// <param name="actionToPerform">The functionality to perform when this timer expires</param>
        /// <param name="canHaveMultipleInstaces">Can multiple timers with this ID exist? (For example multiple enemies having the same type of timer)</param>
        /// <param name="source">The object were this timer originated from</param>
        /// <param name="multipleInstancesFromSource">Can multiple timers with the same ID exist from the same source? (This can only be true if multiple instances are allowed)</param>
        public Timer(string timerID, float timerDuration, Action actionToPerform, bool canHaveMultipleInstaces = false, GameObject source = null, bool multipleInstancesFromSource = false, int repeats = 0)
        {
            this.m_CurrentTime = 0;
            this.m_TimerID = timerID;
            this.m_TimerDuration = timerDuration;
            this.ActionToPerform = actionToPerform;
            this.m_CanHaveMultipleInstances = canHaveMultipleInstaces;
            this.m_Source = source;
            this.m_multipleInstancesFromSource = multipleInstancesFromSource;
            this.m_Repeats = repeats;
        }

        [Tooltip("The ID of this Timer")]
        public string TimerID => m_TimerID;
        public bool CanHaveMultipleInstances => m_CanHaveMultipleInstances;
        public bool MultipleInstacesFromSource => m_multipleInstancesFromSource;
        public GameObject Source => m_Source;

        [Tooltip("Is this Timer completed?")]
        public bool TimerCompleted => m_TimerCompleted;
        [Tooltip("Returns a completion rate ranging from 0 to 1")]
        public float TimerNormalizedCompletion => m_CurrentTime / m_TimerDuration;
        [Tooltip("The current time of this Timer")]
        public float CurrentTime => m_CurrentTime;

        public int Repeats => m_Repeats;
        #endregion

        #region Methods
        //Public

        /// <summary>
        /// Run this Timer
        /// </summary>
        public void RunTimer()
        {
            if(m_TimerPaused) return;
            m_CurrentTime += Time.deltaTime; 
            if (m_CurrentTime >= m_TimerDuration)
            {
                ActionToPerform?.Invoke();

                if (Repeats == 0)
                {
                    m_TimerCompleted = true;
                }
                else
                {
                    if(Repeats == -1) //Infinite Loop
                    {
                        ResetTimer();
                        return;
                    }
                    else
                    {
                        if(m_CurrentRepeats < Repeats)
                        {
                            m_CurrentRepeats++;
                            ResetTimer();
                            return;
                        }
                        else
                        {
                            m_TimerCompleted = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add time to this Timers total duration
        /// </summary>
        /// <param name="durationToAdd">How much time should be added?</param>
        public void AddTime(float durationToAdd)
        {
            m_TimerDuration += durationToAdd;
        }

        /// <summary>
        /// Pause this Timer
        /// </summary>
        public void PauseTimer()
        {
            m_TimerPaused = true;
        }

        /// <summary>
        /// Unpause this Timer
        /// </summary>
        public void UnpauseTimer()
        {
            m_TimerPaused = false;
        }


        /// <summary>
        /// Reset this Timer, setting it's CurrentTime value to 0
        /// </summary>
        public void ResetTimer()
        {
            m_CurrentTime = 0;
        }
        #endregion
    }
}