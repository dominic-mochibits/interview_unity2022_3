using System;
using System.Collections.Generic;
using System.Linq;

/*
 * 1. Capitalize on Funtion Names, and 
 * 
 */


#region Protected
/* 
   A member access modifier.
   A protected member is accessible within its class and by derived class instances.
*/
class A
{
    protected int x = 10;
}
class B : A
{
    static void bgetX()
    {
        var a = new A();
        var b = new B();
        var c = new C();

        //   a.x = 5; 
        /* compiler error, a is the instance of class 'A', because a.x can only be accessed within the scope
           of the class where it is defined('A' or its derived class 'B'). In this case, b is the instance of class 'B', which inherbits from
           class 'A', so b.x is valid. But a.x is not defined here and 'B' is derived class from 'A'.
        */
        b.x = 5;
        c.x = 5; // valid, because class 'C' is derived class from 'B'.
    }
}
class C : B
{
    static void cgetX()
    {
        var a = new A();
        var b = new B();
        var c = new C();

        //    a.x = 5; // compiler error a.x is not defined here and 'C' is derived class from 'B'.
        //    b.x = 5; // compiler error b.x is not defined here and 'C' is derived class from 'B'.
        c.x = 5;
    }
}
#endregion
#region Abstract-Interface-Virtual 
/*
Abstract:
    Declare 'abstract class' & 'abstract method'.
    Abstract class can't be an instance, used as a foundation for other classes.
    Abstract method have only method singnatures, with no actual method body. Enforce derived classes to implement these methods.
    Derived classes must implement abstract methods, or they must also be declared as abstract. Abstract methods ensure that specific behaviors.
 */
abstract class Shape
{
    public abstract void draw();
    public int length = 2;
    public void erase() //you can also declare a non-abstract method in abstract class.
    {
    }

}
class Triangle : Shape
{
    public override void draw() //class triangle must override shape.draw().
    {
    }

}
abstract class Square : Shape //class square didn't implement abstract methods, it declared as abstract.
{
    public abstract void count();
}
class Rectangle : Square //class rectangle must override shape.draw() and square.count()
{
    public override void draw()
    {
    }
    public override void count()
    {
    }
}
/* Interface: 
        defined using `interface` keyword. An interface can contain declarations for methods, properties,
        events, and indexers, but it cannot include fields or concrete implementation. 
        Interface members are `public` by default, there's no need to add `public` keyword.
*/
public interface IBasicInterface
{
    //    int i;                 //can't declare fields
    //    public void IMethod()  //you don't have to use `public` keyword
    //    void IMethod(){ }      //interface can't have a true implementation
    void IMethod();
    int IProperty { get; set; }
}
public interface IBasicInterface2
{
    void IMethod2();
}
public interface IBasicInterface3
{
    int IProperty3 { get; set; }
}
public class IImplementation : IBasicInterface
{
    public void IMethod()
    {
        //Console.WriteLine("Implementation of Imethod.");
    }
    public int IProperty { get; set; }
}
/* Multiple Inheritance:
        C# supports multiple inheritance through interfaces. While a class can inherit from only one base class,
        but it can implement multiple interfaces, allowing it to gain functionality from different interfaces.
*/
public class IImplementation2 : IBasicInterface2, IBasicInterface3
{
    public int IProperty3 { get; set; } //IBasicInterface3

    public void IMethod2() //IBasicInterface2
    {
        //Console.WriteLine("Implementation of Imethod2.");
    }
}
/* Virtual: 
        The keyword is used to enable polymorphism and method overrinding.
        It is used to declare a method in a base class that can be inherited and overridden by derived classes,
        but it is optional to override.
*/
class virtualClass
{
    public virtual void virtualMethod()
    {
        Console.WriteLine("This is a virtual method.");
	}
}
class derivedClass : virtualClass
{
    public override void virtualMethod()
    //The override is optional. If you didn't override, the output will be
    //"This is a virtual method."
    {
        Console.WriteLine("The virtual method had been override.");
	}
}
/*Conclusion:
Abstract:
    Inheritance is used in the context of 'is a' relationship.
    It should inherit the characteristics of its parent class, 
    and maintain the same functionality as the abstract class. 
    It cannot implement program logic and must be overridden.
Interface:
    In the context of composition, composition represents a 'has a' relationship.
    Regardless of whether the classes are similar in nature, when implementing an interface, 
    it is mandatory to implement specific methods.
Virtual:
    In the base class being inherited, 
    you can initially use the 'virtual' keyword to implement logic within a method. 
    When inheriting in the derived class, 
    you have the choice to either override this method to replace the original functionality or leave it as is.
    However, even after overriding, you can still call 'base.xxx()' to apply the original logic.
 */
