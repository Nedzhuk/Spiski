using System.Text.RegularExpressions;
namespace Spiski
{
    internal class Program
    {
        public struct Listik
        {
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public override string ToString()
            {
                return $"{Surname} {Name} {Phone} {Age} {Gender}";
            }
            public string ToStringAdd()
            {
                return $"{Surname};{Name};{Phone};{Age};{Gender}\n";
            }
        }

        public class Listiki : List<Listik>
        {
            public override string ToString()
            {
                string s = string.Empty;
                foreach (Listik item in this)
                    s += item.ToStringAdd();
                return s;
            }
        }

        static void Main(string[] args)
        {
            string _path = "Данные.csv";
            List<string> line = File.ReadAllLines(_path).ToList();
            Listiki User = new();
            for (int i = 0; i < line.Count; i++)
            {
                string[] strings = line[i].Split(';');
                User.Add(new()
                {
                    Surname = strings[0],
                    Name = strings[1],
                    Phone = strings[2],
                    Age = Convert.ToInt32(strings[3]),
                    Gender = strings[4],
                });
                Console.WriteLine(User[i]);
            }
            Console.Write("Сделать новую запись? (1-да / 2-нет):  ");
            int num = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine();
            if (num == 1)
            {
                Console.Write("Введите фамилию: ");
                string surnameNew = Console.ReadLine();
                Regex surN = new Regex("^[А-Я][а-я]+");

                Console.Write("Введите имя: ");
                string nameNew = Console.ReadLine();

                Console.Write("Введите номер телфона: ");
                string phoneNew = Console.ReadLine();
                Regex phN = new Regex(@"^(8)\([0-9]{3}\)[0-9]{3}-[0-9]{2}-[0-9]{2}$");

                Console.Write("Введите возраст: ");
                string ageNew = Console.ReadLine();

                Console.Write("Введите гендер: ");
                string genderNew = Console.ReadLine();
                Regex genN = new Regex("^(мужской|женский)$");

                int yeee = 0;
                Console.WriteLine();
                if (surN.IsMatch(surnameNew)) yeee++;
                else Console.WriteLine("Фамилия введена неверно");
                if (surN.IsMatch(nameNew)) yeee++;
                else Console.WriteLine("Имя введено неверно");
                if (phN.IsMatch(phoneNew)) yeee++;
                else Console.WriteLine("Номер введен неверно");
                if (Convert.ToInt32(ageNew) >= 18) yeee++;
                else Console.WriteLine("Увы, несовершеннолетние нам не нужны...");
                if (genN.IsMatch(genderNew)) yeee++;
                else Console.WriteLine("Гендер введен неверноневерный");

                if (yeee == 5)
                {
                    Listik newUser = (new()
                    {
                        Surname = surnameNew,
                        Name = nameNew,
                        Phone = phoneNew,
                        Age = Convert.ToInt32(ageNew),
                        Gender = genderNew
                    });
                    User.Add(newUser);
                    File.WriteAllText(_path, newUser.ToStringAdd());
                    Console.WriteLine("Данные добавлены");
                }
            }

            Console.Write("Введите имя, которое вы хотите найти: ");
            string NameSeach = Console.ReadLine();
            Listiki UserLinq = User;
            Listiki SortSurname = new();
            foreach (Listik l in UserLinq.OrderBy(x => x.Surname).ToList())
                SortSurname.Add(l);//Сортировка по алфавиту
            Listiki UserAge = new();
            foreach (Listik l in UserLinq.OrderBy(x => x.Age >= 40).ToList())
                UserAge.Add(l);//40+ люди
            Listiki SearchName = new();
            foreach (Listik l in UserLinq.OrderBy(x => x.Name == NameSeach).ToList())
                SearchName.Add(l);//Поиск по имени
            Listiki SortGirl = new();
            foreach (Listik l in UserLinq.OrderBy(x => x.Gender == "женский").ToList())
                SortGirl.Add(l);//женщины
            Listiki DeletePhone = new();
            foreach (Listik l in UserLinq.Distinct().ToList())
                DeletePhone.Add(l);//Удаляет дубликации, ну и ладно, что их нет...

            File.WriteAllText("SortSurname.csv", SortSurname.ToString());
            File.WriteAllText("UserAge.csv", UserAge.ToString());
            File.WriteAllText("SortSurname.csv", SortSurname.ToString());
            File.WriteAllText("SearchName.csv", SearchName.ToString());
            File.WriteAllText("DeletePhone.csv", DeletePhone.ToString());
            Console.WriteLine("Линк запросы выполнены и записаны в файлы.");
        }
    }
}