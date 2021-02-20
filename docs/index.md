# Guard

## Getting Started

Install the Guard package from [NuGet](https://www.nuget.org/packages/O9d.Guard):

```
dotnet add package O9d.Guard
```

Import the Guard Extensions into your classes:

```c#
using O9d.Guard;
```

Enjoy cleaner argument checking:

```c#
public Customer(string name, PhoneNumber phone)
{
    Name = name.NotNullOrWhiteSpace(nameof(name));
    Phone = phone.NotNull(nameof(phone));
}
```

To see the full list of extensions browse the [API Docs](xref:O9d.Guard).