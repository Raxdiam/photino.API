# Photino.API
API for performing back-end logic via [Photino.NET](https://github.com/tryphotino/photino.NET) and .NET 5 from the font-end.

[![NuGet](https://img.shields.io/nuget/v/Photino.API.svg)](https://img.shields.io/nuget/v/Photino.API.svg)

## Install and Use
Install Photino.API via NuGet using your IDE, package manager, the .NET CLI, or which ever other method of installing NuGet packages you prefer.

After installing, you will need to register the API with a `PhotinoWindow`

```c#
// Program.cs

[STAThread]
private static void Main(string[] args)
{
    var api = new PhotinoApi();
    var window = new PhotinoWindow()
        .SetTitle("Example")
        .RegisterApi(api)
        .Load("wwwroot/index.html");
    window.WaitForClose();
}
```

### photino.js
The point of Photino.API is to make it possible to write most your application logic on your front-end.
To do this, you will need [photino.js](https://github.com/Raxdiam/photino.js).

**Install as an NPM module**
```
npm i photino.js
```

**Usage**
```js
import { Photino } from 'photino.js';

(async function() {
  await Photino.window.setTitle('Custom Title');
})();
```

...or

**As a UMD module** (download [here](https://unpkg.com/photino.js))
```html
<script src="https://unpkg.com/photino.js"></script>
```
**Usage**
```js
const Photino = PhotinoAPI.Photino;

(async function() {
  await Photino.window.setTitle('Custom Title');
})();
```

# The API
The API is broken up into "modules" which act as categories for "methods".

## Modules

### Window
`Photino.window`

| Method                                                   | Description                                    |
| -------------------------------------------------------- | ---------------------------------------------- |
| `getTitle(): Promise<string>`                            | Get the window title                           |
| `getMaximized(): Promise<boolean>`                       | Get whether or not the window is maximized     |
| `getMinimized(): Promise<boolean>`                       | Get whether or not the window is minimized     |
| `getDevToolsEnabled(): Promise<boolean>`                 | Get whether or not dev tools are enabled       |
| `getContextMenuEnabled(): Promise<boolean>`              | Get whether or not the context menu is enabled |
| `getTopMost(): Promise<boolean>`                         | Get whether or not the window is top-most      |
| `getChromeless(): Promise<boolean>`                      | Get whether or not the window is chromeless    |
| `getFullScreen(): Promise<boolean>`                      | Get whether or not the window is full-screen   |
| `getResizable(): Promise<boolean>`                       | Get whether or not the window is resizable     |
| `getWidth(): Promise<number>`                            | Get the width of the window                    |
| `getHeight(): Promise<number>`                           | Get the height of the window                   |
| `getTop(): Promise<number>`                              | Get the top location of the window             |
| `getLeft(): Promise<number>`                             | Get the left location of the window            |
| `setTitle(title: string): Promise<void>`                 | Set the window title                           |
| `setMaximized(maximized: boolean): Promise<void>`        | Set whether or not the window is maximized     |
| `setMinimized(minimized: boolean): Promise<void>`        | Set whether or not the window is minimized     |
| `setDevToolsEnabled(enabled: boolean): Promise<void>`    | Set whether or not dev tools are enabled       |
| `setContextMenuEnabled(enabled: boolean): Promise<void>` | Set whether or not the context menu is enabled |
| `setTopMost(topMost: boolean): Promise<void>`            | Set whether or not the window is top-most      |
| `setChromeless(chromeless: boolean): Promise<void>`      | Set whether or not the window is chromeless    |
| `setFullScreen(fullScreen: boolean): Promise<void>`      | Set whether or not the window is full-screen   |
| `setResizable(resizable: boolean): Promise<void>`        | Set whether or not the window is resizable     |
| `setWidth(width: number): Promise<void>`                 | Set the width of the window                    |
| `setHeight(height: number): Promise<void>`               | Set the height of the window                   |
| `setTop(top: number): Promise<void>`                     | Set the top location of the window             |
| `setLeft(left: number): Promise<void>`                   | Set the left location of the window            |
| `close(): Promise<void>`                                 | Close the window                               |
| `load(path: string): Promise<void>`                      | Load an HTML document via URL or file path     |
| `loadRawString(content: string): Promise<void>`          | Load a raw string                              |
| `center(): Promise<void>`                                | Center the window                              |

Windows Only:

| Method                           | Description                                    |
| -------------------------------- | ---------------------------------------------- |
| `hitTest(hitTest: string): void` | Perform a hit-test on the window.<br />Hit-Tests: `drag`, `n`, `s`, `e`, `w`, `ne`, `nw`, `se`, `sw`
| `drag(): void`                   | Perform the `drag` hit-test (`HTCAPTION`)      |
| `resizeTopLeft(): void`          | Perform the `nw` hit-test (`HTTOPLEFT`)        |
| `resizeTop(): void`              | Perform the `n` hit-test (`HTTOP`)             |
| `resizeTopRight(): void`         | Perform the `ne` hit-test (`HTTOPRIGHT`)       |
| `resizeRight(): void`            | Perform the `e` hit-test (`HTRIGHT`)           |
| `resizeBottomRight(): void`      | Perform the `se` hit-test (`HTBOTTOMRIGHT`)    |
| `resizeBottom(): void`           | Perform the `s` hit-test (`HTBOTTOM`)          |
| `resizeBottomLeft(): void`       | Perform the `sw` hit-test (`HTBOTTOMLEFT`)     |
| `resizeLeft(): void`             | Perform the `w` hit-test (`HTLEFT`)            |

Hit-Test Example:
```js
const elDragArea = document.getElementById('drag-area');

elDragArea.addEventListener('mousedown', function (ev) {
  Photino.window.drag();
  ev.preventDefault();
  ev.stopPropagation();
})
```

* All hit-test methods are synchronous
* More about Windows hit-tests [here](https://docs.microsoft.com/en-us/windows/win32/inputdev/wm-nchittest)

* Hit-tests must be enabled on the .NET end of your app:
    ```c#
    // Program.cs
  
    [STAThread]
    private static void Main(string[] args)
    {
        var api = new PhotinoApi()
            .SetHandleHitTest(true);
        ...
    }
    ```


### IO
`Photino.io`

| Method                                                                                                   | Description                 |
| -------------------------------------------------------------------------------------------------------- | --------------------------- |
| `readFile(path: string): Promise<Uint8Array>`                                                            | Read data of a file         |
| `readFileText(path: string, encoding: Encoding = null): Promise<string>`                                 | Read all text in a file     |
| `readFileLines(path: string, encoding: Encoding = null): Promise<string[]>`                              | Read all lines in a file    |
| `writeFile(path: string, data: Uint8Array): Promise<void>`                                               | Write data to a file        |
| `writeFileText(path: string, contents: string, encoding: Encoding = null): Promise<void>`                | Write all text to a file    |
| `writeFileLines(path: string, contents: string[], encoding: Encoding = null): Promise<void>`             | Write all lines to a file   |
| `listFiles(path: string, searchPattern: string = null, recursive: boolean = false): Promise<string[]>`   | List files in a folder      |
| `listFolders(path: string, searchPattern: string = null, recursive: boolean = false): Promise<string[]>` | List folders in a folder    |
| `createFolder(path: string): Promise<void>`                                                              | Create a folder             |
| `deleteFile(path: string): Promise<void>`                                                                | Delete a file               |
| `deleteFolder(path: string, recursive: boolean = false): Promise<void>`                                  | Delete a folder             |
| `fileExists(path: string): Promise<boolean>`                                                             | Check if a file exists      |
| `folderExists(path: string): Promise<boolean>`                                                           | Check if a folder exists    |
| `resolvePath(path: string): Promise<string>`                                                             | Resolve to an absolute path |
| `getExtension(path: string): Promise<string>`                                                            | Get the extension of a file |


### OS
`Photino.os`

| Method                                   | Description                                                  |
| ---------------------------------------- | ------------------------------------------------------------ |
| `isWindows(): Promise<boolean>`          | Whether or not the current operating system is Windows       |
| `isLinux(): Promise<boolean>`            | Whether or not the current operating system is Linux         |
| `isMacOs(): Promise<boolean>`            | Whether or not the current operating system is MacOS         |
| `getEnvar(key: string): Promise<string>` | Get the value of an environment variable                     |
| `cmd(command: string): Promise<string>`  | Execute a command using the operating system's default shell |


### App
`Photino.app`

| Method                                  | Description                                     |
| --------------------------------------- | ----------------------------------------------- |
| `exit(exitCode: number): Promise<void>` | Exit the application with a given exit code     |
| `cwd(): Promise<string>`                | Get the application's current working directory | 
