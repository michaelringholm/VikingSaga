using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VikingSaga.Code
{
    internal static class Ui
    {
        public static int UimanagedThreadId;

        internal static SynchronizationContext _ctx;
        public static SynchronizationContext Ctx
        {
            get
            {
                if (_ctx == null)
                    throw new InvalidOperationException("CardEngineStatic.Init has not been called");
                return _ctx;
            }
        }

        public static void Send(Action action)
        {
            Ctx.Send((x) => action(), null);
        }

        public static void Post(Action action)
        {
            Ctx.Post((x) => action(), null);
        }
    }

    public static class CardEngineStatic
    {
        public static void Init(SynchronizationContext uiContext)
        {
            Ui._ctx = uiContext;
            Ui.UimanagedThreadId = Thread.CurrentThread.ManagedThreadId;
        }
    }
}
