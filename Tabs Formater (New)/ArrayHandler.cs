using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;

namespace Tabs_Formater
{
    abstract class ArrayHandler
    {
        private static char _tabulationSymbol = '\t';
        private static char[] _unnecessaryChars =
        {
            ' ',
            '+',
            '-',
            '(',
            ')',
        };
        private static char[] _numbers =
        {
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
        };

        public static List<ContactCell> HandleFileToArray(string fullPath, bool isNewTabVersion)
        {
            Random random = new Random();
            List<ContactCell> contactCells = new List<ContactCell>();
            ContactCell newContactCell;
            string[] lineCells;
            int numberOfLineCell;

            var fileLines = File.ReadAllLines(fullPath);

            foreach (var line in fileLines)
            {
                newContactCell = null;

                if (isNewTabVersion)
                    lineCells = new string[2];
                else
                    lineCells = new string[4];

                numberOfLineCell = 0;

                foreach (var symbol in line)
                {
                    if (symbol != _tabulationSymbol)
                        lineCells[numberOfLineCell] += symbol;
                    else
                        numberOfLineCell++;

                    if (numberOfLineCell > lineCells.Length)
                        break;
                }

                for (int i = 0; i < lineCells.Length; i++)
                {
                    if (lineCells[i] == null)
                        lineCells[i] = "Default";
                }

                if (TryCreateContactCell(random, lineCells, out newContactCell))
                    contactCells.Add(newContactCell);
            }

            return contactCells;
        }

        public static void HandleArrayToFile(List<ContactCell> contactCells, string fullPath)
        {
            string lineForAppending;

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            List<string> linesForAppending = new List<string>()
            {
                "First Name" + _tabulationSymbol + "Mobile Phone",
            };

            foreach (var contactCell in contactCells)
            {
                lineForAppending = contactCell.FullName + _tabulationSymbol + contactCell.PhoneNumber;
                linesForAppending.Add(lineForAppending);
                Console.WriteLine(lineForAppending);
                Thread.Sleep(20);
            }

            File.AppendAllLines(fullPath, linesForAppending);
        }

        private static bool TryCreateContactCell(Random random, string[] lineCells, out ContactCell newContactCell)
        {
            List<string> correctPhoneNumbersTrim = new List<string>();
            bool isPhoneNumberCorrect;
            bool isPhoneNumberTrimRepeating;
            string clearedPhoneNumber;
            string clearedPhoneNumberTrim;
            char clearedPhoneNumberTrimFirstSymbol;

            newContactCell = new ContactCell();
            newContactCell.SetFullName(lineCells[0]);

            for (int i = 1; i < lineCells.Length; i++)
            {
                isPhoneNumberCorrect = true;
                isPhoneNumberTrimRepeating = true;
                clearedPhoneNumber = ClearPhoneNumber(lineCells[i]);

                if (TryTrimClearedPhoneNumber(clearedPhoneNumber, out clearedPhoneNumberTrim))
                {
                    clearedPhoneNumberTrimFirstSymbol = clearedPhoneNumberTrim[0];

                    foreach (var symbol in clearedPhoneNumberTrim)
                    {
                        if (symbol != clearedPhoneNumberTrimFirstSymbol)
                        {
                            isPhoneNumberTrimRepeating = false;
                            break;
                        }
                    }

                    if (clearedPhoneNumberTrim == "12345678" || clearedPhoneNumberTrim == "01234567" || isPhoneNumberTrimRepeating)
                        isPhoneNumberCorrect = false;
                    else
                        correctPhoneNumbersTrim.Add(clearedPhoneNumberTrim);
                }
            }

            if (correctPhoneNumbersTrim.Count > 0)
                newContactCell.SetPhoneNumber(FormatClearedPhoneNumberTrim(correctPhoneNumbersTrim[random.Next(0, correctPhoneNumbersTrim.Count)]));

            return (correctPhoneNumbersTrim.Count > 0);
        }

        private static string ClearPhoneNumber(string phoneNumber)
        {
            string clearedPhoneNumber = "";

            foreach (var symbol in phoneNumber)
            {
                if (!_unnecessaryChars.Contains(symbol) && _numbers.Contains(symbol))
                    clearedPhoneNumber += symbol;
            }

            return clearedPhoneNumber;
        }

        private static bool TryTrimClearedPhoneNumber(string clearedPhoneNumber, out string clearedPhoneNumberTrim)
        {
            bool isSuccess = true;
            clearedPhoneNumberTrim = "";

            for (int i = 0; i < clearedPhoneNumber.Length; i++)
            {
                if (clearedPhoneNumber.Length == 11)
                {
                    if (i > 1)
                        clearedPhoneNumberTrim += clearedPhoneNumber[i];
                }
                else if (clearedPhoneNumber.Length == 10)
                {
                    if (i > 0)
                        clearedPhoneNumberTrim += clearedPhoneNumber[i];
                }
                else if (clearedPhoneNumber.Length == 9)
                {
                    clearedPhoneNumberTrim = clearedPhoneNumber;
                }
                else
                {
                    isSuccess = false;
                    break;
                }
            }
            Console.WriteLine("Обрезан номер: " + clearedPhoneNumberTrim);

            return isSuccess;
        }

        private static string FormatClearedPhoneNumberTrim(string clearedPhoneNumber)
        {
            bool isSymbolANumber;
            int symbolNumber;
            int numbersGone;
            Dictionary<int, char> charsAndIndexesByNumberQuanity = new Dictionary<int, char>();
            charsAndIndexesByNumberQuanity.Add(0, '+');
            charsAndIndexesByNumberQuanity.Add(1, '(');
            charsAndIndexesByNumberQuanity.Add(4, ')');
            charsAndIndexesByNumberQuanity.Add(7, '-');
            charsAndIndexesByNumberQuanity.Add(9, '-');

            clearedPhoneNumber = clearedPhoneNumber.Insert(0, "79");

            foreach (var charAndIndex in charsAndIndexesByNumberQuanity)
            {
                symbolNumber = 0;
                numbersGone = 0;

                while (numbersGone <= charAndIndex.Key)
                {
                    isSymbolANumber = _numbers.Contains(clearedPhoneNumber[symbolNumber]);

                    if (numbersGone == charAndIndex.Key)
                        clearedPhoneNumber = clearedPhoneNumber.Insert(symbolNumber, charAndIndex.Value.ToString());

                    if (isSymbolANumber)
                        numbersGone++;

                    symbolNumber++;
                }
            }

            clearedPhoneNumber = clearedPhoneNumber.Insert(0, "'");

            return clearedPhoneNumber;
        }
    }
}

