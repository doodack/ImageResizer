ImageResizer
============
This is a simple image resizing class library. It consists of a single class, `ImageResizer`, with two public methods:

* `ResizeTo(int width, int? height = null)` - Resizes the image to the specified dimensions. If either height or width is not set, it preserves the aspect ratio.
* `ResizeToFit(int? maxWidth, int? maxHeight)` - Resizes the image so it is not larger than specified dimensions. Preserves the aspect ratio.

***
author: [Michał Dudak](http://dudak.pl/) | [Digital Creations](http://digitalcreations.pl)
