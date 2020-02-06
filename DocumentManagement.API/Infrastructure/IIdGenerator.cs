using System;

namespace DocumentManagement.API.Infrastructure
{
    public interface IIdGenerator
    {
        Guid NewId();
    }
}
