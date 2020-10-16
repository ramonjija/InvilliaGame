using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces
{
    public interface IServiceResult<T> where T : class
    {
       List<string> ValidationMessages { get; }
       T Result { get;  }
       bool Success { get; }
       void AddMessage(string message);
       void AddMessage(IEnumerable<string> messages);
       void SetResult(T Result);
    }
}
