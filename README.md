# The Lazen Programming Language
Lazen is an imperative, object-oriented programming language, which supports type-inference. It is meant to be "100% handmade" : it uses no lexing/parsing/compilation library.

[Click here to access Lazen documentation](https://github.com/Jomtek/LazenLang/blob/master/DOC.md)

# Code examples
**Hello world**

    class Program
    {
	    func Main()
	    {
	    	println("Hello, world!")
	    	0
	    }
	}

**Bubblesort algorithm**

    func BubbleSort<T>(list: List<T>) -> List<T>
    {
    	var n = list.Len()
    	for i in range(0, n-1)
    		for j in range(0, n-i-1)
    			if arr[j] > arr[j + 1]
    			{
    				arr[j] = arr[j + 1]
    				arr[j + 1] = arr[j]
    			}
    }

# Project evolution

 - [x] Lexing
 - [ ] Parsing
 - [ ] Typechecking
 - [ ] IR code generation
	 - [ ] Optimisation
 - [ ] ASM or bytecode generation