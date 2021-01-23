using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11_2
{
    public class Manager : Worker
    {

        /// <summary>
        /// Минимальная зарплата менеджера
        /// </summary>
        private static int minSalary = 1300;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        public Manager(string name, string post) : base(name, post)
        {
            
        }

        /// <summary>
        /// Считает зарплату
        /// </summary>
        public override void CalculateSlary()
        {
            // оплата труда менеджера составляет 15 % от общей выплаченной суммы всем сотрудникам
            // числящихся в его отделе, но не менее $1300.

            int sum = 0;
            for (int i = 0; i < Department.SubDepartments.Count; i++)
            {
                Department.SubDepartments[i].Manager.CalculateSlary();//запускает рекурсию для менеджера каждого подразделения
                sum += Department.SubDepartments[i].Manager.Salary;//считаем сумму зп менеджеров, подразделений данного отдела

            }

            if (Department.GetType() == typeof(Department))
            {
                for (int j = 0; j < (Department as Department).Workers.Count; j++)
                {
                    if (Department.Workers[j].GetType() != typeof(Manager))
                    {
                        Department.Workers[j].CalculateSlary();//считаем зп сотрудников отдела
                        sum += Department.Workers[j].Salary;//добавляем сумму всех сотрудников отдела
                    }
                }
            }

            sum = (int)(sum * 0.15);//берем 15%
            if (sum >= minSalary)//устанавливаем зп, в зависимости от полученного значения
            {
                Salary = sum;
            }
            else Salary = minSalary;
        }

        /// <summary>
        /// Считает зарплату для менеджера организации
        /// </summary>
        public void CalculateSlary(Organization organization)
        {
            int sum = 0;

            // оплата труда менеджера составляет 15 % от общей выплаченной суммы всем сотрудникам
            // числящихся в его отделе, но не менее $1300.
            for (int i = 0; i < organization.SubDepartments.Count; i++)
            {
                organization.SubDepartments[i].Manager.CalculateSlary();//запускает рекурсию для менеджера каждого подразделения
                sum += organization.SubDepartments[i].Manager.Salary;//считаем сумму зп менеджеров, подразделений данной организации

            }

            sum = (int)(sum * 0.15);//берем 15%
            if (sum >= minSalary)//устанавливаем зп, в зависимости от полученного значения
            {
                Salary = sum;
            }
            else Salary = minSalary;
        }

        public void AssignDepartmentToManagement(Department department)
        {
            Department = department;
        }

        /// <summary>
        /// Редактирует информацию о менеджере
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="rate">Ставка</param>
        public override void Edit(string name, string post, int rate)
        {
            base.Edit(name, post, 0);
        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <returns>класс в строковом формате</returns>
        public override string ToString()
        {
            return String.Format(
                "{0}\n" + "Зарплата: {1}\n",
                base.ToString(),
                Salary
                );
        }
    }
}
