﻿using System;

namespace Key2Btn.Base.Helper
{
    public interface IAbstractFactory<T>
    {
        T Create();
    }

    public class AbstractFactory<T> : IAbstractFactory<T>
    {
        private readonly Func<T> _factory;

        public AbstractFactory(Func<T> factory)
        {
            _factory = factory;
        }

        public T Create()
        {
            return _factory.Invoke();
        }
    }
}
