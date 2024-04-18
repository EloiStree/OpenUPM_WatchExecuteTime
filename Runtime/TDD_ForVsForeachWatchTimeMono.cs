using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Eloi.WatchAndDate;
public class TDD_ForVsForeachWatchTimeMono : MonoBehaviour
{


    public WatchAndDateTimeActionResult m_allTiming;
    public WatchAndDateTimeActionResult m_foreachLog;
    public WatchAndDateTimeActionResult m_forLog;

    public int m_forCount = 10;

    void Update()
    {
        m_allTiming.WatchTheAction(() => {

            byte[] hello = new byte[m_forCount];

            m_foreachLog.WatchTheAction(() =>
            {
                foreach (var item in hello)
                {
                    Debug.Log("Hello World!");
                }
            });
            m_forLog.WatchTheAction(() =>
            {
                for (int i =0; i<hello.Length; i++)
                {
                    Debug.Log("Hello World!");
                }
            });


    });


    }
}
