using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
namespace HW11_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Organization organization = new Organization("Org", new Manager("Mick", "Director"));
            Department dep1 = new Department("dep1", new Manager("Gosha", "Director of dep1"));
            Department dep2 = new Department("dep2", new Manager("Bob", "Director of dep2"));

            dep1.AddWorker(new Employee("Masha", "Worcker", dep1));
            dep1.AddWorker(new Employee("Pasha", "Worcker", dep1));
            dep2.AddWorker(new Employee("Dasha", "Worcker", dep2));


            Department dep11 = new Department("dep11", new Manager("Alisa", "Director of dep11"));
            Department dep12 = new Department("dep12", new Manager("Bob", "Director of dep12"));
            dep11.AddWorker(new Intern("Bill", "Worcker", dep11));
            dep12.AddWorker(new Employee("Gosha", "Worcker", dep12));
            Department dep111 = new Department("dep111", new Manager("Katerina", "Director of dep111"));
            dep111.AddWorker(new Employee("Oleg", "Programmer", dep111));
            dep11.AddSubDepartment(dep111);

            Department dep21 = new Department("dep21", new Manager("Gosha", "Director of dep21"));
            Department dep22 = new Department("dep22", new Manager("Bob", "Director of dep22"));
            Department dep23 = new Department("dep23", new Manager("Gosha", "Director of dep23"));
            dep21.AddWorker(new Intern("Bill", "Worcker", dep21));
            dep21.AddWorker(new Employee("Gosha", "Worcker", dep21));

            dep22.AddWorker(new Intern("Bill", "Worcker", dep22));
            dep22.AddWorker(new Employee("Gosha", "Worcker", dep22));

            dep23.AddWorker(new Intern("Bill", "Worcker", dep23));
            dep23.AddWorker(new Employee("Gosha", "Worcker", dep23));

            organization.AddSubDepartment(dep1);
            organization.AddSubDepartment(dep2);

            dep1.AddSubDepartment(dep11);
            dep1.AddSubDepartment(dep12);

            dep2.AddSubDepartment(dep21);
            dep2.AddSubDepartment(dep22);
            dep2.AddSubDepartment(dep23);

            WriteLine(organization);
            //organization.Serialization("Org1.json");

            //organization.DeleteDepartmentByID(2, false);
            organization.Serialization("Org1.json");

            WriteLine("Удаление");
            //WriteLine(organization);

            Organization organization1 = Organization.Deserialization("Org1.json");
            WriteLine(organization1);
        }
    }
}
