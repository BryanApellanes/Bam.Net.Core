# BamSvc

Use `bamsvc` to expose a class definition as a web service.  Alternatively, use `bamsvc` to expose classes in a registry, or set of registries, as web services. 

# TL;DR
Serve services:	

bamsvc /serve:[className] /AssemblySearchPattern:[searchPattern]

or

bamsvc /registries:[commaSeparatedListOfRegistryNames] /AssemblySearchPattern:[searchPattern]

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
web service, `bamsvc` will also automatically generate clients.

#### C# Clients
To obtain C# client code simply download the code from a running `bamsvc` server using the following path:

```
/ServiceProxy/CSharpProxies
```

You may also specify an optional namespace that the clients are defined in

```
/ServiceProxy/CSharpProxies?namespace=My.Name.Space
```

#### JavaScript Clients
`bamsvc` also generates JavaScript clients, which
are downloaded in a similar way as the C# clients.  The recommended way
of acquiring JavaScript clients would be to include a script tag in your pages
with the src attribute set to the JavaScript proxies path:

```html
<script src="/serviceproxy/proxies.js"></script>
```

## Service Registries
Some class models are very complex and require dependency injection to function properly.  To support these scenarios, define a ServiceRegistry container and serve types from it.

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
bamsvc /serve:YourRegistryName
```