public class Vehicle
{
    protected interface IVehicle //this interface declare the method `turn`, it will return string value.
    {
        public string turn(); //derived class must implement turn().

    }
    protected abstract class Car : IVehicle
    {
        public string turn()
        {
            return "turn the steering wheel.";
        }
        public virtual string Accelerate() //derived class can optional override Accelerate().
        {
            return "step on the accelerator.";
        }
        public abstract string brake(); //derived class must override brake().
    }
    protected class Bicycle : IVehicle
    {
        public string turn()
        {
            return "turn the handle.";
        }
    }
    protected class Taxi : Car
    {
        public override string brake()
        {
            return "taxi is reducing speed";
        }
    }
    protected class FlyCar : Car
    {
        public override string Accelerate()
        {
            return "turn on the fly button!";
        }
        public override string brake()
        {
            return "fly car is reducing speed";
        }
    }
    public void Run()
    {
        Bicycle bicycle = new Bicycle();
        Taxi taxi = new Taxi();
        FlyCar flycar = new FlyCar();

        //Console.WriteLine(bicycle.turn());
        //Console.WriteLine(taxi.turn());
        //Console.WriteLine(flycar.turn());

        //Console.WriteLine(taxi.Accelerate());
        //Console.WriteLine(flycar.Accelerate());

        //Console.WriteLine(taxi.brake());
        //Console.WriteLine(flycar.brake());

    }
}
#endregion
#region LINQ

class Lambda
{
    /* Lambda expression:
           is a concise synstax for representing anonymous functions.
           Commonly used like LINQ queries, delegates, event handlers, etc.

           A Lambda expression has the following basic syntax:
            (parameters) => expression

            `parameters`: The parameter list of Lambda expression, which can have zero or more parameters.
            `=>`: The Lambda operator, used to seperate the parameter list from the expression body.
            `expression`: The body of the Lambda expression, involving  operations  or processing in parameters. 
    */
    public static void lambdaExp() //get even numbers
    {
        int[] numbers = new int[] { 1, 2, 3, 4, 5 };

        var evenNumbers1 = new List<int>();
        foreach (var num in numbers)
        {
            if (num % 2 == 0)
            {
                evenNumbers1.Add(num);
            }
        }
        var evenNumbers2 = numbers.Where(n => n % 2 == 0).ToList();
        //Both `evenNumbers1` and `evenNumbers2` contains the same even numbers,
        //but the code for `evenNumbers2` is much more concise.

    }
    public static void lambdaClick() //event handler
    {
        //   button.Click += (sender, e) => { Console.WriteLine("Button clicked!"); };

    }
}
class LinqDemo
{
    /*
    Language Ingtegrated Query can allows you to query and manipulate data.
    Linq provides a set of standard query operators that can be used to query collections, databases, etc.
    Common operator:
    `From`: Declared specific list or databases.
    `Select`: Projects elements into a new shape.
    `Where`: Filters elements based on a specified condition.
    */
    public static void LinqtoObjects_Syntax()
    {
        List<int> numbers = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //Query Syntax Example
        var evenNumbers = from num in numbers // Query to find even numbers
                          where num % 2 == 0
                          select num;

        foreach (var num in evenNumbers)
        {
            Console.WriteLine(num);
        }
        //output: 2 4 6 8

        //Method Syntax Example
        var oddNumbers = numbers.Where(num => num % 2 != 0);// Query to find odd numbers
        foreach (var num in oddNumbers)
        {
            Console.WriteLine(num);
        }
        //output: 1 3 5 7 9
    }

