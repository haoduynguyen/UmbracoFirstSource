using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Customs.Helper
{
    public class StringHelper
    {
        public static bool IsValidIdNo(string idNo)
        {
            string strRegex = "^[0-9]+$";
            Regex check = new Regex(strRegex, RegexOptions.IgnorePatternWhitespace);
            bool isValidIdNo = check.IsMatch(idNo) && (idNo.Length >= 9 && idNo.Length <= 12);
            if (!isValidIdNo)
            {
                return false;
            }
            return true;
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            string strRegex = "^[0-9]+$";
            Regex check = new Regex(strRegex, RegexOptions.IgnorePatternWhitespace);
            bool isValidIdNo = check.IsMatch(phoneNumber) && (phoneNumber.Length >= 5 && phoneNumber.Length <= 20);
            if (!isValidIdNo)
            {
                return false;
            }
            return true;

        }

        public static bool IsValidEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions
                .Regex(strRegex, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            if (!check.IsMatch(email))
            {
                return false;
            }
            return true;
        }

        public static string SubWord(object strss, int maxLength = 100, string FINISH_TEXT_STRING = "...")
        {
            string str = "";
            try
            {
                if (strss == null || String.IsNullOrEmpty(strss.ToString())) return "";

                str = Regex.Replace(strss.ToString(), "<.*?>", string.Empty).Trim();
                str = Regex.Replace(str.ToString(), "\n", string.Empty).Trim();
                string[] chars = new string[] { "!", "@", "#", "$", "%", "^", "*", "'", "\"", "(", ")", "|", "[", "]" };
                for (int i = 0; i < chars.Length; i++)
                {
                    if (str.Contains(chars[i]))
                    {
                        str = str.Replace(chars[i], "");
                    }
                }
                if (str.Length > maxLength)
                {
                    str = str.Substring(0, maxLength + 1);
                    if (str.Contains(" "))
                    {
                        str = str.Substring(0, Math.Min(str.Length, str.LastIndexOf(" ") == -1 ? 0 : str.LastIndexOf(" ")));
                    }
                    else
                    {
                        str = str.Substring(0, maxLength);
                    }
                    str += FINISH_TEXT_STRING;
                }
                str = System.Net.WebUtility.HtmlDecode(str);
            }
            catch (Exception ex)
            {
            }
            return str;
        }


        public static string GetYoutubeID(string youTubeUrl)
        {
            if (string.IsNullOrWhiteSpace(youTubeUrl)) return "";
            string strRegex = "^(?:https?\\:\\/\\/)?(?:www\\.)?(?:youtu\\.be\\/|youtube\\.com\\/(?:embed\\/|v\\/|watch\\?v\\=))([\\w-]{5,20})(?:[\\&\\?\\#].*?)*?(?:[\\&\\?\\#]t=([\\dhm]+s))?$";
            if (youTubeUrl.Contains("youtu.be"))
            {
                strRegex = @"youtu(?:\.be)/([a-zA-Z0-9-_]+)";
            }
            System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(strRegex, System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace);
            Match regexMatch = check.Match(youTubeUrl);
            if (regexMatch.Success)
            {
                return regexMatch.Groups[1].ToString();

            }
            else if (youTubeUrl.Contains("http://") || youTubeUrl.Contains("https://"))
            {
                Uri myUri = new Uri(youTubeUrl);
                var query = HttpUtility.ParseQueryString(myUri.Query);
                if (query.Get("v") != null)
                {
                    youTubeUrl = query.Get("v").Trim();
                }
            }
            return youTubeUrl;
        }

        public static string Slug(object strTxt, int maxLength = 120, string sperator = "-")
        {
            if (strTxt == null || string.IsNullOrEmpty(strTxt.ToString())) return "";
            string str = strTxt.ToString().ToLower();
            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    str = str.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();
            if (maxLength > 0)
                str = str.Substring(0, str.Length <= maxLength ? str.Length : maxLength).Trim();

            str = Regex.Replace(str, @"\s", sperator);
            return str;
        }

        private static readonly string[] VietNamChar = new string[]
        {
	            "aAeEoOuUiIdDyY",
	            "ăáàạảãâấầậẩẫăắằặẳẵ",
	            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",            
	            "éèẹẻẽêếềệểễ",
	            "ÉÈẸẺẼÊẾỀỆỂỄ",
	            "óòọỏõôốồộổỗơớờợởỡ",
	            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
	            "úùụủũưứừựửữ",
	            "ÚÙỤỦŨƯỨỪỰỬỮ",
	            "íìịỉĩ",
	            "ÍÌỊỈĨ",
	            "đ",
	            "Đ",
	            "ýỳỵỷỹ",
	            "ÝỲỴỶỸ"
        };

        public static string GetAppSetting(string configkey)
        {
            return System.Configuration.ConfigurationManager.AppSettings[configkey];
        }
        public static string ParsePostString(object text)
        {
            if (text == null || string.IsNullOrEmpty(text.ToString())) return "";
            string strText = text.ToString().Trim();
            strText = Regex.Replace(strText, @"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
            return strText;
        }
        public static string ParseQueryString(object text)
        {
            if (text == null || string.IsNullOrEmpty(text.ToString())) return "";
            string strText = Regex.Replace(text.ToString(), @"<[(=^>]*>", string.Empty);
            strText = strText.Replace("'", string.Empty);
            strText = strText.Replace("=", string.Empty);
            strText = Regex.Replace(strText, @"(;|\s)(exec|execute|select|insert|update|delete|create|alter|drop|rename|truncate|backup|restore)\s", string.Empty, RegexOptions.IgnoreCase);
            strText = Regex.Replace(strText, @"[*/]+", string.Empty);
            return strText;
        }
        public static int ParseInt(object str)
        {
            int Out = 0;
            if (str == null || string.IsNullOrEmpty(str.ToString())) return 0;
            int.TryParse(str.ToString(), out Out);
            return Out;
        }

        public static string removeWidthHeightAtt(string strHtml)
        {
            string patternWidth = "(width=\"\\d+\")+";
            string patternHeight = "(height=\"\\d+\")+";
            strHtml = Regex.Replace(strHtml, patternWidth, "", RegexOptions.IgnoreCase);
            strHtml = Regex.Replace(strHtml, patternHeight, "", RegexOptions.IgnoreCase);
            return strHtml;
        }

        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static string FormatPrice(int price)
        {

            return price.ToString("N0").Replace('.', ',') + " VNĐ";
        }

        public static string DateToString(object dateinput, string format = "dd/MM/yyyy h:mm", string sMsg = "Cách đây ")
        {
            try
            {
                DateTime date = (DateTime)dateinput;
                TimeSpan diff = DateTime.Now - date;
                if (diff.Days != 0 && diff.Days < 7)
                {
                    sMsg += diff.Days + " ngày.";
                }
                else if (diff.Days > 7)
                {
                    sMsg = date.ToString(format);
                }
                else if (diff.Hours != 0)
                {
                    sMsg += diff.Hours + " giờ.";
                }
                else if (diff.Minutes != 0)
                {
                    sMsg += diff.Minutes + " phút.";
                }
                else if (diff.Seconds != 0)
                {
                    sMsg += diff.Seconds + " giây.";
                }
                else if (diff.Seconds == 0)
                {
                    sMsg = "Mới đây.";
                }
                else
                {
                    sMsg = "";
                }
            }
            catch (Exception ex)
            {
                sMsg = "";
            }

            return sMsg;
        }

        public static string Random()
        {
            Random rnd = new Random();
            return rnd.Next(1, 999).ToString();
        }
    }
}