/**************************************************************************
 * Copiright(C) 2016 Sergey Lupov
 *
 * English-in-Use (v.1.X.X) is an application for studying english.
 * Prepared english phrases are spoken in a random order.
 * You have to recall the translation of the phrase
 * and after a short pause they are spoken by the program automatically.
 *
 * Web Site: http://english-in-use.com
 * E-mail: sergey.lupov {at} english-in-use.com
 *
 * This file is part of English-in-Use.
 *
 * English-in-Use is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Foobar is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

using English_in_Use.Models.Phrases;

namespace English_in_Use.Models.DataReadUtils
{
    public class xmlDataReader : DataReader
    {
        public xmlDataReader(String xmlFile)
        {
            this.xmlFile = xmlFile;
        }
        //    Context context;
        String xmlFile;

        public List<VerbComponents> GetVerbs()
        {
            if (verbs.Count == 0) GetData();
            return verbs;
        }

        public List<int> GetSelectedVerbs()
        {
            if (selectedVerbs.Count == 0) GetData();
            return selectedVerbs;
        }

        private void GetData()
        {
            String str = ReadFile();
            if (str == null) return;
        }

        public String ReadFile()
        {
#if DEBUG
//            Environment.CurrentDirectory = "..\\..";
#endif
            string text = "";
            String xml_file_name = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Lesson1.xml");

            if (!File.Exists(xml_file_name))
            {
                string md = Environment.GetFolderPath(Environment.SpecialFolder.Personal) +
                                                    @"\english-in-use.com"; //My Documents path
                if (Directory.Exists(md) == true)
                    Environment.CurrentDirectory = md;
            }
            try
            {
                text = File.ReadAllText(xml_file_name, System.Text.Encoding.GetEncoding(65001));
                XMLParser(text);
            }
            catch (System.IO.FileNotFoundException)
            {
            }
            return text;
        }

        void XMLParser(string text)
        {
            string main_dir;

            Regex regex_maindir = new Regex(@"<MainDir>((.|\n)*?)</MainDir>", RegexOptions.IgnoreCase);
            Regex regex_dir = new Regex(@"<Dir>((.|\n)*?)</Dir>", RegexOptions.IgnoreCase);
            Regex regex_verb = new Regex(@"<Verb>((.|\n)*?)</Verb>", RegexOptions.IgnoreCase);
            Regex regex_present = new Regex(@"<PresentIndef>((.|\n)*?)</PresentIndef>", RegexOptions.IgnoreCase);
            Regex regex_past = new Regex(@"<PastIndef>((.|\n)*?)</PastIndef>", RegexOptions.IgnoreCase);
            Regex regex_rus = new Regex(@"[^(?=<Text>)][.\n]*<Rus>((.|\n)*?)</Rus>", RegexOptions.IgnoreCase);
            Regex regex_text = new Regex(@"<Text>((.|\n)*?)</Text>", RegexOptions.IgnoreCase);
            Regex regex_text_eng = new Regex(@"<Eng>((.|\n)*?)</Eng>", RegexOptions.IgnoreCase);
            Regex regex_text_rus = new Regex(@"<Rus>((.|\n)*?)</Rus>", RegexOptions.IgnoreCase);
            Regex regex_text_file = new Regex(@"<File>((.|\n)*?)</File>", RegexOptions.IgnoreCase);
            Regex regex_replace_space = new Regex(@"\s+");
            Regex regex_delete_znaks = new Regex(@"[\[\].,'?!-:]");

            Match match_maindir = regex_maindir.Match(text);
            if (match_maindir.Groups.Count > 1) main_dir = match_maindir.Groups[1].Value;
            else main_dir = "mp3";
            MatchCollection matches = regex_verb.Matches(text);

            for (int i = 0; i < matches.Count; i++)
            {
                Match match_dir = regex_dir.Match(matches[i].Groups[1].Value);
                Match match_present = regex_present.Match(matches[i].Groups[1].Value);

                Match match_past = regex_past.Match(matches[i].Groups[1].Value);
                Match match_rus = regex_rus.Match(matches[i].Groups[1].Value);
                string verb_dir;
                if (match_dir.Groups.Count > 1) verb_dir = match_dir.Groups[1].Value;
                else
                {
                    verb_dir = match_present.Groups[1].Value;
                    verb_dir = regex_delete_znaks.Replace(verb_dir, "");
                }
                if (match_present.Groups.Count > 1 || match_past.Groups.Count > 1 || match_rus.Groups.Count > 1)
                {
                    List<Phrase> phrases = new List<Phrase>();

                    MatchCollection matches_text = regex_text.Matches(matches[i].Groups[1].Value);
                    for (int n = 0; n < matches_text.Count; n++)
                    {
                        String engPhrase;
                        String rusPhrase;
                        String fileName="";
                        Match match_text_eng = regex_text_eng.Match(matches_text[n].Groups[1].Value);
                        Match match_text_rus = regex_text_rus.Match(matches_text[n].Groups[1].Value);
                        Match match_text_file = regex_text_file.Match(matches_text[n].Groups[1].Value);
                        if (match_text_eng.Groups.Count > 1) engPhrase = match_text_eng.Groups[1].Value;
                        else engPhrase = "";
                        if (match_text_rus.Groups.Count > 1) rusPhrase = match_text_rus.Groups[1].Value;
                        else rusPhrase = "";
                        if (match_text_file.Groups.Count > 1) fileName = match_text_file.Groups[1].Value;
                        else if (match_text_eng.Groups.Count > 1)
                        {
                            fileName = engPhrase;
                            fileName = regex_replace_space.Replace(fileName, "_");
                            fileName = regex_delete_znaks.Replace(fileName, "");
                        }
                        else fileName = "";
                        fileName = main_dir + "/" + match_dir.Groups[1].Value + "/" + fileName;
                        phrases.Add(new Phrase(engPhrase, rusPhrase, fileName));
                    }

                    string[] words = match_present.Groups[1].Value.Split('[');

                    String verb_present = match_present.Groups[1].Value;
                    String verb_past = match_past.Groups[1].Value;
                    String verb_rus = match_rus.Groups[1].Value;

                    verbs.Add(new VerbComponents(phrases, verb_present, verb_past, verb_rus, verb_dir));
                    selectedVerbs.Add(i);
                }
            }
        }

        public List<VerbComponents> verbs = new List<VerbComponents>();
        public List<int> selectedVerbs = new List<int>();
    }
}