    public static void Execute()
    {
        List<int> numbers1 = new List<int> { 1, 2, 3, 4, 5, 6 };
        var deflist = numbers1.Where(num => num % 2 == 0);
        foreach (var num in deflist)
        {
            Console.WriteLine("first Linq:" + num);
        }
        numbers1[0] = 100;
        foreach (var num in deflist)
        {
            Console.WriteLine("second Linq:" + num);
        }
        /* output: first Ling: 2 4 6 second Linq: 100 2 4 6
        Deferred Execution: This queries are lazily executed,
        they are not executed until the result is explicitly enumerated.
        When the first Linq is done, the second Linq will wait for 'foreach' funtion calling,
        so it will load the fixed number.
        */

        List<int> numbers2 = new List<int> { 1, 2, 3, 4, 5, 6 };
        var immlist = numbers2.Where(num => num % 2 == 0).ToList();
        /* 
        Immediate Execution: To force immediate execution and obtain a concrete result,
        can be used with ToArray() or ToList().
        */
        foreach (var num in immlist)
        {
            //Console.WriteLine("first Linq:" + num);
        }
        numbers2[0] = 100;

        foreach (var num in immlist)
        {
            //Console.WriteLine("second Linq:" + num);
        }
        // output: first Linq: 2 4 6 second Linq: 2 4 6
    }

}


#endregion
#region Generic
/* Generic is "not specific to a particular data type"
               add <T> to : classes, methods, fields, etc.
               allows for code reusability for different data types
*/
class basicGenericClass<T>
{
    private T Tvalue; //Declred generic field.
    public basicGenericClass(T value) //When `Main` instance this generic class, you need to declared the type.
    {
        Tvalue = value;
    }
    public T GetValue() //T in here is the type parameter of basicGenricClass, if the instance type is int,
                        //it will return int, so does string, double, etc.
    {
        return Tvalue;
    }
}
class basicGenericMethod
{
    public static int[] intArray = { 1, 2, 3 };
    public static double[] doubleArray = { 1.0, 2.0, 3.0, };
    public static string[] stringArray = { "1", "2", "3" };

    //If you want to display all the Array, you have to create three method to diplay them.
    public static void dumbArray(int[] array) //Repeat three times for int, double and string.
    {
        foreach (int item in array)
        {
			Console.Write(item);
        }
		Console.WriteLine();
    }
    //But use Generic <T>, it can solve any type you want to display, WOW!
    public static void displayArray<T>(T[] array)
    {
        foreach (T item in array)
        {
			Console.Write(item);
        }
		Console.WriteLine();
    }
}

public interface IbasicInterfaceGen<T>
//When another class inherit this interface, you have to declared the type.
{
    T getValue(); // You must implement the type of <T>.
}
class InterfaceStringDisplay : IbasicInterfaceGen<string>
{
    public string getValue()
    {
        return "This is another string.";
    }
}
class InterfaceIntDisplay : IbasicInterfaceGen<int>
{
    public int getValue()
    {
        return 100;
    }
}
class GenericConstraints_shapeMaker
{
    public GenericConstraints_shapeMaker()
    {
        Console.WriteLine("this is shapeMaker.");
    }
    class Shapemake
    {
        public Shapemake()
        {
            Console.WriteLine("shapeMaker ready.");
        }
        public object GetShape<T>() where T : new()
            //This generic method with a constraint `where T : new()`:
            //it requires that `T` must have a constructor with no arguments,
            //so that it can be instantiated using `new T()`
        {
            Console.WriteLine("generate custom shape....");
            return new T();
        }

    }
    public void GenericConstraints_start()
    {
        Console.WriteLine("generate start.");
        Shapemake maker = new Shapemake();
        maker.GetShape<Square>();
        maker.GetShape<Triangle>();
        //When you call the `GetShape<T>() method`, it displays the constructor of class.
    }
    class Square
    {
        public Square()
        {
            Console.WriteLine("this is a Square.");
        }
    }
    class Triangle
    {
        public Triangle()
        {
            Console.WriteLine("this is a Triangle");
        }
    }
}
#endregion
#region Extension
/*
Extension:
    This methods are a special type of static method that allow you to add new methods to existing types 
    without modifying the source code or those types. You can extend the functionally of built-in your own
    custom types by adding your own methods to them, without the need for inheritance or altering the source code.
*/


/*
Extension methods can be defined in any suitable namespace,
but to use them, you need to import the corresponding namespace into your code file.
e.g using {namespace}
*/
public static class StringExtensions //Extension methods must be defined in a static class.
{
    public static int CountLetters(this string input) //Extansion methods themselves must be static.
    //The first parameter of an extension method must be marked with the `this` keyword,
    //which specifies the type the extension method should be applied to.
    {
        int count = 0;
        foreach (char c in input)
        {
            if (char.IsLetter(c))
            {
                count++;
            }

        }
        return count;
    }
    public static int CountDigits(this string input)
    {
        int count = 0;
        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                count++;
            }
        }
        return count;
    }
}
// Omg extension is so beautiful...
#endregion