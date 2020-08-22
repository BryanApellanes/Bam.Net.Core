# BamDb Shell and ArgZero
The BamDb command line tool adopts a convention based shell class method delegation paradigm enabling pattern based interaction with .net classes from the command line.

TODO: Document ShellGenProvider (generator for bam shell and argzero)

To execute a bam shell provider method follow this pattern:

```bash
bamdb [methodName] [concreteProviderName]
```

For example to execute the 'generate' method on the SchemaProvider class execute the following:

```bash
bamdb generate schema
```

The method implementation should handle acquisition of additional data in a way appropriate to the implementation, for example, using the "/argName:argValue" parsed command line arguments. 

## Components
There are three primary components used to delegate command line input to class method execution.  The provider, the delegator and the implementation. 

### Provider
The provider is an abstract base class defining the methods supported by the shell delegation definition.  Each method intended for delegation must have the following signature: 

```c#
(Action<string> output = null, Action<string> error = null)
```

### Delegator
The delegator is responsible for the instantiation of the requested implementation and the execution of the requested method.

### Implementation
The implementation is a concrete class that implements the abstract methods defined by the provider.

