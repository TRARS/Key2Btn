using Key2Btn.MainView.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Key2Btn.MainView.Services
{
    internal class StringFactoryService : IStringFactoryService
    {
        private Random random = new Random();
        private HashSet<string> generatedStrings = new HashSet<string>();

        public string GenerateRandomString(int length)
        {
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder stringBuilder = new StringBuilder();
            string newString;

            do
            {
                stringBuilder.Clear();

                for (int i = 0; i < length; i++)
                {
                    int index = random.Next(chars.Length);
                    stringBuilder.Append(chars[index]);
                }

                newString = stringBuilder.ToString();
            }
            while (!generatedStrings.Add(newString)); // 只要添加失败，说明有重复，则继续生成

            return newString;
        }
    }
}
