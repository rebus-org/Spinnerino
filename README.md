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

# License

[The MIT License (MIT)](http://opensource.org/licenses/MIT)