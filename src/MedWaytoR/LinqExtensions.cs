namespace MWR.MedWaytoR;

internal static class LinqExtensions
{
    /*
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Apply<TIn>(this IEnumerable<TIn> enumerable, Action<TIn> action)
        {
            foreach (var item in enumerable) action(item);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async Task RunAllInSequence(
            this IEnumerable<Task> tasks,
            CancellationToken ct = default)
        {
            foreach (var task in tasks)
            {
                ct.ThrowIfCancellationRequested();
                await task.ConfigureAwait(false);
            }
        }*/

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task RunAllInSequence(this IEnumerable<Func<Task>> tasks, CancellationToken ct = default)
    {
        foreach (var task in tasks)
        {
            ct.ThrowIfCancellationRequested();
            await task().ConfigureAwait(false);
        }
    }

    public static Task RunAllInParallel(this IEnumerable<Func<Task>> tasks)
    {
        return Task.WhenAll(tasks.Select(static t => t()));
    }

    /*//[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Task RunAllInParallel(this IEnumerable<Task> tasks)
    {
        return Task.WhenAll(tasks);
    }

    //[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ConfiguredTaskAwaitable RunAllInParallelConfigured(
        this IEnumerable<Task> tasks,
        bool continueOnCapturedContext = false,
        CancellationToken ct = default)
    {
        return Task.WhenAll(tasks).ConfigureAwait(continueOnCapturedContext);
    }*/
}