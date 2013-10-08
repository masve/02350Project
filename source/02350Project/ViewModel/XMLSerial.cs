using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.IO;

namespace _02350Project.ViewModel
{
    class XMLSerial
    {
        public XMLSerial(String name, ObservableCollection<object> collection)
        {
            XmlSerializer xml = new XmlSerializer(typeof(ObservableCollection<object>));
            using (StreamWriter writer = new StreamWriter(name))
            {
                xml.Serialize(writer, collection);
            }
        }
    }
} //monitor commands
