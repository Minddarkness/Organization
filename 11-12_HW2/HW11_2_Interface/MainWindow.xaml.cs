using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HW11_2;
using Microsoft.Win32;

namespace HW11_2_Interface
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Организация
        /// </summary>
        private Organization organization;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                LoadFromFile("Example.json"); //загружает организацию
            }
            catch (Exception)
            {

                MessageBox.Show("Загрузите организацию из файла");
            }
            this.DataContext = organization;
        }

        /// <summary>
        /// Считает зарплату для всех сотрудников и рукводителей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void CalculateSalaryOnClick(object sender, RoutedEventArgs e)
        //{
        //    if (organization == null)
        //    {
        //        MessageBox.Show("Нужно загрузить организацию");
        //        return;
        //    }
        //    organization.CalculateSalary();
        //    WorkersListView.Items.Refresh();
        //}

        /// <summary>
        /// Сохраняет организацию в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            string filename;
            SaveFileDialog SF = new SaveFileDialog();
            SF.Filter = "Text files(*.json)|*.json";

            if (SF.ShowDialog() == true)
            {
                filename = SF.FileName;

                try
                {
                    organization.Serialization(filename);
                    MessageBox.Show("Запись выполнена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else return;
        }

        private void LoadFromFileOnClick(object sender, RoutedEventArgs e)
        {
            string filename;
            OpenFileDialog OPF = new OpenFileDialog();
            OPF.Filter = "Text files(*.json)|*.json";
            if (OPF.ShowDialog() == true)
            {
                filename = OPF.FileName;
                LoadFromFile(filename);
            }
            else return;
        }

        private void LoadFromFile(string filename)
        {
            organization = Organization.Deserialization(filename);
            treeView1.ItemsSource = organization.SubDepartments;
            treeView1.GotFocus += TreeView1_GotFocus;
            WorkersListView.ItemsSource = null;
            //WorkersListView.View.
            organization.CalculateSalary();
            //OrgNameTreeView.Text = organization.Name;
        }

        /// <summary>
        /// При выборе отдела в дереве, отображает информацию об этом отделе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView1_GotFocus(object sender, RoutedEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            Department tvItem = (Department)treeView.SelectedItem;//выбранный отдел
            if (tvItem != null)
            {
                WorkersListView.ItemsSource = tvItem.Workers;//привязывает список сотрудников выбранного отдела к ListView
                lastHeaderClicked = null;
            }

        }

        /// <summary>
        /// Редактирует название организации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditOrgNameOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(OrgName.Text))
            {
                MessageBox.Show("Название организации не введено");
                return;
            }
            if (organization.Name != OrgName.Text)
            {
                organization.СhangeName(OrgName.Text);
                //OrgNameTreeView.Text = organization.Name;
            }

        }

        /// <summary>
        /// Выводит информацию о директоре организации в ListView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDirectorInformationOnClick(object sender, RoutedEventArgs e)
        {
            //DivisionID.Text = null;
            //DivisionName.Clear();
            ObservableCollection<Worker> allWorkers = new ObservableCollection<Worker>() { organization.Manager };
            WorkersListView.ItemsSource = allWorkers;
        }

        /// <summary>
        /// Удаляет отдел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteDivisionOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                MessageBox.Show("Отдел не выбран");
                return;
            }
            if (MessageBox.Show("Сотрудники данного отдела будут уволены. Вы уверены, что хотите ликвидировать отдел?",
                "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (MessageBox.Show("Перенести подразделения данного отдела в вышестоящий отдел?",
                    "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    organization.DeleteDepartmentByID(int.Parse(DivisionID.Text), false);
                }
                else
                {
                    organization.DeleteDepartmentByID(int.Parse(DivisionID.Text), true);
                }
                WorkersListView.ItemsSource = null;
                //DivisionID.Text = null;
                //DivisionName.Text = null;
            }

        }

        /// <summary>
        /// Редактирует название отдела
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditDivisionNameOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionName.Text))
            {
                MessageBox.Show("Отдел не выбран или имя не введено");
                return;
            }
            Department department = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (department == null)
            {
                MessageBox.Show($"Отдел с ID = {DivisionID.Text} не найден");
                return;
            }
            if (department.Name != DivisionName.Text)
            {
                department.СhangeName(DivisionName.Text);
            }

        }

        /// <summary>
        /// Изменяет менеджера отдела
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HireManagerOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                MessageBox.Show("Отдел не выбран");
                return;
            }
            if (string.IsNullOrEmpty(newManagerName.Text) || string.IsNullOrEmpty(newManagerPost.Text))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            Department division = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (division == null)
            {
                MessageBox.Show("Отдел не найден");
                return;
            }

            division.СhangeManager(new Manager(newManagerName.Text, newManagerPost.Text));
            organization.CalculateSalary();
            //ObservableCollection<Worker> allWorkers = new ObservableCollection<Worker>(division.Workers);
            //allWorkers.Add(division.Manager);
            //WorkersListView.ItemsSource = allWorkers;

            newManagerName.Text = null;
            newManagerPost.Text = null;
        }

        /// <summary>
        /// Добавляет подразделение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDepartmentOnClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                result = MessageBox.Show(
                    "Отдел не выбран. Добавить подразделение в организацию?",
                    "Подтверждение", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.OK)
                {
                    organization.AddSubDepartment(new Department(newDivisionName.Text, new Manager(newDivManagerName.Text, newDivManagerPost.Text)));
                    //treeView1.Items.Refresh();
                }
                newDivisionName.Text = null;
                newDivManagerName.Text = null;
                newDivManagerPost.Text = null;
                return;
            }
            if (string.IsNullOrEmpty(newDivisionName.Text) || string.IsNullOrEmpty(newDivManagerName.Text)
                || string.IsNullOrEmpty(newDivManagerPost.Text))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            Department division = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (division == null)
            {
                MessageBox.Show("Отдел не найден");
                return;
            }
            result = MessageBox.Show("Добавить подразделение в выбранный отдел? Если нет, то оно будет добавлено в организацию",
               "Подтверждение", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                division.AddSubDepartment(new Department(newDivisionName.Text, new Manager(newDivManagerName.Text, newDivManagerPost.Text)));
                //treeView1.Items.Refresh();
            }
            else if (result == MessageBoxResult.No)
            {
                organization.AddSubDepartment(new Department(newDivisionName.Text, new Manager(newDivManagerName.Text, newDivManagerPost.Text)));
                //treeView1.Items.Refresh();
            }
            newDivisionName.Text = null;
            newDivManagerName.Text = null;
            newDivManagerPost.Text = null;
        }

        /// <summary>
        /// Удаляет сотрудника из организации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FireWorkerOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                MessageBox.Show("Отдел не выбран");
                return;
            }
            if (string.IsNullOrEmpty(WorkerID.Text))
            {
                MessageBox.Show("Сотрудник не выбран");
                return;
            }
            Department department = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (department == null)
            {
                MessageBox.Show("Отдел не найден");
                return;
            }
            try
            {
                department.DeleteWorker(int.Parse(WorkerID.Text));
                organization.CalculateSalary();
                //ObservableCollection<Worker> allWorkers = new ObservableCollection<Worker>(department.Workers);
                //allWorkers.Add(department.Manager);
                //WorkersListView.ItemsSource = allWorkers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Редактирует информацию о сотруднике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditWorkerOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                if (organization.Manager.ID == int.Parse(WorkerID.Text))
                {
                    organization.Manager.Edit(WorkerName.Text, WorkerPost.Text, 0);
                    return;
                }
                else
                    MessageBox.Show("Отдел не выбран");
                return;
            }
            if (string.IsNullOrEmpty(WorkerID.Text))
            {
                MessageBox.Show("Сотрудник не выбран");
                return;
            }
            if (string.IsNullOrEmpty(WorkerName.Text) || string.IsNullOrEmpty(WorkerRate.Text)
                || string.IsNullOrEmpty(WorkerPost.Text))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }
            if (organization.Manager.ID == int.Parse(WorkerID.Text))
            {
                organization.Manager.Edit(WorkerName.Text, WorkerPost.Text, 0);
                return;
            }
                Department department = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (department == null)
            {
                MessageBox.Show("Отдел не найден");
                return;
            }

            try
            {
                department.EditWorker(int.Parse(WorkerID.Text), WorkerName.Text,
                    WorkerPost.Text, int.Parse(WorkerRate.Text));
                organization.CalculateSalary();
                //ObservableCollection<Worker> allWorkers = new ObservableCollection<Worker>(department.Workers);
                //allWorkers.Add(department.Manager);
                //WorkersListView.ItemsSource = allWorkers;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        /// <summary>
        /// Добавляет сотрудника в отдел
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HireWorkerOnClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DivisionID.Text))
            {
                MessageBox.Show("Отдел не выбран");
                return;
            }
            if (string.IsNullOrEmpty(newWorkerName.Text) || string.IsNullOrEmpty(newWorkerRate.Text)
                || string.IsNullOrEmpty(newWorkerPost.Text))
            {
                MessageBox.Show("Не все поля заполнены");
                return;
            }

            Department department = organization.FindDepartmentByID(int.Parse(DivisionID.Text));
            if (department == null)
            {
                MessageBox.Show("Отдел не найден");
                return;
            }
            try
            {
                if (IsNewWorkerIntern.IsChecked == true)
                {
                    department.AddWorker(
                        new Intern(newWorkerName.Text, newWorkerPost.Text, department, int.Parse(newWorkerRate.Text))
                        );
                }
                else
                {
                    department.AddWorker(
                        new Employee(newWorkerName.Text, newWorkerPost.Text, department, int.Parse(newWorkerRate.Text))
                        );
                }
                organization.CalculateSalary();
                //ObservableCollection<Worker> allWorkers = new ObservableCollection<Worker>(department.Workers);
                //allWorkers.Add(department.Manager);
                //WorkersListView.ItemsSource = allWorkers;
                newWorkerName.Clear();
                newWorkerPost.Clear();
                newWorkerRate.Clear();
                IsNewWorkerIntern.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


        }

        /// <summary>
        /// Последний выбранный заголовок
        /// </summary>
        GridViewColumnHeader lastHeaderClicked = null;

        /// <summary>
        /// Последнее направление сортировки
        /// </summary>
        Dictionary<string, ListSortDirection> lastDirectionsListView = new Dictionary<string, ListSortDirection>()
        {
            {"ID",ListSortDirection.Ascending },
            {"Name",ListSortDirection.Ascending },
            {"Post",ListSortDirection.Ascending },
            {"Salary",ListSortDirection.Ascending },
            {"Rate",ListSortDirection.Ascending },
            {"Hours",ListSortDirection.Ascending },

        };

        /// <summary>
        /// Последнее направление сортировки
        /// </summary>
        Dictionary<string, ListSortDirection> lastDirectionsTreeView = new Dictionary<string, ListSortDirection>()
        {
            {"ID",ListSortDirection.Descending },
            {"Name",ListSortDirection.Descending },
        };

        /// <summary>
        /// Сортирует работников выбранного отдела по нажатию на заголовок
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewColumnHeaderOnClick(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;

            if (WorkersListView.ItemsSource == null || headerClicked == null) return;

            ListSortDirection direction;
            Binding columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
            string sortBy = columnBinding.Path.Path.ToString();

            if (headerClicked != lastHeaderClicked)
            {
                direction = ListSortDirection.Ascending;
            }
            else
            {
                if (lastDirectionsListView[sortBy] == ListSortDirection.Ascending)
                {
                    direction = ListSortDirection.Descending;
                }
                else
                {
                    direction = ListSortDirection.Ascending;
                }
            }

            Department tvItem = (Department)treeView1.SelectedItem;//выбранный отдел
            switch (sortBy)
            {
                case "ID":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.ID, direction);                     
                    }                       
                    break;

                case "Name":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.Name, direction);
                    }
                    break;

                case "Post":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.Post, direction);
                    }
                    break;

                case "Salary":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.Salary, direction);
                    }
                    break;

                case "Rate":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.Rate, direction);
                    }
                    break;

                case "Hours":
                    if (tvItem != null)
                    {
                        tvItem.SortWorkersBy(Worker.SortedCriterion.Hours, direction);
                    }
                    break;

            }
            WorkersListView.ItemsSource = tvItem.Workers;
            lastHeaderClicked = headerClicked;
            lastDirectionsListView[sortBy] = direction;
        }

        /// <summary>
        /// Сортирует отделы по ID в TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortDepartmentsByIDOnClick(object sender, RoutedEventArgs e)
        {
            if (organization == null)
            {
                MessageBox.Show("Загрузите организацию из файла");
                return;
            }
            SortDepartmentsBy("ID");
            lastDirectionsTreeView["Name"] = ListSortDirection.Descending;

        }

        /// <summary>
        /// Сортирует отделы по названию в TreeView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortDepartmentsByNameOnClick(object sender, RoutedEventArgs e)
        {
            if (organization == null)
            {
                MessageBox.Show("Загрузите организацию из файла");
                return;
            }
            SortDepartmentsBy("Name");
            lastDirectionsTreeView["ID"] = ListSortDirection.Descending;
        }

        /// <summary>
        /// Сортирует отделы по заданному критерию в TreeView
        /// </summary>
        /// <param name="sortBy">критерий сортировки</param>
        private void SortDepartmentsBy(string sortBy)
        {

            ListSortDirection direction;

            if (lastDirectionsTreeView[sortBy] == ListSortDirection.Ascending)
            {
                direction = ListSortDirection.Descending;
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }
            if(sortBy == "ID")
            {
                organization.SortDepartmentsBy(Department.SortedCriterion.ID, direction);
            }
            else
            {
                organization.SortDepartmentsBy(Department.SortedCriterion.Name, direction);
            }

            treeView1.ItemsSource = organization.SubDepartments;
            WorkersListView.ItemsSource = null;
            //WorkersListView.View.
            organization.CalculateSalary();

            lastDirectionsTreeView[sortBy] = direction;
        }
    }
}
