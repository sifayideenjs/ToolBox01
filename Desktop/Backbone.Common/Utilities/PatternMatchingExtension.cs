using System;

namespace Backbone.Common.Utilities
{
    public interface IFunctionBuilder<T, U>
    {
        IFunctionBuilder<T, U> Predecessor { get; }
        U Build();
        U Build(Func<T, U> @default);
    }

    public interface IActionBuilder<T>
    {
        IActionBuilder<T> Predecessor { get; }
        void Build();
        void Build(Action<T> @default);
    }

    /// <summary>
    ///     Defines pattern matching monads.
    /// </summary>
    public static class PatternMatchingExtension
    {
        public static FunctionBuilder<T, U> Match<T, U>(this T source)
        {
            return new FunctionBuilder<T, U>(source);
        }

        public static ActionBuilder<T> Act<T>(this T source)
        {
            return new ActionBuilder<T>(source);
        }
    }

    public class FunctionBuilder<T, U> : IFunctionBuilder<T, U>, IDisposable
    {
        private readonly T _src;
        private Func<T, U> _fun;
        private Predicate<T> _pred;

        public FunctionBuilder(T src)
        {
            this._src = src;
        }

        private FunctionBuilder(T src, Predicate<T> pred, Func<T, U> fun, IFunctionBuilder<T, U> predecessor)
        {
            this._src = src;
            this._pred = pred;
            this._fun = fun;
            Predecessor = predecessor;
        }

        #region ** IDisposable

        public void Dispose()
        {
            _fun = null;
            Predecessor = null;
            _pred = null;
        }

        #endregion //IDisposable

        #region Members

        public IFunctionBuilder<T, U> Predecessor { get; private set; }

        #endregion

        #region  Constructs

        public FunctionBuilder<T, U> When(Predicate<T> pred, Func<T, U> fun)
        {
            return new FunctionBuilder<T, U>(_src, pred, fun, this);
        }

        public FunctionBuilder<T, U> IfEquals(T val, Func<T, U> fun)
        {
            return new FunctionBuilder<T, U>(_src, x => x.Equals(val), fun, this);
        }

        public FunctionBuilder<T, U> OfType<Tx>(Func<Tx, U> fun)
        {
            return new FunctionBuilder<T, U>(_src, _ => _src is Tx, t => fun((Tx) (t as object)), this);
        }

        public U Build()
        {
            Func<U> throwEx = () => { throw new InvalidOperationException("No match or default case found."); };

            var res = (_pred == null)
                ? throwEx()
                : _pred(_src)
                    ? _fun(_src)
                    : (Predecessor == null)
                        ? throwEx()
                        : Predecessor.Build();

            Dispose();

            return res;
        }

        public U Build(Func<T, U> @default)
        {
            var res = _pred == null
                ? @default(_src)
                : _pred(_src)
                    ? _fun(_src)
                    : Predecessor == null
                        ? @default(_src)
                        : Predecessor.Build(@default);

            Dispose();

            return res;
        }

        #endregion
    }

    public class ActionBuilder<T> : IActionBuilder<T>, IDisposable
    {
        private readonly T _src;
        private Action<T> @do;
        private Predicate<T> _pred;

        public ActionBuilder(T src)
        {
            this._src = src;
        }

        private ActionBuilder(T src, Predicate<T> pred, Action<T> @do, IActionBuilder<T> predecessor)
        {
            this._src = src;
            this._pred = pred;
            this.@do = @do;
            Predecessor = predecessor;
        }

        #region Members

        public IActionBuilder<T> Predecessor { get; private set; }

        #endregion

        #region ** IDisposable

        public void Dispose()
        {
            @do = null;
            Predecessor = null;
            _pred = null;
        }

        #endregion //IDisposable

        #region  Constructs

        public ActionBuilder<T> When(Predicate<T> pred, Action<T> @do)
        {
            return new ActionBuilder<T>(_src, pred, @do, this);
        }

        public ActionBuilder<T> IfEquals(T val, Action<T> @do)
        {
            return new ActionBuilder<T>(_src, x => x.Equals(val), @do, this);
        }

        public ActionBuilder<T> OfType<Tx>(Action<Tx> @do)
        {
            return new ActionBuilder<T>((T) _src, _ => _src is Tx, t => @do((Tx) (t as object)), this);
        }

        public void Build()
        {
            Action throwEx = () => { throw new InvalidOperationException("No match or default case found."); };

            if (_pred == null)
                throwEx();
            else if (_pred(_src))
                @do(_src);
            else if (Predecessor != null)
                Predecessor.Build();
            else
                throwEx();

            Dispose();
        }

        public void Build(Action<T> @default)
        {
            if (_pred == null)
                @default(_src);
            else if (_pred(_src))
                @do(_src);
            else if (Predecessor != null)
                Predecessor.Build(@default);
            else
                @default(_src);

            Dispose();
        }

        #endregion
    }
}