using System;
using System.Collections.Generic;
using System.Linq;
using GoogleSheet;
using GoogleSheet.Protocol.v2.Res;
using System.Reflection;
using UGS;
using UnityEngine.Assertions;
using System.Diagnostics;

public class DataHandler<T> where T : ITable
{
    private readonly Dictionary<int, T> dataDictionary;
    private readonly List<T> dataCollection;
    private readonly Action<bool> loadAction;
    private readonly Action<T, Action<WriteObjectResult>> writeAction;

    public DataHandler()
    {
        // UGS Data 클래스의 인스턴스 생성
        // var dataInstance = Activator.CreateInstance<T>(); 
        
        // 리플렉션으로 필요한 멤버 가져오기 
        var type = typeof(T);  
        
        // Dictionary와 List 필드 찾기
        var dictionaryField = type.GetField($"{type.Name}Map", BindingFlags.Public | BindingFlags.Static); 
        var listField = type.GetField($"{type.Name}List", BindingFlags.Public | BindingFlags.Static);
        
        Debug.Assert(dictionaryField != null, $"Required fields not found in type {type.Name}");
        Debug.Assert(listField != null, $"Required fields not found in type {type.Name}"); 

        // Load와 Write 메서드 찾기
        var loadMethod = type.GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
        var writeMethod = type.GetMethod("Write", BindingFlags.Public | BindingFlags.Static);
        
        Debug.Assert(loadMethod != null, $"Required methods not found in type {type.Name}");
        Debug.Assert(writeMethod != null, $"Required methods not found in type {type.Name}"); 

        // 초기 데이터 로드
        loadMethod.Invoke(null, new object[] { false });

        // 데이터 초기화
        dataDictionary = (Dictionary<int, T>)dictionaryField.GetValue(null);
        dataCollection = (List<T>)listField.GetValue(null);
        
        loadAction = (forceReload) => loadMethod.Invoke(null, new object[] { forceReload });
        writeAction = (data, callback) => writeMethod.Invoke(null, new object[] { data, callback });
    }

    /// <summary>
    /// 인덱스로 데이터를 조회합니다.
    /// </summary>
    public T GetByIndex(int index)
    {
        if (dataDictionary.TryGetValue(index, out var item))
        {
            return item;
        }
        return default;
    }

    /// <summary>
    /// 모든 데이터를 반환합니다.
    /// </summary>
    public List<T> GetAll()
    {
        return dataCollection;
    }

    /// <summary>
    /// 데이터 총 개수를 반환합니다.
    /// </summary>
    public int GetCount()
    {
        return dataCollection.Count;
    }

    /// <summary>
    /// 조건에 맞는 데이터들을 필터링하여 반환합니다.
    /// </summary>
    public List<T> GetByCondition(Func<T, bool> predicate)
    {
        return dataCollection.Where(predicate).ToList();
    }

    /// <summary>
    /// 데이터를 업데이트합니다.
    /// </summary>
    public void UpdateData(T data, Action<bool> onComplete = null)
    {
        if (data == null)
        {
            onComplete?.Invoke(false);
            return;
        }

        writeAction(data, (result) =>
        {
            bool success = result != null && result.error == null;
            if (success)
            {
                loadAction(true);
            }
            onComplete?.Invoke(success);
        });
    }

    /// <summary>
    /// 데이터를 강제로 리로드합니다.
    /// </summary>
    public void Reload()
    {
        loadAction(true);
    }
}