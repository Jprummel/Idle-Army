using Sirenix.OdinInspector;
using StardustInteractive.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StardustInteractive.Tools
{
    /// <summary>
    /// This class manages timers. It holds the functionality to Add, Remove or Get/Check timers.
    /// </summary>
    public class TimerManager : MonoBehaviour
    {
        #region Inspector Fields
        [Tooltip("Keep this alive throughout scene transitions?")]
        [SerializeField] private bool m_KeepAlive;
        #endregion

        #region Fields
        [ShowInInspector,ReadOnly]private static List<Timer> m_Timers = new List<Timer>();
        #endregion

        #region Methods
        //Public

        /// <summary>
        /// Adds a timer to the TimeManagers Timer list, if the timer already exists, it resets it.
        /// </summary>
        /// <param name="timerID">Timer ID values can be found in TimerIDs.cs. Add ID's as necessary.</param>
        /// <param name="timerDuration">Full duration of the timer</param>
        /// <param name="actionToPerform">Callback to perform</param>
        /// <param name="canHaveMultipleInstances">Can the timer with this ID have multiple instances? (Could be from different sources)</param>
        /// <param name="source">What initiated this timer?</param>
        /// <param name="multipleInstancesFromSource">Can multiple timers with the same ID be created from the same source?</param>
        public static void AddTimer(string timerID, float timerDuration, Action actionToPerform, bool canHaveMultipleInstances = false, GameObject source = null, bool multipleInstancesFromSource = false, int repeats = 0, bool shouldReset = true)
        {
            foreach (Timer timer in m_Timers)
            {
                //If timer with same ID already exists
                if (timer.TimerID == timerID)
                {
                    //Timer can not have multiple instances
                    if (!timer.CanHaveMultipleInstances)
                    {
                        if (shouldReset)
                        {
                            timer.ResetTimer();//Reset timer if we're trying to add same timer again
                        }
                        return;
                    }

                    if (timer.CanHaveMultipleInstances)
                    {
                        //Timer can not have multiple instances from same source
                        if (timer.Source == source && !timer.MultipleInstacesFromSource)
                        {
                            return;
                        }
                    }
                }
            }
            Timer newTimer = new Timer(timerID, timerDuration, actionToPerform, canHaveMultipleInstances, source, multipleInstancesFromSource, repeats);
            m_Timers.Add(newTimer);
        }

        /// <summary>
        /// Returns a timer
        /// </summary>
        /// <param name="timerID">Timer ID to check</param>
        /// <param name="source">Object that was the source of the timer to check</param>
        /// <returns></returns>
        public static Timer GetTimer(string timerID, GameObject source)
        {
            Timer timerToReturn = null;
            foreach (Timer timer in m_Timers)
            {
                if (timer.TimerID == timerID && timer.Source == source)
                {
                    timerToReturn = timer;
                }
            }
            return timerToReturn;
        }

        /// <summary>
        /// Returns a timer
        /// </summary>
        /// <param name="timerID">Timer ID to check</param>
        /// <returns></returns>
        public static Timer GetTimer(string timerID)
        {
            foreach (Timer timer in m_Timers)
            {
                if (timer.TimerID == timerID)
                {
                    return timer;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a timer exists based on Timer ID
        /// </summary>
        /// <param name="timerID"></param>
        /// <returns></returns>
        public static bool DoesTimerExist(string timerID)
        {
            foreach (Timer timer in m_Timers)
            {
                if (timer.TimerID == timerID)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes a timer that is currently running
        /// </summary>
        /// <param name="timerIndex">Index in the Timer List</param>
        private void DisposeOfTimer(int timerIndex)
        {
            m_Timers.RemoveAt(timerIndex);
        }

        /// <summary>
        /// Removes a timer that is currently running
        /// </summary>
        /// <param name="timerID">Timer ID of timer to remove</param>
        public static void DisposeOfTimer(string timerID)
        {
            for (int i = 0; i < m_Timers.Count; i++)
            {
                if (m_Timers[i].TimerID == timerID)
                {
                    m_Timers.RemoveAt(i);
                }
            }
        }

        public static void DisposeOfAllTimers()
        {
            for (int i = 0; i < m_Timers.Count; i++)
            {
                Destroy(m_Timers[i]);
                m_Timers.Clear();
            }
        }

        /// <summary>
        /// Removes a timer that is currently running, use this if there can be multiple instances of the same timer
        /// </summary>
        /// <param name="timerID">Timer ID of timer to remove</param>
        /// <param name="source">Source of the timer</param>
        public static void DisposeOfTimer(string timerID, GameObject source)
        {
            for(int i = 0;i < m_Timers.Count; i++)
            {
                if (m_Timers[i].TimerID == timerID && m_Timers[i].Source == source)
                {
                    m_Timers.RemoveAt(i);
                }
            }
        }
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if(m_KeepAlive)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Update()
        {
            for (int i = 0; i < m_Timers.Count; i++)
            {
                m_Timers[i].RunTimer();
                if (m_Timers[i].TimerCompleted)
                {
                    DisposeOfTimer(i);
                }
            }
        }

        private void OnDestroy()
        {
            m_Timers.Clear();
        }
        #endregion
    }
}