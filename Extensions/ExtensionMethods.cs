using PetrovichBot.Properties;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

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
        public static ReplyKeyboardMarkup MenuKeyboardMarkup(CultureInfo culture) => _menuKeyboardMarkup(culture);
        private static ReplyKeyboardMarkup _menuKeyboardMarkup(CultureInfo culture) =>
                new ReplyKeyboardMarkup(
                    new List<List<KeyboardButton>>()
                    {
                        new List<KeyboardButton>
                        {
                            new KeyboardButton(nameof(Resources.RandomJokeButton).UseCulture(culture)),
                            new KeyboardButton(nameof(Resources.RandomBezdnaButton).UseCulture(culture)),
                        },
                        new List<KeyboardButton>
                        {
                            new KeyboardButton(nameof(Resources.TopJokeButton).UseCulture(culture)),
                            new KeyboardButton(nameof(Resources.TopBezdnaButton).UseCulture(culture)),
                        }
                    }
                    )
                { 
                    ResizeKeyboard = true
                };

        public static string? GetStringBetweenTwoStrings(this string text, string startDelimeter, string finishDelimeter)
        {
            try
            {
                int pFrom = text.IndexOf(startDelimeter) + startDelimeter.Length;
                int pTo = text.LastIndexOf(finishDelimeter);

                return text[pFrom..pTo];
            }
            catch(Exception ex)
            {
                Log.Error(ex, nameof(GetStringBetweenTwoStrings));
                return default;
            }
        }
    }
}
