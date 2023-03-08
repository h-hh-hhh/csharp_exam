﻿using System;
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
            using(StringWriter stringWriter = new Utf8StringWriter())
            {
                serializer.Serialize(stringWriter, this);

                using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    fs.SetLength(0); // clear file
                    fs.Write(Encoding.UTF8.GetBytes(stringWriter.ToString()), 0, stringWriter.ToString().Count());
                }
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
