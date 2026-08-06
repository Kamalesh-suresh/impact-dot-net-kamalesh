// Several tests redirect the global Console.Out to capture output; running
// test classes in parallel would race on that shared static state.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
