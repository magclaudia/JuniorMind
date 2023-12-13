using Linq;
using Xunit;

namespace LinqFacts
{
    public class LinqMethodsFacts
    {

        [Fact]
        public void Method_All_Return_ArgumentNullException()
        {
            var elements = new [] { 2, -5, 31 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.All<int>(null, element => element.Equals(20)));
            Assert.Throws<ArgumentNullException>(() => LinqMethods.All(elements, null));
        }

        [Fact]
        public void Method_All_Return_True_WhenAllElementsFulfillTheRequirement()
        {
            var elements = new[] { "sun", "summer", "sara" };
            Assert.True(LinqMethods.All(elements, element => element.StartsWith("s")));
        }

        [Fact]
        public void Method_All_Return_False_WhenNotAllElementsFulfillTheRequirement()
        {
            var elements = new[] { "sun", "summer", "Sara" };
            Assert.False(LinqMethods.All(elements, element => element.StartsWith("s")));
        }

        [Fact]
        public void Method_Any_Return_ArgumentNullException()
        {
            var elements = new[] { 2, -5, 31 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Any<string>(null, elelemnt => elelemnt.StartsWith('s')));
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Any(elements, null));
        }

        [Fact]
        public void Method_Any_Return_True_WhenFindAnyElementFulfillTheRequirement()
        {
            var elements = new[] { 7, 2, 0 };
            Assert.True(LinqMethods.Any(elements, element => element > 3));
        }

        [Fact]
        public void Method_Any_Retun_False_WhenDontFindAnyElementThatFulfillTheRequirement()
        {
            var elements = new[] { 7, 0, 25 };
            Assert.False(LinqMethods.Any(elements, element => element < 0));
        }

        [Fact]
        public void Method_First_Return_ArgumentNullException()
        {
            var elements = new[] { 2, -5, 31 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.First<int>(null, element => element > 2));
            Assert.Throws<ArgumentNullException>(() => LinqMethods.First(elements, null));
        }

        [Fact]
        public void Method_First_ReturnFistElementThatFulfillTheRequirement()
        {
            var elements = new[] { 2, -5, 31 };
            Assert.Equal(31, LinqMethods.First(elements, element => element > 2));
        }

        [Fact]
        public void Method_First_ReturnInvalidOperationExceptionWhenNoItemsMatchedThePredicate()
        {
            var elements = new[] { 2, 5, 4 };
            Assert.Throws<InvalidOperationException>(() => (LinqMethods.First(elements, element => element < 0)));
        }

