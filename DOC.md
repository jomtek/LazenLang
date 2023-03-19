
  
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
 9. Modules
 10. Classes 
	 1. OOP (basics and genericity)
	 2. OOP (inheritance)
	 3. OOP (interfaces and their use)
 11. Type casting
 12. Exceptions

## **1. Essential types**
Lazen has various essential types (which are either built-in and defined or implemented in Lazen's *standard library*).

**Atom types for literal expressions**
 - `Int`, the Integer type (example: `580`)
 - `Double` (example: `3.255`)
 - `Bool`, the Boolean type (`true`, `false`)
 - `Char` (example: `'\n'`)
 - `String` (example: `"test"`)

**Arrays**
Lazen has a native type describing an array

 - Example of an array of `Int`
	 - `[1, 2, 3]` 
 - Example of a method taking an array of `Int` as parameter
	-  `func foo(arr: [Int]) {}`

## 2. Boolean and ternary operators
There are 10 boolean operators in the Lazen programming language

 - `expr == expr` compares two expressions
 - `expr != expr` finds out whether or not the operands are different from each other
 - `Number > Number` finds out if the left operand is greater than the right operand
 - `Number < Number` ^ smaller
 - `Number >= Number` ^ greater or equal
 - `Number <= Number` ^ smaller or equal
 - `expr && expr` finds out if both operands are `true`
 - `expr || expr` finds out if at least one operand is `true`
 - `!expr`, finds `true` if expr is `false`, vice-versa
 - `if expr expr else expr` the famous ternary operator

## 3. Variables & constants
Variables are surely one of the most used features in an imperative programming language. Lazen believes their type should be consistent.
<br>They are always mutable in Lazen.

**Usage**
 <br>`baz: T = expr`
 Where T represents the explicit type of the variable
 Explicit typing is mandatory at the moment

## 4. String interpolation
wip

## 5. Functions and genericity
Lazen has higher-order functions which cannot be overloaded.<br>
The syntax for function declaration is very intuitive:

In the following example, `foo` takes one argument named `arg`, of type `Bool`, and returns a value of type `Int`. 

    func foo(arg: Bool): Int
    {
        return 0
    }

 - Functions always return something

## 6. Lambdas
wip

## 7. Blocks
A block is considered as an instruction.
 - It is introduced and ended by curly braces
 - It introduces a new scope
	 - Values inside a scope are called "locals"
	 - Once the block has reached its end, these values are to be wiped from memory

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
Tip: use the `..` operator for a "classical" a to b iteration (ex: `for i in 0..10`)
 - Can be broken or continued at any moment
	 - `break`
	 - `continue`

## 9. Modules
wip

## 10.1 OOP (classes: basics and genericity)
### Basics
A class is a useful structure which stands behind each Lazen type.
A class is implemented the way shown below - here, a simple class with a constructor.<br>

````
class MyClass {
    number: Int = 0
    func __init__(self, number) {
    	self.number = number
    }
}
````
    	
**Member access**
Whenever your object is instantiated, you are able to access its public members.

    myObject.object

**Instantiation**
You may want to intialize a new object from your class. Do it the following way.

    myObject: MyClass = MyClass("hello")

### Genericity
wip

## 10.2 OOP (classes: how to code safe and organized)
wip

## 10.3 OOP (interfaces and their use)
wip

## 11. Type casting
wip

## 12. Exceptions
wip