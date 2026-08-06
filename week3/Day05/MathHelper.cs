// ===================================================================
// Task 3.13 (part 2) — static vs instance
// ===================================================================
//
// MathHelper is `static` because every method is a pure function:
// output depends only on the arguments, there is no state to carry
// between calls, and nothing about "which MathHelper" would ever be
// meaningful (there is exactly one correct way to compute a GCD).
// Forcing callers to write `new MathHelper().GCD(a, b)` would add
// ceremony with zero benefit — same reason Math.Sqrt is static.
//
// OrderProcessor (see OrderProcessor.cs) is instance-based because
// each processor accumulates state across calls (the running list of
// orders for one customer/session). Two customers checking out at the
// same time need two independent totals, so the state must live on
// an instance, not be shared globally the way a static field would.
// Instance methods also make it substitutable behind an interface for
// DI/testing, which a static class can never be.
// ===================================================================

public static class MathHelper
{
    public static long Factorial(int n)
    {
        if (n < 0)
            throw new ArgumentOutOfRangeException(nameof(n), "Factorial is undefined for negative numbers.");

        long result = 1;
        for (int i = 2; i <= n; i++)
            result *= i;
        return result;
    }

    public static bool IsPrime(int n)
    {
        if (n < 2) return false; // 0, 1, and negatives are not prime
        if (n == 2) return true;
        if (n % 2 == 0) return false;

        for (int i = 3; (long)i * i <= n; i += 2)
        {
            if (n % i == 0) return false;
        }
        return true;
    }

    public static int GCD(int a, int b)
    {
        a = Math.Abs(a);
        b = Math.Abs(b);
        while (b != 0)
        {
            (a, b) = (b, a % b);
        }
        return a; // GCD(0, 0) = 0 by convention
    }
}
