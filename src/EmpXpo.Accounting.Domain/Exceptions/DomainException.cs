﻿namespace EmpXpo.Accounting.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string mensagem)
           : base(mensagem) { }
    }
}
