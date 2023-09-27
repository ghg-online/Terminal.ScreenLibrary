Terminal.ScreenLibrary
======================

## Introduction

This is a library for creating a terminal screen 
with a text-based user interface.

The library was originally created to provide a
console interface for the [ghg-online](https://github.com/ghg-online/ghg-online)
game, but I believe it can be useful for other
projects as well.

## Usage

### Step 1: Install the library

You can install the [NuGet Package](https://www.nuget.org/packages/Terminal.ScreenLibrary/0.1.0-beta)
with the following command.
```
dotnet add package Terminal.ScreenLibrary --version 0.2.0-beta
```

Also, you can add package reference in 
your `.csproj` file.
```
<PackageReference Include="Terminal.ScreenLibrary" Version="0.2.0-beta" />
```

All the classes are in the `Terminal.ScreenLibrary` namespace.
```
using Terminal.ScreenLibrary;
```


### Step 2: Create a screen driver

[Terminal.ScreenLibrary](https://github.com/ghg-online/Terminal.ScreenLibrary)
is a library for creating a terminal,
but it does not provide any display implementation.
This gives you great flexibility to choose
your own display implementation.


To write a screen driver, the only thing
you need to do is to implement the `IScreenDriver` interface.
```
public interface IScreenDriver
{
    int Width { get; }
    int Height { get; }
    void Redraw();
    void Update(int x, int y, char c, Color foreground, Color background);
    void UpdateCursor(int x, int y, bool show);
}
```
As you can see, the interface is very simple.

One thing you need to remember is that `Width`
and `Height` should be constant, since screen
resizing is not supported yet.

`Update` method should updates the screen buffer, but it is not needed
to apply the changes when `Update` is called.
It is promised that when changes need to be shown,
`Redraw()` will be called.

### Step 3: Use it as you like!

Now, you can create
a `Screen` object and use it as a normal terminal.
```
var screen = new Screen(myScreenDriver, Color.White, Color.Black);
screen.HandleString("Hello, world!");
```

The library support some basic color setting escape
strings, such as `"\x1b[31m"`. Other escape strings
are not supported, but they can be detected and not shown.

## License
The library is licensed under GPLv2, the same
as [ghg-online](https://github.com/ghg-online/ghg-online)
project.

## Contribution
I believe no one would like to contribute to this
project, but if you do, you are welcome to do so.
Leave a issue or pull request, and I will check it.

## Contact
If you have any questions, you can leave an issue
or contact me via email: `nictheboy@outlook.com`