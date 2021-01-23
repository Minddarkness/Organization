using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11_2
{
    public class Intern : Worker
    {
        /// <summary>
        /// Конструктор со случайным выбором ставки
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        public Intern(string name, string post, Department department) : base(name, post, department)
        {
            if (!post.ToLower().Contains("интерн"))
            {
                Post = "Интерн - " + post;
            }

            Rate = randomize.Next(3, 6) * 100; // от 300$ до 500$ в месяц
            Salary = Rate;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        /// <param name="rate">Ставка</param>
        [JsonConstructor]
        public Intern(string name, string post, Department department, int rate) : base(name, post, department, rate, rate)
        {
            if (!post.ToLower().Contains("интерн"))
            {
                Post = "Интерн - " + post;
            }

            if (rate < 300 || rate > 500)
            {
                throw new Exception("Ставка у интерна должна быть в пределах от 300$ до 500$");
            }
        }

        /// <summary>
        /// Редактирует информацию о сотруднике
        /// </summary>
        /// <param name="name"></param>
        /// <param name="post"></param>
        /// <param name="rate"></param>
        public override void Edit(string name, string post, int rate)
        {
            if (rate < 300 || rate > 500)
            {
                throw new Exception("Ставка у интерна должна быть в пределах от 300$ до 500$");
            }
            if (!post.ToLower().Contains("интерн"))
            {
                base.Edit(name, "Интерн - " + post, rate);
            }
            else base.Edit(name, post, rate);

        }

        /// <summary>
        /// Считает зароботную плату
        /// </summary>
        public override void CalculateSlary()
        {
            Salary = Rate;
        }
    }
}

