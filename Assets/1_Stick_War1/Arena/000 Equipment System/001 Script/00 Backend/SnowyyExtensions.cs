using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Snowyy.Ultilities
{
    public static class SnowyyExtensions
    {
        public static string FormatLargeNumber(long number)
        {
            if (number <= 0) return "0";

            long mag = (long)(Mathf.Floor(Mathf.Log10(number)) / 3);
            double divisor = Mathf.Pow(10, mag * 3);

            double shortNumber = number / divisor;

            string suffix = "";
            switch (mag)
            {
                case 0:
                    suffix = string.Empty;
                    break;
                case 1:
                    suffix = "K";
                    break;
                case 2:
                    suffix = "M";
                    break;
                case 3:
                    suffix = "B";
                    break;
                case 4:
                    suffix = "T";
                    break;
            }
            //return shortNumber.ToString("N1") + suffix;
            return $"{shortNumber:.##}{suffix}";
        }

        public static bool CheckConditionDay(string stringTimeCheck, int maxDays)
        {
            if (string.IsNullOrEmpty(stringTimeCheck))
            {

                return false;
            }
            try
            {
                DateTime timeNow = DateTime.Now;
                DateTime timeOld = DateTime.Parse(stringTimeCheck);
                DateTime timeOldCheck = new DateTime(timeOld.Year, timeOld.Month, timeOld.Day, 0, 0, 0);
                long tickTimeNow = timeNow.Ticks;
                long tickTimeOld = timeOldCheck.Ticks;

                long elapsedTicks = tickTimeNow - tickTimeOld;
                TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                double totalDay = elapsedSpan.TotalDays;

                if (totalDay >= maxDays)
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }

            return false;
        }

        public static string DisplayStringSortType(int sortType)
        {
            return sortType switch
            {
                (int)SortType.BYLEVEL => "BY LEVEL",
                (int)SortType.BYRARITY => "BY RARITY",
                (int)SortType.BYSLOT => "BY SLOT",
                _ => string.Empty,
            };
        }
    }
}
