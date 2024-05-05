using UnityEngine;
using UnityEngine.Events;

namespace Eloi.WatchAndDate { 

    public class WatchAndDateTimeActionResultMono : MonoBehaviour
    {

        public UnityEvent m_wathToExecute;
        public WatchAndDateTimeActionResult m_watchTime;

        [ContextMenu("Execute")]
        public void Execute()
        {
            m_watchTime.WatchTheAction(m_wathToExecute.Invoke);
        }
        [ContextMenu("Execute  in Catch Exception Log")]
        public void ExecuteAndDebugCatchInLog()
        {
            m_watchTime.WatchTheActionAndCatchExceptionAsLog(m_wathToExecute.Invoke);
        }
        [ContextMenu("Start Counting and Reset Timer")]
        public void StartCountingAndResetTimer()
        {
            m_watchTime.StartCounting();
        }
        [ContextMenu("Stop Counting")]
        public void StopCounting()
        {
            m_watchTime.StopCounting();
        }
    }
}