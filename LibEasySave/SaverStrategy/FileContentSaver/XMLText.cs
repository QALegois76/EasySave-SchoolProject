using LibEasySave.Model.LogMng.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace LibEasySave
{
    class XMLText : LogBaseSaver
    {

        public override string GetFormatingText (object log, bool completeType = false)
        {
            XmlSerializer xSerialize = new XmlSerializer(typeof(object));
            using (var stringWriter = new StringWriter())
            {
                using (XmlTextWriter textWriter = new XmlTextWriter(stringWriter) { Formatting = Formatting.Indented })
                {
                    xSerialize.Serialize(textWriter, log);
                    return stringWriter.ToString();
                }
            }
        }
    }
}
