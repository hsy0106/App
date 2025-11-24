using AppTools.Serialize.XmlEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AppTools.Serialize.Server
{
    public class XmlOperation
    {
        private KocosXML kocosXML;

        public bool OpenXML(string path, ref string err)
        {
            try
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(KocosXML));
                FileStream stream = new FileStream(path, FileMode.Open);
                kocosXML = (KocosXML)xmlSerializer.Deserialize(stream);
                return true;
            }
            catch (Exception ex)
            {
                err = ex.Message;
            }

            return false;
        }

        public KocosTable GetPara(string product_id)
        {
            return kocosXML?.Job?.TestObjects?.Brk?.Find((KocosBreaker a) => a.Name == product_id)?.Table;
        }

        public KocosResults GetResult(string brk_name, string teststep_tag)
        {
            return kocosXML?.Job?.TestObjects?.Brk?.Find((KocosBreaker a) => a.Name == brk_name)?.TestPlans?.testplan?[0].TestSteps?.TestStep?.Find((KocosTestStep a) => a.Tag.Contains(teststep_tag))?.Tests?.Test?.OrderByDescending((KocosTest a) => a.Date).FirstOrDefault().Results;
        }

        private string GetValueByKeyC(string product_id, string key)
        {
            return kocosXML?.Job?.TestObjects?.Brk?.Find((KocosBreaker a) => a.Name == product_id)?.TestPlans?.testplan?[0].TestSteps?.TestStep?.Find((KocosTestStep a) => a.Name.Contains("Close"))?.Tests?.Test?.OrderByDescending((KocosTest a) => a.Date).FirstOrDefault().Results?.Result?.Find((KocosResult a) => a.Key == key).Value;
        }

        private string GetValueByKeyO(string product_id, string key)
        {
            return kocosXML?.Job?.TestObjects?.Brk?.Find((KocosBreaker a) => a.Name == product_id)?.TestPlans?.testplan?[0].TestSteps?.TestStep?.Find((KocosTestStep a) => a.Name.Contains("Open"))?.Tests?.Test?.OrderByDescending((KocosTest a) => a.Date).FirstOrDefault().Results?.Result?.Find((KocosResult a) => a.Key == key).Value;
        }

    }
}
