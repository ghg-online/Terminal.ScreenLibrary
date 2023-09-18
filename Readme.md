Terminal.ScreenLibrary
======================

## Introduction

This is a library for creating a terminal screen 
with a text-based user interface.

The library was originally created to provide a
console interface for the [ghg-online](https://github.com/ghg-online/ghg-online)
game, but I believe it can be useful for other
projects as well.

----------------------
## Usage

To use the library, the only thing you need to do
is to implement the `IScreenDriver` interface.
```
public interface IScreenDriver
{
    int Width { get; }
    int Height { get; }
    void Redraw();
    void Update(int x, int y, char c, Color foreground, Color background);
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

After implementing the interface, you can create
a `Screen` object and use it!
```
var screen = new Screen(myScreenDriver, Color.White, Color.Black);
screen.HandleString("Hello, world!");
```

The library support some basic color setting escape
strings, such as `"\x1b[31m"`. Other escape strings
are not supported, but they can be detected and not shown.

-----------------
## License
The library is licensed under GPLv2, the same
as [ghg-online](https://github.com/ghg-online/ghg-online)
project.

-----------------
## Contribution
I believe no one would like to contribute to this
project, but if you do, you are welcome to do so.
Leave a issue or pull request, and I will check it.

-----------------
## Contact
If you have any questions, you can leave an issue
or contact me via email: `nictheboy@outlook.com`