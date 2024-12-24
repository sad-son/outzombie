using UnityEngine;

namespace BootSystem
{
    public class TimeMeasure
    {
        private object _source;
        private double _originTime, _totalTime;
        
        public TimeMeasure(object source)
        {
            _source = source;
        }
        
        public void CommitStartTime()
        {
            _originTime = GetTime();
        }
        
        public void CommitEndTime()
        {
            _totalTime = GetTime() - _originTime;

            Debug.Log($"[{_source.GetType().Name}] - execution time {_totalTime:F3}");
        }

        private double GetTime() => Time.realtimeSinceStartupAsDouble;
    }
}