using System;
using System.Collections.Generic;
namespace Joker.CodeAnalysis.Binding
{
    internal abstract class BoundNode
    {
        public abstract BoundNodeKind Kind { get; }        
    }
}