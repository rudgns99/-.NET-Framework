using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CustomParameterCollection
{
    ArrayList ParameterList;

    public int Count
    {
        get { return ParameterList.Count; }
    }

    public CustomParameter this[int index]
    {
        get { return (CustomParameter)ParameterList[index]; }
        set { ParameterList[index] = value; }
    }

    public CustomParameter this[string ParameterName]
    {
        get { return FindParameter(ParameterName); }
    }

    public CustomParameterCollection()
    {
        ParameterList = new ArrayList();
    }

    public void Add(CustomParameter param)
    {
        ParameterList.Add(param);
    }

    private CustomParameter FindParameter(string PName)
    {
        CustomParameter retVal = new CustomParameter();

        foreach (CustomParameter cp in ParameterList)
        {
            if (cp.Name == PName)
            {
                retVal = cp;
            }
        }

        return retVal;
    }
}