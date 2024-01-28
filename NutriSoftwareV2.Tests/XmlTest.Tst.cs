using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace NutriSoftwareV2.UnitTest
{
    [TestClass]
    public class XmlTest 
    {
        [TestMethod]
        public void TestXmlToObject ()
        {

            XmlSerializer xsSubmit = new XmlSerializer(typeof(Xml));
            var subReq = new Xml { Nome="Fredolato dos Santos",
                                   Idade= 60,
                                   Cpf="05565878944"
                                 };
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xsSubmit.Serialize(writer, subReq);
                    xml = sww.ToString(); // Your XML
                }
            }

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Xml));

            using var stringReader = new StringReader(xml);
            var xmlReader = new XmlTextReader(stringReader);
            var resultado =  (Xml)xmlSerializer.Deserialize(xmlReader);

            // Construct an instance of the XmlSerializer with the type
            // of object that is being deserialized.
            var mySerializer = new XmlSerializer(typeof(Xml));
            // To read the file, create a FileStream.
            using var myFileStream = new FileStream(xml, FileMode.Open);
            // Call the Deserialize method and cast to the object type.
            var myObject = (Xml)mySerializer.Deserialize(myFileStream);
        }
    }

    [Serializable]
    public class Xml
    {
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cpf { get; set; }
    }

    public class ObjetoParaXmlSerializer<T> where T : class
    {
        public static string SerializarObjetoParaXml(T obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(sww) { Formatting = Formatting.Indented })
                {
                    xsSubmit.Serialize(writer, obj);
                    return sww.ToString();
                }
            }

        }
        public T SerializarXmlParaObjeto<T>(string xmlStringName)
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));

            using var stringReader = new StringReader(xmlStringName);
            var xmlReader = new XmlTextReader(stringReader);
            return (T)xmlSerializer.Deserialize(xmlReader);
        }

    }


}
