using System;

namespace HiloCodeChallenge
{
    public static class Commands
    {
        public static string Execute(string command)
        {
            string output = "";
            switch (command.ToUpper().Replace("\n", ""))
            {
                case "DATE":
                    output = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "TIME":
                    output = DateTime.Now.ToString($"hh:mm:ss, {TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString()}");
                    break;
                case "DATETIME":
                    output = DateTime.Now.ToString($"yyyy-MM-ddThh:mm:ss, {TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString()}");
                    break;
                default:
                    throw new ArgumentException("Commande doesn't exist.");
            }
            return output + Environment.NewLine;
        }
    }
}
