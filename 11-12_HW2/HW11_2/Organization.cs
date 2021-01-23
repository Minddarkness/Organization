using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW11_2
{
    public class Organization : INotifyPropertyChanged
    {
        /// <summary>
        /// Название организации
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
        /// Менеджер организации
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

        public Organization(string name, Manager manager)
        {
            Manager = manager;
            SubDepartments = new ObservableCollection<Department>();
            Name = name;
        }

        /// <summary>
        /// Запускает подсчёт зарплаты для всей организации
        /// </summary>
        public void CalculateSalary()
        {
            Manager.CalculateSlary(this);
        }

        /// <summary>
        /// Меняет название организации
        /// </summary>
        /// <param name="name">новое название организации</param>
        public void СhangeName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Меняет менеджера организации
        /// </summary>
        /// <param name="manager">новый менеджер организации</param>
        public void СhangeManager(Manager manager)
        {
            Manager = manager;
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
                    SubDepartments[i].DeleteName(SubDepartments[i].Name);
                    SubDepartments.Remove(SubDepartments[i]);
                    return;
                }
                else if (id == SubDepartments[i].ID && !deleteSubDepartments)
                {
                    for (int j = 0; j < SubDepartments[i].SubDepartments.Count; j++)
                    {
                        SubDepartments.Add(SubDepartments[i].SubDepartments[j]);
                    }
                    SubDepartments[i].DeleteName(SubDepartments[i].Name);
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
        /// Добавляет подразделение
        /// </summary>
        /// <param name="department"></param>
        public void AddSubDepartment(Department department)
        {
            SubDepartments.Add(department);
        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <returns>класс в строковом формате</returns>
        public override string ToString()
        {
            return ToString(0);
        }

        /// <summary>
        /// Представляет данный класс в строковом формате
        /// </summary>
        /// <param name="deep"></param>
        /// <returns>класс в строковом формате</returns>
        public string ToString(int deep)
        {
            string department = string.Empty;

            department = AddTab(department, deep) + $"Организация: {this.Name} ({Manager.Name} - {Manager.Salary})\n";

            if (SubDepartments.Count != 0)
            {
                for (int i = 0; i < SubDepartments.Count; i++)
                {
                    department += SubDepartments[i].ToString(deep + 1);
                }
                return department;
            }
            else
                return department;
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

        /// <summary>
        /// Записывает данные об организации в файл
        /// </summary>
        /// <param name="fileName">имя файла для записи</param>
        public void Serialization(string fileName)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json;
            json = JsonConvert.SerializeObject(this, jsonSerializerSettings);
            File.WriteAllText(fileName, json);

        }

        /// <summary>
        /// Считывает данные об организации из файла
        /// </summary>
        /// <param name="fileName">имя файла для чтения</param>
        /// <returns>организацию</returns>
        public static Organization Deserialization(string fileName)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string json;
            json = File.ReadAllText(fileName);
            Organization organization2 = JsonConvert.DeserializeObject<Organization>(json, jsonSerializerSettings);
            return organization2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void SortDepartmentsBy(Department.SortedCriterion sortedCriterion, ListSortDirection direction)
        {
            List<Department> list = new List<Department>(SubDepartments);
            list.Sort(Department.SortedBy(sortedCriterion));
            if (ListSortDirection.Descending == direction)
            {
                list.Reverse();
            }
            SubDepartments = new ObservableCollection<Department>(list);

            for (int i = 0; i < SubDepartments.Count; i++)
            {
                SubDepartments[i].SortDepartmentsBy(sortedCriterion, direction);
            }
        }

    }
}
