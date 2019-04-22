using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anonymiser
{
    class Program
    {
        static void Main(string[] args)
        {
            var texts = new List<string>();
            texts.Add("This is a simple ");
            texts.Add(" test");
            texts.Add("ing m");
            texts.Add("y a");
            texts.Add("n");
            texts.Add("onym");
            texts.Add("i");
            texts.Add("sation");
            texts.Add(" verificatio");
            texts.Add("n algorithm");

            var anonymised = Anonymise(texts);
        }

        private static List<string> Anonymise(List<string> texts)
        {
            var endsWithSpace = false;
            var charToUse = "";
            var result = new List<string>();

            for(int i = 0; i < texts.Count; i++)
            {
                var text = texts[i];
                var matches = System.Text.RegularExpressions.Regex.Matches(text, @"\w+");
                var anonym = new System.Text.StringBuilder();
                var lastMatchEnd = -1;

                if(matches.Count > 0 && matches[0].Index > 0)
                {
                    // append preceding spaces
                    anonym.Append(text.Substring(0, matches[0].Index));

                    // and clear any remainder from previous block
                    endsWithSpace = true;
                    charToUse = "";
                }
                for(int m = 0; m < matches.Count; m++)
                {
                    System.Text.RegularExpressions.Match match = matches[m];
                    if(lastMatchEnd >= 0)
                    {
                        anonym.Append(text.Substring(lastMatchEnd, match.Index - lastMatchEnd));
                    }
                    if (m == 0 && !endsWithSpace && !String.IsNullOrEmpty(charToUse))
                    {
                        anonym.Append(new string(charToUse[0], match.Length));
                    }
                    else
                    {
                        anonym.Append(new string(match.Value[0], match.Length));
                    }
                    lastMatchEnd = match.Index + match.Length;
                }
                if(lastMatchEnd < text.Length)
                {
                    endsWithSpace = true;
                    charToUse = "";
                    anonym.Append(text.Substring(lastMatchEnd));
                }
                else
                {
                    endsWithSpace = false;

                    // don't update character if we were within a word (i.e. no new word started)
                    if (matches.Count > 1)
                    {
                        charToUse = matches[matches.Count - 1].Value[0].ToString();
                    } else if(matches[0].Length != text.Length)
                    {
                        // match is not entire text, and no space at end -> update character
                        charToUse = matches[matches.Count - 1].Value[0].ToString();
                    }
                }

                result.Add(anonym.ToString());
            }

            return result;
        }
    }
}
