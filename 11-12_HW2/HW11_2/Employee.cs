using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11_2
{
    public class Employee : Worker
    {
        /// <summary>
        /// количество часов отработанных за месяц
        /// </summary>
        private int hours;

        public int Hours
        {
            get { return hours; }
            private set
            {
                hours = value;
                OnPropertyChanged("Hours");
            }
        }

        /// <summary>
        /// Конструктор с выбором стаки случайным образом
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        public Employee(string name, string post, Department department) : base(name, post, department)
        {
            Rate = randomize.Next(7, 26); // от 7$ до 25$ в час
            Hours = randomize.Next(120, 201); //генерируем новое кол-во часов (от 120 до 200 часов в месяц);
            Salary = Rate * Hours;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        /// <param name="rate">Ставка</param>
        [JsonConstructor]
        public Employee(string name, string post, Department department, int rate) : base(name, post, department, rate,0)
        {
            if (rate < 7 || rate > 25)
            {
                throw new Exception("Ставка у сотрудника должна быть в пределах от 7$ до 25$");
            }
            Hours = randomize.Next(120, 201); //генерируем новое кол-во часов (от 120 до 200 часов в месяц);
            Salary = Rate * Hours;
        }

        /// <summary>
        /// Редактирует информацию о сотруднике
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="rate">Ставка</param>
        public override void Edit(string name, string post, int rate)
        {
            if (rate < 7 || rate > 25)
            {
                throw new Exception("Ставка у сотрудника должна быть в пределах от 7$ до 25$");
            }
            base.Edit(name, post, rate);
        }

        /// <summary>
        /// Считает зарплату
        /// </summary>
        public override void CalculateSlary()
        {
            //Hours = randomize.Next(120, 201); //генерируем новое кол-во часов (от 120 до 200 часов в месяц)
            Salary = Rate * Hours;
        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <returns>класс в строковом формате</returns>
        public override string ToString()
        {
            return String.Format(
                "{0}\n" + "Часы: {1}\n" + "Зарплата: {2}",
                base.ToString(),
                Hours,
                Salary
                );
        }
    }
}
