﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using CompilerApp.PolishReverseNotation;

namespace CompilerApp
{
    public static class LaunchActions // Класс для реализации пункта меню "Пуск"
    {
        public static void Launch(MainMenuForm form) // Метод запуска компилятора для "Объявление прототипа функции на языке C/C++"
        {
            // Получаем объекты области ввода и вывода
            var inputArea = form.GetInputArea();
            var outputTable = form.GetOutputTable();
            var errorsTable = form.GetErrorsTable();

            string[] codeLines = inputArea.Lines; // Получаем текст из поля ввода

            // Создаем и запускаем лексический анализатор (сканер)
            LexicalAnalyzer scanner = new LexicalAnalyzer(codeLines);
            List<Token> tokens = scanner.Analyze();

            // Создаем и запускаем синтаксический анализатор (парсер)
            ParserFromText parser = new ParserFromText(codeLines);
            parser.Parse();

            // Очищаем таблицы перед выводом новых данных
            outputTable.Rows.Clear();
            errorsTable.Rows.Clear();

            // Заполняем таблицы результатами сканера
            int index = 1;
            foreach (var token in tokens)
            {
                outputTable.Rows.Add(index++, token.TypeCode, token.Name, token.Value, token.Position);
            }

            // Заполняем таблицу результатами парсера (синтаксические ошибки)
            index = 1;
            foreach (var error in parser.Errors)
            {
                errorsTable.Rows.Add(index++, error.Message, error.ErrorValue, error.Position);
            }

            // Подсветка синтаксических и лексических ошибок в редакторе
            form.HighlightErrors(parser.Errors);

            // Переключаемся на вкладку с ошибками, если они есть
            if (errorsTable.Rows.Count > 0)
            {
                form.SelectErrorsTab();
                form.UpdateStatus($"Обнаружено ошибок: {errorsTable.Rows.Count}"); // Обновляем статус
            }
            else
            {
                form.SelectTokensTab();
                form.UpdateStatus("Ошибок не обнаружено"); // Обновляем статус
            }
        }

        public static void LaunchPolish(MainMenuForm form) // Метод запуска компилятора для "Польская инверсная запись (ПОЛИЗ)"
        {
            // Получаем объекты области ввода и вывода
            var inputArea = form.GetInputArea();
            var outputTable = form.GetOutputTable();
            var errorsTable = form.GetErrorsTable();
            var outputArea = form.GetOutputArea();

            // Очищаем старые результаты
            outputTable.Rows.Clear();
            errorsTable.Rows.Clear();
            outputArea.Clear();

            // Проверяем, что текст для анализа есть
            if (!string.IsNullOrWhiteSpace(inputArea.Text))
            {
                // Создаем синтаксический анализатор (парсер)
                Parser parser = new Parser(inputArea.Text);

                if (parser.Tokens.Count != 0)
                {
                    parser.Parse(); // Запускаем синтаксический анализатор (парсер)

                    // Собираем все ошибки: сначала синтаксические, затем лексические
                    List<Error> Errors = parser.Errors;
                    Errors.AddRange(parser.Scanner.Errors);

                    // Сортируем ошибки по начальной позиции
                    Errors = Errors.OrderBy(e => e.Position.start).ToList();

                    // Заполняем таблицу ошибок
                    int index = 1;
                    foreach (var error in Errors)
                    {
                        errorsTable.Rows.Add(index++, error.Message, error.Fragment,
                            $"символы {error.Position.start + 1}-{error.Position.end}");
                    }

                    // Сбрасываем выделение и очищаем фон
                    inputArea.SelectAll();
                    inputArea.SelectionBackColor = inputArea.BackColor;

                    // Подсветка синтаксических и лексических ошибок в редакторе
                    foreach (var error in Errors)
                    {
                        inputArea.Select(error.Position.start, error.Position.end - error.Position.start);
                        inputArea.SelectionBackColor = Color.Pink;
                    }

                    // Снимаем выделение
                    inputArea.Select(0, 0);
                    inputArea.SelectionBackColor = inputArea.BackColor;

                    if (Errors.Count == 0)
                    {
                        // Если ошибок нет — считаем ПОЛИЗ
                        PolishConverter polishConverter = new PolishConverter(parser.Tokens);
                        polishConverter.ConvertToPolishReverseNotation();
                        double result = polishConverter.CalculatePolishReverseNotation();

                        // Выводим результат в outputArea
                        outputArea.AppendText($"Исходное арифметическое выражение:\n{inputArea.Text}\n");

                        outputArea.AppendText($"\nАрифметическое выражение в ПОЛИЗ:\n");
                        foreach (var token in polishConverter.outToken)
                        {
                            outputArea.AppendText(token.Value);
                        }

                        outputArea.AppendText($"\n\nРезультат вычисления:\n{result}");

                        // Переключаемся на вкладку с результатом ПОЛИЗ
                        form.SelectResultsTab();
                        form.UpdateStatus("Ошибок не обнаружено"); // Обновляем статус
                    }
                    else
                    {
                        // Переключаемся на вкладку с ошибками
                        form.SelectErrorsTab();
                        form.UpdateStatus($"Обнаружено ошибок: {errorsTable.Rows.Count}"); // Обновляем статус
                    }
                }
            }
        }

    }
}
