using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2 {

   //Delagate declaration
   public delegate int ComparerSSN (IPayable obj1, IPayable obj2);


   public class PayrollSystemTest {


      public static void Main (string[] args) {

         IPayable[] payableObjects = new IPayable[8];
         payableObjects[0] = new SalariedEmployee("John", "Smith", "111-11-1111", 700M);
         payableObjects[1] = new SalariedEmployee("Antonio", "Smith", "555-55-5555", 800M);
         payableObjects[2] = new SalariedEmployee("Victor", "Smith", "444-44-4444", 600M);
         payableObjects[3] = new HourlyEmployee("Karen", "Price", "222-22-2222", 16.75M, 40M);
         payableObjects[4] = new HourlyEmployee("Ruben", "Zamora", "666-66-6666", 20.00M, 40M);
         payableObjects[5] = new CommissionEmployee("Sue", "Jones", "333-33-3333", 10000M, .06M);
         payableObjects[6] = new BasePlusCommissionEmployee("Bob", "Lewis", "777-77-7777", 5000M, .04M, 300M);
         payableObjects[7] = new BasePlusCommissionEmployee("Lee", "Duarte", "888-88-888", 5000M, .04M, 300M);
         /*
         Console.WriteLine("Not sorted in any way yet \n");

         for (int j = 0; j < payableObjects.Length; j++) {
            Console.WriteLine("Employee new {0} is a {1}", j,
               payableObjects[j]);
         }
         */
         foreach (IPayable p in payableObjects) {
            Console.WriteLine(p + "\n");
         }
         string menuOption;
         bool menuLogic = true;

         while (menuLogic) {
            Console.WriteLine("1: Sort last name in ascending order using IComparable");
            Console.WriteLine("2: Sort pay amount in descending order using IComparer");
            Console.WriteLine("3: Sort by social security number in ascending order using a selection sort and delegate");
            Console.WriteLine("4: Exit");
            menuOption = Console.ReadLine();
            switch (menuOption) {
               case "1":
                  Array.Sort(payableObjects);
                  foreach (IPayable p in payableObjects) {
                     Console.WriteLine(p + "\n");
                  }
                  Console.WriteLine();
                  break;

               case "2":
                  Array.Sort(payableObjects, Employee.payAmountSorter());

                  foreach (IPayable p in payableObjects) {
                     Console.WriteLine(p + "\n");
                  }
                  Console.WriteLine();
                  break;

               case "3":
                  PayrollSystemTest payRoll = new PayrollSystemTest();

                  ComparerSSN ssnSort = new ComparerSSN(Employee.SortAsSSN);
                  payRoll.SelectionSort(payableObjects, ssnSort);
                 
                  break;

               case "4":
                  Console.WriteLine("Option 4 ");
                  menuLogic = false;
                  break;
            }
         }

         Console.ReadKey();
      } // end Main

      //Selection sort logic for SSN's 
      public void SelectionSort (IPayable[] items, ComparerSSN comparer) {        
         int i, j, min;
         IPayable temp;
         for (i = 0; i < items.Length - 1; i++) {
            min = i;
            for (j = i + 1; j < items.Length; j++) {
               Employee emp1 = (Employee)items[j];
               Employee emp2 = (Employee)items[min];

               if (comparer(items[j],items[min]) < 0) {
                  min = j;
               }
            }
            temp = items[i];
            items[i] = items[min];
            items[min] = temp;

         }
         foreach (IPayable p in items) {
            Console.WriteLine(p + "\n");
         }
      }

   } // end class PayrollSystemTest
}