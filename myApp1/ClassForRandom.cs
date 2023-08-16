using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myApp1
{
    internal class ClassForRandom
    {
        //50
        private static string[] FirstFemaleNames = { "Mary", "Elizabeth", "Ann", "Sarah", "Jane", "Margaret", "Susan",
            "Martha", "Hannah", "Catherine", "Alice", "Frances", "Eleanor", "Dorothy", "Rebecca", "Isabel", "Grace",
            "Joan", "Rachel", "Agnes", "Ellen", "Maria", "Lydia", "Ruth", "Deborah", "Judith", "Esther", "Joanna", "Amy",
            "Marjorie", "Phoebe", "Jenny", "Barbara", "Bridget", "Fanny", "Lucy", "Betty", "Eliza", "Nancy",
            "Emma", "Charlotte", "Dinah", "Sally", "Harriet", "Jemima", "Kitty", "MaryAnn", "Caroline", "Peggy", "Sophia"};

        //50
        private static string[] FirstMaleNames = {"Cecil", "Ricky", "Jerome", "Darrell", "Marco", "Joseph", "Garry",
            "Liam", "Benjamin", "Wilson", "Curtis", "Kristopher", "Isaac", "George", "Leroy", "Larry", "Franklin",
            "Vincent", "Benjamin", "Rudolph", "Sidney", "Robert", "Wayne", "Maurice", "Jack", "Mitchell", "Leonard",
            "Gerald", "Jesus", "Spencer", "David", "Tony", "Frederick", "Edwin", "Kelly", "Marvin", "Cristian", "Archie",
            "Nathaniel", "Jimmie", "Rex", "Micah", "Billy", "Hector", "Adolph", "Duane", "Terry", "Brett", "Donald", "Kaleb" };

        //254
        private static string[] LastNames = { "Abramson","Adamson","Adderiy","Addington","Adrian","Albertson","Aldridge","Allford","Alsopp","Anderson",
            "Andrews","Archibald","Arnold","Arthurs","Atcheson","Attwood","Audley","Austin","Ayrton","Babcock","Backer","Baldwin","Bargeman","Barnes",
            "Barrington","Bawerman","Becker","Benson","Berrington","Birch","Bishop","Black","Blare","Blomfield","Boolman","Bootman","Bosworth",
            "Bradberry","Bradshaw","Brickman","Brooks","Brown","Bush","Calhoun","Campbell","Carey","Carrington","Carroll","Carter","Chandter","Chapman",
            "Charlson","Chesterton","Clapton","Clifford","Coleman","Conors","Cook","Cramer","Creighton","Croftoon","Crossman","Daniels","Davidson",
            "Day","Dean","Derrick","Dickinson","Dodson","Donaldson","Donovan","Douglas","Dowman","Dutton","Duncan","Dunce","Durham","Dyson","Eddington",
            "Edwards","Ellington","Elmers","Enderson","Erickson","Evans","Faber","Fane","Farmer","Farrell","Ferguson","Finch","Fisher","Fitzgerald","Flannagan",
            "Flatcher","Fleming","Ford","Forman","Forster","Foster","Francis","Fraser","Freeman","Fulton","Galbraith","Gardner","Garrison","Gate","Gerald","Gibbs","Gilbert",
            "Gill","Gilmore","Gilson","Gimson","Goldman","Goodman","Gustman","Haig","Hailey","Hamphrey","Hancock","Hardman","Harrison","Hawkins","Higgins","Hodges",
            "Hoggarth","Holiday","Holmes","Howard","Jacobson","James","Jeff","Jenkin","Jerome","Johnson","Jones","Keat","Kelly","Kendal","Kennedy","Kennett","Kingsman",
            "Kirk","Laird","Lamberts","Larkins","Lawman","Leapman","Leman","Lewin","Little","Livingston","Longman","MacAdam","MacAlister","MacDonald","Macduff",
            "Macey","Mackenzie","Mansfield","Marlow","Marshman","Mason","Mathews","Mercer","Michaelson","Miers","Miller","Miln","Milton","Molligan","Morrison",
            "Murphy","Nash","Nathan","Neal","Nelson","Nevill","Nicholson","Nyman","Oakman","Ogden","Oldman","Oldridge","Oliver","Osborne","Oswald","Otis","Owen","Page",
            "Palmer","Parkinson","Parson","Pass","Paterson","Peacock","Pearcy","Peterson","Philips","Porter","Quincy","Raleigh","Ralphs","Ramacey","Reynolds","Richards",
            "Roberts","Roger","Russel","Ryder","Salisburry","Salomon","Samuels","Saunder","Shackley","Sheldon","Sherlock","Shorter","Simon","Simpson","Smith",
            "Stanley","Stephen","Stevenson","Sykes","Taft","Taylor","Thomson","Thorndike","Thornton","Timmons","Tracey","Turner","Vance","Vaughan",
            "Wainwright","Walkman","Wallace","Waller","Walter","Ward","Warren","Watson","Wayne","Webster","Wesley","White","WifKinson","Winter","Wood","Youmans","Young"};

        //19
        private static string[] LastNameOnlyF = {"Faber","Fane","Farmer","Farrell","Ferguson","Finch","Fisher","Fitzgerald","Flannagan",
            "Flatcher","Fleming","Ford","Forman","Forster","Foster","Francis","Fraser","Freeman","Fulton"};

        private static string ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=DBforMyApp1;Integrated Security=True;";

        public static DateTime RandomDate()
        {
            Random random = new Random();
            var startDate = new DateTime(1980, 1, 1);
            var newDate = startDate.AddDays(random.Next(12053));
            return newDate;
        }

        public static void RandomPeople(int randomSex)
        {
            string firstName, lastName, patronymic, sexType;
            DateTime dateBirth;
            Random random = new Random();
            if (randomSex == 1)
            {
                firstName = FirstMaleNames[random.Next(50)];
                lastName = LastNames[random.Next(254)];
                patronymic = FirstMaleNames[random.Next(50)];
                sexType = "M";
                dateBirth = RandomDate();

                AddGuy(firstName, lastName, patronymic, sexType, dateBirth);
            }
            if (randomSex == 0)
            {

                firstName = FirstFemaleNames[random.Next(50)];
                lastName = LastNames[random.Next(254)];
                patronymic = FirstMaleNames[random.Next(50)];
                sexType = "F";
                dateBirth = RandomDate();

                AddGuy(firstName, lastName, patronymic, sexType, dateBirth);
            }
        }

        private static void AddGuy(string firstName, string lastName, string patronymic, string sexType, DateTime dateBirth)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO TableForApp (LastName, FirstName, Patronymic, BirthDate, Sex) " +
                        "VALUES (@LastName, @FirstName, @Patronymic, @BirthDate, @Sex);");
                    command.Parameters.AddWithValue("LastName", lastName);
                    command.Parameters.AddWithValue("FirstName", firstName);
                    command.Parameters.AddWithValue("Patronymic", patronymic);
                    command.Parameters.AddWithValue("BirthDate", $"{dateBirth.Month}/{dateBirth.Day}/{dateBirth.Year}");
                    command.Parameters.AddWithValue("Sex", sexType);

                    command.Connection = con;
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
