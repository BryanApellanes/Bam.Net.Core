# ServiceRegistryService

Provides a central point of management for registering and retrieving services and their implementations.

# TL;DR

- Define a class adorned with the custom attribute ServiceRegistryContainer.
- Adorn a method in that class with the ServiceRegistryLoader attribute providing a name to identify the ServiceRegistry loaded by the adorned method.
- Optionally start with an existing registry: `CoreServiceRegistryContainer.Get`