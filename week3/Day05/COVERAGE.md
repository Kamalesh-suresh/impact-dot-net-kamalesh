# Week 3 — Test Coverage Summary

Ran via:

```
dotnet test week3/Day05.Tests --collect:"XPlat Code Coverage"
```

**48/48 tests passed.**

## Coverage on the "Testing Focus" scope

The task brief scoped ≥80% coverage to specific pattern implementations
(Strategy, Factory, Singleton, Repository/UoW, MathHelper,
IComparable/IComparer) rather than every class touched this week. Restricted
to exactly those classes:

| Pattern | Class(es) | Line coverage |
|---|---|---|
| Singleton | `Logger` | 100% |
| Factory | `VehicleFactory`, `VehicleFactoryBase`, `CarFactory`, `BikeFactory` | 100% |
| Strategy | `CreditCardPayment`, `UpiPayment`, `NetBankingPayment`, `ShoppingCart` | 100% |
| Repository / UoW | `StudentRepository`, `CourseRepository`, `UnitOfWork` | 100% |
| MathHelper | `MathHelper` | 100% (branch 93.75% — the one `IsPrime` even-number short-circuit not hit twice) |
| IComparable / IComparer | `Employee`, `EmployeeNameComparer` | 100% |
| **Weighted total (in-scope classes)** | | **99% (99/100 lines)** |

## Whole-repo aggregate (for context, not the target)

`dotnet test --collect` counts every class it can see across the referenced
Day02/Day03/Day05 assemblies, including code this week's brief didn't ask to
be tested: each day's top-level `Program.cs` demo script, the Observer
pattern (`StockTicker`/`Investor`/`StockTickerEvents`), Adapter/Facade
(`XmlReportAdapter`, `OrderFacade`, …), and the `Report`/`Invoice`/`Shape`
family used only to demonstrate the interface-vs-abstract write-up in
Task 3.13. None of those were in the Testing Focus list, so they pull the
raw repo-wide number down to ~43% — that number is expected and not the
one that matters here.
