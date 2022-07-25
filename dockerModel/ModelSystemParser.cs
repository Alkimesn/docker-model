using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace dockerModel
{
    static class ModelSystemParser
    {
        public static ModelComponentBase Parse(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ModelComponentBase), new Type[] { typeof(ModelComponentPrimitive), typeof(ModelComponentComplexAND), typeof(ModelComponentComplexOR) });
            ModelComponentBase res = null;
            using (FileStream fs = new FileStream("res.xml", FileMode.OpenOrCreate))
            {
                res=serializer.Deserialize(fs) as ModelComponentBase;
            }
            return res;
        }
        public static void SerializeTest()
        {
            ModelComponentPrimitive primitive1 = new ModelComponentPrimitive();
            ModelComponentPrimitive primitive2 = new ModelComponentPrimitive();
            ModelComponentPrimitive primitive3 = new ModelComponentPrimitive();
            ModelComponentPrimitive primitive4 = new ModelComponentPrimitive();
            ModelComponentPrimitive primitive5 = new ModelComponentPrimitive();
            primitive1.Set("1-1",ModelTimelineType.PROBABILITY, ModelTimelineType.PROBABILITY, 10, 0.5f);
            primitive2.Set("1-2", ModelTimelineType.PROBABILITY, ModelTimelineType.PROBABILITY, 10, 0.5f);
            primitive3.Set("2-1", ModelTimelineType.PROBABILITY, ModelTimelineType.PROBABILITY, 5, 1f);
            primitive4.Set("2-2", ModelTimelineType.PROBABILITY, ModelTimelineType.PROBABILITY, 5, 1f);
            primitive5.Set("2-3", ModelTimelineType.PROBABILITY, ModelTimelineType.PROBABILITY, 5, 1f);
            ModelComponentComplexOR or = new ModelComponentComplexOR();
            ModelComponentComplexAND and = new ModelComponentComplexAND();
            or.AddComponent(primitive1);
            or.AddComponent(primitive2);
            and.AddComponent(or);
            ModelComponentComplexOR or2 = new ModelComponentComplexOR();
            or2.AddComponent(primitive3);
            or2.AddComponent(primitive4);
            or2.AddComponent(primitive5);
            and.AddComponent(or2);
            ModelComponentComplexAND and2 = new ModelComponentComplexAND();
            and2.AddComponent(primitive1);
            and2.AddComponent(primitive2);
            XmlSerializer serializer = new XmlSerializer(typeof(ModelComponentBase),new Type[] { typeof(ModelComponentPrimitive), typeof(ModelComponentComplexAND), typeof(ModelComponentComplexOR)});
            using (FileStream fs = new FileStream("res.xml", FileMode.Create))
            {
                serializer.Serialize(fs, and);
            }
        }
    }
}
