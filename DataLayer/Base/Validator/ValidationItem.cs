using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Base.Validator;
public class ValidationItem
{
	public Func<Task<bool>> Rule { get; set; }
	public string Value { get; set; }
}