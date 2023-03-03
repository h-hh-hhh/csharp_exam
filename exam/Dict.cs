using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace exam
{
    public class Dict
    {
        [XmlAttribute("source")]
        public string SourceLanguage { get; set; }
        [XmlAttribute("dest")]
        public string DestinationLanguage { get; set; }
        public List<DictEntry> Entries { get; } = new List<DictEntry>();
        public void Add(DictEntry entry)
        {
            Entries.Add(entry);
        }
        public void SubstituteSourceWord(string source, string translation, string substSource)
        {
            Entries.Find(entry => entry.SourceWord == source && entry.Translation == translation).SourceWord = substSource;
        }
        public void SubstituteTranslation(string source, string translation, string substTranslation)
        {
            Entries.Find(entry => entry.SourceWord == source && entry.Translation == translation).Translation = substTranslation;
        }
        // returns number of deleted words
        public void DeleteSourceWord(string source)
        {
            var query = from entry in Entries
                        where entry.SourceWord == source
                        select entry;
            foreach (var entry in query)
            {
                Entries.Remove(entry);
            }
        }
        public void DeleteTranslation(string source, string translation)
        {
            var query = from entry in Entries
                        where entry.SourceWord == source
                        where entry.Translation == translation
                        select entry;
            foreach (var entry in query)
            {
                Entries.Remove(entry);
            }
        }
        public List<DictEntry> Search(string source)
        {
            return (from entry in Entries
                    where entry.SourceWord == source
                    select entry).ToList();
        }
        public void SearchWithExport(string source, string filename)
        {
            var serializer = new XmlSerializer(typeof(List<DictEntry>));
            var searchResult = (from entry in Entries
                                where entry.SourceWord == source
                                select entry).ToList();
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, searchResult);
            }
        }
    }
}
