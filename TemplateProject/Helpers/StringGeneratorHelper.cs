using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TemplateProject.Helpers
{
    public class StringGeneratorHelper
    {
        public string LicenseGenerator()
        {
            List<string> OldItemList = new List<string>();
            Random random = new Random();
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string numbers = "1234567890";
            string characters = alphabets + numbers;
            int length = 4;
            int no = 5;
            string Key = "";

            for (int j = 0; j < no; j++)
            {
                string item = string.Empty;
                for (int i = 0; i < length; i++)
                {

                    string character = string.Empty;

                    item = new string(Enumerable.Repeat(characters, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                }

                Key += "-" + item;

            }
            Key = Key.Substring(1);

            return Key;
        }



        public string IdGenerator()
        {
            List<string> OldItemList = new List<string>();
            Random random = new Random();
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string smallalphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string characters = alphabets + numbers + smallalphabets;
            int length = 4;
            int no = 5;
            string Key = "";

            for (int j = 0; j < no; j++)
            {
                string item = string.Empty;
                for (int i = 0; i < length; i++)
                {

                    string character = string.Empty;

                    item = new string(Enumerable.Repeat(characters, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                }

                Key += item;

            }

            return Key;
        }

        public string GenerateId(int rowCount, string prefix, int stringLength)
        {
            Random random = new Random();
            string alphabets = "ABCDEFGHIJKLMNPQRSTUVWXYZ";
            string numbers = "123456789";
            string characters = alphabets + numbers;
            int length = 2;
            int no = 2;
            string Key = "";

            for (int j = 0; j < no; j++)
            {
                string item = string.Empty;
                for (int i = 0; i < length; i++)
                {

                    string character = string.Empty;

                    item = new string(Enumerable.Repeat(characters, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());

                }

                Key += item;
            }

            rowCount = rowCount + 1;

            string recordId = prefix + Key + rowCount.ToString("D" + stringLength);

            return recordId;
        }
    }
}
