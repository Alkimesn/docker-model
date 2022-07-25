using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dockerModel
{
    public class ModelComponentPrimitive:ModelComponentBase
    {
        List<(float, float)> timeline;
        public ModelTimelineType failType;
        public ModelTimelineType restoreType;
        public float failTime;
        public float restoreTime;
        Dictionary<float, bool> hash = new Dictionary<float, bool>();
        public override bool IsFunctional(float time)
        {
            if (hash.ContainsKey(time)) return hash[time];
            foreach (var item in timeline)
            {
                if (time > item.Item2) continue;
                if (time < item.Item1) break;
                hash.Add(time, false);
                return false;
            }
            hash.Add(time, true);
            return true;
        }
        public void Set(string name, ModelTimelineType failType, ModelTimelineType restoreType, float timeFail, float timeRestore)
        {
            this.Name = name;
            this.failType = failType;
            this.restoreType = restoreType;
            this.failTime = timeFail;
            this.restoreTime = timeRestore;
        }
        public override void FillTimeline(float timeMax)
        {
            if (timeline != null) return;
            timeline = new List<(float, float)>();
            if(failType==ModelTimelineType.CONST && restoreType==ModelTimelineType.CONST)
            {
                float curtime = 0;
                while(curtime<timeMax)
                {
                    float timestart = curtime + failTime;
                    float timeend = timestart + restoreTime;
                    timeline.Add((timestart, timeend));
                    curtime = timeend;
                }
            }
            if (failType == ModelTimelineType.PROBABILITY && restoreType == ModelTimelineType.CONST)
            {
                float curtime = 0;
                while (curtime < timeMax)
                {
                    float add = ModelUtils.GetNextWithProbability(1.0f / failTime, timeMax-curtime);
                    if (add < 0) break;
                    float timestart = curtime + add;
                    float timeend = timestart + restoreTime;
                    timeline.Add((timestart, timeend));
                    curtime = timeend;
                }
            }
            if (failType == ModelTimelineType.PROBABILITY && restoreType == ModelTimelineType.PROBABILITY)
            {
                float curtime = 0;
                while (curtime < timeMax)
                {
                    float add = ModelUtils.GetNextWithProbability(1.0f / failTime, timeMax - curtime);
                    if (add < 0) break;
                    float timestart = curtime + add;
                    float add2 = ModelUtils.GetNextWithProbability(1.0f / restoreTime, timeMax - curtime);
                    if (add2 < 0) break;
                    float timeend = timestart + add2;
                    timeline.Add((timestart, timeend));
                    curtime = timeend;
                }
            }
        }
        public override string GetFunctionalTotal(float time)
        {
            return this.Name+": "+(IsFunctional(time) ? "1" : "0")+"; ";
        }
    }
}
