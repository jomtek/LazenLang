# **Lazen Documentation**

Lazen is an imperative, object-oriented, statically-typed and infered programming language, which offers higher-order functions, threading, and more. This language could be learned in a matter of hours.

NB: This documentation is very incomplete and is still in work. Please  be patient.

## **Summary**

 1. Main types
 2. Boolean and ternary operators
 3. Variables & constants
 4. String interpolation
 5. Functions & overloading
 6. Lambdas
 7. Flow-of-control statements
 8. Classes & interfaces
 9. Abstract types
 10. Type casting
 11. Exceptions
 
## **1. Main types**
Lazen has various main types, which are either built-in or defined/implemented in Lazen's *standard library*.

**The standard types for literal expressions**
 - `Int`, the Integer type (example: `5800`)
 - `Double` (example: `3.255`)
 - `Bool`, the Boolean type (`True`, `False`)
 - `Char` (example: `'\n'`)
 - `String` (example: `"test"`)

**Other main types**

 - `List<T>` where typevar T stands for the type of the List's elements
 - `Func(T) -> T1`, the type representing each function. After the arrow stands the return type.

## 2. Boolean and ternary operators
There are 10 boolean operators in the Lazen programming language, including one ternary operator.
Every boolean operator returns a boolean literal.

NB: `Number` is an abstract type covering both `Int` and `Double` types.
 - `expr == expr` compares two expressions
 - `expr != expr` finds out if both operands are different each other
 - `Number > Number` finds out if the left operand is greater than the right operand
 - `Number < Number` ^ is smaller
 - `Number >= Number` compares both operands and finds out if the left operand is greater or equal
 - `Number <= Number` ^ smaller or equal
 - `expr && expr` finds out if both operands are True
 - `expr || expr` finds out if at least one of the operands is True
 - `!expr`, finds `True` if expr is `False`, `False` if expr is `True`
 - `expr if expr else expr`, the ternary operator

## 3. Variables & constants
Variables are surely one of the most used features in an imperative programming language. They are static, maybe mutable, and can store values of any type.

**Usage**
 1. `var baz: T = expr`
 2. `const baz: T = expr`
 Where T represents the type of the variable.

- Constants cannot be redefined.
- Should be specified at least the type or the value of the variable.

## 4. String interpolation
String interpolation is a useful feature which can be used for string concatenation, pretty printing, and more.

It is syntactically good-looking and can be used with every type of expression, as long as the concerned type has a public `__to_string__` method, knowing that the main Lazen types have one.

**Usage:** `$expr` where `expr` stands for every expr under the condition mentionned above
**Examples**

    var baz = "world"
    var caz = 2
    "Hello, $baz !" // "Hello, world !"
    "Result: $(caz + 5)" // "Result: 7"

## 5. Functions & overloading
Lazen has higher-order functions which can be inferred and overloaded.
 The syntax for function declaration is very intuitive:
 
In the following example, `foo` takes one argument of type `Bool` and returns a value of type `Int`. 

    func foo(arg: Bool) -> Int
	    return 0
    end

 - Functions always have a return value.
 - When the return value is implicit, the compiler creates an instance of the function's return type.
 - Functions can have a direct-return value.
	 Usage: `func foo() = expr`
- Functions can take an infinite amount of arguments
	Usage: `func foo(...args: String): List<String> = args`

**Overloading**
In the following example, `foo` is being overloaded one time.

    func foo(x: Int) = "Integer !"
    func foo(x: String) = "String !"

