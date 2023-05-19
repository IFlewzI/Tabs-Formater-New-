using System;
using System.Collections.Generic;

namespace Tabs_Formater
{
    class Program
    {
        static void Main()
        {
            bool isNewTabVersion;
            string fullPath = GetPathToFile();
            string pathForNewFile = fullPath.Insert(fullPath.Length - 4, " (Formated)");

            Console.Write("\nЕсли указан путь к таблице старого формата введите '0' " +
                "\nЕсли указан путь к таблице нового формата введите '1'" +
                "\nПоле для ввода: ");
            isNewTabVersion = Convert.ToBoolean(GetBooleanNumber());
            
            Console.WriteLine("\n");
            List<ContactCell> contactCells = ArrayHandler.HandleFileToArray(fullPath, isNewTabVersion);
            ArrayHandler.HandleArrayToFile(contactCells, pathForNewFile);

            Console.WriteLine("\nФорматирование окончено. \nНажмите любую кнопку, чтобы закрыть окно.");
            Console.ReadKey();
        }

        private static string GetPathToFile()
        {
            Console.Write("Введите полный путь к файлу. \nПоле для ввода: ");
            string fileLink = Console.ReadLine();
            Console.Write("\nВведите имя файла. \nПоле для ввода: ");
            string fileName = Console.ReadLine();

            string fullFileLink = fileLink + "/" + fileName + ".txt";

            return fullFileLink;
        }

        private static int GetBooleanNumber()
        {
            bool isSuccess = false;
            string userInput = Console.ReadLine();
            isSuccess = userInput == "0" || userInput == "1";

            while (!isSuccess)
            {
                Console.Write("\nВведены неверные данные. Повторите попытку. \nПоле для ввода: ");
                userInput = Console.ReadLine();
                isSuccess = userInput == "0" || userInput == "1";
            }

            return Convert.ToInt32(userInput);
        }
    }
}
