# Blazor.IndexedDB

An easy way to interact with IndexedDB and make it feel like EF Core but `async`.

## Version history

- 1.0.1.1:
    - Upgraded from `.NET Core 3.0.0-preview` to `.NET Core 3.2.1`
    - Upgraded form `netstandard2.0` to `netstandard2.1`
    - Upgraded form `C# 7.3` to `C# 8.0`
    - Upgraded `TG.Blazor.IndexedDB` from `0.9.0-beta` to `1.5.0-preview`
    - Changed `private IndexedDBManager connector;` to `protected IndexedDBManager connector;` in `IndexedDb`
    - Changed `IndexedSet<T> : IEnumerable<T>` to `IndexedSet<T> : ICollection<T>`
- 1.0.1:
    - Original code by [Reshiru](https://github.com/Reshiru)
    - Original repository at [Blazor.IndexedDB.Framework](https://github.com/Reshiru/Blazor.IndexedDB.Framework)

## NuGet installation
```powershell
PM> Install-Package Blazor.IndexedDB
```

## Current features
- Connect and create database
- Add record
- Remove record
- Edit record

## Planned features or optimizations 
- FK implementation
- Optimize change tracker (currently using snapshotting mechanism based on using hashes)
- Query data without loading everything first (use `async` + `yield` with `IAsyncEnumerable`)
- Remove PK dependencies from IndexedSet
- Versioning (eg. merging database)
- SaveChanges should await all transactions and rollback everything within the scope if something went wrong while other data was already saved

## How to use
1. Register `IndexedDbFactory` as a service.
```CSharp
services.AddSingleton<IIndexedDbFactory, IndexedDbFactory>();
```
- `IIndexedDbFactory` is used to create the database connection and will create the database instance for you.

- `IndexedDbFactory` requires an instance of `IJSRuntime` which should normally already be registered.

2. Create any code first database model and inherit from `IndexedDb`. Only properties with the type `IndexedSet<>` will be used, any other properties will be ignored.
```CSharp
public class ExampleDb : IndexedDb
{
  public ExampleDb(IJSRuntime jSRuntime, string name, int version) : base(jSRuntime, name, version) { }
  public IndexedSet<Person> People { get; set; }
}
```
- Your model (eg. `Person`) should contain an `Id` property or a property marked with the `Key` attribute.
```CSharp
public class Person
{
  [System.ComponentModel.DataAnnotations.Key]
  public long Id { get; set; }
  public string FirstName { get; set; }
  public string LastName { get; set; }
}
```

3. Now you can start using your database.

- Usage in Razor via inject: `@inject IIndexedDbFactory DbFactory`

### Adding records
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
{
  db.People.Add(new Person()
  {
    FirstName = "First",
    LastName = "Last"
  });
  await db.SaveChanges();
}
```
### Removing records
To remove an element it is faster to use an already created reference. You should also be able to remove an object only by it's `Id` but you have to use the `.Remove(object)` method (eg. `.Remove(new object() { Id = 1 })`)
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
{
  var firstPerson = db.People.First();
  db.People.Remove(firstPerson);
  await db.SaveChanges();
}
```
### Modifying records
```CSharp
using (var db = await this.DbFactory.Create<ExampleDb>())
{
  var personWithId1 = db.People.Single(x => x.Id == 1);
  personWithId1.FirstName = "This is 100% a first name";
  await db.SaveChanges();
}
```

## License

Original [license](https://github.com/Reshiru/Blazor.IndexedDB.Framework/blob/master/LICENSE).

Licensed under the [MIT](LICENSE) license.
