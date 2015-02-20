using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JG.Duplicates.Client.Behaviours
{
    public interface IDraggable
    {
        /// <summary>
        /// Object item that is dragged
        /// </summary>
        object DragItem { get; }
    }
}
