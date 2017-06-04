using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD
{
    static class LocalizationData
    {
        private static List<List<string>> localizationData;
        public static int LaunguageID { get; private set; }

        public static void Load() {
            TextAsset localization = Resources.Load<TextAsset>("locale");
            Stream localeStream = new MemoryStream(localization.bytes);
            using (StreamReader sr = new StreamReader(localeStream)) {
                string[] locales=  sr.ReadLine().Split('$');
                localizationData = new List<List<string>>(locales.Length);
                for (int i = 0; i < locales.Length; i++)
                {
                    localizationData.Add(new List<string>());
                    localizationData[i].Add(locales[i]);
                }
                Utilities.ReadBlock(sr, (string locStr) => {
                    string[] localesStrs= locStr.Split('$');
                    for (int i = 0; i < locales.Length; i++)
                    {
                        localizationData[i].Add(localesStrs[i]);
                    }
                });
            }
        }
        public static string GetLocalizedString(int id) {
            return localizationData[LaunguageID][id];
        }
        public static void SetLaungageID(int id) {
            LaunguageID = id;
        }
    }
}
