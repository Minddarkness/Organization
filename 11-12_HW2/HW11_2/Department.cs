using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HW11_2
{
    public class Department : INotifyPropertyChanged
    {
        /// <summary>
        /// Уникальный ID отдела
        /// </summary>
        private int id;

        public int ID
        {
            get { return id; }
            private set
            {
                id = value;
                OnPropertyChanged("ID");
            }
        }
        /// <summary>
        /// Название отдела
        /// </summary>
        private string name;

        public string Name
        {
            get { return name; }
            private set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        /// <summary>
        /// Менеджер отдела
        /// </summary>
        private Manager manager;

        public Manager Manager
        {
            get { return manager; }
            private set
            {
                manager = value;
                OnPropertyChanged("Manager");
            }
        }
        /// <summary>
        /// Сотрудники отдела
        /// </summary>
        private ObservableCollection<Worker> workers;

        public ObservableCollection<Worker> Workers
        {
            get { return workers; }
            private set
            {
                workers = value;
                OnPropertyChanged("Workers");
            }
        }

        /// <summary>
        /// Подразделения
        /// </summary>
        private ObservableCollection<Department> subDepartments;

        public ObservableCollection<Department> SubDepartments
        {
            get { return subDepartments; }
            private set
            {
                subDepartments = value;
                OnPropertyChanged("SubDepartments");
            }
        }

        public enum SortedCriterion
        {
            ID,
            Name
        }

        /// <summary>
        /// Максимальное значение ID среди всех отделов
        /// </summary>
        private static int maxID = 0;
        /// <summary>
        /// Занятые названия отделов
        /// </summary>
        private static List<string> Names = new List<string>();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название</param>
        /// <param name="manager">Менеджер</param>
        public Department(string name, Manager manager)
        {
            ID = maxID++;
            if (!Names.Contains(name))
                Name = name;
            else Name = "Отдел" + ID;
            Names.Add(name);
            Manager = manager;
            manager.AssignDepartmentToManagement(this);
            Workers = new ObservableCollection<Worker>
            {
                Manager
            };
            SubDepartments = new ObservableCollection<Department>();
        }

        /// <summary>
        /// Меняет название отдела
        /// </summary>
        /// <param name="name">новое название отдела</param>
        public void СhangeName(string name)
        {
            string oldName = Name;
            if (!Names.Contains(name))
                Name = name;
            else
            {
                Name = $"Отдел{ID}";
            }

            Names.Remove(oldName);
            Names.Add(name);
        }

        /// <summary>
        /// Меняет менеджера отдела
        /// </summary>
        /// <param name="manager">новый менеджер</param>
        public void СhangeManager(Manager manager)
        {
            Workers.Remove(Manager);
            Manager = manager;
            Workers.Add(Manager);
            manager.AssignDepartmentToManagement(this);
        }

        /// <summary>
        /// Добавляет подразделение
        /// </summary>
        /// <param name="department"></param>
        public void AddSubDepartment(Department department)
        {
            if (department.GetType() != typeof(Organization))
                SubDepartments.Add(department);
        }

        /// <summary>
        /// Добавить сотрудника в отдел
        /// </summary>
        /// <param name="worker">сотрудник</param>
        public void AddWorker(Worker worker)
        {
            if (worker.GetType() != typeof(Manager))
                Workers.Add(worker);
        }

        /// <summary>
        /// Редактировать информацию о сотруднике с заданным ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">Имя</param>
        /// <param name="post">Должность</param>
        /// <param name="rate">Ставка</param>
        public void EditWorker(int id, string name, string post, int rate)
        {
            if (id == Manager.ID)
            {
                Manager.Edit(name, post, rate);
                return;
            }

            try
            {
                Worker worker = FindWorkerByID(id);
                worker.Edit(name, post, rate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Удалить сотрудника с заданным ID
        /// </summary>
        /// <param name="id">ID</param>
        public void DeleteWorker(int id)
        {
            try
            {
                Worker worker = FindWorkerByID(id);
                Workers.Remove(worker);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Найти сотрудника по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>найденного сотрудника или null</returns>
        private Worker FindWorkerByID(int id)
        {
            if (id == Manager.ID)
            {
                throw new Exception("Нельзя уволить менеджера");
            }
            for (int i = 0; i < Workers.Count; i++)
            {
                if ((Workers[i].GetType() != typeof(Manager)) && (Workers[i].ID == id))
                {
                    return Workers[i];
                }
            }
            throw new Exception("Сотрудник не был найден");
        }

        /// <summary>
        /// Ищет отдел в организации по ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>найденный отдел или null</returns>
        public Department FindDepartmentByID(int id)
        {
            for (int i = 0; i < SubDepartments.Count; i++)
            {
                if (id == SubDepartments[i].ID) return SubDepartments[i];
                else
                {
                    Department department = SubDepartments[i].FindDepartmentByID(id);
                    if (department != null)
                    {
                        return department;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Удаляет отедел с заданным ID
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="deleteSubDepartments">флаг: true - удалить все подразделения, 
        /// false - перенести их в вышестоящий отдел</param>
        public void DeleteDepartmentByID(int id, bool deleteSubDepartments)
        {
            for (int i = 0; i < SubDepartments.Count; i++)
            {
                if (id == SubDepartments[i].ID && deleteSubDepartments)
                {
                    //WriteLine(SubDepartments[i]);
                    Names.Remove(SubDepartments[i].Name);
                    SubDepartments.Remove(SubDepartments[i]);
                    return;
                }
                else if (id == SubDepartments[i].ID && !deleteSubDepartments)
                {
                    for (int j = 0; j < SubDepartments[i].SubDepartments.Count; j++)
                    {
                        SubDepartments.Add(SubDepartments[i].SubDepartments[j]);
                    }
                    Names.Remove(SubDepartments[i].Name);
                    SubDepartments.Remove(SubDepartments[i]);
                    return;
                }
                else
                {
                    SubDepartments[i].DeleteDepartmentByID(id, deleteSubDepartments);
                }
            }
        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <param name="deep">глубина</param>
        /// <returns>класс в строковом формате</returns>
        public string ToString(int deep)
        {
            string department = string.Empty;

            department = AddTab(department, deep) + $"{this.Name} ({Manager.Name} - {Manager.Salary})\n";

            //if (SubDepartments.Count != 0)
            //{
                for (int i = 0; i < SubDepartments.Count; i++)
                {
                    department += SubDepartments[i].ToString(deep + 1);
                    //if (SubDepartments[i].GetType() == typeof(Department))
                    //{
                    //    string str = string.Empty;
                    //    //str = AddTab(str, deep + 1);
                    //    for (int j = 0; j < SubDepartments[i].Workers.Count; j++)
                    //    {
                    //        str = (AddTab(str, deep + 2) + $"{SubDepartments[i].Workers[j].ID} " +
                    //           $"{SubDepartments[i].Workers[j].Name} " +
                    //           $"{SubDepartments[i].Workers[j].Post} " +
                    //           $"{SubDepartments[i].Workers[j].Salary}\n");
                    //    }
                    //    WriteLine(this.Name);
                    //    WriteLine(str);
                    //    if (!string.IsNullOrEmpty(str))
                    //        department += str;
                    //}

                }
                string str = string.Empty;
                //str = AddTab(str, deep + 1);
                for (int j = 0; j < Workers.Count; j++)
                {
                    str = (AddTab(str, deep + 1) + $"{Workers[j].ID} " +
                       $"{Workers[j].Name} " +
                       $"{Workers[j].Post} " +
                       $"{Workers[j].Salary}\n");
                }
                //WriteLine(this.Name);
                //WriteLine(str);
                if (!string.IsNullOrEmpty(str))
                    department += str;

                return department;
            //}
            //else
                //return department;
        }
        public void DeleteName(string name)
        {
            Names.Remove(name);
        }
        /// <summary>
        /// Добавляет нужное количество табуляций в начало строки
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="tabCount">количество табуляций</param>
        /// <returns>строка с табуляциями в начале</returns>
        private string AddTab(string str, int tabCount)
        {
            for (int i = 0; i < tabCount; i++)
            {
                str += "\t";
            }
            return str;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// Сортирует сотрудников по заданному критерию
        /// </summary>
        /// <param name="sortedCriterion"></param>
        /// <param name="direction"></param>
        public void SortWorkersBy(Worker.SortedCriterion sortedCriterion, ListSortDirection direction)
        {
            List<Worker> list = new List<Worker>(Workers);
            list.Sort(Worker.SortedBy(sortedCriterion));
            if (ListSortDirection.Descending == direction)
            {
                list.Reverse();
            }

            Workers = new ObservableCollection<Worker>(list);
        }

        /// <summary>
        /// Сортирует подразделения по заданному критерию
        /// </summary>
        /// <param name="sortedCriterion"></param>
        /// <param name="direction"></param>
        public void SortDepartmentsBy(SortedCriterion sortedCriterion, ListSortDirection direction)
        {
            List<Department> list = new List<Department>(SubDepartments);
            list.Sort(SortedBy(sortedCriterion));
            if (ListSortDirection.Descending == direction)
            {
                list.Reverse();
            }

            SubDepartments = new ObservableCollection<Department>(list);
        }

        /// <summary>
        /// Возвращает компаратор для заданного критерия сортировки
        /// </summary>
        /// <param name="sortedCriterion">критерий сортировки</param>
        /// <returns></returns>
        public static IComparer<Department> SortedBy(SortedCriterion sortedCriterion)
        {
            switch (sortedCriterion)
            {
                case SortedCriterion.ID:
                    return new SortByID();

                case SortedCriterion.Name:
                    return new SortByName();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Компаратор по ID
        /// </summary>
        private class SortByID : IComparer<Department>
        {
            public int Compare(Department x, Department y)
            {
                if (x.ID == y.ID) return 0;
                else if (x.ID > y.ID) return 1;
                else return -1;
            }
        }

        /// <summary>
        /// Компаратор по названию отдела
        /// </summary>
        private class SortByName : IComparer<Department>
        {
            public int Compare(Department x, Department y)
            {
                return String.Compare(x.Name, y.Name);
            }
        }
    }
}
