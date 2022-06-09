﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using Newtonsoft.Json;


namespace betterAutostart
{
    class LanguageSupport
    {
        private String[] allLanguageJSONFiles;
        private List<LanguageObj> languages = new List<LanguageObj>();
        private List<LanguageObj> possibleLanguages = new List<LanguageObj>();
        private List<String> possibleLanguageNames = new List<String>();


        public void LoadAllLanguages()
        {
            if (Utility.DesignMode)
            {
                this.allLanguageJSONFiles = Directory.GetFiles("./../../languagePackages");
            }
            else
            {
                this.allLanguageJSONFiles = Directory.GetFiles("./languagePackages");
            }
            
            for (int i = 0; i < this.allLanguageJSONFiles.Length; i++)
            {
                LanguageObj tempLang = JsonConvert.DeserializeObject<LanguageObj>(File.ReadAllText(this.allLanguageJSONFiles[i]));

                languages.Add(tempLang);
                if (tempLang.active == 1) 
                {
                    possibleLanguages.Add(tempLang);
                    possibleLanguageNames.Add(tempLang.name);
                }
            }

        }

        public Object getTranslation(String property)
        {
            try
            {
                return Config.ActiveLanguage.strings.GetType().GetField(property).GetValue(Config.ActiveLanguage.strings).ToString();
            }
            catch (Exception e)
            {
                Config.ErrorLog.LogError(e);
                return "Not found";
            }
        }

        public LanguageObj GetLanguageByName(String name) 
        {
            return possibleLanguages.Find(lang => lang.name == name);
        }

        public List<String> GetPossibleLanguagesNames() 
        {
            return possibleLanguageNames;
        }
    }
}