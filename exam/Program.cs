using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace exam
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;

            Console.WriteLine("тест1488");

            DictCollection dictCollection = new DictCollection();
            int? currentDict = null;
            dictCollection.Load("dictionaries.xml");
            string input1, input2, input3 = "";
            int inputInt = 0;
            while (true)
            {
                if (currentDict.HasValue)
                {
                    switch (Menu(dictCollection, true))
                    {
                        case "1":
                            Console.WriteLine("Enter source word");
                            input1 = Console.ReadLine();
                            Console.WriteLine("Enter translation");
                            input2 = Console.ReadLine();
                            dictCollection[(int)currentDict].Add(new DictEntry { SourceWord = input1, Translation = input2 });
                            break;
                        case "2":
                            Console.WriteLine("Enter source word");
                            input1 = Console.ReadLine();
                            Console.WriteLine("Enter translation");
                            input2 = Console.ReadLine();
                            Console.WriteLine("Enter new source word");
                            input3 = Console.ReadLine();
                            dictCollection[(int)currentDict].SubstituteSourceWord(input1, input2, input3);
                            break;
                        case "3":
                            Console.WriteLine("Enter source word");
                            input1 = Console.ReadLine();
                            Console.WriteLine("Enter translation");
                            input2 = Console.ReadLine();
                            Console.WriteLine("Enter new translation");
                            input3 = Console.ReadLine();
                            dictCollection[(int)currentDict].SubstituteTranslation(input1, input2, input3);
                            break;
                        case "4":
                            Console.WriteLine("Enter source word to be deleted");
                            input1 = Console.ReadLine();
                            dictCollection[(int)currentDict].DeleteSourceWord(input1);
                            break;
                        case "5":
                            Console.WriteLine("Enter source word to be deleted");
                            input1 = Console.ReadLine();
                            Console.WriteLine("Enter translation to be deleted");
                            input2 = Console.ReadLine();
                            dictCollection[(int)currentDict].DeleteTranslation(input1, input2);
                            break;
                        case "6":
                            Console.WriteLine("Enter source word to be searched");
                            input1 = Console.ReadLine();
                            foreach (var entry in dictCollection[(int)currentDict].Search(input1))
                                Console.WriteLine(entry);
                            break;
                        case "7":
                            Console.WriteLine("Enter source word to be searched");
                            input1 = Console.ReadLine();
                            Console.WriteLine("Enter destination xml file");
                            input2 = Console.ReadLine();
                            dictCollection[(int)currentDict].SearchWithExport(input1, input2);
                            break;
                        case "8":
                            dictCollection.Save("dictionaries.xml");
                            break;
                        case "9":
                            Console.WriteLine("Enter number of results to be displayed");
                            inputInt = Convert.ToInt32(Console.ReadLine());
                            foreach (var entry in dictCollection[(int)currentDict].GetSearchHistory(inputInt))
                                Console.WriteLine(entry);
                            break;
                        case "10":
                            dictCollection[(int)currentDict].ClearSearchHistory();
                            break;
                    }
                }
                else
                {
                    input1 = Menu(dictCollection, false);
                    if (input1 == "a")
                    {
                        Dict dict = new Dict();
                        Console.WriteLine("Enter source language");
                        dict.SourceLanguage = Console.ReadLine();
                        Console.WriteLine("Enter destination language");
                        dict.DestinationLanguage = Console.ReadLine();
                        dictCollection.Add(dict);
                    }
                    else if (int.TryParse(input1, out inputInt))
                    {
                        try
                        {
                            _ = dictCollection[inputInt];
                            currentDict = inputInt;
                        }
                        catch
                        {
                            Console.WriteLine("Not a valid dictionary, try another.");
                        }
                    }
                }
            }
        }

        private static string Menu(DictCollection dc, bool isDictionaryOpen)
        {
            if (isDictionaryOpen)
            {
                Console.Write(
                    "1 - add\n" +
                    "2 - substitute by source word\n" +
                    "3 - substitute by translation\n" +
                    "4 - delete source word\n" +
                    "5 - delete translation\n" +
                    "6 - search\n" +
                    "7 - search and export\n" +
                    "8 - save\n" +
                    "9 - show search history\n" +
                    "10 - clear search history\n" +
                    "Your choice: "
                    );
            }
            else
            {
                Console.WriteLine("Select dictionary by number:");
                foreach (var (dict, i) in dc.Select((dict, i) => (dict, i)))
                {
                    Console.WriteLine($"{i}: {dict.SourceLanguage} - {dict.DestinationLanguage}");
                }
                Console.WriteLine("Type 'a' to add a new dictionary");
            }
            return Console.ReadLine();
        }
    }
}
