using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            LoadUsers();
            LoadEmployees();
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public void LoadUsers()
        {
            User user = new User() { UserId = 1, FirstName = "Mohan", LastName = "Chandra", Email = "Mohan@abc.com", UserName = "MohanC" };
            Users.Add(user);
            user = new User() { UserId = 2, FirstName = "Harinath", LastName = "Reddy", Email = "Harinath@abc.com", UserName = "HarinathR" };
            Users.Add(user);
            user = new User() { UserId = 3, FirstName = "Sachin", LastName = "Yadav", Email = "Sachin@abc.com", UserName = "SachinY" };
            Users.Add(user);
            user = new User() { UserId = 4, FirstName = "Gopal", LastName = "Sharma", Email = "Gopal@abc.com", UserName = "GopalS" };
            Users.Add(user);
            user = new User() { UserId = 5, FirstName = "Ashish", LastName = "Tyagi", Email = "Ashish@abc.com", UserName = "AshishT" };
            Users.Add(user);
        }

        public void LoadEmployees()
        {
            Employee employee = new Employee() { EmployeeId = 1, Name = "Arjun", Email = "Arjun@xyz.com", Salary = 10000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 2, Name = "Aditya", Email = "Aditya@xyz.com", Salary = 12000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 3, Name = "Jitendra", Email = "Jitendra@xyz.com", Salary = 15000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 4, Name = "Rahul", Email = "Rahul@xyz.com", Salary = 10000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 5, Name = "Kundan", Email = "Kundan@xyz.com", Salary = 20000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 6, Name = "Deepak", Email = "Deepak@xyz.com", Salary = 25000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 7, Name = "Raju", Email = "Raju@xyz.com", Salary = 11000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 8, Name = "Keshav", Email = "Keshav@xyz.com", Salary = 27000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 9, Name = "Lalit", Email = "Lalit@xyz.com", Salary = 12000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 10, Name = "Ashish", Email = "Ashish@xyz.com", Salary = 10000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 11, Name = "Rajiv", Email = "Rajiv@xyz.com", Salary = 16000 };
            Employees.Add(employee);
            employee = new Employee() { EmployeeId = 12, Name = "Puran", Email = "Puran@xyz.com", Salary = 23000 };
            Employees.Add(employee);
        }

        public List<User> GetUsers()
        {
            return Users.Local.ToList<User>();
        }

        public List<Employee> GetEmployees()
        {
            return Employees.Local.ToList<Employee>();
        }
    }
}
