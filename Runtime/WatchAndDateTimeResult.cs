using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace Eloi.WatchAndDate
{

    public class StaticWatchAndDateResultExisting {

        private static List<WatchAndDateOverwatchResult> m_register = new List<WatchAndDateOverwatchResult>();
      
        public static IEnumerable<WatchAndDateOverwatchResult> GetListAsStoredCopy(string category="")
        {
            if(string.IsNullOrWhiteSpace(category))
            return m_register.Where(k => k != null &&  k.m_debugListOrderPriority >= 0).OrderBy(k => k.m_debugListOrderPriority);
            else return m_register.Where(k => k!=null && k.m_debugListOrderPriority >= 0 && Contains(category, k.m_debugListCategory)).OrderBy(k => k.m_debugListOrderPriority);
        }

        private static bool Contains(string toLookFor, string[] categories)
        {
            toLookFor = toLookFor.ToLower().Trim();
            for (int i = 0; i < categories.Length; i++)
            {
                if (categories[i].ToLower().Trim() == toLookFor)
                    return true;
            }
            return false;
        }

        public static void PushInExistance(WatchAndDateStartStopResult watch)
        {
            if (watch.m_debugListOrderPriority > -1 || watch.m_debugListCategory.Length>0) {
                for (int i = m_register.Count-1; i >=0; i--)
                {
                    if (m_register[i] == watch)
                        m_register.RemoveAt(i);
                }
                m_register.Add(watch);
            }
        }
    }
    public class StaticWatchAndDateResultNotification
    {

        public static Action<WatchAndDateOverwatchResult> m_newResultReceived;

        public static void PushNewResult(in WatchAndDateOverwatchResult toPushResult)
        {
            if (m_newResultReceived != null)
                m_newResultReceived.Invoke(toPushResult);

        }
    }
    [System.Serializable]
    public class WatchAndDateOverwatchResult
    {

        [Tooltip("What are you trying to measure ? Reminder and description us at runtime.")]
        [UnityEngine.SerializeField] protected string m_watchTimeLabelDescription = "Watch Time of ..";
        [UnityEngine.SerializeField] protected double m_watchTimeInTick;
        [UnityEngine.SerializeField] protected double m_watchTimeInMilliseconds;
        [UnityEngine.SerializeField] protected double m_dateTimeInTick;
        [UnityEngine.SerializeField] protected double m_dateTimeInMilliseconds;


        [Tooltip("Allow to order the list at runtime when your want to debug time")]
        public int m_debugListOrderPriority = -1;
        [Tooltip("Allow to filter by categoy at runtime. (Ingore case)")]
        public string[] m_debugListCategory = new[] { "Default" };
        public bool m_useNotification = true;

        public string GetDescription()
        {
            return m_watchTimeLabelDescription;
        }
        public void GetTick(out double watchStopTick, out double dateTimeTick) {
            watchStopTick = m_watchTimeInTick;
            dateTimeTick = m_dateTimeInTick;
        }
        public void GetMilliseconds(out double watchStopMilliseconds, out double dateTimeMilliseconds)
        {
            watchStopMilliseconds = m_watchTimeInMilliseconds;
            dateTimeMilliseconds = m_dateTimeInMilliseconds;

        }
    }

    public class WatchAndDateStartStopResult : WatchAndDateOverwatchResult
    {

        DateTime m_start, m_now;
        Stopwatch m_stopWatch;
        public void StartCounting()
        {
            CheckInit();
            m_start = DateTime.Now;
            m_stopWatch = new Stopwatch();
            m_stopWatch.Start();
            m_dateTimeInMilliseconds = 0;
            m_dateTimeInTick = 0;
            m_watchTimeInMilliseconds = 0;
            m_watchTimeInTick = 0;
        }

        private void CheckInit()
        {
            if (m_stopWatch == null)
            { 
                m_start = DateTime.Now;
                m_now = DateTime.Now;
                m_stopWatch = new Stopwatch();
                m_stopWatch.Start();
                m_stopWatch.Stop();
            }
        }

        public void StopCounting()
        {

            CheckInit();

            m_stopWatch.Stop();
            m_now = DateTime.Now;
            m_dateTimeInMilliseconds = (m_now - m_start).TotalMilliseconds;
            m_dateTimeInTick = (m_now - m_start).Ticks;
            m_watchTimeInMilliseconds = m_stopWatch.ElapsedMilliseconds;
            m_watchTimeInTick = m_stopWatch.ElapsedTicks;
            if (m_useNotification)
                StaticWatchAndDateResultNotification.PushNewResult(this);
            StaticWatchAndDateResultExisting.PushInExistance(this);

        }

    }


    [System.Serializable]
    public class WatchAndDateTimeActionResult : WatchAndDateStartStopResult
    {

        public void WatchTheAction(Action action)
        {
            WatchAndDateTimeActionResult result = this;
            WatchTheAction(action, ref result);
        }
        public void WatchTheAction(Action action, ref WatchAndDateTimeActionResult result)
        {
            if (result == null) result = new WatchAndDateTimeActionResult();

            StartCounting();
            action.Invoke();
            StopCounting();
        }

        public void WatchTheActionAndCatchExceptionAsLog(Action action)
        {
            try
            {
                WatchAndDateTimeActionResult result = this;
                WatchTheAction(action, ref result);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.Log("Exception during watch time: " + exception.StackTrace);
            }
        }
    }
}