        [Fact]
        public void Method_Select_Return_ArgumentNullException()
        {
            var elements = new[] { 2, 5, 4 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Select<int, int>(null, element => element).GetEnumerator().MoveNext());
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Select<int, int>(elements, null).GetEnumerator().MoveNext());
        }

        [Fact]
        public void Method_Select_Return_TrueIfElementFulfillTheRequirement()
        {
            var elements = new[] { 2, -5, 3 };
            var result = new[] { 4, -10, 6 };
            Assert.Equal(result, LinqMethods.Select(elements, element => element * 2));
        }

        [Fact]
        public void Method_Select_ReturnFalseIfElementDontFulfillTheRequirement()
        {
            var elements = new[] { 2, -5, 3 };
            var result = new[] { 4, -10, 6 };
            Assert.NotEqual(result, LinqMethods.Select(elements, element => element / 2));
        }

        [Fact]
        public void Method_SelectMany_ReturnArgumentNullException()
        {
            var elements = new List<List<string>> { new List<string> { "dumitru", "doru" }, new List<string> { "drum", "dunare", "dumbrava" } };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.SelectMany<List<string>, string>(null, element => element).GetEnumerator().MoveNext());
            Assert.Throws<ArgumentNullException>(() => LinqMethods.SelectMany<List<string>, string>(elements, null).GetEnumerator().MoveNext());
        }

        [Fact]
        public void Method_SelectMany_ReturnTrueIfElementFulfillTheRequirement()
        {
            var elements = new List<List<string>> { new List<string> { "dumitru", "doru" }, new List<string> { "drum", "dunare", "dumbrava" } };
            var result = new List<string> { "dumitru", "doru", "drum", "dunare", "dumbrava" };
            Assert.Equal(result, LinqMethods.SelectMany(elements, element => element));
        }

        [Fact]
        public void Method_SelectMany_ReturnTFalseIfElementFulfillTheRequirement()
        {
            var elements = new List<List<string>> { new List<string> { "dumitru", "doru" }, new List<string> { "drum", "dunare", "dumbrava" } };
            var result = new List<string> { "dumitru", "doru", "drum", "dunare" };
            Assert.NotEqual(result, LinqMethods.SelectMany(elements, element => element));
        }

        [Fact]
        public void Method_Where_ReturnArgumentNullException()
        {
            var elements = new List<int> { 2, 7, 6, -10 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Where<int>(null, element => element.Equals(21)).GetEnumerator().MoveNext());
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Where<int>(elements, null).GetEnumerator().MoveNext());
        }

        [Fact]
        public void Method_Where_ReturnTrueifElementFulfillTheRequirement()
        {
            var elements = new List<int> { 2, 4, -4, 13, 11, 6 };
            var result = new List<int> { 2, 4, -4, 6 };
            Assert.Equal(result, LinqMethods.Where(elements, element => element % 2 == 0));
        }

        [Fact]
        public void Method_Where_Return_FalseifElementDontFulfillTheRequirement()
        {
            var elements = new List<int> { 2, 4, -4, 13, 11, 6 };
            var newElements = LinqMethods.Where(elements, element => element % 2 == 0);
            var verification = new List<int> { 2, -4, 6, 11 };
            Assert.NotEqual(verification, newElements);
        }

        [Fact]
        public void Method_ToDictionary_ReturnArgumentNullExcetion()
        {
            var elements = new List<Employee> { new Employee { SerialNumber = 10547, Name = "Pop Ioan", Occupation = "manager" },
                 new Employee { SerialNumber = 98974, Name = "Muresan Maria", Occupation = "engineering"},
                 new Employee { SerialNumber = 4648798, Name = "Doru Ana", Occupation = "teacher" } };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.ToDictionary<int, string, string>(null, element => element.ToString(), element => element.ToString()));
        }

        [Fact]
        public void Method_ToDictionary_ReturnTrueIfElementFulfillTheRequirement()
        {
            var elements = new List<Employee>{ new Employee { SerialNumber = 10547, Name = "Pop Ioan", Occupation = "manager" },
                new Employee { SerialNumber = 98974, Name = "Muresan Maria", Occupation = "engineering" },
                new Employee { SerialNumber = 4648798, Name = "Doru Ana", Occupation = "teacher" } };

            var dictionary = LinqMethods.ToDictionary(elements, element => element.SerialNumber, element => element);
            foreach (var element in elements)
            {
                Assert.True(dictionary.ContainsKey(element.SerialNumber));
                Assert.Equal(element, dictionary[element.SerialNumber]);
            }
        }

        [Fact]
        public void Method_Zip_ReturnArgumentNullExcetion()
        {
            var first = new List<int> { 1, 2, 3, 4 };
            var second = new List<string> { "unu", "doi", "trei", "patru" };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Zip<int, string, string>(null, second, (firstSeq, secondSeq) => firstSeq + " " + secondSeq).GetEnumerator().MoveNext());
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Zip<int, string, string>(first, null, (firstSeq, secondSeq) => firstSeq + " " + secondSeq).GetEnumerator().MoveNext());
        }

        [Fact]
        public void Method_Zip_ReturnTrueIfElementFulfillTheRequirement()
        {
            var first = new List<int> { 1, 2, 3, 4 };
            var second = new List<string> { "unu", "doi", "trei", "patru" };
            var resultExpected = new List<string> { "1 unu", "2 doi", "3 trei", "4 patru" };
            Assert.Equal(resultExpected, LinqMethods.Zip(first, second, (firstSeq, secondSeq) => firstSeq + " " + secondSeq));
        }

        [Fact]
        public void Method_Aggregate_ReturnArgumentNullExcetion()
        {
            var elements = new List<int> { 2, 3, 8, 4 };
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Aggregate<int, int>(null, 1, (a, b) => a + b));
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Aggregate(elements, 1, null));
        }

        [Fact]
        public void Method_Aggregate_ReturnTrueIfElementFulfillTheRequirement()
        {
            var elements = new List<int> { 2, -4, 10, 15 };
            Assert.Equal(25, LinqMethods.Aggregate(elements, 2, (a, b) => a + b));
        }

        [Fact]
        public void Method_Aggregate_ReturnFalseIfElementDontFulfillTheRequirement()
        {
            var elements = new List<int> { 2, 7, -1, 6 };
            Assert.NotEqual(14, LinqMethods.Aggregate(elements, 1, (a, b) => a + b));
        }

        [Fact]
        public void Method_Join_ReturnArgumentNullExcetion()
        {
            var outer = new List<Employee> { new Employee { SerialNumber = 124, Name = "Pop Ioan", Occupation = "Manager" },
                { new Employee { SerialNumber = 154, Name = "Doru Mihai", Occupation = "Hr"} }};
            var inner = new List<Salary> { new Salary { SerialNumber = 124, MonthlySalary = 1200 }, new Salary { SerialNumber = 154, MonthlySalary = 800 } };

            Assert.Throws<ArgumentNullException>(() => LinqMethods.Join<Employee, Salary, int, string>(null, inner,
                employee => employee.SerialNumber, salary => salary.SerialNumber, (employee, salary) => employee.Name + " => " + salary.MonthlySalary).GetEnumerator().MoveNext());
            
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Join<Employee, Salary, int, string>
                (outer, null, employee => employee.SerialNumber, salary => salary.SerialNumber, 
                (employee, salary) =>  employee.Name + " => " + salary.MonthlySalary ).GetEnumerator().MoveNext());
            
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Join<Employee, Salary, int, string>
                (outer, inner, null, salary => salary.SerialNumber, (employee, salary) => employee.Name + " => " + salary.MonthlySalary).GetEnumerator().MoveNext());
           
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Join<Employee, Salary, int, string>
                (outer, inner, employee => employee.SerialNumber, null, (employee, salary) => employee.Name + " => " + salary.MonthlySalary).GetEnumerator().MoveNext());
            
            Assert.Throws<ArgumentNullException>(() => LinqMethods.Join<Employee, Salary, int, string>
                (outer, inner, employee => employee.SerialNumber, salary => salary.SerialNumber, null).GetEnumerator().MoveNext());
        }

        [Fact]
        public void Method_Join_ReturnTrueIfElementFulfillTheRequirement()
        {
            var outer = new List<Employee> { new Employee { SerialNumber = 124, Name = "Pop Ioan", Occupation = "Manager" },
                { new Employee { SerialNumber = 154, Name = "Doru Mihai", Occupation = "Hr"} }};
            var inner = new List<Salary> { new Salary { SerialNumber = 124, MonthlySalary = 1200 }, new Salary { SerialNumber = 154, MonthlySalary = 800 } };

            var resultExpected = new [] { "Pop Ioan => 1200", "Doru Mihai => 800" };
                Assert.Equal(resultExpected, LinqMethods.Join<Employee, Salary, int, string>(outer, inner,
                employee => employee.SerialNumber, salary => salary.SerialNumber, (employee, salary) => employee.Name + " => " + salary.MonthlySalary));
        }

        [Fact]
        public void Method_Join_ReturnFalseIfElementDontFulfillTheRequirement()
        {
            var outer = new List<Employee> { new Employee { SerialNumber = 124, Name = "Pop Ioan", Occupation = "Manager" },
                { new Employee { SerialNumber = 154, Name = "Doru Mihai", Occupation = "Hr"} }};
            var inner = new List<Salary> { new Salary { SerialNumber = 100, MonthlySalary = 1200 }, new Salary { SerialNumber = 154, MonthlySalary = 800 } };

            var resultExpected = new[] { "Pop Ioan => 1200", "Doru Mihai => 800" };
            Assert.NotEqual(resultExpected, LinqMethods.Join<Employee, Salary, int, string>(outer, inner,
            employee => employee.SerialNumber, salary => salary.SerialNumber, (employee, salary) => employee.Name + " => " + salary.MonthlySalary));
        }
    } 

    public class Employee
    {
        public int SerialNumber { get; set; }
        public string? Name { get; set; }
        public string? Occupation { get; set; }
    }

    public class Salary
    {
        public int SerialNumber { get; set; }
        public int MonthlySalary { get; set; }
    }
}