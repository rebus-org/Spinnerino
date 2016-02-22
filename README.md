# Spinnerino

A simple .NET console spinner thingie.

Different ways of entertaining the user while progress is progressing in console applications.



## Spinner

	using(var spinner = new Spinner())
	{
		// (...)
		// do stuff and periodically:
		spinner.SetProgress(progressPercentage);
		// (...)
	}

which will look something like this:

	Spinning on its own line...
	/ 22 %

	Spinning on its own line...
	- 35 %

	Spinning on its own line...
	\ 86 %

	Spinning on its own line...
	| 96 %

### Customizations

A `Spinner` can be customized by overriding its animation characters like this:

	var myAnimationChars = "0123456789";

	using(var spinner = new Spinner(animationCharacters: myAnimationChars))
	{
		// (...)
	}



## Progress bar

	using(var bar = new ProgressBar())
	{
		// (...)
		// do stuff and periodically:
		spinner.SetProgress(progressPercentage);
		// (...)
	}

which will look something like this:

	|===================----------------| 25 % |-----------------------------------|

	|===================================| 68 % |==========-------------------------|

	|==================================| 100 % |===================================|

### Customizations

The `ProgressBar` can have its characters customized like this:

	using(var bar = new ProgressBar(completedChar: '#')) 
	{
		// (...)
	}

which will turn it into this:

	|###################----------------| 25 % |-----------------------------------|




## Inline progress bar

	Console.Write("Doing something: ")

	using(var bar = new InlineProgressBar())
	{
		// (...)
		// do stuff and periodically:
		spinner.SetProgress(progressPercentage);
		// (...)
	}

which will look something like this:

	Doing something: [##--------] 29 %
	
	Doing something: [######----] 63 %
	
	Doing something: [##########] 100 %

### Customizations

The `InlineProgressBar` can have its characters customized like this:

	Console.Write("See plus plus: ")

	using(var bar = new InlineProgressBar(completedChar: '+', notCompletedChar: ' ', width: 20)) 
	{
		// (...)
	}

which will turn it into this:

	See plus plus: [+++++++++++++++++   ] 85 %
	

# License

[The MIT License (MIT)](http://opensource.org/licenses/MIT)