
# **Lazen Documentation**

Lazen is an imperative, object-oriented, statically-typed and type-inferred programming language, which offers higher-order functions and more. This language is meant to be learned in a matter of hours.

Note: This documentation is very incomplete and is still in work.

## **Summary**
 1. Essential types
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
 
## **1. Essential types**
Lazen has various essential types (which are either built-in and defined or implemented in Lazen's *standard library*).

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
Hint: With `arr: [T]`, initialize a new `List<T>` using `new List(arr)`

**Other essential types**

 - `List<T>` where T is associated with type of the first element contained in the list
 - `Func(T) => T1`, the type representing each function. After the arrow stands the function's return type.
 - `Void`, the uninstantiable type returned by default by functions to whom the compiler could not associate a return type


## 2. Boolean and ternary operators
There are 10 boolean operators in the Lazen programming language
Note: both `Int` and `Double` types inherit from the `Number` class

 - `expr == expr` compares two expressions
 - `expr != expr` finds out whether or not the operands are different from each other
 - `Number > Number` finds out if the left operand is greater than the right operand
 - `Number < Number` ^ smaller
 - `Number >= Number` ^ greater or equal
 - `Number <= Number` ^ smaller or equal
 - `expr && expr` finds out if both operands are `true`
 - `expr || expr` finds out if at least one operand is `true`
 - `!expr`, finds `true` if expr is `false`, vice-versa
 - `if (expr) expr else expr` the famous ternary operator

## 3. Variables & constants
Variables are surely one of the most used features in an imperative programming language. Lazen believes their type should be consistent. Obviously, they can be mutable or constant.

**Usage**
 1. `var baz: T = expr`
 2. `const baz: T = expr`
 Where T represents the (inferable) type of the variable
 
- Constants cannot be re-defined
- You should indicate at least the type or the value of the variable
- If no value is indicated, a new instance is created using the default constructor of the specified type

## 4. String interpolation
String interpolation is a useful feature which can be used for string concatenation, pretty-printing, and more.

It deals with any type of expression, as long as it finds a (public) `ToString` method, knowing that almost all essential Lazen types implement this method.

**Usage:** `$(expr)` where `expr` stands for every expr under the condition mentionned above
**Examples**

    var baz = "world"
    var caz = 2
    "Hello, $(baz) !" // "Hello, world !"
    "Result: $(caz + 5)" // "Result: 7"

## 5. Functions, overloading & genericity
Lazen has higher-order functions which can be overloaded.
The syntax for function declaration is very intuitive:
 
In the following example, `foo` takes one argument named `arg`, of type `Bool`, and returns a value of type `Int`. 

    func foo(arg: Bool) -> Int
    {
	    return 0
    }

 - Functions always return something
	 - When the return value is implicit, the function returns an instance of its return type
 - Functions can have a direct return value
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
Note: when no `return` statement is found on a function's body, the last expression (in this case : `result`) is chosen to be the return type

## 6. Lambdas
Lambdas are anonymous functions considered as expressions. They have no implicit return type and can't hide any instruction.

An example of a self-explanatory lambda
`func(x) -> x * 2`

Notes
 - Lambda parameters can be explicitly typed
	 - ` func(x: Int) -> x * 2`
 - Because lambdas are considered to be expressions, they can be stored in variables
	 - `var myLambda = func(x) -> x * 2`
	 - `var result: Int = myLambda(5) // 10`

## 7. Blocks
A block is considered as an instruction.
 - It is introduced and ended by curly braces
 - It introduces a new scope
	 - Values inside a scope are called "locals"
		 - Once the block has reached its end, these values are wiped from memory

Example

    {
    println("hello, world")
    }

## 8. Flow-of-control statements (parsing rules)
Note: in the following examples, `instruction` is to be replaced by either a block or any instruction 
### 1.  If statement
`if expression instruction`

### 2. While loop
`while expression instruction`
 - Can be broken or continued at any moment
	 - `break`
	 - `continue`

### 3. For loop
`for x in array instruction`
Tip: use the `range` function for a "classical" a to b iteration
 - Can be broken or continued at any moment
	 - `break`
	 - `continue`

## 9. Namespaces
A program is constitued of at least one namespace which contains functions and classes. A root namespace contains the `Program` class, itself containing a public static `Main` method defined by `[string] -> Int`. <br>Here you find the program's entry-point.

Thus, a valid Lazen program looks like below

    namespace Project
    {
	    class Program
		{
		    static func Main([string] args)
			{
				// ...
				0
			}
		}
    }

 - Public functions are marked PascalCase
   - There is no `public` keyword  : every object behaves as public by default
   - Note that the `private` keyword is usable

<br>
In order to access objects from a specific distant namespace - in this example, the `Abs` function from the `Math` namespace - you need to use the same syntax as when you try to access a member from some class

    Math.Abs(x, y)

## 10. Classes
A class is a useful structure which stands behind each Lazen type. Like in all programming languages, it can be instantiated. Classes' members can be either public or private - they're public by default and that is a reason why the `public` keyword does not exist in Lazen.

A class is implemented this way - here, a simple class with a constructor, initializing the `Number` public integer

    class MyClass
    {
	    var Number: Int
	    constructor (number)
		{
			this Number = number
		}
    }

Notes

 - Constructors are introduced using the `constructor` keyword, and can be overloaded
 - Anywhere in the class, the usage of the `this` keyword helps the compiler figure out which object is being referred to

**Private members**
A class can have private members, which are only accessible inside the scope if the concerned class.
Just add the `private` keyword before any variable or function declaration.

**Static members**
A class can also have static members (which includes static functions). Static members can be either public or private, and, if private, can only be used by other static members from the same class if private, or, if public, can also be used directly from an external class.
Just add the `static` keyword before any variable or function declaration.

**Generic classes**
Classes can hold type variables. And the syntax is pretty much intuitive.

    class MyClass2<T>
    {
	    var Object: T
		constructor(object)
		{
			this Object = object
		}
    }

**Initialization**
You may want to intialize a new object from your class. Do it the following way.

    var myObject: MyClass2<string> = new MyClass2("hello")
    
Note that between the parentheses stand the arguments passed to the class' constructor.

**Member access**
Whenever your object is instantiated, you are able to access its public members.

    myObject.Object
Note: If the member is marked `public` **and** `static`, you can access it using `ClassName.Object`

## 11. Type casting
## 12. Exceptions