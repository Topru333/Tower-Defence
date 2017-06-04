using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TD
{

    static class SettingsHolder
    {
        public static float SoundVolume;
        public static int GraphicsQualityLevel;
        public static int LaunguageID;

        public static void Load() {
            if(!File.Exists(Application.dataPath + "/settings.set"))
            {
                ResetDefaults();
                Save();
                return;
            }

            Stream s = File.OpenRead(Application.dataPath + "/settings.set");
            using (StreamReader sr=new StreamReader(s))
            {
                string str= sr.ReadLine();
                if (!float.TryParse(str, out SoundVolume))
                {
                    ResetDefaults(); Save();
                    return;
                }
                str = sr.ReadLine();
                if (!int.TryParse(str, out GraphicsQualityLevel))
                {
                    ResetDefaults(); Save();
                    return;
                }
                str = sr.ReadLine();
                if (!int.TryParse(str, out LaunguageID))
                {
                    ResetDefaults(); Save();
                    return;
                }
            }
            AudioListener.volume = SoundVolume;
            LocalizationData.SetLaungageID(LaunguageID);
            QualitySettings.SetQualityLevel(GraphicsQualityLevel);
        }
        public static void SetLocale(int loc)
        {
            LaunguageID = loc;
            LocalizationData.SetLaungageID(LaunguageID);
        }
        public static void Save()
        {
            Stream s = File.Create(Application.dataPath+"/settings.set");
            using (StreamWriter sw = new StreamWriter(s))
            {
                sw.WriteLine(SoundVolume);
                sw.WriteLine(GraphicsQualityLevel);
                sw.WriteLine(LaunguageID);
            }
        }
        public static void ResetDefaults()
        {
            SoundVolume = 1;
            GraphicsQualityLevel = 0;
            LaunguageID = 0;
        }
    }
}
