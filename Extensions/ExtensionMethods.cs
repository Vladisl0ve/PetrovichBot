using PetrovichBot.Properties;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PetrovichBot.Extensions
{
    public static class ExtensionMethods
    {
        public static bool IsEqual(this BotCommand[] bcFirst, BotCommand[] bcSecond)
        {
            if (bcFirst == null && bcSecond == null)
                return true;

            if (bcFirst == null || bcSecond == null)
                return false;

            if (bcFirst.Length != bcSecond.Length)
                return false;

            for (int i = 0; i < bcFirst.Length; i++)
            {
                if (bcFirst[i].Description != bcSecond[i].Description
                    || bcFirst[i].Command != bcSecond[i].Command)
                    return false;
            }

            return true;
        }

        public static string? UseCulture(this string toTranslate, string culture)
        {
            if (string.IsNullOrEmpty(culture) || string.IsNullOrEmpty(toTranslate))
                return null;

            CultureInfo cultureInfo;
            try
            {
                cultureInfo = CultureInfo.GetCultureInfo(culture);
            }
            catch (Exception e)
            {
                Log.Error(e, "ResourceManager");
                return null;
            }

            return toTranslate.UseCulture(cultureInfo);
        }

        public static string? UseCulture(this string toTranslate, CultureInfo culture)
        {
            if (culture == null || string.IsNullOrEmpty(toTranslate))
                return null;

            return Resources.ResourceManager.GetString(toTranslate, culture);
        }

    }
}
