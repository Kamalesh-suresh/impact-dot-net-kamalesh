# Patterns Lab — Week 3

Every pattern below was implemented as a small, runnable scenario this week.
This file is the map from "toy example now" to "where this shows up for
real" later in the curriculum (ASP.NET Core / EF Core weeks).

| Pattern | This week's implementation | Where it reappears later | Why it maps there |
|---|---|---|---|
| **Singleton** | `Logger` (week3/Day02/Program.cs) — thread-safe via `Lazy<T>`, private constructor, proven identical across `Thread`s and `Task`s. | **DI container service lifetimes** (ASP.NET Core `AddSingleton`) | ASP.NET Core's built-in container *is* Singleton-as-a-pattern, formalized: one instance per app for the lifetime of the process, same motivation (shared, expensive-to-create state like a logger or config cache) instead of a hand-rolled private static field. |
| **Factory (Simple + Factory Method)** | `VehicleFactory.CreateVehicle(type)` and `VehicleFactoryBase`/`CarFactory`/`BikeFactory` (week3/Day02/Program.cs) | **Service creation / DI registration & `IServiceProvider`** | When a controller or handler needs "the right implementation for this request" (e.g. picking a payment gateway, a notification channel), that decision moves into a factory registered with the DI container instead of a `switch` scattered through business code — same shape, now resolved through `IServiceProvider.GetRequiredService<T>()`. |
| **Observer** | `IStockObserver`/`StockTicker` and the plain-event `StockTickerEvents` (week3/Day02/Program.cs) | **Events, SignalR, and background notifications** | The custom-interface vs C# `event` comparison made this week is exactly the choice ASP.NET Core code faces: `event`-style pub/sub for in-process notifications, growing into SignalR hubs pushing state changes (price updates, order status) to connected clients — the same "subject notifies subscribers" shape, over a network. |
| **Strategy** | `IPaymentStrategy` / `ShoppingCart.SetPaymentStrategy` swapped at runtime (week3/Day03/Program.cs) | **Runtime-selected behaviour in request handling** (payment gateways, auth schemes, middleware branching) | Whenever a controller needs to pick behaviour based on runtime input (which payment provider, which auth scheme, which pricing rule) without an `if/else` ladder, it's this pattern again — injected as an `IPaymentStrategy`-shaped interface via DI instead of `new`'d directly. |
| **Repository + Unit of Work** | `IRepository<T>`/`StudentRepository`/`CourseRepository` and `IUnitOfWork`/`UnitOfWork.Save()` (week3/Day03/IRepository.cs) | **Data access layer over EF Core** | The stub `Save()` comment already says it: a real version calls `DbContext.SaveChanges()`. `IRepository<T>` becomes the seam between business logic and `DbSet<T>`, and `IUnitOfWork` becomes the boundary for one transactional `SaveChanges()` call — this is the standard EF Core repository layering. |
| **Adapter** | `XmlReportAdapter` wrapping `IXmlReportGenerator` (week3/Day03/ReportAdapter.cs) | **Third-party/external API integration** | Any time a later project wraps a vendor SDK or legacy service behind our own interface (payment gateway SDKs, external report/PDF generators, a legacy SOAP service) so the rest of the app never sees the third-party shape directly. |
| **Facade** | `OrderFacade.PlaceOrder` coordinating `InventoryService`/`PaymentService`/`ShippingService` (week3/Day03/ReportAdapter.cs) | **Application/service layer over multiple domain services** (e.g. an `OrderService` called by a controller) | Controllers in later weeks shouldn't orchestrate multiple domain services directly — a facade/application service does the coordinating so the controller stays a thin HTTP-to-service translation layer. |

## This week's interface-vs-abstract and static-vs-instance material

Not a "pattern" in the Gang-of-Four sense, but the same reasoning recurs
constantly once real services and DI arrive:

- **Interface vs abstract class** (week3/Day05/InterfaceVsAbstract.cs) is
  the same decision behind every `IRepository<T>`/`IPaymentStrategy`-style
  interface above — no shared state, need multiple contracts → interface;
  shared state + constructor invariants → abstract base.
- **Static vs instance** (week3/Day05/MathHelper.cs, OrderProcessor.cs)
  is the same decision behind "should this be a stateless static helper
  (`MathHelper`, `Math`, most `*Extensions` classes) or a DI-registered
  instance service with per-request/per-session state (`OrderProcessor`)".
