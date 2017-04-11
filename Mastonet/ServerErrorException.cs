using Mastonet.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mastonet
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException(Error error)
            :base(error.Description)
        {
        }
    }
}
