using System;

namespace SimpleCore
{
    public interface IUser
    {
        Guid Id { get;  }
        string Name { get;  }
        bool Anonymous { get; }

        string SessionId { get; }
    }
}