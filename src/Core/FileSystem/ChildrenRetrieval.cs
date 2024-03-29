﻿using System;
using System.Collections.Generic;

namespace Core.FileSystem {
    public class ChildrenRetrieval<T> {
        private ChildrenRetrieval(ChildrenRetrievalType action, IReadOnlyList<T> children) : this(action) {
            _children = children;
        }

        private ChildrenRetrieval(ChildrenRetrievalType action) {
            _action = action;
        }

        public static ChildrenRetrieval<T> Create() =>
            new(ChildrenRetrievalType.Create);

        public static ChildrenRetrieval<T> Take(IReadOnlyList<T> children) =>
            new(ChildrenRetrievalType.Take, children);

        public static ChildrenRetrieval<T> TakeStateOnly(IReadOnlyList<T> children) =>
            new(ChildrenRetrievalType.TakeStateOnly, children);

        private readonly ChildrenRetrievalType _action;

        private readonly IReadOnlyList<T> _children;

        public IReadOnlyList<T> Retrieve(Func<IReadOnlyList<T>> create,
            Func<IReadOnlyList<T>, IReadOnlyList<T>> take,
            Func<IReadOnlyList<T>, IReadOnlyList<T>> takeStateOnly) =>
            _action switch {
                ChildrenRetrievalType.Create => create(),
                ChildrenRetrievalType.Take => take(_children),
                ChildrenRetrievalType.TakeStateOnly => takeStateOnly(_children),
                _ => throw new NotSupportedException(),
            };
    }
}
