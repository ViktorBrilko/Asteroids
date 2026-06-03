using System.Collections.Generic;

namespace MVVM
{
    public class BinderComposite : IBinder
    {
        private readonly List<IBinder> children;

        public BinderComposite(List<IBinder> children)
        {
            this.children = children;
        }

        public virtual void Bind()
        {
            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                child.Bind();
            }
        }

        public virtual void Unbind()
        {
            for (int i = 0, count = children.Count; i < count; i++)
            {
                var child = children[i];
                child.Unbind();
            }
        }
    }
}