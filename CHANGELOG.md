# Changelog

## 1.0.0
* Made Spinnerino

## 1.0.1
* Added inline progress bar

## 1.0.2
* Added indefinite progress bar

## 1.0.3
* Add indefinite spinner

## 1.0.4
* Add console lock for during console writes on separate threads - thanks [davenewza]

## 2.0.0
* Target .NET Standard 2.0

## 3.0.0
* Target .NET 10
* Don't crash when console input or output is redirected - when output is redirected, nothing is animated, and only the final state is written
* Fix race conditions where animation frames could be written after (or interleaved with) the final frame when disposing
* Fix `IndefiniteProgressBar` freezing after a while when using `ProgressDirection.RightToLeft`
* Fix full-width bars wrapping/scrolling when written in the last console column, and adapt to the console window being resized
* Don't depend on reading the cursor position while animating
* Validate arguments (empty animation character sequences, non-positive widths) and treat `NaN` progress as 0
* Disposing more than once is now harmless
* Nullable annotations, XML docs, and NuGet package metadata

[davenewza] - https://github.com/davenewza