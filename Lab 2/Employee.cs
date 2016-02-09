using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lab_2 {

   public delegate bool ssnSorter (int a, int b);

   public interface IPayable {
      decimal GetPaymentAmount (); // calculate payment; no implementation
   } // end interface IPayable

   public abstract class Employee : IPayable, IComparable {
      // read-only property that gets employee's first name
      public string FirstName {
         get; private set;
      }

      // read-only property that gets employee's last name
      public string LastName {
         get; private set;
      }

      // read-only property that gets employee's social security number
      public string SocialSecurityNumber {
         get; private set;
      }

      // three-parameter constructor
      public Employee (string first, string last, string ssn) {
         FirstName = first;
         LastName = last;
         SocialSecurityNumber = ssn;
      } // end three-parameter Employee constructor

      //Delagate for Option 3, using selection sort and and delegates for sorting SSN's
      public static int SortAsSSN (Object emp1, Object emp2) {
         string n1 = ( (Employee)emp1 ).SocialSecurityNumber;
         string n2 = ( (Employee)emp2 ).SocialSecurityNumber;
         if (String.Compare(n1, n2) > 0) {
            return 1;
         }
         else if (String.Compare(n1, n2) < 0) {
            return -1;
         }
         else {
            return 0;
         }
      }

      public delegate bool ComparisonHandler (string a, string b);

      public static bool StringCompare (string first, string second) {
         return first.CompareTo(second) < 0;
      }
      
      // return string representation of Employee object, using properties
      public override string ToString () {
         return string.Format("{0} {1}\nsocial security number: {2}",
            FirstName, LastName, SocialSecurityNumber);
      } // end method ToString

      public abstract decimal GetPaymentAmount ();

      public int CompareTo (Object other) {
         Employee e = (Employee)other;
         return String.Compare(this.LastName, e.LastName);
         throw new NotImplementedException();
      }

      //Using IComparer to help sort pay ammounts 
      private class payAmountHelper : IComparer {

         public int Compare (object x, object y) {

            Employee c1 = (Employee)x;
            Employee c2 = (Employee)y;
            if (c1.GetPaymentAmount() < c2.GetPaymentAmount())
               return 1;
            if (c1.GetPaymentAmount() > c2.GetPaymentAmount())
               return -1;
            else
               return 0;
            throw new NotImplementedException();
         }

      }
      public static IComparer payAmountSorter () {
         return (IComparer)new payAmountHelper();
      }

   } // end abstract class Employee

   public class HourlyEmployee : Employee {
      private decimal wage; // wage per hour
      private decimal hours; // hours worked for the week

      // five-parameter constructor
      public HourlyEmployee (string first, string last, string ssn,
         decimal hourlyWage, decimal hoursWorked)
         : base(first, last, ssn) {
         Wage = hourlyWage; // validate hourly wage via property
         Hours = hoursWorked; // validate hours worked via property
      } // end five-parameter HourlyEmployee constructor

      // property that gets and sets hourly employee's wage
      public decimal Wage {
         get {
            return wage;
         } // end get
         set {
            if (value >= 0) // validation
               wage = value;
            else
               throw new ArgumentOutOfRangeException("Wage",
                  value, "Wage must be >= 0");
         } // end set
      } // end property Wage

      // property that gets and sets hourly employee's hours
      public decimal Hours {
         get {
            return hours;
         } // end get
         set {
            if (value >= 0 && value <= 168) // validation
               hours = value;
            else
               throw new ArgumentOutOfRangeException("Hours",
                  value, "Hours must be >= 0 and <= 168");
         } // end set
      } // end property Hours

      // calculate earnings; override Employee’s abstract method Earnings
      public override decimal GetPaymentAmount () {
         if (Hours <= 40) // no overtime                          
            return Wage * Hours;
         else
            return ( 40 * Wage ) + ( ( Hours - 40 ) * Wage * 1.5M );
      } // end method Earnings                                      

      // return string representation of HourlyEmployee object
      public override string ToString () {
         return string.Format(
            "hourly employee: {0}\n{1}: {2:C}; {3}: {4:F2}",
            base.ToString(), "hourly wage", Wage, "hours worked", Hours + "\nTotal: " + this.GetPaymentAmount());
      } // end method ToString                                            
   } // end class HourlyEmployee


   public class CommissionEmployee : Employee {
      private decimal grossSales; // gross weekly sales
      private decimal commissionRate; // commission percentage

      // five-parameter constructor
      public CommissionEmployee (string first, string last, string ssn,
         decimal sales, decimal rate) : base(first, last, ssn) {
         GrossSales = sales; // validate gross sales via property
         CommissionRate = rate; // validate commission rate via property
      } // end five-parameter CommissionEmployee constructor

      // property that gets and sets commission employee's gross sales
      public decimal GrossSales {
         get {
            return grossSales;
         } // end get
         set {
            if (value >= 0)
               grossSales = value;
            else
               throw new ArgumentOutOfRangeException(
                  "GrossSales", value, "GrossSales must be >= 0");
         } // end set
      } // end property GrossSales

      // property that gets and sets commission employee's commission rate
      public decimal CommissionRate {
         get {
            return commissionRate;
         } // end get
         set {
            if (value > 0 && value < 1)
               commissionRate = value;
            else
               throw new ArgumentOutOfRangeException("CommissionRate",
                  value, "CommissionRate must be > 0 and < 1");
         } // end set
      } // end property CommissionRate

      // calculate earnings; override abstract method Earnings in Employee
      public override decimal GetPaymentAmount () {
         return CommissionRate * GrossSales;
      } // end method Earnings              

      // return string representation of CommissionEmployee object
      public override string ToString () {
         return string.Format("{0}: {1}\n{2}: {3:C}\n{4}: {5:F2}",
            "commission employee", base.ToString(),
            "gross sales", GrossSales, "commission rate", CommissionRate + "\nTotal: " + this.GetPaymentAmount());
      } // end method ToString 
   }

   // ----------------------------------------------------------------------------------------

   public class BasePlusCommissionEmployee : CommissionEmployee {
      private decimal baseSalary; // base salary per week

      // six-parameter constructor
      public BasePlusCommissionEmployee (string first, string last,
         string ssn, decimal sales, decimal rate, decimal salary) :
         base(first, last, ssn, sales, rate) {
         BaseSalary = salary; // validate base salary via property
      } // end six-parameter BasePlusCommissionEmployee constructor

      // property that gets and sets 
      // base-salaried commission employee's base salary
      public decimal BaseSalary {
         get {
            return baseSalary;
         } // end get
         set {
            if (value >= 0)
               baseSalary = value;
            else
               throw new ArgumentOutOfRangeException("BaseSalary",
                  value, "BaseSalary must be >= 0");
         } // end set
      } // end property BaseSalary


      // calculate earnings; override method Earnings in CommissionEmployee
      public override decimal GetPaymentAmount () {
         return BaseSalary + base.GetPaymentAmount();
      } // end method Earnings               

      // return string representation of BasePlusCommissionEmployee object
      public override string ToString () {
         return string.Format("base-salaried {0}; base salary: {1:C}",
            base.ToString(), BaseSalary + "\nTotal: " + this.GetPaymentAmount());
      } // end method ToString                                            
   } // end class BasePlusCommissionEmployee


   public class SalariedEmployee : Employee, IComparable<Employee> {
      private decimal weeklySalary;

      // four-parameter constructor
      public SalariedEmployee (string first, string last, string ssn,
         decimal salary) : base(first, last, ssn) {
         WeeklySalary = salary; // validate salary via property
      } // end four-parameter SalariedEmployee constructor

      // property that gets and sets salaried employee's salary
      public decimal WeeklySalary {
         get {
            return weeklySalary;
         } // end get
         set {
            if (value >= 0) // validation
               weeklySalary = value;
            else
               throw new ArgumentOutOfRangeException("WeeklySalary",
                  value, "WeeklySalary must be >= 0");
         } // end set
      } // end property WeeklySalary

      public int CompareTo (Employee other) {
         throw new NotImplementedException();
      }

      // calculate earnings; override abstract method Earnings in Employee
      public override decimal GetPaymentAmount () {
         return WeeklySalary;
      } // end method Earnings          

      // return string representation of SalariedEmployee object
      public override string ToString () {
         return string.Format("salaried employee: {0}\n{1}: {2:C}",
            base.ToString(), "weekly salary", WeeklySalary + "\nTotal: " + this.GetPaymentAmount());
      } // end method ToString                                      
   } // end class SalariedEmployee

}
