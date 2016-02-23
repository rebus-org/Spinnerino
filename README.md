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
		bar.SetProgress(progressPercentage);
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
		bar.SetProgress(progressPercentage);
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
	
## Indefinite progress bar

	Console.WriteLine("Processing data..");

	using (var bar = new IndefiniteProgressBar())
	{
		// (...)
		// do stuff and from time to time:
		bar.SetAction($"Processed {dataProcessedMb:0.00} MB");
		// (...)
	}

which will look something like this:

	|-------------------=========| Processed 0,94 MB |==========-------------------|

	|===================---------| Processed 2,62 MB |--------=====================|

	|-----=======================| Processed 3,83 MB |-----------------------------|

### Customizations

The `IndefiniteProgressBar` can have its characters customized too, and also the direction of the mover. This
can be used to neat effect to e.g. "download" something:

	var direction = IndefiniteProgressBar.ProgressDirection.RightToLeft;

    using (var bar = new IndefiniteProgressBar(moverChar: '<', backgroundChar: ' ', moverWidth: 5, direction: direction))
	{
		// (...)
		// do stuff and from time to time:
		bar.SetAction($"Downloading {downloadedDataMb:0.00} MB");
		// (...)
	}

which will look something like this:

	|                           | Downloading 0,50 MB |             <<<<<          |

	|                           | Downloading 0,81 MB |       <<<<<                |

	|                           | Downloading 1,11 MB |<<<<<                       |
	

# License

[The MIT License (MIT)](http://opensource.org/licenses/MIT)