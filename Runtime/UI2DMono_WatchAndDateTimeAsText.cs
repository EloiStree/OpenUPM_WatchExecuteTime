using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
namespace Eloi.WatchAndDate
{
    public class UI2DMono_WatchAndDateTimeAsText : MonoBehaviour
    {

        public string m_category;
        [TextArea(2, 5)]
        public string m_text;
        public UnityEvent<string> m_onFresh;
        public int m_maxTextLenght=9000;
        public int m_maxTextElement=100;

        public bool m_useUpdateToRefresh=true;

        public void Update()
        {
            if (m_useUpdateToRefresh)
                Refresh();
        }

        [ContextMenu("Refresh")]
        public void Refresh() {

            StringBuilder sb = new StringBuilder();
            int count= 0;
            foreach (WatchAndDateOverwatchResult result in StaticWatchAndDateResultExisting.GetListAsStoredCopy(m_category)) {

                sb.AppendLine(">> " + result.GetDescription());
                result.GetTick(out double w, out double d);
                result.GetMilliseconds(out double wms, out double dms);
                sb.AppendLine($"Watch {w}t {wms}ms / Date {d}t {dms}ms");
                sb.AppendLine();
                count++;
                if (count >= m_maxTextElement)
                    break;
            }
            string s = sb.ToString();
            if (s.Length > m_maxTextLenght)
                s = s.Substring(0, m_maxTextLenght);
            m_text = s;
            m_onFresh.Invoke(s);
        }
    }
}