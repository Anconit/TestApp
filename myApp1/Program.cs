using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using static myApp1.ClassForRandom;

namespace myApp1
{
    internal class Program
    {
        private static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=DBforMyApp1;Integrated Security=True;";

        private static void Main(string[] args)
        {
            Console.WriteLine("Что выполнить?");
            string answer = Console.ReadLine();
            string[] forSpecialCmd = answer.Split(' ');
            if (forSpecialCmd[0] == "myApp")
            {
                switch (forSpecialCmd[1])
                {
                    case "1":
                        CreateTab();
                        Console.WriteLine("Готово!");
                        Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
                        Console.ReadKey();
                        break;
                    case "2":
                        if (forSpecialCmd.Length == 7 && Regex.IsMatch(answer, @"^myApp\s{1}2\s{1}\D+\s{1}\D+\s{1}\D+\s{1}[0-9]{2}-[0-9]{2}-[0-9]{4}\s{1}\D{1}$", RegexOptions.IgnoreCase))
                        {
                            try
                            {
                                DateTime dateForVoid = DateTime.Parse(forSpecialCmd[5], CultureInfo.InvariantCulture);
                                //Console.WriteLine(dateForVoid);
                                CreateOneRow(forSpecialCmd[2], forSpecialCmd[3], forSpecialCmd[4], dateForVoid, forSpecialCmd[6]);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Console.ReadKey();
                            }
                            Console.WriteLine("Готово! \n Нажмиие любую клавишу, чтобы завершить работу.");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Введенные данные неверны");
                            Console.ReadKey();
                        }
                        break;
                    case "3":
                        Console.WriteLine("Какую функцию выполнить? \n 1 - введите для вывода строк с уникальм значением \n 2 - введите для вывода строк с кол-вом полных лет");
                        string answer2 = Console.ReadLine();
                        if (answer2 == "1") PrintDistinctRows();
                        if (answer2 == "2") PrintAgeRows();
                        Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
                        Console.ReadKey();
                        break;
                    case "4":
                        Console.WriteLine("Какую функцию выполнить? \n 1 - введите для создания 1м строк \n 2 - введите для создания 100 строк, в которых пол мужской и ФИО начинается с 'F'");
                        string answer3 = Console.ReadLine();
                        if (answer3 == "1") CreateMillionRows();
                        if (answer3 == "2") CreateHundredRows();
                        break;
                    case "5":
                        SearchSpecialRows();
                        Console.WriteLine("Готово!");
                        Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
                        Console.ReadKey();
                        break;
                    default:
                        Console.WriteLine("Неверная команда");
                        Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private static void CreateTab()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("IF OBJECT_ID(N'DBforMyApp1.TableForApp', N'U') IS NULL BEGIN CREATE TABLE TableForApp(id int identity(1, 1) NOT NULL," +
                        " LastName nvarchar(50),FirstName nvarchar(50), Patronymic nvarchar(50), BirthDate date, Sex nvarchar(1), PRIMARY KEY(ID ASC)); END;");
                    command.Connection = con;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static void CreateOneRow(string lastName, string firstName, string patronymic, DateTime birthDate, string sexType)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();
                SqlCommand command = new SqlCommand("INSERT INTO TableForApp (LastName, FirstName, Patronymic, BirthDate, Sex) " +
                    "VALUES (@LastName, @FirstName, @Patronymic, @BirthDate, @Sex);");
                command.Parameters.AddWithValue("LastName", lastName);
                command.Parameters.AddWithValue("FirstName", firstName);
                command.Parameters.AddWithValue("Patronymic", patronymic);
                command.Parameters.AddWithValue("BirthDate", $"{birthDate.Month}/{birthDate.Day}/{birthDate.Year}");
                command.Parameters.AddWithValue("Sex", sexType);

                command.Connection = con;
                command.ExecuteNonQuery();
            }
        }

        private static void CreateMillionRows()
        {
            Random random = new Random();
            Console.WriteLine("Подождите, пока не появится уведомление о добавлении.\n Запрос обрабатывается...");
            for (int q = 0; q < 1000000; q++)
            {
                int randomSex = random.Next(2);
                RandomPeople(randomSex);
            }
            Console.WriteLine("Готово!");
            Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
            Console.ReadKey();
        }

        private static void CreateHundredRows()
        {
            Random random = new Random();
            Console.WriteLine("Подождите, пока не появится уведомление о добавлении.\n Запрос обрабатывается...");
            for (int q = 0; q < 100; q++)
            {
                int randomSex = random.Next(2);
                RandomPeople(1);
            }
            Console.WriteLine("Готово!");
            Console.WriteLine("Нажмиие любую клавишу, чтобы завершить работу.");
            Console.ReadKey();
        }

        private static void PrintDistinctRows()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("Select DISTINCT LastName, FirstName, Patronymic, BirthDate from TableForApp Order By LastName, FirstName, Patronymic;");
                    command.Connection = con;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string columnName1 = reader.GetName(0);
                        string columnName2 = reader.GetName(1);
                        string columnName3 = reader.GetName(2);
                        string columnName4 = reader.GetName(3);

                        Console.WriteLine($"{columnName1}\t{columnName2}\t{columnName3}\t{columnName4}");

                        while (reader.Read())
                        {
                            object LastName = reader.GetValue(0);
                            object FirstName = reader.GetValue(1);
                            object Patronymic = reader.GetValue(2);
                            object BirthDate = reader.GetValue(3);
                            string[] birthOnlyDate = BirthDate.ToString().Split(' ');

                            Console.WriteLine($"{LastName} \t{FirstName} \t{Patronymic} \t{birthOnlyDate[0]}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        private static void PrintAgeRows()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("SELECT *, CASE WHEN MONTH(GETDATE()) >= MONTH(BirthDate) AND DAY(GETDATE()) >= DAY(BirthDate) " +
                        "THEN YEAR(GETDATE()) - YEAR(BirthDate) ELSE(YEAR(GETDATE()) - YEAR(BirthDate) - 1) END  AS[Количество полных лет] FROM TableForApp");
                    command.Connection = con;
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        string columnName1 = reader.GetName(0);
                        string columnName2 = reader.GetName(1);
                        string columnName3 = reader.GetName(2);
                        string columnName4 = reader.GetName(3);
                        string columnName5 = reader.GetName(4);
                        string columnName6 = reader.GetName(5);
                        string columnName7 = reader.GetName(6);

                        Console.WriteLine($"{columnName2}\t{columnName3}\t{columnName4}\t{columnName5}\t{columnName6}\t{columnName7}");

                        while (reader.Read())
                        {
                            object id = reader.GetValue(0);
                            object LastName = reader.GetValue(1);
                            object FirstName = reader.GetValue(2);
                            object Patronymic = reader.GetValue(3);
                            object BirthDate = reader.GetValue(4);
                            string[] birthOnlyDate = BirthDate.ToString().Split(' ');
                            object Sex = reader.GetValue(5);
                            object Age = reader.GetValue(6);


                            Console.WriteLine($"{LastName} \t{FirstName} \t{Patronymic} \t{birthOnlyDate[0]} \t{Sex} \t{Age}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }
    
        private static void SearchSpecialRows()
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("SELECT count(id) FROM TableForApp WHERE LastName LIKE 'F%' and Sex = 'M'");
                    command.Connection = con;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            stopwatch.Stop();
            Console.WriteLine("Elapsed Time is {0} ms", stopwatch.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}
