using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace exam
{
    public class DictCollection
    {
        [XmlText(typeof(List<Dict>))]
        private List<Dict> dicts = new List<Dict>();
        public List<Dict> Dicts => dicts;

        public Dict this[int i]
        {
            get => dicts[i];
        }
        public void Add(Dict dict)
        {
            dicts.Add(dict);
        }
        public void Save(string filename)
        {
            var serializer = new XmlSerializer(typeof(DictCollection));
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, this);
            }
        }
        public void Load(string filename)
        {
            var serializer = new XmlSerializer(typeof(DictCollection));
            DictCollection loadedDicts = new DictCollection();
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    loadedDicts = (DictCollection)(serializer.Deserialize(fs));
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File {filename} not found!");
                return;
            }
            dicts = loadedDicts.Dicts;
        }
    }
}
