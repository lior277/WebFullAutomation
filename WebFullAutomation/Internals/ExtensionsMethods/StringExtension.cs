// Ignore Spelling: rgb Json sha Pdf

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AirSoftAutomationFramework.Internals.ExtensionsMethods
{
    public static class StringExtension
    {
        public static bool StringContains(this string actualText, string expectedText)
        {
            var compare = actualText
                .TrimEnd()
                .TrimStart()
                .Contains(expectedText.TrimEnd()
                .TrimStart(), StringComparison.OrdinalIgnoreCase);

            if (!compare)
            {
                var exceMessage = $"actualText: [{actualText}], not contains: [{expectedText}]";

                throw new Exception(exceMessage);
            }

            return true;
        }
        
        public static string ToBold(this string boldText)
        {
            if (boldText != null)
            {
                boldText = $"<b> {boldText} </b>";
            }

            return boldText;
        }

        public static JObject ConvertToJObject(this string actualText)
        {
            return JObject.Parse(actualText);
        }

        public static void CopyStream(Stream input, Stream output)
        {
            var buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        public static string GetPdfFileText(this Stream streamPdfContent)
        {
            var document = UglyToad.PdfPig.PdfDocument.Open(streamPdfContent);
            string text = null;

            for (var i = 1; i == document.NumberOfPages; i++)
            {
                var page = document.GetPage(i);
                text = string.Join(" ", page.GetWords());
            }

            return text;
        }

        public static string RemoveMultipleSpaces(this string text)
        {

            var temp = Regex.Replace(text, @"\s{2,}", " ");

            return temp;
        }

        public static bool IsValidJson(this string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue))
            {
                return false;
            }

            var value = stringValue.Trim();

            if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                (value.StartsWith("[") && value.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = JToken.Parse(value);
                    return true;
                }
                catch (JsonReaderException)
                {
                    return false;
                }
            }

            return false;
        }

        public static string EncodeBase64(this string text)
        {
            var textAsBytes = Encoding.UTF8.GetBytes(text);

            return Convert.ToBase64String(textAsBytes);
        }

        public static string DecodeBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);

            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string ConvertToSha256(this string sha256Value)
        {
            var Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var enc = Encoding.UTF8;
                var result = hash.ComputeHash(enc.GetBytes(sha256Value));

                foreach (var b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string RemoveNewLine(this string text)
        {
            return text.Replace(Environment.NewLine, " ");
        }

        public static Color ConvertRgbToColor(this string rgb)
        {
            var rgbValue = rgb.Split("(")[1].Split(')').First();

            return ColorTranslator.FromHtml(rgbValue);
        }

        public static string RemoveWhiteSpace(this string text)
        {
            if (text != null)
            {
                return string.Concat(text.Where(c => !char.IsWhiteSpace(c)));
            }

            return string.Empty;
        }

        public static string StringAddCharBetweenWords(this string inputString, char charToAdd)
        {
            var tempString = "";

            foreach (var letter in inputString)
            {
                if (char.IsUpper(letter))
                    tempString += $"{charToAdd}" + letter;
                else
                    tempString += letter;
            }

            return tempString.TrimStart(charToAdd).ToLower();
        }

        public static string AllWordsToTitleCase(this string text)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
        }

        public static string UpperCaseFirstLetter(this string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }

        public static string LowerCaseFirstLetter(this string text)
        {
            return char.ToLower(text[0]) + text.Substring(1);
        }

        public static int StringToInt(this string text)
        {
            var dec = decimal.Parse(text,
                NumberStyles.AllowDecimalPoint |
                NumberStyles.Number |
                NumberStyles.AllowThousands);

            return (int)dec.MathRoundFromGeneric(0);
        }

        public static string PrintJson(this string json)
        {
            using var stringReader = new StringReader(json);
            using var stringWriter = new StringWriter();
            var jsonReader = new JsonTextReader(stringReader);
            var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
            jsonWriter.WriteToken(jsonReader);

            return stringWriter.ToString();
        }
    }
}
