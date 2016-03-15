using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramer.Utilities
{
    public interface IAsyncEnumerator<T> : IEnumerator<T>
    {
        Task<bool> MoveNextAsync();

        Task ResetAsync();
    }
}
