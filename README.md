# EmBER+ provider lib
A library that gives you the possibility to create an EmBER+ provider.

# How to use?
1. Add the NuGet package to your solution
2. Initiate the EmBER+ provider tree
    ```csharp
        // Initiate EmBER+ tree
        var _emberTree = new EmberPlusProvider(
            9000,
            "RootName",
            "Root description field");

        _emberTree.CreateIdentityNode(
            1,
            "Identity",
            "Product Name",
            "Company Name",
            "v1.0.0");

        // General utility node
        var utilityNode = _emberTree.AddChildNode("Utilities");
        utilityNode.AddStringParameter(1, _emberTree, false, Environment.MachineName);

    ```
3. Rock on with your creations

License
=======
The base provider is using a library 'EmberLib.net' from Lawo GmbH.
```
EmberLib.net -- .NET implementation of the Ember+ Protocol
Copyright (c) 2012-2019 Lawo GmbH (http://www.lawo.com).
Distributed under the Boost Software License, Version 1.0.
```
There has been some modifications to the source code for .NET Standard adaption.
And we are not running on the latest commit. There is something not working with the
101 communication implementation. The files in the library that are using an
older version is marked in the header with 'XXX'.

```
NuGet Ember Provider Library
Copyright (c) 2021 Roger Sandholm & Fredrik Bergholtz, Stockholm, Sweden
The code is licensed under the BSD 3-clause license.
```

## Responsible maintainer
- [Team Unicorn](mailto:teamunicorn@sr.se)
