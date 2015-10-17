using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for BusinessRuleException
/// </summary>
/// //no name space, b/c can be accessed by website or eRestaurantSystem, it is known to everyone
[Serializable] //you can handle it in a special way
public class BusinessRuleException : Exception
{
    public List<string> RuleDetails { get; set; }
    public BusinessRuleException(string message, List<string> reasons)
        : base(message)
    {
        this.RuleDetails = reasons;
    }
}

