# **Lazen Documentation**

Lazen is an imperative, object-oriented, statically-typed and infered programming language, which offers higher-order functions, threading, and more. This language could be learned in a matter of hours.

Note: This documentation is very incomplete and is still in work.

## **Summary**
 1. Main types
 2. Boolean and ternary operators
 3. Variables & constants
 4. String interpolation
 5. Functions, overloading & genericity
 6. Lambdas
 7. Blocks
 8. Flow-of-control statements
 9. Namespaces
 10. Classes
 11. Type casting
 12. Exceptions
 
## **1. Main types**
Lazen has various main types, which are either built-in or defined/implemented in Lazen's *standard library*.

**Atom types for literal expressions**
 - `Int`, the Integer type (example: `5800`)
 - `Double` (example: `3.255`)
 - `Bool`, the Boolean type (`true`, `false`)
 - `Char` (example: `'\n'`)
 - `String` (example: `"test"`)

**Arrays**
Lazen has a native type describing an array

 - Example of an array of `Int`
	 - `[1, 2, 3]` 
 - Example of a method taking an array of `Int` as parameter
	-  `func foo([Int] arr) {}`

Note: Arrays are immutable. Lists aren't.
Initialize a list using `new List(arr)`.

**Other essential types**

 - `List<T>` where typevar T stands for elements' type
 - `Func(T) => T1`, the type representing each function. After the arrow stands the return type.
 - `Void`, the uninstantiable type returned by default when a function doesn't have any codomain or explicit return type


## 2. Boolean and ternary operators
There are 10 boolean operators in the Lazen programming language
Note: both types `Int` and `Double` inherit from the `Number` class
 - `expr == expr` compares two expressions
 - `expr != expr` finds out if operands are different each other
 - `Number > Number` finds out if the left operand is greater than the right operand
 - `Number < Number` ^ is smaller
 - `Number >= Number` compares both operands and finds out if the left operand is greater or equal
 - `Number <= Number` ^ smaller or equal
 - `expr && expr` finds out if both operands are `true`
 - `expr || expr` finds out if at least one operand is `true`
 - `!expr`, finds `True` if expr is `False`, vice-versa
 - `if (expr) expr else expr`, use it as a ternary operator

## 3. Variables & constants
Variables are surely one of the most used features in an imperative programming language. They are static, they can be mutable, and they store values of any type.

**Usage**
 1. `var baz: T = expr`
 2. `const baz: T = expr`
 Where T represents the (inferable) type of the variable
 
- Constants cannot be redefined.
- You should indicate at least the type or the value of the variable

## 4. String interpolation
String interpolation is a useful feature which can be used for string concatenation, pretty-printing, and more.

It deals with any type of expression, as long as it finds a (public) `ToString` method, knowing that essential Lazen types implement this method.

**Usage:** `$(expr)` where `expr` stands for every expr under the condition mentionned above
**Examples**

    var baz = "world"
    var caz = 2
    "Hello, $(baz) !" // "Hello, world !"
    "Result: $(caz + 5)" // "Result: 7"

## 5. Functions, overloading & genericity
Lazen has higher-order functions which can be overloaded.
The syntax for function declaration is very intuitive:
 
In the following example, `foo` takes one argument of type `Bool` and returns a value of type `Int`. 

    func foo(arg: Bool) -> Int
    {
	    return 0
    }

 - Functions always return something
	 - When the return value is implicit, the function returns an instance of its return type
 - Functions can have a direct-return value
	 - Usage: `func foo() = expr`

**Overloading**
In the following example, `foo` is being overloaded

    func foo(x: Int) = "Integer !"
    func foo(x: String) = "String !"
    
    foo(5) // "Integer !"
    foo("a") // "String !"
 

**Genericity**
Functions can hold multiple type variables

    func reverse<T>(x: List<T>) -> List<T>
    {
	    List<T> result
	    for i in range(x.Len(), 0) result.Add(x[i])
	    result
    }

## 6. Lambdas
Lambdas are anonymous functions considered as expressions. They have no implicit return type and which can't hide any instruction.

An example of a self-explanatory lambda :
`func(x) -> x * 2`

Notes
 - Lambda parameters can be explicitly typed
	 - ` func(x: Int) -> x * 2`
 - Lambdas are **considered as expressions**, they can be stored in variables
	 - `var myLambda = func(x) -> x * 2`
	 - `var result: Int = myLambda(5) // 10`

## 7. Blocks
A block is considered as an instruction.

 - It is introduced and ended by curly braces
 - It introduces a new scope
	 - Values inside a scope are called "locals"
		 - They're cleared by the garbage collector once the block has reached its end

Example

    {
    println("hello, world")
    }

## 8. Flow-of-control statements
Note: in the examples, `instruction` is to be replaced by either a block or any instruction 
### 1.  If statement
`if expression instruction`

### 2. While loop
`while expression instruction`
 - Can be broken are continued at any moment
	 - `break`
	 - `continue`

### 3. For loop
`for x in array instruction`
Tip: use the `range` function for "classical" a to b iteration
 - Can be broken are continued at any moment
	 - `break`
	 - `continue`

## 9. Namespaces
In Lazen, a program is constitued of at least one namespace, which contains functions, classes and members.
The main obligatory namespace is `Program`. It contains the `main` int-returning function, which represents the program's entry-point.

Thus, a Lazen program should normally start like this :

    namespace Program
    {
	    func Main()
		{
			// ...
			0
		}
    }

 - Note that the function's name isn't `main` but `Main` : it is a public function ; public functions are PascalCase
   - There is no `public` keyword in this language; each object is public by default.
   - Note that the `private` keyword is usable

For accessing objects from specific namespace - in this example, the Abs function from the `Math` namespace - you need to use the same syntax as when you try to access a member of some class :

    Math.Abs(x, y)

## 10. Classes
A class is a useful structure which stand behind each Lazen type. Like in all programming languages, it can be instantiated. When writing a class, you should think about its interface - which way will the objects from your class be used ?

Thus, classes' members can be either public or private - they're public by default and that's why the `public` keyword doesn't exist in Lazen.

A class is implemented this way - here, a simple class with a constructor, initializing the `Number` public integer -

    class MyClass
    {
	    var Number: Int
	    constructor(number)
		{
			this Number = number
		}
    }

Notes

 - Constructors are introduced using the `constructor` keyword
 - In the constructor, the usage of the `this` keyword helps figuring out which object we're initializing. In some situations, this keyword must necessarily be used. Note the space after `this`.

**Private members**<br>
A class can have private members, which are only accessible inside the scope if the concerned class.
Just add the `private` keyword before any variable or function declaration.

**Generic classes**<br>
Classes can hold typevariables. And the syntax is pretty much intuitive

    class MyClass<T>
    {
	    var Object: T
		constructor(object)
		{
			this Object = object
		}
    }

**Initialization**<br>
You may want to intialize a new object from your class. Do it this way

    var myObject = new MyClass("hello")
    
Note that between the parentheses stand the arguments passed to the class' constructor.

**Member access**<br>
Whenever your object is instantiated, you are able to access its public members.

    myObject.Object
