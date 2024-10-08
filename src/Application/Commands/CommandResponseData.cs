﻿using Shared.Commands;

namespace Application.Commands
{
    public class CommandResponseData<T> : ICommandResult
    {
        public CommandResponseData(T data)
        {
            Data = data;
            Success = true;
        }

        public T Data { get; set; }
        public bool Success { get; }
    }
}
