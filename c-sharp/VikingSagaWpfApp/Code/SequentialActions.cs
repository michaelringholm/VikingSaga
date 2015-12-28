using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace VikingSaga.Code
{
    public class SequentialActions
    {
        public class Multiple
        {
            private List<IEnumerable<int>> _actions = new List<IEnumerable<int>>();
            private List<ManualResetEventSlim> _waitEvents = new List<ManualResetEventSlim>();

            public void Add(IEnumerable<int> action)
            {
                _actions.Add(action);
            }

            public void RunBlocking()
            {
                foreach (var action in _actions)
                    _waitEvents.Add(RunAsync(action));

                foreach (var waitEvent in _waitEvents)
                    waitEvent.Wait();
            }
        }

        public static void RunBlocking(IEnumerable<int> actions)
        {
            if (Thread.CurrentThread.ManagedThreadId == Ui.UimanagedThreadId)
            {
                throw new InvalidOperationException("Don't call this from UI thread! It will deadlock messagepump!");
            }

            RunAsync(actions).Wait();
        }

        public static ManualResetEventSlim RunAsync(IEnumerable<int> actions)
        {
            var e = actions.GetEnumerator();
            ManualResetEventSlim doneEvent = new ManualResetEventSlim(false);
            PostIteration(e, doneEvent);

            return doneEvent;
        }

        private static void Iterate(IEnumerator<int> e, ManualResetEventSlim doneEvent)
        {
            if (!e.MoveNext())
            {
                doneEvent.Set();
                return;
            }

            int delay = (int)e.Current;
            Task.Delay(delay).ContinueWith((dummyState) => PostIteration(e, doneEvent));
        }

        private static void PostIteration(IEnumerator<int> e, ManualResetEventSlim doneEvent)
        {
            Ui.Post(() => Iterate(e, doneEvent));
        }
    }
}
