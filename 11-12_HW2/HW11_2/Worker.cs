using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HW11_2
{
    public abstract class Worker : INotifyPropertyChanged
    {
        /// <summary>
        /// уникальный ID сотрудника
        /// </summary>
        private int id;

        public int ID
        {
            get { return id; }
            protected set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }
        /// <summary>
        /// Имя
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            protected set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        /// <summary>
        /// Должность
        /// </summary>
        private string post; 

        public string Post
        {
            get { return post; }
            protected set
            {
                post = value;
                OnPropertyChanged("Post");
            }
        }
        /// <summary>
        /// Заработная плата
        /// </summary>
        private int salary;

        public int Salary
        {
            get { return salary; }
            protected set
            {
                salary = value;
                OnPropertyChanged("Salary");
            }
        }
        /// <summary>
        /// Ставка
        /// </summary>
        private int rate;

        public int Rate
        {
            get { return rate; }
            protected set
            {
                rate = value;
                OnPropertyChanged("Rate");
            }
        }
        /// <summary>
        /// Подразделение, в котором работает сотрудник
        /// </summary>
        private Department department;

        public Department Department
        {
            get { return department; }
            protected set
            {
                department = value;
                OnPropertyChanged("Department");
            }
        }

        /// <summary>
        /// Максимальное ID среди всех сотрудников
        /// </summary>
        protected static int maxID = 0;
        /// <summary>
        /// Рандом
        /// </summary>
        protected static Random randomize = new Random();

        public enum SortedCriterion
        {
            ID,
            Name,
            Post,
            Salary,
            Rate,
            Hours
        }

        /// <summary>
        /// Вычисляет зарплату
        /// </summary>
        public abstract void CalculateSlary();

        /// <summary>
        /// Редактирует информацию о сотруднике
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="rate">Ставка</param>
        public virtual void Edit(string name, string post, int rate)
        {
            Name = name;
            Post = post;
            Rate = rate;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        protected Worker(string name, string post)
        {
            Name = name;
            Post = post;
            Salary = 0;
            Rate = 0;
            ID = maxID++;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        protected Worker(string name, string post, Department department) : this(name, post)
        {
            Department = department;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="department">Подразделение</param>
        /// <param name="rate">Ставка</param>
        protected Worker(string name, string post, Department department, int rate, int salary)
        {
            Name = name;
            Post = post;
            Salary = salary;
            Rate = rate;
            ID = maxID++;
            Department = department;

        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <returns>класс в строковом формате</returns>
        public override string ToString()
        {
            return String.Format(
                "Отдел: {0}\n" + "Имя: {1}\n" + "Должность: {2}\n" + "Ставка: {3}\n" + "ID: {4}",
                Department.Name,
                Name,
                Post,
                Rate,
                ID
                );
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Возвращает компаратор для заданного критерия сортировки
        /// </summary>
        /// <param name="sortedCriterion">критерий сортировки</param>
        /// <returns></returns>
        public static IComparer<Worker> SortedBy(SortedCriterion sortedCriterion)
        {
            switch(sortedCriterion)
            {
                case SortedCriterion.ID:
                    return new SortByID();

                case SortedCriterion.Name:
                    return new SortByName();

                case SortedCriterion.Post:
                    return new SortByPost();

                case SortedCriterion.Rate:
                    return new SortByRate();

                case SortedCriterion.Salary:
                    return new SortBySalary();

                case SortedCriterion.Hours:
                    return new SortByHours();
                default:
                    return null;
            }
        }


        /// <summary>
        /// Компаратор по ID
        /// </summary>
        private class SortByID : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                if (x.ID == y.ID) return 0;
                else if (x.ID > y.ID) return 1;
                else return -1;
            }
        }

        /// <summary>
        /// Компаратор по имени
        /// </summary>
        private class SortByName : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.Name, y.Name);
            }
        }

        /// <summary>
        /// Компаратор по должности
        /// </summary>
        private class SortByPost : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                return String.Compare(x.Post, y.Post);
            }
        }

        /// <summary>
        /// Компаратор по зарплате
        /// </summary>
        private class SortBySalary : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                if (x.Salary == y.Salary) return 0;
                else if (x.Salary > y.Salary) return 1;
                else return -1;
            }
        }

        /// <summary>
        /// Компаратор по ставке
        /// </summary>
        private class SortByRate : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                if (x.Rate == y.Rate) return 0;
                else if (x.Rate > y.Rate) return 1;
                else return -1;
            }
        }

        /// <summary>
        /// Компаратор по часам
        /// </summary>
        private class SortByHours : IComparer<Worker>
        {
            public int Compare(Worker x, Worker y)
            {
                if((x.GetType() == typeof(Employee)) && (y.GetType() == typeof(Employee)))
                {
                    if ((x as Employee).Hours == (y as Employee).Hours) return 0;
                    else if ((x as Employee).Hours > (y as Employee).Hours) return 1;
                    else return -1;
                }
                else if(x.GetType() == typeof(Employee))
                {
                    return 1;
                }
                else if (y.GetType() == typeof(Employee))
                {
                    return -1;
                }
                else // оба не Employee 
                {
                    return 0;
                }
            }
        }
    }
}
