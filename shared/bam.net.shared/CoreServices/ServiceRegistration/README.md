# ServiceRegistryService

Provides a central point of management for registering and retrieving services and their implementations.

# TL;DR

- Define a class adorned with the custom attribute ServiceRegistryContainer.
- Adorn a method in that class with the ServiceLoader attribute
- Optionally start with an existing registry: `CoreServiceRegistryContainer.Get`