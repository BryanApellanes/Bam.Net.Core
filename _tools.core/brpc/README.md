# Brpc

Use Brpc to expose a class definition as a web service.

# TL;DR
Serve services:	

brpc /serve:[className] /AssemblySearchPattern:[searchPattern]

or

brpc /registries:[commaSeparatedListOfRegistryNames] /AssemblySearchPattern:[searchPattern]

### Web Service Class
```C#
// Echo.cs
[Proxy]
public class Echo
{
	public string Test(string value)
	{
		return value;
	}
}

// Application_Start in global.asax
ServiceProxySystem.Initialize();
ServiceProxySystem.Register<Echo>();
```

### Web Service Clients
In addition to automatically exposing any class that you choose as a
web service, the ServiceProxySystem will also automatically generate clients
on your behalf.

#### C# Clients
To obtain C# client code simply download the code from a running ServiceProxySystem
installation using the following path:

```
/ServiceProxy/CSharpProxies
```

You may also specify an optional namespace that the clients will be defined in

```
/ServiceProxy/CSharpProxies?namespace=My.Name.Space
```

#### JavaScript Clients
The ServiceProxySystem also generates JavaScript clients as well which
can be downloaded in a similar fashion as the C# clients.  But, the recommended way
of acquiring JavaScript clients would be to include a script tag in your pages
with the src attribute set to the JavaScript proxies path:

```html
<script src="/ServiceProxy/JSProxies"></script>
```

## Service Registries
Some class models are very complex and require dependency injection to function properly.  To support these scenarios you can define a ServiceRegistry container and serve types from it.

To define a ServiceRegistry container do the following:

- Define a class adorned with the ServiceRegistryContainer attribute
- Define a static method in the class adorned with the ServiceRegistryLoader attribute making sure to specify a registry name.

```
[ServiceRegistryContainer]
public class YourClassName
{
    [ServiceRegistryLoader("YourRegistryName")]
    public static ServiceRegistry YourMethodName()
    {
        // ... build service registry
    }
}
```

To serve your registry do the following:

```
brpc /serve:YourRegistryName
```