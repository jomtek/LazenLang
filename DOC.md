
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
 9. Classes
 10. Type casting
 11. Exceptions
 
## **1. Main types**
Lazen has various main types, which are either built-in or defined/implemented in Lazen's *standard library*.

**Atom types for literal expressions**
 - `Int`, the Integer type (example: `5800`)
 - `Double` (example: `3.255`)
 - `Bool`, the Boolean type (`true`, `false`)
 - `Char` (example: `'\n'`)
 - `String` (example: `"test"`)

**Other essential types**

 - `List<T>` where typevar T stands for elements' type
 - `Func(T) -> T1`, the type representing each function. After the arrow stands the return type.

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
Lambdas, are anonymous functions considered as expressions, which have no implicit return type and which can't hide any instruction.

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