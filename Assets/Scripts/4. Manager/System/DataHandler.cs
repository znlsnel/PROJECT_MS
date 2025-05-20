using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public abstract class DataHandler<T> where T : class
{
    protected Dictionary<int, T> datas = new Dictionary<int, T>();
    protected List<T> dataList = new List<T>();

    public DataHandler()
    {
        Init();
    }

    protected abstract void Init();

    
    public List<T> GetAll()
    {
        return dataList;
    }

    public T GetByIndex(int index)
    {
        if (datas.TryGetValue(index, out T data))
            return data;

        return null;
    }

    public T GetByCondition(Func<T, bool> condition)
    {
        foreach (T data in dataList)
        {
            if (condition(data))
                return data;
        }

        return null;
    }

    public List<T> GetAllByCondition(Func<T, bool> condition)
    {
        List<T> items = new List<T>();
        foreach (T data in dataList)
        {
            if (condition(data))
                items.Add(data);
        }

        return items;
    }
}
