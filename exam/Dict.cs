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

        private List<DictEntry> entries = new List<DictEntry>();
        public List<DictEntry> Entries => entries;

        private List<string> searchHistory = new List<string>();
        public List<string> SearchHistory => searchHistory;

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
        public int DeleteSourceWord(string source)
        {
            int deleted = 0;
            Entries.RemoveAll(entry => { deleted++; return entry.SourceWord == source; });
            return deleted;
        }
        public void DeleteTranslation(string source, string translation)
        {
            var query = from entry in Entries
                        where entry.SourceWord == source
                        select entry;
            if(query.Count() <= 1)
            {
                Console.WriteLine("Last translation cannot delete!");
                return;
            }
            Entries.RemoveAll(entry => (entry.SourceWord == source &&
                                        entry.Translation == translation));
        }
        public List<DictEntry> Search(string source)
        {
            searchHistory.Add(source);
            return (from entry in Entries
                    where entry.SourceWord == source
                    select entry).ToList();
        }
        public void SearchWithExport(string source, string filename)
        {
            var serializer = new XmlSerializer(typeof(List<DictEntry>));
            var searchResult = Search(source);
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, searchResult);
            }
        }
        public void Save(string filename)
        {
            var serializer = new XmlSerializer(typeof(Dict));
            using (var fs = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                serializer.Serialize(fs, this);
            }
        }
        public void Load(string filename)
        {
            var serializer = new XmlSerializer(typeof(Dict));
            Dict loadedDict = new Dict();
            try
            {
                using (var fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    loadedDict = (Dict)(serializer.Deserialize(fs));
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"File {filename} not found!");
                return;
            }
            SourceLanguage = loadedDict.SourceLanguage;
            DestinationLanguage = loadedDict.DestinationLanguage;
            entries = loadedDict.Entries;
        }
        public void ClearSearchHistory()
        {
            searchHistory = new List<string>();
        }
        public List<string> GetSearchHistory(int amount)
        {
            return ((IEnumerable<string>)SearchHistory).Reverse().Take(amount).ToList();
        }
    }
}
