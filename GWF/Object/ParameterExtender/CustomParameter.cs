using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Oracle.ManagedDataAccess.Client;

/// <summary>
/// Summary description for CustomParameter
/// </summary>
public class CustomParameter : Parameter
{
	private object _objectValue;
	private string _propertyName;
	private Control _control;
	private HttpContext _context;
    private ParameterType _paramType;
    private OracleDbType _paramOracleType;

	public object DataValue
	{
		get { return _objectValue; }
		set { _objectValue = value; }
	}

	public string PropertyName
	{
		get { return _propertyName; }
		set { _propertyName = value; }
	}

	public Control Control
	{
		get { return _control; }
		set { _control = value; }
	}

	public HttpContext Context
	{
		get { return _context; }
		set { _context = value; }
	}

	public ParameterType ParamType
	{
		get { return _paramType; }
		set { _paramType = value; }
	}

    public OracleDbType ParamOracleType
    {
        get { return _paramOracleType; }
        set { _paramOracleType = value; }
    }

    public ParameterDirection ParamDirection
    {
        get { return base.Direction; }
        set { base.Direction = value; }
    }

	public CustomParameter()
	{
	}

	public CustomParameter(string name)
	{
		this.Name = name;
	}

    public CustomParameter(string name, object objectValue)
        : this(name)
    {
        _objectValue = objectValue;
        _paramType = ParameterType.Object;
    }

	public CustomParameter(string name, int objectValue, ParameterDirection paramDirection)
		: this(name)
	{
		_objectValue = objectValue;
		_paramType = ParameterType.Object;
        base.Direction = paramDirection;
	}

    public CustomParameter(string name, string objectValue, ParameterDirection paramDirection)
        : this(name)
    {
        _objectValue = objectValue;
        _paramType = ParameterType.Object;
        base.Direction = paramDirection;
    }

    public CustomParameter(string name, object objectValue, DbType dbType)
        : this(name)
    {
        _objectValue = objectValue;
        _paramType = ParameterType.Object;
        base.DbType = dbType;
    }

    public CustomParameter(string name, object objectValue, DbType dbType, int size, ParameterDirection paramDirection)
        : this(name)
    {
        _objectValue = objectValue;
        _paramType = ParameterType.Object;
        base.Direction = paramDirection;
        base.DbType = dbType;
        base.Size = size;
    }

    public CustomParameter(string name, object objectValue, int size, ParameterDirection paramDirection)
        : this(name)
    {
        _objectValue = objectValue;
        _paramType = ParameterType.Object;
        base.Direction = paramDirection;
        base.Size = size;
    }


    public CustomParameter(string name, Control control, string propertyName)
		: this(name)
	{
		_control = control;
		_paramType = ParameterType.Control;
		_propertyName = propertyName;
	}

	public CustomParameter(string name, HttpContext context, ParameterType paramType, string propertyName)
		: this(name)
	{
		_context = context;
		_paramType = paramType;
		_propertyName = propertyName;
	}

    //잠시 꺼져줄래?
    //public CustomParameter(string name, OracleDbType paramType, string objectValue) 
    //    : this(name)
    //{
    //    _objectValue = objectValue;
    //    _paramOracleType = paramType;
    //}
    
	public object GetValue()
	{
		if (_paramType == ParameterType.Object)
		{
			return _objectValue;
		}
		else if (_paramType == ParameterType.Control)
		{
			if (_control != null)
			{
				return _control.GetType().GetProperty(_propertyName).GetValue(_control, null);
			}

			throw new ApplicationException("Control instance is null");
		}
		else if (_paramType == ParameterType.Cache
			|| _paramType == ParameterType.Cookie
			|| _paramType == ParameterType.Form
			|| _paramType == ParameterType.QueryString
			|| _paramType == ParameterType.Session)
		{
			if (_context != null)
			{
				if (_paramType == ParameterType.Cache)
				{
					return _context.Cache[_propertyName];
				}
				else if (_paramType == ParameterType.Cookie)
				{
					return _context.Request.Cookies[_propertyName].Value;
				}
				else if (_paramType == ParameterType.Form)
				{
					return _context.Request.Form[_propertyName];
				}
				else if (_paramType == ParameterType.QueryString)
				{
					return _context.Request.QueryString[_propertyName];
				}
				else if (_paramType == ParameterType.Session)
				{
					return _context.Session[_propertyName];
				}
			}
			else
			{
				throw new ApplicationException("HttpContext is null");
			}
		}

		throw new ApplicationException("Value is nothing.");
	}

	public void SetValue(object oValue)
	{
		if (_paramType == ParameterType.Object)
		{
			_objectValue = oValue;
		}
		else if (_paramType == ParameterType.Control)
		{
			if (_control != null)
			{
				_control.GetType().GetProperty(_propertyName).SetValue(_control, oValue, null);
			}
		}
		else if (_paramType == ParameterType.Cache
			|| _paramType == ParameterType.Cookie
			|| _paramType == ParameterType.Form
			|| _paramType == ParameterType.QueryString
			|| _paramType == ParameterType.Session)
		{
			if (_context != null)
			{
				if (_paramType == ParameterType.Cache)
				{
					_context.Cache[_propertyName] = oValue;
				}
				else if (_paramType == ParameterType.Cookie)
				{
					_context.Request.Cookies[_propertyName].Value = oValue.ToString();
				}
				else if (_paramType == ParameterType.Form)
				{
					_context.Request.Form[_propertyName] = oValue.ToString();
				}
				else if (_paramType == ParameterType.QueryString)
				{
					_context.Request.QueryString[_propertyName] = oValue.ToString();
				}
				else if (_paramType == ParameterType.Session)
				{
					_context.Session[_propertyName] = oValue;
				}
			}
			else
			{
				throw new ApplicationException("HttpContext is null");
			}
		}
	}
}
