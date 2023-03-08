using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace exam
{
    public class DictCollection : IEnumerable<Dict>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [XmlText(typeof(List<Dict>))]
        private List<Dict> dicts = new List<Dict>();
        public List<Dict> Dicts => dicts;

        public int Count => dicts.Count;

        public Dict this[int i] => dicts[i];

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
            logger.Info($"Serialized successfully to {filename}");
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
                logger.Info($"Deserialized successfully from {filename}");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File {filename} not found!");
                logger.Warn($"File {filename} not found!");
                return;
            }
            catch (Exception e)
            {
                logger.Error($"Error when deserializing {filename}: {e.Message}");
            }
            dicts = loadedDicts.Dicts;
        }

        public IEnumerator<Dict> GetEnumerator()
        {
            return ((IEnumerable<Dict>)Dicts).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dicts).GetEnumerator();
        }
    }
}
