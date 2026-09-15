using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace XmlSerializerApp
{
    // 1. Создаем класс пользователя (модель данных)
    [Serializable]
    public class User
    {
        public string Username { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
    }

    // 2. Создаем класс-обертку, чтобы в XML был красивый корневой тег <Users>
    [Serializable]
    [XmlRoot("Users")]
    public class UserDatabase
    {
        [XmlElement("User")]
        public List<User> UsersList { get; set; } = new List<User>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Программа генерации XML (Сериализация) ===");

            UserDatabase database = new UserDatabase();

            // 3. Цикл для интерактивного ввода данных с консоли
            while (true)
            {
                Console.WriteLine("\n--- Добавление нового пользователя ---");
                Console.Write("Введите имя (или нажмите Enter для завершения и создания XML): ");
                string name = Console.ReadLine();

                // Если ввели пустую строку - выходим из цикла ввода
                if (string.IsNullOrWhiteSpace(name))
                {
                    break;
                }

                Console.Write("Введите пол (Male/Female): ");
                string sex = Console.ReadLine();

                Console.Write("Введите возраст: ");
                int age;
                while (!int.TryParse(Console.ReadLine(), out age))
                {
                    Console.Write("Ошибка! Введите возраст цифрами: ");
                }

                // Создаем объект пользователя и добавляем в список
                User newUser = new User
                {
                    Username = name,
                    Sex = sex,
                    Age = age
                };

                database.UsersList.Add(newUser);
                Console.WriteLine($"[+] Пользователь {name} добавлен в очередь.");
            }

            // 4. Проверяем, есть ли кого сохранять
            if (database.UsersList.Count > 0)
            {
                string filePath = "UsersData.xml";

                // 5. Та самая сериализация (как у вас на скриншоте)
                XmlSerializer serializer = new XmlSerializer(typeof(UserDatabase));

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    serializer.Serialize(fs, database);
                }

                Console.WriteLine("\n=========================================");
                Console.WriteLine($"Успех! Данные сериализованы и сохранены в файл: {filePath}");

                // Для наглядности выводим получившийся XML прямо в консоль
                Console.WriteLine("\n--- Содержимое сгенерированного XML ---\n");
                string xmlContent = File.ReadAllText(filePath);
                Console.WriteLine(xmlContent);
            }
            else
            {
                Console.WriteLine("\nНет данных для сохранения.");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}