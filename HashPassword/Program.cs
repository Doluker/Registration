using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HashPassword.Models;
using HashPasswords;

namespace HashPassword
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Создание новой учетной записи для пользователя \n");
            using (LeasingCompanyEntities db = new LeasingCompanyEntities().GetContext())
            {
                Console.Write("Введите фамилию пользователя:");
                string surname = Console.ReadLine();
                Console.Write("Введите имя пользователя:");
                string name = Console.ReadLine();
                User user = new User();
                Employees employee = new Employees();
                if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(surname))
                {
                    employee = db.Employees.FirstOrDefault(e =>
                    e.Name == name &&
                    e.Surname == surname
                    );
                }
                if (employee == null)
                {
                    Console.WriteLine("Сотрудник с указанными ФИО не найден.");
                    return;
                }
                if (db.User.FirstOrDefault(u => u.EmployeeId == employee.id) == null)
                {
                    while (true)
                    {
                        user.EmployeeId = employee.id;
                        Console.Write("Введите логин пользователя:");
                        user.Login = Console.ReadLine();
                        if (db.User.FirstOrDefault(u => u.Login.Equals(user.Login)) == null)
                        {
                            while (true)
                            {
                                Console.Write("Введите пароль пользователя:");
                                user.HashedPassword = Hash.PasswordHash(Console.ReadLine());
                                if (user.HashedPassword == string.Empty)
                                {
                                    Console.WriteLine("Вы не ввели пароль");
                                    continue;
                                }
                                break;
                            }
                            db.User.Add(user);
                            try
                            {
                                db.SaveChanges();
                                Console.WriteLine("Пользователь успешно добавлен");
                                break;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Ошибка: {e.Message}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Логин существует");
                            continue;
                        }
                    }
                }
                else 
                {
                    Console.WriteLine("Пользователь с таким ФИО существует");
                }
            }
        }
    }
}
