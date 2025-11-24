using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppTools.Serialize.XmlEntity
{
    public class KocosBreaker
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("table")]
        public KocosTable Table { get; set; }

        [XmlElement("testplans")]
        public KocosTestPlans TestPlans { get; set; }
    }


    public class KocosField
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("tag")]
        public string Tag
        {
            get;
            set;
        }

        [XmlAttribute("index")]
        public string Index
        {
            get;
            set;
        }

        [XmlAttribute("type")]
        public string Type
        {
            get;
            set;
        }

        [XmlAttribute("value")]
        public string Value
        {
            get;
            set;
        }
    }


    public class KocosJob
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("testobjects")]
        public KocosTestObjects TestObjects { get; set; }


    }

    public class KocosResult
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tag")]
        public string Tag { get; set; }
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
        [XmlAttribute("unit")]
        public string Unit { get; set; }
        [XmlAttribute("tol_error")]
        public string TolError { get; set; }
    }


    public class KocosResults
    {
        [XmlElement("Result")]
        public List<KocosResult> Result { get; set; }
        [XmlElement("Record")]
        public string Record { get; set; }


    }

    public class KocosTable
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlElement("field")]
        public string Field { get; set; }
    }

    public class KocosTest
    {
        [XmlAttribute("tester")]
        public string Tester
        {
            get;
            set;
        }

        [XmlAttribute("date")]
        public string Date
        {
            get;
            set;
        }

        [XmlAttribute("ok")]
        public bool OK
        {
            get;
            set;
        }

        [XmlElement("Results")]
        public KocosResults Results
        {
            get;
            set;
        }
    }

    public class KocosTestObjects
    {

        [XmlElement("brk")]
        public List<KocosBreaker> Brk
        {
            get;
            set;
        }
    }

    public class KocosTestPlan
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlElement("teststeps")]
        public KocosTestSteps TestSteps
        {
            get;
            set;
        }
    }


    public class KocosTestPlans
    {
        [XmlElement("testplan")]
        public List<KocosTestPlan> testplan { get; set; }
    }

    public class KocosTests
    {
        [XmlElement("Test")]
        public List<KocosTest> Test
        {
            get;
            set;
        }
    }

    public class KocosTestStep
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        }

        [XmlAttribute("tag")]
        public string Tag
        {
            get;
            set;
        }

        [XmlElement("Tests")]
        public KocosTests Tests
        {
            get;
            set;
        }
    }

    public class KocosTestSteps
    {
        [XmlElement("Teststep")]
        public List<KocosTestStep> TestStep { get; set; }
    }


    [XmlRoot("XML")]
    public class KocosXML
    {
        [XmlElement("job")]
        public KocosJob Job
        {
            get;
            set;
        }
    }

    public class KocosXmlResult
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("tag")]
        public string Tag { get; set; }
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlAttribute("value")]
        public string Value { get; set; }
        [XmlAttribute("unit")]
        public string Unit { get; set; }
        [XmlAttribute("tol_error")]
        public string TolError { get; set; }
    }
}
