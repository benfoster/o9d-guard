---
uid: O9d.Guard.StringGuards.NotNullOrWhiteSpace(System.String,System.String)
example: [*content]
# https://github.com/dotnet/docfx/issues/1803
---

```c#
public Customer(string name)
{
    Name = name.NotNullOrWhiteSpace(nameof(name));
}
```